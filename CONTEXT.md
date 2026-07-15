# Mikepattyn platform

Umbrella repository for personal applications and shared AWS CDK infrastructure.

## Language

**Platform**:
The shared infrastructure umbrella that owns DNS imports, GitHub OIDC, and per-app stacks.
_Avoid_: monorepo (this repo uses git submodules, not a package workspace)

**Application**:
A deployable product submodule under `apps/` (Kapsalon, Fish).
_Avoid_: service (too generic)

**InfrastructureConstruct**:
Reusable CDK building block (web hosting, API gateway, ECS service, etc.).
_Avoid_: module, component

**PlatformStack**:
Shared stack for domain imports or GitHub Actions OIDC.
_Avoid_: root stack

**ApplicationStack**:
Per-app CDK stack (Frontend, Backend, Api, Data).
_Avoid_: service stack

## Boundaries

- Owns shared IaC under `infra/cdk/`, platform docs, and submodule pointers.
- Does not own application business logic; submodules under `apps/` do.
- Imports `mikepattyn.nl` DNS/TLS; does not create hosted zones or certificates.

## Hostnames

| App | Development | Staging | Production |
|-----|-------------|---------|------------|
| Kapsalon | kapsalon-dev.mikepattyn.nl | kapsalon-acc.mikepattyn.nl | kapsalon.mikepattyn.nl |
| Fish | fish-dev.mikepattyn.nl | fish-acc.mikepattyn.nl | fish.mikepattyn.nl |
