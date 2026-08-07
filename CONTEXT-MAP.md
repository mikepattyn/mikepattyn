# Context Map

Navigation index for the mikepattyn platform repository. Apps and packages may be **owned** (lives in this repo) or a **git submodule** (see [.gitmodules](.gitmodules)).

## Platform

| Path | Role |
|------|------|
| [CONTEXT.md](CONTEXT.md) | Platform domain language |
| [docs/Project_Architecture_Blueprint.md](docs/Project_Architecture_Blueprint.md) | Architecture reference |
| [docs/adr/](docs/adr/) | Platform architectural decisions |
| [infra/cdk/](infra/cdk/) | AWS CDK (.NET) — all IaC |
| [Makefile](Makefile) | CDK and artifact build targets |
| [scripts/](scripts/) | Lambda, fish web, CDK test scripts |

## CDK projects

| Path | Role |
|------|------|
| [infra/cdk/Mikepattyn.CDK/CONTEXT.md](infra/cdk/Mikepattyn.CDK/CONTEXT.md) | Deploy app / stack composition |
| [infra/cdk/Mikepattyn.CDK.Constructs/CONTEXT.md](infra/cdk/Mikepattyn.CDK.Constructs/CONTEXT.md) | Shared constructs library |

## Applications

| App | Path | Source |
|-----|------|--------|
| Mikepattyn | [apps/mikepattyn/](apps/mikepattyn/) | Owned — [CONTEXT.md](apps/mikepattyn/CONTEXT.md) |
| Lumen | [apps/prompt-engineering/](apps/prompt-engineering/) | Owned — [CONTEXT.md](apps/prompt-engineering/CONTEXT.md) |
| Kapsalon (barbershop) | [apps/kapsalon/](apps/kapsalon/) | Submodule — github.com/mikepattyn/kapsalon |
| Fish | [apps/fishi-tracking-app/](apps/fishi-tracking-app/) | Submodule — github.com/mikepattyn/fish-tracking-app |
| AlienButNice | [apps/alienbutnice/](apps/alienbutnice/) | Submodule — github.com/mikepattyn/alienbutnice |
| Echo LiveKit | [apps/alienbutnice/livekit/](apps/alienbutnice/livekit/) | Nested submodule — github.com/mikepattyn/echo-livekit |
| Staying Grounded | [apps/alienbutnice/apps/Staying-Grounded/](apps/alienbutnice/apps/Staying-Grounded/) | Nested submodule — github.com/mikepattyn/Staying-Grounded |

## Packages

| Package | Path | Source |
|---------|------|--------|
| Authress Flutter | [packages/authress-flutter/](packages/authress-flutter/) | Submodule — github.com/mikepattyn/mikepattyn-authress-flutter |
| Authress Angular | [packages/authress-angular/](packages/authress-angular/) | Submodule — github.com/mikepattyn/mikepattyn-authress-angular |

## Tools

| Path | Role |
|------|------|
| [tools/db-explorer/barbershop/](tools/db-explorer/barbershop/) | Local Vue + .NET 10 ops UI for Kapsalon DynamoDB (dev/acc/prod) |

## Research

| Path | Topic |
|------|-------|
| [docs/research/fish-aws-hosting.md](docs/research/fish-aws-hosting.md) | Fish AWS hosting options |
