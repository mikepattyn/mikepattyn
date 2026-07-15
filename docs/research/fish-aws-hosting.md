# Fish AWS hosting research

Decision-oriented summary for `fish.mikepattyn.nl` MVP IaC.

## Compute (ASP.NET Core + SignalR)

| Option | Fit | Notes |
|--------|-----|-------|
| **ECS Fargate + ALB** | Best | ALB native WebSockets; target group stickiness for SignalR |
| App Runner | Avoid | Closed to new customers; 120s timeout; poor long-lived hub fit |
| Elastic Beanstalk | Possible | Heavier; less CDK-native than Fargate |

Sources: [AWS ALB listeners](https://docs.aws.amazon.com/elasticloadbalancing/latest/application/load-balancer-listeners.html), [ASP.NET SignalR scale](https://learn.microsoft.com/en-us/aspnet/core/signalr/scale?view=aspnetcore-9.0)

## Database (PostGIS)

| Option | MVP | Later |
|--------|-----|-------|
| **RDS PostgreSQL** | Yes | Aurora if HA/failover needed |

PostGIS via `CREATE EXTENSION postgis` on RDS PostgreSQL.

Sources: [RDS PostGIS](https://docs.aws.amazon.com/AmazonRDS/latest/UserGuide/Appendix.PostgreSQL.CommonDBATasks.PostGIS.html)

## Object storage

**S3** for catch photos on AWS (replace local Azure Blob / Azurite for cloud).

## Edge / same hostname

CloudFront path routing:

- `/*` → S3 (Flutter web SPA)
- `/api/*`, `/hubs/*` → ALB origin

Same-origin avoids SignalR CORS + credential stickiness issues.

Sources: [CloudFront WebSockets](https://docs.aws.amazon.com/AmazonCloudFront/latest/DeveloperGuide/distribution-working-with.websockets.html)

## Deferred

- Aurora, Redis SignalR backplane, App Runner
- Full CI/CD parity with kapsalon GitHub workflows
