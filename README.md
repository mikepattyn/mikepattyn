# mikepattyn

Platform umbrella repository for personal applications and shared AWS CDK infrastructure.

## Applications

| App | Path | Hostnames |
|-----|------|-----------|
| Portfolio | `apps/mikepattyn` | (local for now — not a submodule) |
| Kapsalon | `apps/kapsalon` | kapsalon-dev / kapsalon-acc / kapsalon.mikepattyn.nl |
| Fish tracking | `apps/fishi-tracking-app` | fish-dev / fish-acc / fish.mikepattyn.nl |

## Quick start

```bash
git submodule update --init --recursive
cp infra/cdk/Mikepattyn.CDK.Constructs/Constants.Deployment.cs.example \
   infra/cdk/Mikepattyn.CDK.Constructs/Constants.Deployment.cs
# Edit Constants.Deployment.cs with your AWS account, zone, and cert ARNs
make cdk-build
make test-cdk
```

See [CONTEXT-MAP.md](CONTEXT-MAP.md) and [docs/Project_Architecture_Blueprint.md](docs/Project_Architecture_Blueprint.md).
