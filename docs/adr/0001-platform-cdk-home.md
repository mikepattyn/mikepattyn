# ADR 0001: Platform CDK home at mikepattyn root

## Status

Accepted

## Context

Kapsalon owned its CDK under `apps/kapsalon/infra/cdk`. Multiple applications need shared DNS on `mikepattyn.nl`, GitHub OIDC, and consistent construct patterns.

## Decision

Centralize all AWS CDK (.NET) in `infra/cdk/` at the mikepattyn umbrella repository:

- `Mikepattyn.CDK` — thin deploy app (stack composition)
- `Mikepattyn.CDK.Constructs` — shared construct library and application stacks

Application submodules retain business code only; IaC lives in the platform repo.

## Consequences

- Single `make` entrypoint at repo root for CDK operations
- Submodule repos must drop local CDK and point deploy docs here
- Greenfield stacks on `mikepattyn.nl`; old kapsalon domain migration is a separate effort
