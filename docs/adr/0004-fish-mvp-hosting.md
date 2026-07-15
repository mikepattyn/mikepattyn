# ADR 0004: Fish MVP hosting on AWS

## Status

Accepted

## Context

Fish tracking is Flutter mobile + ASP.NET Core 9 API with PostGIS, SignalR, and S3-compatible photo storage. It needs `fish.mikepattyn.nl` with Flutter web and API on the same hostname.

## Decision

MVP AWS layout per environment:

| Layer | Service |
|-------|---------|
| Edge | CloudFront — default `/*` → S3 (Flutter web); `/api/*`, `/hubs/*` → ALB |
| API | ECS Fargate + ALB (sticky sessions, 120s idle timeout for SignalR) |
| Data | RDS PostgreSQL 16 + S3 bucket for catch photos |
| Network | VPC per environment (Fish Data stack owns VPC; Api stack reuses it) |

Placeholder container image (`nginx`) until Fish API Dockerfile and ECR pipeline land.

## Consequences

- Not serverless/Lambda — different from Kapsalon but same platform CDK repo
- PostGIS extension applied at deploy/migration time (not in CDK MVP)
- Mobile apps use same API origin; app store distribution unchanged

## References

- [docs/research/fish-aws-hosting.md](../research/fish-aws-hosting.md)
