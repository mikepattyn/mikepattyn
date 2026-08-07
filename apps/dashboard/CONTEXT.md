# Lumen Stats Dashboard

Internal read-only stats page at `dashboard.mikepattyn.nl` for the Lumen game (`lumen.mikepattyn.nl`).

## Language

**Dashboard**: Static stats viewer; fetches aggregate metrics from same-origin `/api/stats`.

**Stats API**: Lambda + DynamoDB backend (`Dashboard-Backend-Stack-Production`); receives anonymous events from Lumen via `/api/events`.

## Data

- Anonymous visitor id (`lumen.vid` in browser localStorage)
- Events: `visit`, `lesson_view`, `lesson_walked`, `lesson_unwalked`, `completed`
- Funnel: unique visitors → viewed lesson → walked ≥1 → all 6 lessons walked

## Boundaries

- Owns the dashboard UI only; does not own Lumen game logic.
- Reads stats from the shared Dashboard backend; does not store data locally.
