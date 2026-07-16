# ADR 0006: Multiple platform domains via IPlatformDomain

## Status

Accepted

## Context

The platform originally imported a single apex domain (`mikepattyn.nl`) through `DomainStack`. AlienButNice is a separate brand with its own apex domain (`alienbutnice.nl`), zone, and ACM certificate. Apps under Mikepattyn must keep using `mikepattyn.nl`; AlienButNice must not share that zone or certificate.

## Decision

Introduce `IPlatformDomain` as the import contract for one apex brand domain (hosted zone + certificate).

- `MikepattynPlatformDomainConstruct` — `mikepattyn.nl`
- `AlienButNicePlatformDomainConstruct` — `alienbutnice.nl`

Each `DomainStack` hosts exactly one `IPlatformDomain` implementation. Shared import wiring lives in `PlatformDomainConstruct`; brand identity (domain name) lives in the concrete constructs.

Kapsalon and Fish continue to consume the Mikepattyn platform domain. AlienButNice domain stack is synthesized independently for future brand hosting.

## Consequences

- Deployment constants include zone ID and certificate ARN for both domains
- Hostname scheme in ADR 0002 remains valid for apps under `mikepattyn.nl`
- New brand sites should take `IPlatformDomain` rather than hardcoding `mikepattyn.nl`
