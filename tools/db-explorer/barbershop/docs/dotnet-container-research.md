# Research: .NET Lambda container images (road not taken)

This tool's backend was rewritten in C# as a plain ASP.NET Core Minimal API — **not** a deployed
AWS Lambda container image, and (after an initial attempt) **not locally containerized either**.
It runs directly via `dotnet run`, resolving AWS credentials from the developer's own machine
exactly like the old Node/Express server did. A local Docker container was tried first
(`mcr.microsoft.com/dotnet/aspnet:10.0`), but mounting `~/.aws` into the container added friction
around resolving the host's AWS SSO session cleanly, with no real benefit for a tool that only
ever runs on one developer's machine — so it was dropped in favor of running natively. This
document records what a real Lambda container image implementation would look like, so the
decision is sourced and reversible if this tool is ever promoted to a real, team-shared deployment.

For contrast, this repo's existing .NET 10 Lambda functions (`Kapsalon.Tenant.Api`,
`Kapsalon.Scheduling.Api`, `Kapsalon.Identity.Api` under `apps/kapsalon/backend/`) use
**zip-based** deployment instead — see
[`apps/kapsalon/docs/adr/0001-aws-cdk-dotnet-lambda-backend.md`](../../../apps/kapsalon/docs/adr/0001-aws-cdk-dotnet-lambda-backend.md):

> We use C# AWS CDK constructs (adapted from the flyingdarts pattern) plus .NET 10 Lambda handlers
> bundled as `lambda.zip`. This mirrors a proven stack: construct library for reusable infra, thin
> deploy app, REST API Gateway, and Lambda authorizer. Hard to reverse once deployed; chosen over
> Node/Python Lambdas for consistency with existing .NET expertise and flyingdarts reference
> implementation.

## 1. AWS-provided .NET Lambda base images

Source: https://docs.aws.amazon.com/lambda/latest/dg/csharp-image.html (section "AWS base images
for .NET"). Exact table as published:

| Tags | Runtime | Operating system | Deprecation |
| --- | --- | --- | --- |
| 10 | .NET 10 | Amazon Linux 2023 | Nov 14, 2028 |
| 9 | .NET 9 | Amazon Linux 2023 | Nov 10, 2026 |
| 8 | .NET 8 | Amazon Linux 2023 | Nov 10, 2026 |

Amazon ECR repository: `gallery.ecr.aws/lambda/dotnet` (confirmed at
https://gallery.ecr.aws/lambda/dotnet). Confirmed: a `10` tag exists for .NET 10 / Amazon Linux
2023, deprecating Nov 14, 2028, alongside the `9` and `8` tags, exactly as expected.

## 2. Dockerfile shapes documented by AWS

Source: https://docs.aws.amazon.com/lambda/latest/dg/csharp-image.html

**Using an AWS base image** (`FROM public.ecr.aws/lambda/dotnet:<tag>`). AWS's instructions say to
set `FROM` to the base image URI, matching the `TargetFramework` in the `.csproj` (e.g. `FROM
public.ecr.aws/lambda/dotnet:9` pairs with `<TargetFramework>net9.0</TargetFramework>`), and:

> Set the `CMD` argument to the Lambda function handler. This should match the `image-command` in
> `aws-lambda-tools-defaults.json`.

I.e. `CMD` isn't a shell command — its argument is the Lambda handler string
(`Assembly::Namespace.ClassName::MethodName`), which the base image's built-in runtime interface
client resolves to invoke on each event. This base image bundles the language runtime, a runtime
interface client, and a runtime interface emulator (RIE) for local testing.

**Using an alternative (Microsoft) base image**, closer in spirit to `aspnet-runtime`-style images.
AWS's exact example Dockerfile (from the `lambda.CustomRuntimeFunction` template):

```dockerfile
# You can also pull these images from DockerHub amazon/aws-lambda-dotnet:8
FROM mcr.microsoft.com/dotnet/runtime:9.0

# Set the image's internal work directory
WORKDIR /var/task

# Copy function code to Lambda-defined environment variable
COPY "bin/Release/net9.0/linux-x64"  .

# Set the entrypoint to the bootstrap
ENTRYPOINT ["/usr/bin/dotnet", "exec", "/var/task/bootstrap.dll"]
```

The accompanying `Function.cs` "contains a class with a `Main` method that initializes the
`Amazon.Lambda.RuntimeSupport` library as the bootstrap." `aws-lambda-tools-defaults.json` must set
`"package-type": "image"`.

**The one thing that makes an image Lambda-compatible**, per
https://docs.aws.amazon.com/lambda/latest/dg/images-create.html ("Runtime interface clients"):

> If you use an OS-only base image or an alternative base image, you must include a runtime
> interface client in your image. The runtime interface client must extend the Lambda runtime API,
> which manages the interaction between Lambda and your function code.

For .NET that client is the `Amazon.Lambda.RuntimeSupport` NuGet package. Without it, a Microsoft
base image is "just a container" — adding it (plus the `bootstrap` entrypoint that talks the
Lambda Runtime API) is the entire delta between that and a valid Lambda container image.

## 3. Local testing via the Runtime Interface Emulator (RIE)

The URL `https://docs.aws.amazon.com/lambda/latest/dg/images-test.html` no longer resolves to a
standalone page (verified: it soft-redirects to the guide's `welcome.html`, identical to several
other guessed/nonexistent slugs). The RIE local-testing workflow is documented per-language
instead, e.g. https://docs.aws.amazon.com/lambda/latest/dg/go-image.html
("To run the runtime interface emulator on your local machine"), representative of the same
workflow AWS documents for every language including .NET:

```
docker run -d -p 9000:8080 \
--entrypoint /usr/local/bin/aws-lambda-rie \
docker-image:test ./main
```

> This command runs the image as a container and creates a local endpoint at
> `localhost:9000/2015-03-31/functions/function/invocations`.

Invocation:

```
curl "http://localhost:9000/2015-03-31/functions/function/invocations" -d '{}'
```

> This command invokes the function with an empty event and returns a response. Some functions
> might require a JSON payload. Example:
> `curl "http://localhost:9000/2015-03-31/functions/function/invocations" -d '{"payload":"hello world!"}'`

This is a **single fixed invoke endpoint** taking one JSON event and returning one JSON response —
not a general-purpose HTTP server with arbitrary verbs, paths, or query strings. That's precisely
why this repo's db-explorer tool — whose Vue frontend already expects plain `GET`/`PUT`/`DELETE`
with query strings against arbitrary routes — doesn't build on this interface: every request would
need to be wrapped/unwrapped as a synthetic API-Gateway-shaped event just to reach a handler that
was never a real HTTP server to begin with.

## 4. Deployment differences vs. the existing zip-based Kapsalon APIs

Source: https://docs.aws.amazon.com/lambda/latest/dg/images-create.html

Container-image deployment flow: build the image locally → push it to an **Amazon ECR
repository in the same AWS Region as the Lambda function** ("The Amazon ECR repository must be in
the same AWS Region as the Lambda function") → create/update the function with
`PackageType: Image` and the ECR image URI. This contrasts with `Kapsalon.Tenant.Api` /
`Kapsalon.Scheduling.Api` / `Kapsalon.Identity.Api`, which use zip-asset upload — `dotnet lambda
deploy-function` locally, or a CDK zip asset bundled from `lambda.zip` — with no ECR repository,
image build, or image registry step involved at all.

Required ECR permissions (same-account case), quoted from the docs:

> If you choose to use an Amazon ECR repository policy, add `ecr:BatchGetImage` and
> `ecr:GetDownloadUrlForLayer` permissions.

The identity/role that *creates* the function additionally needs `ecr:GetRepositoryPolicy`,
`ecr:SetRepositoryPolicy`, `ecr:BatchGetImage`, and `ecr:GetDownloadUrlForLayer`. Cross-account
image use requires permissions on *both* sides (consuming role and ECR resource policy).

Package type is permanent per function:

> You cannot change the deployment package type (.zip or container image) for an existing
> function. For example, you cannot convert a container image function to use a .zip file
> archive. You must create a new function.

So switching `Kapsalon.*` from zip to image (or vice versa) would mean standing up new Lambda
functions, not an in-place migration.

## 5. Why we didn't use this

- **Local-only, never deployed**: this tool never runs in AWS — there's no Lambda function, no
  ECR repository, no region to push an image to. The entire container-image-Lambda pipeline
  (build → push to ECR → create function with `PackageType: Image`) has no target to deploy to.
- **Per-request AWS profile/region switching from the UI**: a Lambda function has one execution
  role and runs in one region for its whole lifetime. This tool lets the developer pick an AWS SSO
  profile and region per request from the Vue frontend — fundamentally incompatible with a fixed
  Lambda execution role/region.
- **No fixed IAM execution role**: Lambda container images still run under a single configured
  execution role; this tool intentionally has none, delegating to whatever AWS SSO profile the
  developer is signed into locally.
- **REST/query-string API shape**: the RIE/Lambda invoke interface is one JSON event in, one JSON
  response out (§3) — not arbitrary GET/PUT/DELETE with query strings. The Vue frontend already
  expects a normal REST API, so a plain ASP.NET Core Minimal API running via `dotnet run` is a
  direct fit with no event-adapter layer needed.
- **AWS SSO credentials are easiest resolved natively**: running as a plain local process picks up
  the same `~/.aws` config, credentials, and SSO token cache the developer's shell already has, with
  no container volume-mount or path-translation concerns to work around.

If this tool is ever turned into a real, team-shared, deployed service, §§1–4 above are the
reference path: use `public.ecr.aws/lambda/dotnet:10` (or add `Amazon.Lambda.RuntimeSupport` to a
Microsoft base image), push to a per-region ECR repo, and deploy as a real `PackageType: Image`
Lambda function — following the same CDK pattern already used for `Kapsalon.Tenant.Api` et al.,
adapted for image instead of zip packages.
