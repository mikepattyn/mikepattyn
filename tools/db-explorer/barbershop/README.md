# Barbershop DB Explorer

Local ops UI for Kapsalon (barbershop) DynamoDB single-table data.

Edits operational DynamoDB records across **Development**, **Staging**, and **Production**. Customer-facing display copy remains in the Angular tenant bundle ([ADR-0009](../../../apps/kapsalon/docs/adr/0009-bundled-tenant-display-content.md)).

## Prerequisites

- Node.js 20+ (frontend) and .NET 10 SDK (backend — an ASP.NET Core Minimal API)
- AWS credentials with:
  - `ssm:GetParameter` on `/Kapsalon/{Environment}/Application/DynamoDbTableName`
  - DynamoDB read/write on the Kapsalon application table

```bash
aws sso login --profile your-profile
```

## Run

```bash
cd tools/db-explorer/barbershop
npm install
npm run dev
```

Open http://localhost:5173

- `npm run dev:server` runs the backend (`server-dotnet/BarbershopDbExplorer.Api`) directly via
  `dotnet run`, binding to `127.0.0.1:3847` only. It resolves AWS credentials the same way the
  old Node server did — default credential chain, or a named profile from `~/.aws` when one is
  picked in the UI — so `aws sso login` on the host is all that's needed, no extra setup.
- Optional **AWS profile** and **region** (`eu-central-1` default) in the top bar
- **Tenant ID** defaults to `sabunandsteel` (ignored on Customer tab — uses `TENANT#PLATFORM`)

## Entities

| Tab | Dynamo entity | Notes |
|-----|---------------|-------|
| Profile | TenantProfile | One row per tenant (`SK=PROFILE`) |
| Staff | StaffMember | |
| Service | Service | Duration + price only |
| Appointment | Appointment + SlotLock | Create/delete uses TransactWrite |
| Customer | Customer | Platform partition |

**Appointment tab** includes an orphan SlotLock panel (delete-only cleanup).

## Safety

- Colored env banner (prod highlighted)
- **Production** Save/Delete requires typing `prod`
- Dev/Acc Delete uses a browser confirm

## Scripts

| Command | Description |
|---------|-------------|
| `npm run dev` | API + Vite UI |
| `npm run dev:server` | API only (`dotnet run` on `server-dotnet/BarbershopDbExplorer.Api`) |
| `npm run dev:client` | UI only |
| `npm run build` | Typecheck + production UI build |
| `npm run test` | Vitest unit/component tests for the Vue frontend |
| `dotnet test` (from `server-dotnet/`) | xUnit tests for the .NET backend |
