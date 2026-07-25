# ADR 0008: Same-origin API CloudFront origin request policy

## Status

Accepted

## Context

Fish and Kapsalon serve their API as same-origin `/api/*` on the app hostname (ADR 0005, ADR 0007). CloudFront routes `/api/*` to API Gateway and everything else to S3 (SPA).

The `/api/*` behavior used `OriginRequestPolicy.ALL_VIEWER`, which forwards the viewer's `Host` header (`gofish.mikepattyn.nl`, `barbershop.mikepattyn.nl`) to API Gateway. API Gateway expects `Host` to be the execute-api domain; mismatched Host causes 403. Distribution-wide SPA custom error responses then rewrite 403 → 200 + `/index.html`, so API calls returned HTML with 200.

## Decision

For every same-origin `/api/*` CloudFront cache behavior (Fish and Kapsalon):

- Use managed origin request policy **AllViewerExceptHostHeader** (`OriginRequestPolicy.ALL_VIEWER_EXCEPT_HOST_HEADER`).
- Keep distribution-wide 403/404 → `/index.html` SPA rewrites for now (S3 OAC deep links still need them).

Follow-up (edge unification): replace SPA error rewrites with a CloudFront Function path rewrite on the S3 default behavior only, so API 403/404 are not masked as HTML.

## Consequences

- Successful API requests return JSON from API Gateway instead of SPA HTML.
- SPA deep links unchanged until error-page refactor.
- Real API auth failures (403) may still be rewritten to HTML until the follow-up lands.

## References

- [docs/adr/0005-fish-serverless-hosting.md](./0005-fish-serverless-hosting.md)
- [docs/adr/0007-kapsalon-same-origin-api-edge.md](./0007-kapsalon-same-origin-api-edge.md)
- [AWS: AllViewerExceptHostHeader managed origin request policy](https://docs.aws.amazon.com/AmazonCloudFront/latest/DeveloperGuide/using-managed-origin-request-policies.html#managed-origin-request-policy-all-viewer-except-host-header)
