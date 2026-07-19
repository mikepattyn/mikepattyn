# Mikepattyn platform

Umbrella repository for personal applications and shared AWS CDK infrastructure.

## Language

**Platform**:
The shared infrastructure umbrella that owns DNS imports, GitHub OIDC, and per-app stacks.
_Avoid_: monorepo (this repo uses git submodules, not a package workspace)

**PlatformDomain**:
One apex brand domain whose Route53 zone and ACM certificate are imported into CDK (`mikepattyn.nl`, `alienbutnice.nl`).
_Avoid_: treating app hostnames as platform domains

**Application**:
A deployable product under `apps/` — product submodules (Kapsalon, Fish) or the owned portfolio (`apps/mikepattyn`).
_Avoid_: service (too generic)

**AlienButNice**:
Separate brand platform domain (`alienbutnice.nl`) for creative/personal brand hosting, not product hostnames under mikepattyn.nl.
_Avoid_: treating AlienButNice as an Application slug like kapsalon/fish

**InfrastructureConstruct**:
Reusable CDK building block (web hosting, API gateway, ECS service, etc.).
_Avoid_: module, component

**PlatformStack**:
Shared stack for domain imports or the GitHub Actions deploy role (Auth).
_Avoid_: root stack; creating the account-global GitHub OIDC provider (import by ARN)

**ApplicationStack**:
Per-app CDK stack (Frontend, Backend, Api, Data).
_Avoid_: service stack

## Boundaries

- Owns shared IaC under `infra/cdk/`, platform docs, and submodule pointers.
- Does not own application business logic; submodules under `apps/` do.
- Imports platform-domain DNS/TLS; does not create hosted zones or certificates.

## Hostnames

| App | Development | Staging | Production |
|-----|-------------|---------|------------|
| Kapsalon | kapsalon-dev.mikepattyn.nl | kapsalon-acc.mikepattyn.nl | kapsalon.mikepattyn.nl |
| Fish | fish-dev.mikepattyn.nl | fish-acc.mikepattyn.nl | fish.mikepattyn.nl |
| Portfolio | — | — | mikepattyn.nl (+ www) |
| AlienButNice | — | — | alienbutnice.nl (+ www) |

Product applications expose their API as same-origin `/api/*` on the **AppHostname** (CloudFront → API Gateway). There are no dedicated `*-api` hostnames.

Platform domains (apex): `mikepattyn.nl`, `alienbutnice.nl`.
