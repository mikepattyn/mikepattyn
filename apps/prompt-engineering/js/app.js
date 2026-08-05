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

function getLessons() {
  const lessons = t("lessons");
  return Array.isArray(lessons) ? lessons : [];
}

function getGardenChecks() {
  return GARDEN_CHECK_DEFS.map((def) => ({
    ...def,
    label: t(`gardenChecks.${def.id}.label`),
    hint: t(`gardenChecks.${def.id}.hint`)
  }));
}

function getGardenSeeds() {
  const seeds = t("gardenSeeds");
  return Array.isArray(seeds) ? seeds : [];
}

function getGlowStages() {
  const stages = t("glowStages");
  return Array.isArray(stages) ? stages : [];
}

function escapeHtml(s) {
  return String(s)
    .replace(/&/g, "&amp;")
    .replace(/</g, "&lt;")
    .replace(/>/g, "&gt;")
    .replace(/"/g, "&quot;");
}

function fillTemplate(template, vars) {
  return Object.entries(vars).reduce(
    (acc, [key, value]) => acc.replaceAll(`{${key}}`, String(value)),
    template
  );
}

/* ————— Chrome outside #app ————— */
function paintChrome() {
  const brandTag = document.querySelector(".brand-tag");
  if (brandTag) brandTag.textContent = t("app.brandTag");

  document.querySelectorAll(".site-nav a[data-nav]").forEach((a) => {
    const key = a.dataset.nav;
    a.textContent = t(`nav.${key}`);
  });

  const pill = document.getElementById("progress-pill");
  if (pill) pill.title = t("progress.title");

  const toggle = document.querySelector(".locale-toggle");
  if (toggle) toggle.setAttribute("aria-label", t("locale.ariaLabel"));

  const footer = document.querySelector(".site-footer p");
  if (footer) footer.textContent = t("footer.note");

  document.documentElement.lang = getLocale();
  const title = t("app.title");
  if (title && title !== "app.title") document.title = title;
  document.querySelectorAll("[data-locale]").forEach((btn) => {
    const isActive = btn.getAttribute("data-locale") === getLocale();
    btn.setAttribute("aria-pressed", isActive ? "true" : "false");
    btn.classList.toggle("is-active", isActive);
  });
}

/* ————— Progress ring in the header ————— */
function updateProgressPill() {
  const done = Object.values(getProgress()).filter(Boolean).length;
  const total = getLessons().length;
  const pct = total ? Math.round((done / total) * 100) : 0;
  document.getElementById("ring-fill").style.strokeDasharray = `${pct} 100`;
  document.getElementById("progress-label").textContent = `${done}/${total}`;
}

/* ————— Views ————— */

function homeView() {
  const lessons = getLessons();
  const features = t("home.features");
  const featureCards = Array.isArray(features) ? features : [];

  return `
  <div class="view">
    <section class="hero">
      <span class="hero-eyebrow">${escapeHtml(t("home.eyebrow"))}</span>
      <h1>${escapeHtml(t("home.headline"))}<br/><span class="accent">${escapeHtml(t("home.headlineAccent"))}</span></h1>
      <p class="lede">${escapeHtml(t("home.lede"))}</p>
      <div class="hero-actions">
        <a class="btn btn-primary" href="#/lesson/${lessons[0] ? lessons[0].id : "listening"}">${escapeHtml(t("home.ctaBegin"))}</a>
        <a class="btn btn-ghost" href="#/garden">${escapeHtml(t("home.ctaGarden"))}</a>
      </div>
    </section>

    <div class="card-grid">
      ${featureCards
        .map(
          (f) => `
        <div class="card">
          <span class="icon">${f.icon}</span>
          <h3>${escapeHtml(f.title)}</h3>
          <p class="muted">${escapeHtml(f.body)}</p>
        </div>`
        )
        .join("")}
    </div>

    <section class="card">
      <h3>${escapeHtml(t("home.whyTitle"))}</h3>
      <p class="muted">${t("home.whyBody")}</p>
    </section>
  </div>`;
}

function pathView() {
  const progress = getProgress();
  const lessons = getLessons();
  return `
  <div class="view">
    <h1>${escapeHtml(t("path.title"))}</h1>
    <p class="lede">${escapeHtml(t("path.lede"))}</p>
    <ol class="path-list">
      ${lessons
        .map((l, i) => {
          const done = !!progress[l.id];
          return `
        <a class="path-item ${done ? "done" : ""}" href="#/lesson/${l.id}">
          <span class="step-mark">${done ? "✓" : l.icon}</span>
          <span class="step-body">
            <p class="step-title">${i + 1}. ${escapeHtml(l.title)}</p>
            <p class="step-sub">${escapeHtml(l.subtitle)}</p>
          </span>
          <span class="step-status">${escapeHtml(done ? t("path.walked") : t("path.unwalked"))}</span>
        </a>`;
        })
        .join("")}
    </ol>
  </div>`;
}

function renderBlock(block, lesson, index) {
  switch (block.type) {
    case "p":
      return `<p>${block.html}</p>`;
    case "h3":
      return `<h3>${escapeHtml(block.text)}</h3>`;
    case "note":
      return `<aside class="infp-note"><span class="note-label">${escapeHtml(t("lesson.noteLabel"))}</span>${block.html}</aside>`;
    case "pair":
      return `
      <div class="prompt-pair">
        <div class="prompt-example faint">
          <span class="tag">${escapeHtml(block.weakLabel || t("pair.before"))}</span>
          <pre>${escapeHtml(block.weak)}</pre>
        </div>
        <div class="prompt-example glow">
          <span class="tag">${escapeHtml(block.strongLabel || t("pair.after"))}</span>
          <pre>${escapeHtml(block.strong)}</pre>
        </div>
      </div>`;
    case "quiz":
      return `
      <div class="quiz card" data-quiz>
        <h3 style="margin-top:0">${escapeHtml(t("lesson.quizTitle"))}</h3>
        <p>${escapeHtml(block.question)}</p>
        ${block.options
          .map(
            (o) => `
          <button class="quiz-option" data-correct="${o.correct}" data-feedback="${escapeHtml(o.feedback)}">${escapeHtml(o.text)}</button>
        `
          )
          .join("")}
        <p class="quiz-feedback" aria-live="polite"></p>
      </div>`;
    case "exercise":
      return `
      <div class="exercise" data-exercise data-lesson="${lesson.id}" data-index="${index}">
        <span class="exercise-label">${escapeHtml(block.label)}</span>
        <p>${block.html}</p>
        <textarea placeholder="${escapeHtml(block.placeholder || t("lesson.defaultPlaceholder"))}"></textarea>
        <div style="display:flex; align-items:center; gap:0.75rem; margin-top:0.75rem;">
          <button class="btn btn-soft" data-save-reflection>${escapeHtml(t("lesson.saveReflection"))}</button>
          <span class="muted" data-save-status aria-live="polite"></span>
        </div>
      </div>`;
    default:
      return "";
  }
}

function lessonView(id) {
  const lessons = getLessons();
  const idx = lessons.findIndex((l) => l.id === id);
  if (idx === -1) return notFoundView();
  const lesson = lessons[idx];
  const progress = getProgress();
  const done = !!progress[lesson.id];
  const prev = lessons[idx - 1];
  const next = lessons[idx + 1];

  return `
  <div class="view">
    <header class="lesson-header">
      <span class="lesson-kicker">${escapeHtml(fillTemplate(t("lesson.kicker"), { n: idx + 1, total: lessons.length }))} · ${lesson.icon}</span>
      <h1>${escapeHtml(lesson.title)}</h1>
      <p class="lede">${escapeHtml(lesson.subtitle)}</p>
    </header>
    <div class="lesson-body">
      ${lesson.blocks.map((b, i) => renderBlock(b, lesson, i)).join("")}
    </div>
    <div class="lesson-nav">
      ${prev ? `<a class="btn btn-ghost" href="#/lesson/${prev.id}">← ${escapeHtml(prev.title)}</a>` : `<a class="btn btn-ghost" href="#/path">${escapeHtml(t("lesson.backToPath"))}</a>`}
      <button class="btn ${done ? "btn-soft" : "btn-primary"}" data-toggle-complete="${lesson.id}">
        ${escapeHtml(done ? t("lesson.unmarkWalked") : t("lesson.markWalked"))}
      </button>
      ${next ? `<a class="btn btn-ghost" href="#/lesson/${next.id}">${escapeHtml(next.title)} →</a>` : `<a class="btn btn-ghost" href="#/garden">${escapeHtml(t("lesson.toGarden"))}</a>`}
    </div>
  </div>`;
}

function gardenView() {
  const seeds = getGardenSeeds();
  return `
  <div class="view">
    <h1>${escapeHtml(t("garden.title"))}</h1>
    <p class="lede">${escapeHtml(t("garden.lede"))}</p>

    <div class="seed-row">
      ${seeds.map((s, i) => `<button class="seed-chip" data-seed="${i}">${escapeHtml(s.label)}</button>`).join("")}
    </div>
    <p class="muted" id="seed-scenario" aria-live="polite"></p>

    <div class="card garden-layout">
      <textarea id="garden-input" rows="7" placeholder="${escapeHtml(t("garden.placeholder"))}"></textarea>
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
  const locale = getLocale() === "nl" ? "nl-NL" : "en-GB";
  return `
  <div class="view">
    <h1>${escapeHtml(t("journal.title"))}</h1>
    <p class="lede">${escapeHtml(t("journal.lede"))}</p>
    ${
      entries.length === 0
        ? `<p class="empty-state">${escapeHtml(t("journal.empty"))}</p>`
        : entries
            .slice()
            .reverse()
            .map(
              (e) => `
        <div class="journal-entry">
          <div class="entry-meta">
            <span>${escapeHtml(e.source)} · ${new Date(e.at).toLocaleString(locale)}</span>
            <button class="entry-delete" data-delete-entry="${e.id}">${escapeHtml(t("journal.letGo"))}</button>
          </div>
          <p>${escapeHtml(e.text)}</p>
        </div>`
            )
            .join("")
    }
  </div>`;
}

function notFoundView() {
  return `
  <div class="view">
    <div class="empty-state">
      <h1>${escapeHtml(t("notFound.title"))}</h1>
      <p>${escapeHtml(t("notFound.body"))}</p>
      <p><a class="btn btn-primary" href="#/">${escapeHtml(t("notFound.back"))}</a></p>
    </div>
  </div>`;
}

/* ————— Behaviors wired after each render ————— */

function wireQuizzes(root) {
  root.querySelectorAll("[data-quiz]").forEach((quiz) => {
    const feedback = quiz.querySelector(".quiz-feedback");
    quiz.querySelectorAll(".quiz-option").forEach((btn) => {
      btn.addEventListener("click", () => {
        quiz.querySelectorAll(".quiz-option").forEach((b) => b.classList.remove("correct", "incorrect"));
        const correct = btn.dataset.correct === "true";
        btn.classList.add(correct ? "correct" : "incorrect");
        feedback.textContent = btn.dataset.feedback;
      });
    });
  });
}

function wireExercises(root) {
  root.querySelectorAll("[data-exercise]").forEach((ex) => {
    const textarea = ex.querySelector("textarea");
    const status = ex.querySelector("[data-save-status]");
    const lessonId = ex.dataset.lesson;
    const lesson = getLessons().find((l) => l.id === lessonId);
    ex.querySelector("[data-save-reflection]").addEventListener("click", () => {
      const text = textarea.value.trim();
      if (!text) {
        status.textContent = t("lesson.writeFirst");
        return;
      }
      const journal = getJournal();
      journal.push({
        id: Date.now().toString(36),
        at: Date.now(),
        source: lesson ? lesson.title : t("lesson.reflectionFallback"),
        text
      });
      store.write(STORE_KEYS.journal, journal);
      status.textContent = t("lesson.kept");
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
    render();
  });
}

function wireGarden(root) {
  const input = root.querySelector("#garden-input");
  if (!input) return;
  const fill = root.querySelector("#glow-fill");
  const label = root.querySelector("#glow-label");
  const list = root.querySelector("#feedback-list");
  const scenario = root.querySelector("#seed-scenario");
  const checks = getGardenChecks();
  const stages = getGlowStages();
  const seeds = getGardenSeeds();
  const litSuffix = t("garden.litSuffix");

  function evaluate() {
    const text = input.value;
    const results = checks.map((c) => ({ ...c, lit: text.trim().length > 0 && c.test(text) }));
    const litCount = results.filter((r) => r.lit).length;

    fill.style.width = `${(litCount / checks.length) * 100}%`;

    const stage = stages.slice().reverse().find((s) => litCount >= s.min);
    label.textContent = stage ? stage.label : "";

    list.innerHTML = results
      .map(
        (r) => `
      <li class="${r.lit ? "lit" : "unlit"}">
        <span class="f-icon">${r.lit ? "🏮" : "◦"}</span>
        <span>${r.lit ? `<strong>${escapeHtml(r.label)}</strong>${escapeHtml(litSuffix)}` : `${escapeHtml(r.label)}. <em>${escapeHtml(r.hint)}</em>`}</span>
      </li>`
      )
      .join("");
  }

  input.addEventListener("input", evaluate);
  evaluate();

  root.querySelectorAll("[data-seed]").forEach((chip) => {
    chip.addEventListener("click", () => {
      const seed = seeds[Number(chip.dataset.seed)];
      scenario.textContent = `${t("garden.scenarioPrefix")} ${seed.text}`;
      input.focus();
    });
  });
}

function wireJournal(root) {
  root.querySelectorAll("[data-delete-entry]").forEach((btn) => {
    btn.addEventListener("click", () => {
      const journal = getJournal().filter((e) => e.id !== btn.dataset.deleteEntry);
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
  paintChrome();
  updateProgressPill();

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

  document.querySelectorAll(".site-nav a").forEach((a) => {
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

window.render = render;

window.addEventListener("hashchange", render);
initLocale();
render();
