# Mikepattyn portfolio

Personal static site for Mike Pattyn: who I am, the applications under the mikepattyn platform, and the AlienButNice brand domain.

## Language

**Portfolio**:
This Vite static site under `apps/mikepattyn`.
_Avoid_: treating it as a product submodule like Kapsalon or Fish

**AlienButNice**:
Separate platform domain (`alienbutnice.nl`) for brand surfaces that must not share mikepattyn product hostnames.
_Avoid_: listing AlienButNice as an Application next to Kapsalon/Fish

## Boundaries

- Owns marketing copy and static frontend only.
- Does not own app business logic (submodules under `apps/` do).
- CDK hosting via `Mikepattyn-BrandFrontend-Stack-Production` (`apps/mikepattyn` → `mikepattyn.nl` + `www`).

## Stack

Vite · plain HTML/CSS/JS · Google Fonts (Bricolage Grotesque, Newsreader)
