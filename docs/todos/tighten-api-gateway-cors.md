# Tighten API Gateway CORS after same-origin edge

Kapsalon and Fish API Gateway constructs use `AllowOrigins = Cors.ALL_ORIGINS`. Lambda responses also set `Access-Control-Allow-Origin: *`.

## Goal

Once SPAs call the API same-origin on the app hostname, restrict CORS on execute-api to the per-environment **AppHostname** (and any other known callers such as mobile builds if needed).

## Why deferred

Same-origin SPA traffic does not require a CORS change. Tightening execute-api origins is hardening, not required for the Kapsalon edge cutover.

## Likely steps

1. Set API Gateway `DefaultCorsPreflightOptions.AllowOrigins` to `https://{appHostname}` per environment.
2. Align Lambda `ApiGatewayJsonResponse` CORS headers with the same allowlist.
3. Verify Authress / mobile / curl workflows still work against execute-api if they remain supported.
