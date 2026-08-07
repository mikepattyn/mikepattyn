const LESSON_LABELS = {
  listening: "1. Listening",
  intention: "2. Intention",
  roles: "3. Roles",
  shape: "4. Shape",
  thinking: "5. Thinking",
  integrity: "6. Integrity"
};

const REFRESH_MS = 60_000;

function fmt(n) {
  return Number(n ?? 0).toLocaleString("en-GB");
}

function pct(value, max) {
  if (!max) return 0;
  return Math.round((value / max) * 100);
}

function renderTotals(totals) {
  return `
    <div class="panel">
      <h2>Totals</h2>
      <div class="totals">
        <div class="stat"><span class="stat-value">${fmt(totals.visits)}</span><span class="stat-label">Visits</span></div>
        <div class="stat"><span class="stat-value">${fmt(totals.uniqueVisitors)}</span><span class="stat-label">Unique visitors</span></div>
        <div class="stat"><span class="stat-value">${fmt(totals.lessonViews)}</span><span class="stat-label">Lesson views</span></div>
        <div class="stat"><span class="stat-value">${fmt(totals.completions)}</span><span class="stat-label">Completions (6/6)</span></div>
      </div>
    </div>`;
}

function renderFunnel(funnel) {
  const max = funnel.uniqueVisitors || 1;
  const steps = [
    ["Unique visitors", funnel.uniqueVisitors],
    ["Viewed a lesson", funnel.viewedLesson],
    ["Walked ≥1 lesson", funnel.walkedOne],
    ["Zero to hero (6/6)", funnel.completed]
  ];

  return `
    <div class="panel">
      <h2>Funnel — start → completion</h2>
      ${steps
        .map(
          ([label, count]) => `
        <div class="funnel-row">
          <span class="funnel-label">${label}</span>
          <div class="funnel-bar-wrap">
            <div class="funnel-bar" style="width:${pct(count, max)}%"></div>
          </div>
          <span class="funnel-count">${fmt(count)}</span>
        </div>`
        )
        .join("")}
    </div>`;
}

function renderLessons(lessons) {
  const rows = Object.entries(lessons)
    .map(
      ([id, count]) => `
      <div class="lesson-item">
        <span class="lesson-id">${LESSON_LABELS[id] || id}</span>
        <span class="lesson-count">${fmt(count)} walked</span>
      </div>`
    )
    .join("");

  return `
    <div class="panel">
      <h2>Per-lesson walked counts</h2>
      <div class="lesson-grid">${rows}</div>
    </div>`;
}

function renderDaily(daily) {
  const maxVal = Math.max(
    1,
    ...daily.flatMap((d) => [d.visits, d.completions])
  );

  const cols = daily
    .map((d) => {
      const visitH = pct(d.visits, maxVal);
      const compH = pct(d.completions, maxVal);
      const label = d.date.slice(5);
      return `
        <div class="daily-col" title="${d.date}: ${d.visits} visits, ${d.completions} completions">
          <div class="daily-bar-visits" style="height:${visitH}%"></div>
          <div class="daily-bar-completions" style="height:${compH}%"></div>
          <span class="daily-label">${label}</span>
        </div>`;
    })
    .join("");

  return `
    <div class="panel">
      <h2>Daily — last 30 days</h2>
      <div class="daily-chart">${cols}</div>
      <div class="legend">
        <span class="legend-visits">Visits</span>
        <span class="legend-completions">Completions</span>
      </div>
    </div>`;
}

function renderStats(data) {
  return [
    renderTotals(data.totals),
    renderFunnel(data.funnel),
    renderLessons(data.lessons),
    renderDaily(data.daily)
  ].join("");
}

async function loadStats() {
  const res = await fetch("/api/stats");
  if (!res.ok) throw new Error(`HTTP ${res.status}`);
  return res.json();
}

async function refresh() {
  const app = document.getElementById("app");
  const updated = document.getElementById("updated-at");
  const btn = document.getElementById("refresh-btn");

  btn.disabled = true;
  try {
    const data = await loadStats();
    app.innerHTML = renderStats(data);
    updated.textContent = `Updated ${new Date(data.updatedAt).toLocaleString("en-GB")}`;
  } catch (err) {
    app.innerHTML = `<p class="error">Failed to load stats: ${err.message}</p>`;
    updated.textContent = "Error";
  } finally {
    btn.disabled = false;
  }
}

document.getElementById("refresh-btn").addEventListener("click", refresh);
refresh();
setInterval(refresh, REFRESH_MS);
