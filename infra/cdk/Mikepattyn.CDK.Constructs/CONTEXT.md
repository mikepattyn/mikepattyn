# CDK Constructs

Reusable AWS CDK construct library for platform and application infrastructure.

## Language

**InfrastructureConstruct**:
A reusable CDK construct encapsulating one capability (API Gateway, DynamoDB, web hosting, ECS service).
_Avoid_: deploy app orchestration here.

**PlatformDomain**:
Imported Route53 hosted zone and ACM certificate for one apex brand domain (`IPlatformDomain`).
Implementations: `MikepattynPlatformDomainConstruct` (`mikepattyn.nl`), `AlienButNicePlatformDomainConstruct` (`alienbutnice.nl`).
_Avoid_: creating hosted zones or certificates in CDK; hardcoding a single platform domain.

**PlatformStack**:
Shared stack for Domain imports or GitHub Actions deploy role (Auth).
_Avoid_: application-specific backend logic in platform stacks; creating the account-global GitHub OIDC provider (import by ARN only — IAM allows one issuer URL per account).

**ApplicationStack**:
Per-app deployable stack (Kapsalon Frontend/Backend, Fish Data/Api/Edge).
_Avoid_: mixing two apps in one stack class.

**AppHostname**:
FQDN from `AppHostnames.For(slug, environment, mikepattyn.nl)`.
_Avoid_: hardcoding `dev.` prefix on platform domain.

**BrandHostname**:
Apex + `www` FQDNs from `BrandHostnames.GetDomainNames(platformDomain)` for Production brand sites.
_Avoid_: using `AppHostnames` for apex portfolio/brand domains.

**BrandFrontendStack**:
Production-only static site stack for platform apex domains (`mikepattyn.nl`, `alienbutnice.nl`) with Alias A/AAAA DNS.
_Avoid_: reusing product `FrontendStack` CNAME pattern for apex records.

## Boundaries

- Owns constructs, stack props, aspects, naming conventions.
- Does not own `Program.cs` stack order (belongs to `Mikepattyn.CDK`).
- Does not own application handler code.

## Kapsalon terms

See kapsalon submodule `infra/cdk` CONTEXT (removed) — tenant language remains in kapsalon app docs. Constructs still use **Kapsalon** as `AppName` for resource naming.

## Fish terms

**CatchPhoto**: User-uploaded image stored in S3.
**FogOfWater**: H3-based map exploration (app domain; not in CDK).
