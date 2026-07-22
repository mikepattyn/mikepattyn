# Context Map

Navigation index for the mikepattyn platform repository.

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

| Path | Role |
|------|------|
| [apps/mikepattyn/](apps/mikepattyn/) | Personal portfolio (owned by this repo) — [CONTEXT.md](apps/mikepattyn/CONTEXT.md) |
| [apps/kapsalon/](apps/kapsalon/) | Submodule — github.com/mikepattyn/kapsalon |
| [apps/fishi-tracking-app/](apps/fishi-tracking-app/) | Submodule — github.com/mikepattyn/fish-tracking-app |
| [apps/alienbutnice/](apps/alienbutnice/) | Submodule — github.com/mikepattyn/alienbutnice |

## Packages

| Path | Role |
|------|------|
| [packages/authress-flutter/](packages/authress-flutter/) | Submodule — github.com/mikepattyn/authress-flutter |
| [packages/authress-angular/](packages/authress-angular/) | Submodule — github.com/mikepattyn/authress-angular |

## Research

| Path | Topic |
|------|-------|
| [docs/research/fish-aws-hosting.md](docs/research/fish-aws-hosting.md) | Fish AWS hosting options |
