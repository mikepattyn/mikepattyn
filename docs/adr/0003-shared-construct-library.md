# ADR 0003: Shared construct library

## Status

Accepted

## Context

Kapsalon CDK used a Flyingdarts-style split: construct library + thin deploy app. Fish and future apps need the same patterns (web hosting, OIDC, naming).

## Decision

One construct library (`Mikepattyn.CDK.Constructs`) containing:

- **Shared**: `IPlatformDomain` / `DomainStack`, `GithubActionsOIDCConstruct`, `WebApplicationHostingConstruct`, `AppHostnames`, base classes
- **Kapsalon**: `FrontendStack`, `BackendStack` (Lambda, API Gateway, DynamoDB)
- **Fish**: `FishBackendStack`, `FishEdgeStack`

One deploy app (`Mikepattyn.CDK/Program.cs`) wires one `DomainStack` per platform domain (`mikepattyn.nl`, `alienbutnice.nl`), then all application stacks.

Naming: platform stacks `Mikepattyn-{Resource}-Stack`; app stacks `{App}-{Resource}-Stack-{Environment}`.

## Consequences

- Construct tests live in `Mikepattyn.CDK.Constructs.Tests`
- New apps add stacks under `Stacks/{App}/` and register in `Program.cs`
- Cross-app OIDC role at `/github-actions/mikepattyn/`
