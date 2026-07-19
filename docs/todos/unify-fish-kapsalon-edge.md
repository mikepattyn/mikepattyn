# Unify Fish and Kapsalon edge stacks

Fish uses `FishEdgeStack` (S3 + CloudFront `/api/*` → API Gateway). Kapsalon now adds the same `/api/*` behavior via optional props on `WebApplicationHostingConstruct` / `FrontendStack`.

## Goal

Extract a shared construct or `AppEdgeStack` used by both Fish and Kapsalon so path-split CloudFront, Route53 CNAME, and API origin wiring live in one place.

## Why deferred

Smaller change to ship Kapsalon same-origin API first. Unification is a refactor with no user-visible difference once both apps work.

## Likely steps

1. Extract shared CloudFront distribution logic (SPA default + `/api/*` HTTP origin) from `FishEdgeStack` and `WebApplicationHostingConstruct`.
2. Migrate Fish from `FishEdgeStack` to the shared construct.
3. Remove duplicate origin/behavior code and align SSM outputs if needed.
