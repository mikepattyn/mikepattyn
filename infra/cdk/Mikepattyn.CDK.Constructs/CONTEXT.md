# CDK Constructs

Reusable AWS CDK construct library for platform and application infrastructure.

## Language

**InfrastructureConstruct**:
A reusable CDK construct encapsulating one capability (API Gateway, DynamoDB, web hosting, ECS service).
_Avoid_: deploy app orchestration here.

**PlatformStack**:
Shared stack for Domain imports or GitHub OIDC.
_Avoid_: application-specific backend logic in platform stacks.

**ApplicationStack**:
Per-app deployable stack (Kapsalon Frontend/Backend, Fish Data/Api/Edge).
_Avoid_: mixing two apps in one stack class.

**AppHostname**:
FQDN from `AppHostnames.For(slug, environment, mikepattyn.nl)`.
_Avoid_: hardcoding `dev.` prefix on platform domain.

## Boundaries

- Owns constructs, stack props, aspects, naming conventions.
- Does not own `Program.cs` stack order (belongs to `Mikepattyn.CDK`).
- Does not own application handler code.

## Kapsalon terms

See kapsalon submodule `infra/cdk` CONTEXT (removed) — tenant language remains in kapsalon app docs. Constructs still use **Kapsalon** as `AppName` for resource naming.

## Fish terms

**CatchPhoto**: User-uploaded image stored in S3.
**FogOfWater**: H3-based map exploration (app domain; not in CDK).
