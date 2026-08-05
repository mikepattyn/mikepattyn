# Lumen — Prompt Engineering for INFPs

A gentle, self-contained web course that teaches INFPs the craft of prompt engineering through the way they already think: imagined listeners, inner characters, named feelings, and big luminous dreams broken into askable pieces.

## Language

**Lumen**:
This static lesson site under `apps/prompt-engineering`.
_Avoid_: treating it as a product submodule like Kapsalon or Fish

**The Path**:
Six short lessons covering core prompt-engineering techniques (context, tone, role, format, reasoning, authenticity).
_Avoid_: calling it a "module" or "chapter" in platform docs

**The Practice Garden**:
Live prompt-writing playground with encouraging craft heuristics (lanterns), not grading.
_Avoid_: quiz or test framing in platform docs

**Locale**:
EN/NL preference stored in `localStorage` under `lumen.locale`. On first visit, Dutch browsers (`navigator.language` starts with `nl`) get NL; otherwise EN. Chrome strings and lesson content live in `js/i18n/{en,nl}.js`; views call `t('…')`.
_Avoid_: pulling Transloco, i18next, or a framework for locale

## Boundaries

- Owns lesson content and static frontend only (HTML/CSS/JS, no build step).
- Does not use a backend or accounts; progress and journal live in `localStorage`.
- CDK hosting via `Lumen-Frontend-Stack-Production` (`lumen.mikepattyn.nl`).
- Listed on the mikepattyn.nl portfolio (`#apps`).

## Stack

Plain HTML/CSS/JS · Google Fonts (Fraunces, Nunito Sans) · hash router · vanilla EN/NL locale
