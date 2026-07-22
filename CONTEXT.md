# Mikepattyn platform

Umbrella repository for personal applications, shared packages, and AWS CDK infrastructure.

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

**Package**:
A shared library submodule under `packages/` (e.g. Authress Flutter/Angular clients), consumed by applications.
_Avoid_: treating packages as deployable Applications

**AlienButNice**:
Separate brand platform domain (`alienbutnice.nl`) for creative/personal brand hosting, not product hostnames under mikepattyn.nl.
_Avoid_: treating AlienButNice as an Application slug like barbershop/gofish

**InfrastructureConstruct**:
Reusable CDK building block (web hosting, API gateway, ECS service, etc.).
_Avoid_: module, component

**PlatformStack**:
Shared stack for domain imports or the GitHub Actions deploy role (Auth).
_Avoid_: root stack; creating the account-global GitHub OIDC provider (import by ARN)

**ApplicationStack**:
Per-app CDK stack (Frontend, Backend, Api, Data).
_Avoid_: service stack

**AppSlug**:
The DNS label for an **Application** under `mikepattyn.nl` (e.g. Kapsalon → `barbershop`, Fish → `gofish`).
_Avoid_: treating AppSlug as the product name; conflating with Application stack/resource naming

## Boundaries

- Owns shared IaC under `infra/cdk/`, platform docs, and submodule pointers.
- Does not own application or package business logic; submodules under `apps/` and `packages/` do.
- Imports platform-domain DNS/TLS; does not create hosted zones or certificates.

## Hostnames

| App | Development | Staging | Production |
|-----|-------------|---------|------------|
| Kapsalon | barbershop-dev.mikepattyn.nl | barbershop-acc.mikepattyn.nl | barbershop.mikepattyn.nl |
| Fish | gofish-dev.mikepattyn.nl | gofish-acc.mikepattyn.nl | gofish.mikepattyn.nl |
| Portfolio | — | — | mikepattyn.nl (+ www) |
| AlienButNice | — | — | alienbutnice.nl (+ www) |

Product applications expose their API as same-origin `/api/*` on the **AppHostname** (CloudFront → API Gateway). There are no dedicated `*-api` hostnames.

Platform domains (apex): `mikepattyn.nl`, `alienbutnice.nl`.
