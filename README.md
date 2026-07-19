# mikepattyn

Platform umbrella repository for personal applications and shared AWS CDK infrastructure.

## Applications

| App | Path | Hostnames |
|-----|------|-----------|
| Portfolio | `apps/mikepattyn` | mikepattyn.nl |
| AlienButNice | `apps/alienbutnice` | alienbutnice.nl |
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

See [CONTEXT-MAP.md](CONTEXT-MAP.md), [docs/research/individual-app-deploy.md](docs/research/individual-app-deploy.md), and [docs/Project_Architecture_Blueprint.md](docs/Project_Architecture_Blueprint.md).

## Deploy individual apps

Make is the source of truth for **local** deploys. Three target families:

| Family | Purpose | Examples |
|--------|---------|----------|
| `cdk-deploy-*` | AWS infra only (CDK stacks) | `make cdk-deploy-kapsalon-dev` |
| `sync-*` | Build + upload content (S3 / Lambda code) | `make sync-mikepattyn` |
| `deploy-*` | Infra then content | `make deploy-fish-dev` |

Run `make help` for the full list.

**Prerequisites:** `Constants.Deployment.cs` configured, AWS credentials, and shared domain stacks deployed first (`make cdk-deploy-domain`).

### Examples

```bash
# Mikepattyn portfolio (Production brand site)
make deploy-mikepattyn

# Kapsalon dev environment (infra + frontend + backend)
make deploy-kapsalon-dev

# Fish: content only after infra exists
make sync-fish-frontend-dev
make sync-fish-backend-dev

# AlienButNice brand site
make deploy-alienbutnice
```

### CI

Root workflows under [`.github/workflows/`](.github/workflows/) run build and deploy commands inline (no Make or shell scripts). They deploy content on path-filtered pushes to `main`:

- **Brands** (`apps/mikepattyn`, `apps/alienbutnice`) → Production
- **Kapsalon / Fish** → Development (staging/prod via workflow dispatch)

CDK deploys are **manual only** via the `Deploy CDK` workflow (`workflow_dispatch`).

Workflows in `apps/kapsalon/.github/workflows/` are **legacy** — they do not run from this monorepo. Use the root workflows instead.
