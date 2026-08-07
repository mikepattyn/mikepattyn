const { DynamoDBClient } = require("@aws-sdk/client-dynamodb");
const {
  DynamoDBDocumentClient,
  UpdateCommand,
  PutCommand,
  QueryCommand
} = require("@aws-sdk/lib-dynamodb");

const TABLE_NAME = process.env.TABLE_NAME;
const LESSON_IDS = ["listening", "intention", "roles", "shape", "thinking", "integrity"];
const VALID_TYPES = new Set([
  "visit",
  "lesson_view",
  "lesson_walked",
  "lesson_unwalked",
  "completed"
]);

const doc = DynamoDBDocumentClient.from(new DynamoDBClient({}));

function json(statusCode, body) {
  return {
    statusCode,
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(body)
  };
}

function todayUtc() {
  return new Date().toISOString().slice(0, 10);
}

function ttl90Days() {
  return Math.floor(Date.now() / 1000) + 90 * 24 * 60 * 60;
}

async function incrementCounter(sk, amount = 1) {
  await doc.send(
    new UpdateCommand({
      TableName: TABLE_NAME,
      Key: { PK: "COUNTER", SK: sk },
      UpdateExpression: "ADD #v :n",
      ExpressionAttributeNames: { "#v": "value" },
      ExpressionAttributeValues: { ":n": amount }
    })
  );
}

async function recordMilestone(vid, milestone) {
  try {
    await doc.send(
      new PutCommand({
        TableName: TABLE_NAME,
        Item: {
          PK: `VIS#${vid}`,
          SK: `MILESTONE#${milestone}`,
          at: Date.now()
        },
        ConditionExpression: "attribute_not_exists(PK)"
      })
    );
    await incrementCounter(`unique:${milestone}`);
    return true;
  } catch (err) {
    if (err.name === "ConditionalCheckFailedException") return false;
    throw err;
  }
}

async function writeRawEvent(type, vid, lessonId) {
  const now = Date.now();
  const day = todayUtc();
  await doc.send(
    new PutCommand({
      TableName: TABLE_NAME,
      Item: {
        PK: `DAY#${day}`,
        SK: `EVT#${now}#${Math.random().toString(36).slice(2, 8)}`,
        type,
        vid,
        lessonId: lessonId || null,
        ts: now,
        ttl: ttl90Days()
      }
    })
  );
}

async function handleEvent(body) {
  const { type, vid, lessonId } = body;
  if (!type || !vid || !VALID_TYPES.has(type)) {
    return json(400, { error: "Invalid event" });
  }

  const day = todayUtc();

  switch (type) {
    case "visit":
      await incrementCounter("total:visits");
      await incrementCounter(`day:${day}:visits`);
      await recordMilestone(vid, "visit");
      break;
    case "lesson_view":
      await incrementCounter("total:lesson_views");
      await recordMilestone(vid, "lesson_view");
      break;
    case "lesson_walked":
      if (!lessonId) return json(400, { error: "lessonId required" });
      await incrementCounter(`lesson:${lessonId}:walked`);
      await recordMilestone(vid, "walked_one");
      break;
    case "lesson_unwalked":
      break;
    case "completed":
      await incrementCounter("total:completions");
      await incrementCounter(`day:${day}:completions`);
      await recordMilestone(vid, "completed");
      break;
    default:
      return json(400, { error: "Unknown type" });
  }

  await writeRawEvent(type, vid, lessonId);
  return json(202, { ok: true });
}

async function getCounter(sk) {
  const result = await doc.send(
    new QueryCommand({
      TableName: TABLE_NAME,
      KeyConditionExpression: "PK = :pk AND SK = :sk",
      ExpressionAttributeValues: { ":pk": "COUNTER", ":sk": sk }
    })
  );
  const item = result.Items?.[0];
  return item?.value ?? 0;
}

async function handleStats() {
  const day = todayUtc();
  const counterKeys = [
    "total:visits",
    "total:lesson_views",
    "total:completions",
    "unique:visit",
    "unique:lesson_view",
    "unique:walked_one",
    "unique:completed"
  ];

  const lessonKeys = LESSON_IDS.map((id) => `lesson:${id}:walked`);

  const dailyKeys = [];
  for (let i = 29; i >= 0; i--) {
    const d = new Date();
    d.setUTCDate(d.getUTCDate() - i);
    const date = d.toISOString().slice(0, 10);
    dailyKeys.push({ date, visits: `day:${date}:visits`, completions: `day:${date}:completions` });
  }

  const allKeys = [...counterKeys, ...lessonKeys, ...dailyKeys.flatMap((d) => [d.visits, d.completions])];
  const values = await Promise.all(allKeys.map((sk) => getCounter(sk)));

  const map = Object.fromEntries(allKeys.map((k, i) => [k, values[i]]));

  return json(200, {
    totals: {
      visits: map["total:visits"],
      uniqueVisitors: map["unique:visit"],
      lessonViews: map["total:lesson_views"],
      completions: map["total:completions"]
    },
    funnel: {
      uniqueVisitors: map["unique:visit"],
      viewedLesson: map["unique:lesson_view"],
      walkedOne: map["unique:walked_one"],
      completed: map["unique:completed"]
    },
    lessons: Object.fromEntries(LESSON_IDS.map((id) => [id, map[`lesson:${id}:walked`] ?? 0])),
    daily: dailyKeys.map((d) => ({
      date: d.date,
      visits: map[d.visits] ?? 0,
      completions: map[d.completions] ?? 0
    })),
    updatedAt: new Date().toISOString()
  });
}

exports.handler = async (event) => {
  const method = event.httpMethod || event.requestContext?.http?.method;
  const path = event.path || event.rawPath || "";

  if (method === "GET" && path.endsWith("/stats")) {
    return handleStats();
  }

  if (method === "POST" && path.endsWith("/events")) {
    let body;
    try {
      body = typeof event.body === "string" ? JSON.parse(event.body) : event.body;
    } catch {
      return json(400, { error: "Invalid JSON" });
    }
    return handleEvent(body);
  }

  return json(404, { error: "Not found" });
};
