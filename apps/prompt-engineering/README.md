# Lumen — Prompt Engineering for INFPs

A gentle, self-contained web course that teaches INFPs the craft of prompt
engineering through the way they already think: imagined listeners, inner
characters, named feelings, and big luminous dreams broken into askable pieces.

## What's inside

- **The Path** — six short lessons covering the core techniques:
  1. *Talking to a Very Literal Daydream* — what a model is, why context matters
  2. *Naming the Feeling You're After* — tone vocabulary and iterative refinement
  3. *Casting Characters* — role/persona prompting
  4. *Giving Shape to the Mist* — output formats and few-shot examples
  5. *Thinking Out Loud, Together* — step-by-step reasoning and task decomposition
  6. *Keeping Your Voice* — authenticity, values, and knowing when not to prompt
- **The Practice Garden** — a live prompt-writing playground. Lanterns light up
  as your prompt gains craft elements (context, role, tone, shape, boundaries,
  a clear ask). Encouraging, never grading.
- **Journal** — reflections from lessons are saved locally, plus a delete
  ("let it go") option.
- **Progress** — lessons can be marked "walked"; a progress ring lives in the
  header. Everything is stored in `localStorage` — no accounts, no server.

## Running it

No build step, no dependencies. Open `index.html` directly in a browser, or
serve the folder:

```powershell
python -m http.server 8000
# then visit http://localhost:8000
```

## Files

| File | Purpose |
| --- | --- |
| `index.html` | App shell (header, nav, progress ring) |
| `css/styles.css` | Twilight/lantern visual theme |
| `js/lessons.js` | All lesson content, garden heuristics, seed scenarios |
| `js/app.js` | Hash router, rendering, quizzes, journal, garden logic |
