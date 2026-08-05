---
name: scaffold-lumen-frontend
description: >-
  Scaffolds a Lumen-like static frontend (plain HTML/CSS/vanilla JS, no bundler)
  under apps/, with EN/NL preference, browser-language auto-switch via
  navigator.language, and a required entry on the mikepattyn.nl portfolio.
  Use whenever the user asks to create a new static frontend app like Lumen,
  add EN/NL i18n to a static site, scaffold a plain HTML/JS product under apps/,
  list a new app on the portfolio, or wants a simple locale toggle that follows
  the browser language — even if they do not say "Lumen" or "scaffold".
---

# Scaffold Lumen-like frontend (EN/NL + portfolio)

**Location:** this skill lives in the Platform skills folder at
`.cursor/skills/scaffold-lumen-frontend/` (repo root). Sibling skills are under
`.cursor/skills/` (e.g. `add-frontend-deploy-workflow`).

Create (or retrofit) a **static** frontend the way Lumen is shaped: HTML/CSS/vanilla JS, no build step — plus EN/NL preference with browser auto-detect — and **always** add a Production link on the mikepattyn portfolio (`apps/mikepattyn/index.html` → `#apps`).

Lumen itself (`apps/prompt-engineering`) is English-only today. Locale **policy** comes from Kapsalon’s `LocaleService`; implement it in plain JS, not Angular/Transloco.

## Progress checklist

Copy and track:

```
Progress:
- [ ] 1. Gather inputs
- [ ] 2. Create or retrofit app shell under apps/<slug>/
- [ ] 3. Wire vanilla EN/NL locale (resolve → persist → toggle → re-render)
- [ ] 4. Add portfolio entry in apps/mikepattyn/index.html
- [ ] 5. Optional: platform hosting + deploy workflow
- [ ] 6. Sanity-check
```

## 1. Gather inputs

Ask only for what you cannot infer:

| Input | Notes |
|-------|--------|
| App display name | e.g. `Lumen` |
| Folder slug | e.g. `prompt-engineering` → `apps/<slug>/` |
| AppSlug / hostname | Production URL `{appSlug}.mikepattyn.nl` (lowercase slug) |
| CDK PascalCase name | e.g. `Lumen` — for Constants / SSM later |
| One-line portfolio blurb | Product description for `#apps` |
| Stack line | e.g. `HTML · CSS · Vanilla JS` |
| Mode | **new** app vs **retrofit** existing static app |

Default stack for new Lumen-like apps: `HTML · CSS · Vanilla JS`.

## 2. App shell (like Lumen)

Target layout:

```
apps/<slug>/
├── index.html
├── CONTEXT.md
├── README.md
├── css/styles.css
├── js/
│   ├── locale.js      # resolve / persist / toggle
│   ├── i18n/
│   │   ├── en.js
│   │   └── nl.js
│   └── app.js         # router + views using t('key')
└── (optional content module, e.g. js/content.js)
```

Reference implementation for structure (not for English-only strings): `apps/prompt-engineering/`.

Shell conventions:

- `#app` mount + hash router (`#/`, `#/…`) when the app has multiple views
- `localStorage` helper for app state; locale uses its own key (see below)
- Google Fonts optional; keep CSS simple and branded for the product
- No `package.json` / bundler unless the user explicitly wants Vite later
- `CONTEXT.md` glossary for the app’s domain language

Copy starter locale files from this skill’s `assets/` into the app, then customize keys and chrome.

Script order in `index.html` (locale dictionaries before locale module before app):

```html
<script src="js/i18n/en.js"></script>
<script src="js/i18n/nl.js"></script>
<script src="js/locale.js"></script>
<script src="js/app.js"></script>
```

Header must include an EN/NL control (see `assets/locale-toggle.html` snippet). Footer/nav strings go through `t('…')`, not hardcoded English.

## 3. EN/NL locale (vanilla JS)

Read `assets/locale.js` and ship it (or an equivalent) into the app. Policy matches Kapsalon:

1. Locales: `'en' | 'nl'` only
2. Storage key: `locale` (or `<appSlug>.locale` if the app already namespaces storage)
3. Initial resolve:
   - stored value if `'en'` or `'nl'`
   - else `navigator.language.toLowerCase().startsWith('nl') ? 'nl' : 'en'`
4. On init and on switch: set `document.documentElement.lang`, update `document.title` from translations, persist choice
5. Strings live in parallel dictionaries (`I18N.en` / `I18N.nl`); views call `t('dotted.key')`
6. Locale change re-renders the UI the same way a `hashchange` does (call the existing `render()` / paint function)

Do **not** pull Transloco, i18next, or a framework into a Lumen-like static app. Keep it browser-native so `navigator.language` is the only detection API needed.

When retrofitting an existing static app: extract user-visible strings into `en.js` / `nl.js`, replace literals with `t('…')`, add the toggle, keep behavior identical otherwise.

## 4. Portfolio entry (required)

Every new (or newly public) Lumen-like app **must** get a link on **mikepattyn.nl**.

Edit `apps/mikepattyn/index.html`:

1. In `#apps` → `<ul class="app-list">`, append an `<li class="app-item reveal">` using the shape in `references/portfolio-entry.md`
2. Link the **Production** hostname: `https://{appSlug}.mikepattyn.nl` (not `-dev` / `-acc`)
3. Optionally update `<meta name="description">` so SEO copy mentions the new app

Portfolio list is hardcoded HTML — no JSON/CMS. Do not invent image cards; the design is text-only list items.

Skipping the portfolio link is a failed scaffold.

## 5. Optional platform wiring

If the user wants hosting/CI in the same change (or the app is new and must go live):

| Piece | Where / how |
|-------|-------------|
| CDK app constant + FrontendStack | `infra/cdk/` — follow Lumen / sibling brand stacks |
| Make sync/deploy targets | Root `Makefile` — mirror `sync-lumen` / `deploy-lumen` |
| GitHub content workflow | Use project skill `.cursor/skills/add-frontend-deploy-workflow` (Production-only static sync like Lumen) |

Hostname scheme: `{appSlug}.mikepattyn.nl` (see platform `CONTEXT.md` / ADR on DNS). Agents must not run `cdk deploy` unless the user explicitly asks; create the code and tell them how to deploy.

If hosting already exists, still do steps 2–4; skip CDK.

## 6. Sanity-check

- [ ] `index.html` loads `en.js` → `nl.js` → `locale.js` → `app.js`
- [ ] Fresh profile (no `localStorage.locale`): Dutch browser → NL UI; otherwise EN
- [ ] Toggle persists across reload; `document.documentElement.lang` matches
- [ ] All chrome strings use `t('…')` (no stray English in header/nav/footer)
- [ ] Portfolio `#apps` list includes the new item with correct Production URL
- [ ] `CONTEXT.md` exists for the app

## References

| When | Read |
|------|------|
| Portfolio `<li>` markup | `references/portfolio-entry.md` (this skill folder) |
| Locale module to copy | `assets/locale.js` |
| Toggle markup | `assets/locale-toggle.html` |
| Starter dictionaries | `assets/i18n/en.js`, `assets/i18n/nl.js` |
| Static shell inspiration | `apps/prompt-engineering/` |
| Locale policy origin | `apps/kapsalon/apps/web/src/app/core/locale/locale.service.ts` |
| Deploy workflow skill | `.cursor/skills/add-frontend-deploy-workflow` |
| Skills folder | `.cursor/skills/` at Platform repo root |
