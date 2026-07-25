# ADR 0005: Fish serverless hosting on AWS

## Status

Accepted (supersedes ADR 0004)

## Context

Fish MVP was planned on ECS Fargate + RDS PostGIS for SignalR and geo queries. MVP code uses Haversine (not PostGIS SQL), SignalR is unused from mobile, and the goal is pay-as-you-go idle cost like Kapsalon.

## Decision

Fish AWS layout per environment:

| Layer | Service |
|-------|---------|
| Edge | CloudFront — `/*` → S3 (Flutter web); `/api/*` → API Gateway |
| API | API Gateway REST + .NET 10 Lambda handlers |
| Auth | Authress JWT via Lambda request authorizer |
| Data | DynamoDB PAY_PER_REQUEST (single-table) + S3 catch photos |

No ECS, ALB, RDS, VPC, or SignalR for MVP.

## Consequences

- Idle cost ≈ DynamoDB storage + S3 + control plane (no always-on compute)
- Geo via H3 cell keys + Haversine in Lambda (ADR 002 superseded for hosting)
- Realtime deferred (no GameHub)

## References

- [docs/adr/0004-fish-mvp-hosting.md](./0004-fish-mvp-hosting.md) (superseded)
- [apps/fishi-tracking-app/docs/adr/005-authress-authentication.md](../apps/fishi-tracking-app/docs/adr/005-authress-authentication.md)
- [apps/fishi-tracking-app/docs/adr/006-dynamo-single-table.md](../apps/fishi-tracking-app/docs/adr/006-dynamo-single-table.md)
- [docs/adr/0008-same-origin-api-cloudfront-origin-policy.md](./0008-same-origin-api-cloudfront-origin-policy.md)
