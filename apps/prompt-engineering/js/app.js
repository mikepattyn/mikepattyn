/* Lumen — app shell: hash routing, rendering, progress, garden, journal. */

const STORE_KEYS = {
  progress: "lumen.progress",
  journal: "lumen.journal"
};

const store = {
  read(key, fallback) {
    try {
      const raw = localStorage.getItem(key);
      return raw ? JSON.parse(raw) : fallback;
    } catch {
      return fallback;
    }
  },
  write(key, value) {
    localStorage.setItem(key, JSON.stringify(value));
  }
};

const getProgress = () => store.read(STORE_KEYS.progress, {});
const getJournal = () => store.read(STORE_KEYS.journal, []);

function escapeHtml(s) {
  return s.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/"/g, "&quot;");
}

/* ————— Progress ring in the header ————— */
function updateProgressPill() {
  const done = Object.values(getProgress()).filter(Boolean).length;
  const total = LESSONS.length;
  const pct = total ? Math.round((done / total) * 100) : 0;
  document.getElementById("ring-fill").style.strokeDasharray = `${pct} 100`;
  document.getElementById("progress-label").textContent = `${done}/${total}`;
}

/* ————— Views ————— */

function homeView() {
  const features = [
    { icon: "🌙", title: "Learn by feeling", body: "Six short lessons that teach real prompt-engineering techniques through the way you already think — imagined listeners, inner characters, named feelings." },
    { icon: "🌿", title: "A garden, not a grader", body: "Practice writing prompts in a space that lights lanterns for what's working instead of red-marking what isn't." },
    { icon: "📖", title: "Reflect as you go", body: "Every lesson ends with a gentle reflection, saved to a private journal that never leaves your browser." }
  ];

  return `
  <div class="view">
    <section class="hero">
      <span class="hero-eyebrow">A course for INFPs</span>
      <h1>Prompt engineering,<br/><span class="accent">for the ones who feel first</span></h1>
      <p class="lede">You already imagine your listener, hear tone in a single word, and carry whole worlds in your head. Those aren't obstacles to working with AI — they're the exact skills prompting is made of. This little course shows you how.</p>
      <div class="hero-actions">
        <a class="btn btn-primary" href="#/lesson/${LESSONS[0].id}">Begin the first lesson</a>
        <a class="btn btn-ghost" href="#/garden">Wander into the garden</a>
      </div>
    </section>

    <div class="card-grid">
      ${features.map(f => `
        <div class="card">
          <span class="icon">${f.icon}</span>
          <h3>${f.title}</h3>
          <p class="muted">${f.body}</p>
        </div>`).join("")}
    </div>

    <section class="card">
      <h3>Why a course just for INFPs?</h3>
      <p class="muted">Most prompt-engineering guides are written like engineering manuals — optimize, iterate, extract. Useful, but they skip the questions that actually stop sensitive, idealistic people: <em>Will this flatten my voice? Is it still mine? Why does the output feel emotionally wrong?</em> Lumen teaches the same core techniques — context, roles, examples, step-by-step reasoning — through metaphors that fit how you already think, and takes the values questions seriously instead of waving them away.</p>
    </section>
  </div>`;
}

function pathView() {
  const progress = getProgress();
  return `
  <div class="view">
    <h1>The Path</h1>
    <p class="lede">Six lessons, each a short walk. Take them in order or wander — the path will remember where you've been.</p>
    <ol class="path-list">
      ${LESSONS.map((l, i) => {
        const done = !!progress[l.id];
        return `
        <a class="path-item ${done ? "done" : ""}" href="#/lesson/${l.id}">
          <span class="step-mark">${done ? "✓" : l.icon}</span>
          <span class="step-body">
            <p class="step-title">${i + 1}. ${l.title}</p>
            <p class="step-sub">${l.subtitle}</p>
          </span>
          <span class="step-status">${done ? "walked" : "unwalked"}</span>
        </a>`;
      }).join("")}
    </ol>
  </div>`;
}

function renderBlock(block, lesson, index) {
  switch (block.type) {
    case "p":
      return `<p>${block.html}</p>`;
    case "h3":
      return `<h3>${block.text}</h3>`;
    case "note":
      return `<aside class="infp-note"><span class="note-label">For the INFP in you</span>${block.html}</aside>`;
    case "pair":
      return `
      <div class="prompt-pair">
        <div class="prompt-example faint">
          <span class="tag">${block.weakLabel || "Before"}</span>
          <pre>${escapeHtml(block.weak)}</pre>
        </div>
        <div class="prompt-example glow">
          <span class="tag">${block.strongLabel || "After"}</span>
          <pre>${escapeHtml(block.strong)}</pre>
        </div>
      </div>`;
    case "quiz":
      return `
      <div class="quiz card" data-quiz>
        <h3 style="margin-top:0">A gentle check</h3>
        <p>${block.question}</p>
        ${block.options.map((o, i) => `
          <button class="quiz-option" data-correct="${o.correct}" data-feedback="${escapeHtml(o.feedback)}">${escapeHtml(o.text)}</button>
        `).join("")}
        <p class="quiz-feedback" aria-live="polite"></p>
      </div>`;
    case "exercise":
      return `
      <div class="exercise" data-exercise data-lesson="${lesson.id}" data-index="${index}">
        <span class="exercise-label">${block.label}</span>
        <p>${block.html}</p>
        <textarea placeholder="${escapeHtml(block.placeholder || "Write freely...")}"></textarea>
        <div style="display:flex; align-items:center; gap:0.75rem; margin-top:0.75rem;">
          <button class="btn btn-soft" data-save-reflection>Keep this in my journal</button>
          <span class="muted" data-save-status aria-live="polite"></span>
        </div>
      </div>`;
    default:
      return "";
  }
}

function lessonView(id) {
  const idx = LESSONS.findIndex(l => l.id === id);
  if (idx === -1) return notFoundView();
  const lesson = LESSONS[idx];
  const progress = getProgress();
  const done = !!progress[lesson.id];
  const prev = LESSONS[idx - 1];
  const next = LESSONS[idx + 1];

  return `
  <div class="view">
    <header class="lesson-header">
      <span class="lesson-kicker">Lesson ${idx + 1} of ${LESSONS.length} · ${lesson.icon}</span>
      <h1>${lesson.title}</h1>
      <p class="lede">${lesson.subtitle}</p>
    </header>
    <div class="lesson-body">
      ${lesson.blocks.map((b, i) => renderBlock(b, lesson, i)).join("")}
    </div>
    <div class="lesson-nav">
      ${prev ? `<a class="btn btn-ghost" href="#/lesson/${prev.id}">← ${prev.title}</a>` : `<a class="btn btn-ghost" href="#/path">← The Path</a>`}
      <button class="btn ${done ? "btn-soft" : "btn-primary"}" data-toggle-complete="${lesson.id}">
        ${done ? "✓ Walked — tap to unmark" : "Mark this lesson walked"}
      </button>
      ${next ? `<a class="btn btn-ghost" href="#/lesson/${next.id}">${next.title} →</a>` : `<a class="btn btn-ghost" href="#/garden">To the garden →</a>`}
    </div>
  </div>`;
}

function gardenView() {
  return `
  <div class="view">
    <h1>The Practice Garden</h1>
    <p class="lede">Write a prompt below and watch the lanterns light. Nothing is graded here — each lamp is just one craft element your prompt already carries. Pick a seed if you'd like a scenario to practice with.</p>

    <div class="seed-row">
      ${GARDEN_SEEDS.map((s, i) => `<button class="seed-chip" data-seed="${i}">${s.label}</button>`).join("")}
    </div>
    <p class="muted" id="seed-scenario" aria-live="polite"></p>

    <div class="card garden-layout">
      <textarea id="garden-input" rows="7" placeholder="Dear model... (write the prompt you would actually send)"></textarea>
      <div>
        <div class="glow-meter"><div class="glow-fill" id="glow-fill"></div></div>
        <p class="glow-label" id="glow-label"></p>
        <ul class="feedback-list" id="feedback-list"></ul>
      </div>
    </div>
  </div>`;
}

function journalView() {
  const entries = getJournal();
  return `
  <div class="view">
    <h1>Your Journal</h1>
    <p class="lede">Reflections you chose to keep, newest first. They live only in this browser — no account, no cloud, no one reading over your shoulder.</p>
    ${entries.length === 0
      ? `<p class="empty-state">Nothing here yet. The reflections at the end of each lesson will gather here, like pressed flowers.</p>`
      : entries.slice().reverse().map(e => `
        <div class="journal-entry">
          <div class="entry-meta">
            <span>${escapeHtml(e.source)} · ${new Date(e.at).toLocaleString()}</span>
            <button class="entry-delete" data-delete-entry="${e.id}">let it go</button>
          </div>
          <p>${escapeHtml(e.text)}</p>
        </div>`).join("")}
  </div>`;
}

function notFoundView() {
  return `
  <div class="view">
    <div class="empty-state">
      <h1>A quiet clearing</h1>
      <p>This page doesn't exist — but you found a peaceful spot anyway.</p>
      <p><a class="btn btn-primary" href="#/">Back to the beginning</a></p>
    </div>
  </div>`;
}

/* ————— Behaviors wired after each render ————— */

function wireQuizzes(root) {
  root.querySelectorAll("[data-quiz]").forEach(quiz => {
    const feedback = quiz.querySelector(".quiz-feedback");
    quiz.querySelectorAll(".quiz-option").forEach(btn => {
      btn.addEventListener("click", () => {
        quiz.querySelectorAll(".quiz-option").forEach(b => b.classList.remove("correct", "incorrect"));
        const correct = btn.dataset.correct === "true";
        btn.classList.add(correct ? "correct" : "incorrect");
        feedback.textContent = btn.dataset.feedback;
      });
    });
  });
}

function wireExercises(root) {
  root.querySelectorAll("[data-exercise]").forEach(ex => {
    const textarea = ex.querySelector("textarea");
    const status = ex.querySelector("[data-save-status]");
    const lessonId = ex.dataset.lesson;
    const lesson = LESSONS.find(l => l.id === lessonId);
    ex.querySelector("[data-save-reflection]").addEventListener("click", () => {
      const text = textarea.value.trim();
      if (!text) {
        status.textContent = "Write a little something first — even a fragment counts.";
        return;
      }
      const journal = getJournal();
      journal.push({ id: Date.now().toString(36), at: Date.now(), source: lesson ? lesson.title : "Reflection", text });
      store.write(STORE_KEYS.journal, journal);
      status.textContent = "Kept. You'll find it in your journal. 🌸";
    });
  });
}

function wireLessonComplete(root) {
  const btn = root.querySelector("[data-toggle-complete]");
  if (!btn) return;
  btn.addEventListener("click", () => {
    const id = btn.dataset.toggleComplete;
    const progress = getProgress();
    progress[id] = !progress[id];
    store.write(STORE_KEYS.progress, progress);
    updateProgressPill();
    render(); // re-render to refresh button + path state
  });
}

function wireGarden(root) {
  const input = root.querySelector("#garden-input");
  if (!input) return;
  const fill = root.querySelector("#glow-fill");
  const label = root.querySelector("#glow-label");
  const list = root.querySelector("#feedback-list");
  const scenario = root.querySelector("#seed-scenario");

  function evaluate() {
    const text = input.value;
    const results = GARDEN_CHECKS.map(c => ({ ...c, lit: text.trim().length > 0 && c.test(text) }));
    const litCount = results.filter(r => r.lit).length;

    fill.style.width = `${(litCount / GARDEN_CHECKS.length) * 100}%`;

    const stage = GLOW_STAGES.slice().reverse().find(s => litCount >= s.min);
    label.textContent = stage ? stage.label : "";

    list.innerHTML = results.map(r => `
      <li class="${r.lit ? "lit" : "unlit"}">
        <span class="f-icon">${r.lit ? "🏮" : "◦"}</span>
        <span>${r.lit ? `<strong>${r.label}</strong> — lit.` : `${r.label}. <em>${r.hint}</em>`}</span>
      </li>`).join("");
  }

  input.addEventListener("input", evaluate);
  evaluate();

  root.querySelectorAll("[data-seed]").forEach(chip => {
    chip.addEventListener("click", () => {
      const seed = GARDEN_SEEDS[Number(chip.dataset.seed)];
      scenario.textContent = `Scenario: ${seed.text}`;
      input.focus();
    });
  });
}

function wireJournal(root) {
  root.querySelectorAll("[data-delete-entry]").forEach(btn => {
    btn.addEventListener("click", () => {
      const journal = getJournal().filter(e => e.id !== btn.dataset.deleteEntry);
      store.write(STORE_KEYS.journal, journal);
      render();
    });
  });
}

/* ————— Router ————— */

function currentRoute() {
  const hash = location.hash.replace(/^#/, "") || "/";
  const parts = hash.split("/").filter(Boolean);
  if (parts.length === 0) return { name: "home" };
  if (parts[0] === "path") return { name: "path" };
  if (parts[0] === "garden") return { name: "garden" };
  if (parts[0] === "journal") return { name: "journal" };
  if (parts[0] === "lesson" && parts[1]) return { name: "lesson", id: parts[1] };
  return { name: "404" };
}

function render() {
  const app = document.getElementById("app");
  const route = currentRoute();

  const views = {
    home: () => homeView(),
    path: () => pathView(),
    garden: () => gardenView(),
    journal: () => journalView(),
    lesson: () => lessonView(route.id),
    404: () => notFoundView()
  };

  app.innerHTML = (views[route.name] || views["404"])();

  document.querySelectorAll(".site-nav a").forEach(a => {
    const nav = a.dataset.nav;
    const active =
      (nav === "home" && route.name === "home") ||
      (nav === "path" && (route.name === "path" || route.name === "lesson")) ||
      (nav === "garden" && route.name === "garden") ||
      (nav === "journal" && route.name === "journal");
    a.classList.toggle("active", active);
  });

  wireQuizzes(app);
  wireExercises(app);
  wireLessonComplete(app);
  wireGarden(app);
  wireJournal(app);

  window.scrollTo({ top: 0, behavior: "instant" });
}

window.addEventListener("hashchange", render);
updateProgressPill();
render();
