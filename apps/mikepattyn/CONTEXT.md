# Mikepattyn portfolio

Personal static site for Mike Pattyn: who I am and the applications under the mikepattyn platform.

## Language

**Portfolio**:
This Vite static site under `apps/mikepattyn`.
_Avoid_: treating it as a product submodule like Kapsalon or Fish

## Boundaries

- Owns marketing copy and static frontend only.
- Does not own app business logic (submodules under `apps/` do).
- CDK hosting via `Mikepattyn-BrandFrontend-Stack-Production` (`apps/mikepattyn` → `mikepattyn.nl` + `www`).

## Stack

Vite · plain HTML/CSS/JS · Google Fonts (Bricolage Grotesque, Newsreader)
