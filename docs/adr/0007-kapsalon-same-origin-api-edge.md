# ADR 0007: Kapsalon same-origin API edge

## Status

Accepted

## Context

Kapsalon’s Angular SPA called API Gateway via raw `execute-api` URLs baked into the build at deploy time. Fish already serves its API as same-origin `/api/*` on the app hostname (ADR 0005). Dedicated `*-api` hostnames were considered and rejected in favour of matching Fish.

## Decision

Kapsalon per environment:

| Layer | Service |
|-------|---------|
| Edge | CloudFront on **AppHostname** — `/*` → S3 (Angular SPA); `/api/*` → API Gateway |
| API | API Gateway REST routes mounted under `/api` (like Fish) |
| SPA | Relative `apiBaseUrl: '/api'` — no SSM `ApiUrl` bake in frontend CI |

SSM `/Kapsalon/{Env}/Backend/ApiUrl` publishes the execute-api base including `/api` for ops and curl, not for the SPA.

Hostname scheme remains ADR 0002 (`kapsalon-dev`, `kapsalon-acc`, `kapsalon`).

## Consequences

- Same-origin avoids CORS complexity for the SPA on the app hostname
- Hard cutover: CDK (routes + CloudFront) and SPA redeploy must land together
- Direct execute-api callers must use the `/api` path prefix
- Fish and Kapsalon edge wiring is similar but not yet unified (see `docs/todos/unify-fish-kapsalon-edge.md`)

## References

- [docs/adr/0002-mikepattyn-nl-dns-hostname-scheme.md](./0002-mikepattyn-nl-dns-hostname-scheme.md)
- [docs/adr/0005-fish-serverless-hosting.md](./0005-fish-serverless-hosting.md)
- [docs/adr/0008-same-origin-api-cloudfront-origin-policy.md](./0008-same-origin-api-cloudfront-origin-policy.md)
