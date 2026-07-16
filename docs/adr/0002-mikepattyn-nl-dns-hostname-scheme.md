# ADR 0002: mikepattyn.nl DNS hostname scheme

## Status

Accepted

## Context

Applications need Development, Staging, and Production URLs under `mikepattyn.nl`. ACM certificate covers apex and `*.mikepattyn.nl` (single-level wildcard).

Nested names like `dev.kapsalon.mikepattyn.nl` are not covered by `*.mikepattyn.nl`.

## Decision

Use single-level host labels:

| Environment | Pattern | Kapsalon example | Fish example |
|-------------|---------|------------------|--------------|
| Development | `{app}-dev` | kapsalon-dev.mikepattyn.nl | fish-dev.mikepattyn.nl |
| Staging | `{app}-acc` | kapsalon-acc.mikepattyn.nl | fish-acc.mikepattyn.nl |
| Production | `{app}` | kapsalon.mikepattyn.nl | fish.mikepattyn.nl |

Implemented in `AppHostnames.For(appSlug, environment, platformDomain)`.

Product applications use Route53 **CNAME** records to CloudFront.

Brand/portfolio sites (`Mikepattyn`, `AlienButNice`) use Production-only `BrandFrontendStack` with **Alias A/AAAA** records for apex and `www` on each platform domain.

## Consequences

- Works with existing wildcard certificate
- Hostname helper is the single source of truth for CDK and deploy docs
- Authress login URLs remain in application config, not CDK DNS
