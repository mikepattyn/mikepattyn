# Mikepattyn portfolio

Personal static site for Mike Pattyn: who I am, the applications under the mikepattyn platform, and shared open-source packages.

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

## Locale

EN/NL, same policy as Kapsalon `LocaleService` and the Lumen retrofit: stored
`localStorage.locale` wins, else `navigator.language` starting with `nl` → NL,
else EN. Header toggle persists the choice. Copy lives in `src/i18n/en.js` /
`src/i18n/nl.js`; static markup is keyed with `data-i18n` / `data-i18n-aria`
and translated by `src/locale.js`.
