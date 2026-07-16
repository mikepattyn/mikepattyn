# CDK Deploy App

## Language

**DeployApp**:
The `Mikepattyn.CDK` executable project. It calls `StackComposition.Build`, then `app.Synth()`.
_Avoid_: defining reusable constructs here.

**StackComposition**:
The shared wiring entry point (`StackComposition.Build`) that creates the ordered set of platform and application stacks. Used by `Program.cs` and synth e2e tests.
_Avoid_: scattering stack creation across construct library files or duplicating wiring in tests.

**SynthE2E**:
In-process synth tests in `Mikepattyn.CDK.E2E.Tests` that build the full StackComposition and assert domain output without live AWS calls.
_Avoid_: live DNS/HTTP checks in this suite.

## Boundaries

- Owns CDK app startup and stack instantiation order only.
- Uses constructs from `Mikepattyn.CDK.Constructs`; it should not define reusable constructs.
- Does not own API handler behavior; application backends own request semantics.
- Creates one `DomainStack` per `IPlatformDomain` (Mikepattyn, AlienButNice).

Synth e2e validates **PlatformDomain** apexes via `DomainStack.DomainName`, **AppHostname** FQDNs via CloudFront aliases plus Route53 CNAME records in frontend/edge stacks, and **BrandHostname** apex + `www` via `BrandFrontendStack` Alias records.

## Example dialogue

> **Newcomer:** Where do I add a new Fish environment stack?
>
> **Expert:** In `Mikepattyn.CDK/StackComposition.cs`, following the existing Fish Backend → Edge loop.

> **Newcomer:** Where does `alienbutnice.nl` get imported?
>
> **Expert:** `StackComposition.Build` creates a second `DomainStack` with `AlienButNicePlatformDomainConstruct`.

> **Newcomer:** How do we know kapsalon-dev.mikepattyn.nl is wired correctly?
>
> **Expert:** `Mikepattyn.CDK.E2E.Tests` synths the full composition and asserts the hostname on CloudFront and Route53.
