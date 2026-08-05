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

## Boundaries

- Owns lesson content and static frontend only (HTML/CSS/JS, no build step).
- Does not use a backend or accounts; progress and journal live in `localStorage`.
- CDK hosting via `PromptEngineering-Frontend-Stack-Production` (`prompt-engineering.mikepattyn.nl`).

## Stack

Plain HTML/CSS/JS · Google Fonts (Fraunces, Nunito Sans) · hash router
