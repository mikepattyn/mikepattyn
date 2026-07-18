CDK_DIR := infra/cdk
KAPSALON_DIR := apps/kapsalon
FISH_DIR := apps/fishi-tracking-app
MIKEPATTYN_DIR := apps/mikepattyn
ALIENBUTNICE_DIR := apps/alienbutnice
DEPLOYMENT_CONFIG := infra/cdk/Mikepattyn.CDK.Constructs/Constants.Deployment.cs
CDK ?= cdk
CDK_APPROVAL ?= broadening
PWSH ?= powershell

AWS_ACCOUNT := $(shell grep 'AccountId' $(DEPLOYMENT_CONFIG) 2>/dev/null | sed 's/.*= "\(.*\)".*/\1/')
AWS_REGION := $(shell grep 'Region' $(DEPLOYMENT_CONFIG) 2>/dev/null | sed 's/.*= "\(.*\)".*/\1/')
CDK_ENVIRONMENT := aws://$(AWS_ACCOUNT)/$(AWS_REGION)

STACK_DOMAIN := Mikepattyn-Domain-Stack
STACK_ALIENBUTNICE_DOMAIN := AlienButNice-Domain-Stack
STACK_AUTH := Mikepattyn-Auth-Stack

STACK_KAPSALON_FRONTEND_DEV := Kapsalon-Frontend-Stack-Development
STACK_KAPSALON_FRONTEND_STAGING := Kapsalon-Frontend-Stack-Staging
STACK_KAPSALON_FRONTEND_PROD := Kapsalon-Frontend-Stack-Production
STACK_KAPSALON_BACKEND_DEV := Kapsalon-Backend-Stack-Development
STACK_KAPSALON_BACKEND_STAGING := Kapsalon-Backend-Stack-Staging
STACK_KAPSALON_BACKEND_PROD := Kapsalon-Backend-Stack-Production

STACK_FISH_BACKEND_DEV := Fish-Backend-Stack-Development
STACK_FISH_BACKEND_STAGING := Fish-Backend-Stack-Staging
STACK_FISH_BACKEND_PROD := Fish-Backend-Stack-Production
STACK_FISH_FRONTEND_DEV := Fish-Frontend-Stack-Development
STACK_FISH_FRONTEND_STAGING := Fish-Frontend-Stack-Staging
STACK_FISH_FRONTEND_PROD := Fish-Frontend-Stack-Production

STACK_MIKEPATTYN_BRAND_FRONTEND_PROD := Mikepattyn-BrandFrontend-Stack-Production
STACK_ALIENBUTNICE_BRAND_FRONTEND_PROD := AlienButNice-BrandFrontend-Stack-Production

.PHONY: help bootstrap cdk-check-config cdk-build cdk-synth cdk-diff cdk-deploy \
	cdk-deploy-all cdk-deploy-shared cdk-deploy-domain cdk-deploy-auth cdk-deploy-brand \
	cdk-deploy-mikepattyn cdk-deploy-alienbutnice \
	cdk-deploy-kapsalon-dev cdk-deploy-kapsalon-staging cdk-deploy-kapsalon-prod \
	cdk-deploy-kapsalon-dev-frontend cdk-deploy-kapsalon-dev-backend \
	cdk-deploy-kapsalon-staging-frontend cdk-deploy-kapsalon-staging-backend \
	cdk-deploy-kapsalon-prod-frontend cdk-deploy-kapsalon-prod-backend \
	cdk-deploy-fish-dev cdk-deploy-fish-staging cdk-deploy-fish-prod \
	cdk-deploy-fish-dev-frontend cdk-deploy-fish-dev-backend \
	cdk-deploy-fish-staging-frontend cdk-deploy-fish-staging-backend \
	cdk-deploy-fish-prod-frontend cdk-deploy-fish-prod-backend \
	sync-kapsalon-frontend-dev sync-kapsalon-frontend-staging sync-kapsalon-frontend-prod \
	sync-kapsalon-backend-dev sync-kapsalon-backend-staging sync-kapsalon-backend-prod \
	sync-fish-frontend-dev sync-fish-frontend-staging sync-fish-frontend-prod \
	sync-fish-backend-dev sync-fish-backend-staging sync-fish-backend-prod \
	sync-mikepattyn sync-alienbutnice \
	deploy-kapsalon-dev deploy-kapsalon-staging deploy-kapsalon-prod \
	deploy-fish-dev deploy-fish-staging deploy-fish-prod \
	deploy-mikepattyn deploy-alienbutnice \
	lambda-build fish-lambda-build fish-web-build test-cdk

.DEFAULT_GOAL := help

help:
	@echo "Mikepattyn platform deploy targets:"
	@echo ""
	@echo "Build & validate:"
	@echo "  make cdk-build              Build the CDK .NET solution"
	@echo "  make cdk-synth              Synthesize CloudFormation templates"
	@echo "  make cdk-diff               Diff all stacks"
	@echo "  make test-cdk               Run CDK construct and synth e2e tests"
	@echo ""
	@echo "Deploy platform (infra only):"
	@echo "  make cdk-deploy-all         Deploy all stacks"
	@echo "  make cdk-deploy-shared      Deploy Domain (both) + Auth"
	@echo "  make cdk-deploy-domain      Deploy Mikepattyn + AlienButNice domain stacks"
	@echo "  make cdk-deploy-auth        Deploy Auth only"
	@echo "  make cdk-deploy-brand       Deploy both brand frontend stacks"
	@echo "  make cdk-deploy-mikepattyn  Deploy Mikepattyn brand frontend stack"
	@echo "  make cdk-deploy-alienbutnice Deploy AlienButNice brand frontend stack"
	@echo ""
	@echo "Deploy kapsalon (infra, per environment):"
	@echo "  make cdk-deploy-kapsalon-dev|staging|prod"
	@echo "  make cdk-deploy-kapsalon-{env}-frontend|backend"
	@echo ""
	@echo "Deploy fish (infra, per environment):"
	@echo "  make cdk-deploy-fish-dev|staging|prod"
	@echo "  make cdk-deploy-fish-{env}-frontend|backend"
	@echo ""
	@echo "Sync content (build + upload, no CDK):"
	@echo "  make sync-kapsalon-frontend-{dev|staging|prod}"
	@echo "  make sync-kapsalon-backend-{dev|staging|prod}"
	@echo "  make sync-fish-frontend-{dev|staging|prod}"
	@echo "  make sync-fish-backend-{dev|staging|prod}"
	@echo "  make sync-mikepattyn          Vite build + S3 (Production)"
	@echo "  make sync-alienbutnice        Vite build + S3 (Production)"
	@echo ""
	@echo "Full deploy (infra + content):"
	@echo "  make deploy-kapsalon-{dev|staging|prod}"
	@echo "  make deploy-fish-{dev|staging|prod}"
	@echo "  make deploy-mikepattyn"
	@echo "  make deploy-alienbutnice"
	@echo ""
	@echo "App artifacts:"
	@echo "  make lambda-build           Build kapsalon lambda zip"
	@echo "  make fish-lambda-build      Build fish lambda zip"
	@echo "  make fish-web-build         Build Flutter web for fish"
	@echo ""
	@echo "Requires $(DEPLOYMENT_CONFIG) (copy from Constants.Deployment.cs.example)."
	@echo "See docs/research/individual-app-deploy.md for stack and SSM details."

bootstrap: cdk-check-config
	cd $(CDK_DIR) && $(CDK) bootstrap $(CDK_ENVIRONMENT)

cdk-check-config:
	@test -f $(DEPLOYMENT_CONFIG) || \
	  (echo "Copy Constants.Deployment.cs.example → Constants.Deployment.cs first" && exit 1)

cdk-build:
	cd $(CDK_DIR) && dotnet build

cdk-synth: cdk-check-config cdk-build
	cd $(CDK_DIR) && $(CDK) synth

cdk-diff: cdk-check-config cdk-build
	cd $(CDK_DIR) && $(CDK) diff

cdk-deploy-all: cdk-check-config cdk-build lambda-build fish-lambda-build
	cd $(CDK_DIR) && $(CDK) deploy --all --require-approval $(CDK_APPROVAL)

cdk-deploy: cdk-deploy-all

cdk-deploy-shared: cdk-check-config cdk-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_DOMAIN) --require-approval $(CDK_APPROVAL)
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_ALIENBUTNICE_DOMAIN) --require-approval $(CDK_APPROVAL)
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_AUTH) --require-approval $(CDK_APPROVAL)

cdk-deploy-domain: cdk-check-config cdk-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_DOMAIN) --require-approval $(CDK_APPROVAL)
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_ALIENBUTNICE_DOMAIN) --require-approval $(CDK_APPROVAL)

cdk-deploy-auth: cdk-check-config cdk-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_AUTH) --require-approval $(CDK_APPROVAL)

cdk-deploy-brand: cdk-check-config cdk-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_MIKEPATTYN_BRAND_FRONTEND_PROD) --require-approval $(CDK_APPROVAL)
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_ALIENBUTNICE_BRAND_FRONTEND_PROD) --require-approval $(CDK_APPROVAL)

cdk-deploy-mikepattyn: cdk-check-config cdk-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_MIKEPATTYN_BRAND_FRONTEND_PROD) --require-approval $(CDK_APPROVAL)

cdk-deploy-alienbutnice: cdk-check-config cdk-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_ALIENBUTNICE_BRAND_FRONTEND_PROD) --require-approval $(CDK_APPROVAL)

cdk-deploy-kapsalon-dev-frontend: cdk-check-config cdk-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_KAPSALON_FRONTEND_DEV) --require-approval $(CDK_APPROVAL)

cdk-deploy-kapsalon-dev-backend: cdk-check-config cdk-build lambda-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_KAPSALON_BACKEND_DEV) --require-approval $(CDK_APPROVAL)

cdk-deploy-kapsalon-staging-frontend: cdk-check-config cdk-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_KAPSALON_FRONTEND_STAGING) --require-approval $(CDK_APPROVAL)

cdk-deploy-kapsalon-staging-backend: cdk-check-config cdk-build lambda-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_KAPSALON_BACKEND_STAGING) --require-approval $(CDK_APPROVAL)

cdk-deploy-kapsalon-prod-frontend: cdk-check-config cdk-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_KAPSALON_FRONTEND_PROD) --require-approval $(CDK_APPROVAL)

cdk-deploy-kapsalon-prod-backend: cdk-check-config cdk-build lambda-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_KAPSALON_BACKEND_PROD) --require-approval $(CDK_APPROVAL)

cdk-deploy-kapsalon-dev: cdk-deploy-kapsalon-dev-frontend cdk-deploy-kapsalon-dev-backend

cdk-deploy-kapsalon-staging: cdk-deploy-kapsalon-staging-frontend cdk-deploy-kapsalon-staging-backend

cdk-deploy-kapsalon-prod: cdk-deploy-kapsalon-prod-frontend cdk-deploy-kapsalon-prod-backend

cdk-deploy-fish-dev-backend: cdk-check-config cdk-build fish-lambda-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_FISH_BACKEND_DEV) --require-approval $(CDK_APPROVAL)

cdk-deploy-fish-dev-frontend: cdk-check-config cdk-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_FISH_FRONTEND_DEV) --require-approval $(CDK_APPROVAL)

cdk-deploy-fish-staging-backend: cdk-check-config cdk-build fish-lambda-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_FISH_BACKEND_STAGING) --require-approval $(CDK_APPROVAL)

cdk-deploy-fish-staging-frontend: cdk-check-config cdk-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_FISH_FRONTEND_STAGING) --require-approval $(CDK_APPROVAL)

cdk-deploy-fish-prod-backend: cdk-check-config cdk-build fish-lambda-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_FISH_BACKEND_PROD) --require-approval $(CDK_APPROVAL)

cdk-deploy-fish-prod-frontend: cdk-check-config cdk-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_FISH_FRONTEND_PROD) --require-approval $(CDK_APPROVAL)

cdk-deploy-fish-dev: cdk-deploy-fish-dev-backend cdk-deploy-fish-dev-frontend

cdk-deploy-fish-staging: cdk-deploy-fish-staging-backend cdk-deploy-fish-staging-frontend

cdk-deploy-fish-prod: cdk-deploy-fish-prod-backend cdk-deploy-fish-prod-frontend

sync-kapsalon-frontend-dev:
	./scripts/sync-kapsalon-frontend.sh Development

sync-kapsalon-frontend-staging:
	./scripts/sync-kapsalon-frontend.sh Staging

sync-kapsalon-frontend-prod:
	./scripts/sync-kapsalon-frontend.sh Production

sync-kapsalon-backend-dev:
	./scripts/sync-kapsalon-backend.sh Development

sync-kapsalon-backend-staging:
	./scripts/sync-kapsalon-backend.sh Staging

sync-kapsalon-backend-prod:
	./scripts/sync-kapsalon-backend.sh Production

sync-fish-frontend-dev:
	./scripts/sync-fish-frontend.sh Development

sync-fish-frontend-staging:
	./scripts/sync-fish-frontend.sh Staging

sync-fish-frontend-prod:
	./scripts/sync-fish-frontend.sh Production

sync-fish-backend-dev:
	./scripts/sync-fish-backend.sh Development

sync-fish-backend-staging:
	./scripts/sync-fish-backend.sh Staging

sync-fish-backend-prod:
	./scripts/sync-fish-backend.sh Production

sync-mikepattyn:
	./scripts/sync-brand-frontend.sh Mikepattyn $(MIKEPATTYN_DIR)

sync-alienbutnice:
	./scripts/sync-brand-frontend.sh AlienButNice $(ALIENBUTNICE_DIR)

deploy-kapsalon-dev: cdk-deploy-kapsalon-dev sync-kapsalon-frontend-dev sync-kapsalon-backend-dev

deploy-kapsalon-staging: cdk-deploy-kapsalon-staging sync-kapsalon-frontend-staging sync-kapsalon-backend-staging

deploy-kapsalon-prod: cdk-deploy-kapsalon-prod sync-kapsalon-frontend-prod sync-kapsalon-backend-prod

deploy-fish-dev: cdk-deploy-fish-dev sync-fish-frontend-dev sync-fish-backend-dev

deploy-fish-staging: cdk-deploy-fish-staging sync-fish-frontend-staging sync-fish-backend-staging

deploy-fish-prod: cdk-deploy-fish-prod sync-fish-frontend-prod sync-fish-backend-prod

deploy-mikepattyn: cdk-deploy-mikepattyn sync-mikepattyn

deploy-alienbutnice: cdk-deploy-alienbutnice sync-alienbutnice

lambda-build:
	./scripts/build-lambda.sh

fish-lambda-build:
	./scripts/build-fish-lambda.sh

fish-web-build:
	./scripts/build-fish-web.sh

test-cdk:
	$(PWSH) -NoProfile -ExecutionPolicy Bypass -File ./scripts/test-cdk.ps1
