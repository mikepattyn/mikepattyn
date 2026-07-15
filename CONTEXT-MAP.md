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

## Applications (submodules)

| Path | Remote |
|------|--------|
| [apps/kapsalon/](apps/kapsalon/) | github.com/mikepattyn/kapsalon |
| [apps/fishi-tracking-app/](apps/fishi-tracking-app/) | github.com/mikepattyn/fish-tracking-app |

## Research

| Path | Topic |
|------|-------|
| [docs/research/fish-aws-hosting.md](docs/research/fish-aws-hosting.md) | Fish AWS hosting options |
