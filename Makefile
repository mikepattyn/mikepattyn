CDK_DIR := infra/cdk
KAPSALON_DIR := apps/kapsalon
FISH_DIR := apps/fishi-tracking-app
DEPLOYMENT_CONFIG := infra/cdk/Mikepattyn.CDK.Constructs/Constants.Deployment.cs
CDK ?= cdk
CDK_APPROVAL ?= broadening
PWSH ?= powershell

AWS_ACCOUNT := $(shell grep 'AccountId' $(DEPLOYMENT_CONFIG) 2>/dev/null | sed 's/.*= "\(.*\)".*/\1/')
AWS_REGION := $(shell grep 'Region' $(DEPLOYMENT_CONFIG) 2>/dev/null | sed 's/.*= "\(.*\)".*/\1/')
CDK_ENVIRONMENT := aws://$(AWS_ACCOUNT)/$(AWS_REGION)

STACK_DOMAIN := Mikepattyn-Domain-Stack
STACK_AUTH := Mikepattyn-Auth-Stack

STACK_KAPSALON_FRONTEND_DEV := Kapsalon-Frontend-Stack-Development
STACK_KAPSALON_FRONTEND_STAGING := Kapsalon-Frontend-Stack-Staging
STACK_KAPSALON_FRONTEND_PROD := Kapsalon-Frontend-Stack-Production
STACK_KAPSALON_BACKEND_DEV := Kapsalon-Backend-Stack-Development
STACK_KAPSALON_BACKEND_STAGING := Kapsalon-Backend-Stack-Staging
STACK_KAPSALON_BACKEND_PROD := Kapsalon-Backend-Stack-Production

STACK_FISH_DATA_DEV := Fish-Data-Stack-Development
STACK_FISH_DATA_STAGING := Fish-Data-Stack-Staging
STACK_FISH_DATA_PROD := Fish-Data-Stack-Production
STACK_FISH_API_DEV := Fish-Api-Stack-Development
STACK_FISH_API_STAGING := Fish-Api-Stack-Staging
STACK_FISH_API_PROD := Fish-Api-Stack-Production
STACK_FISH_FRONTEND_DEV := Fish-Frontend-Stack-Development
STACK_FISH_FRONTEND_STAGING := Fish-Frontend-Stack-Staging
STACK_FISH_FRONTEND_PROD := Fish-Frontend-Stack-Production

STACK_MIKEPATTYN_BRAND_FRONTEND_PROD := Mikepattyn-BrandFrontend-Stack-Production
STACK_ALIENBUTNICE_BRAND_FRONTEND_PROD := AlienButNice-BrandFrontend-Stack-Production

.PHONY: help bootstrap cdk-check-config cdk-build cdk-synth cdk-diff cdk-deploy \
	cdk-deploy-shared cdk-deploy-domain cdk-deploy-auth cdk-deploy-brand \
	cdk-deploy-kapsalon-dev cdk-deploy-kapsalon-staging cdk-deploy-kapsalon-prod \
	cdk-deploy-fish-dev cdk-deploy-fish-staging cdk-deploy-fish-prod \
	lambda-build test-cdk fish-web-build

.DEFAULT_GOAL := help

help:
	@echo "Mikepattyn platform CDK targets:"
	@echo ""
	@echo "Build & validate:"
	@echo "  make cdk-build              Build the CDK .NET solution"
	@echo "  make cdk-synth              Synthesize CloudFormation templates"
	@echo "  make cdk-diff               Diff all stacks"
	@echo "  make test-cdk               Run CDK construct and synth e2e tests"
	@echo ""
	@echo "Deploy platform:"
	@echo "  make cdk-deploy-shared      Deploy Domain + Auth"
	@echo "  make cdk-deploy-domain      Deploy Domain only"
	@echo "  make cdk-deploy-auth        Deploy Auth only"
	@echo "  make cdk-deploy-brand       Deploy Mikepattyn + AlienButNice brand frontends"
	@echo ""
	@echo "Deploy kapsalon (per environment):"
	@echo "  make cdk-deploy-kapsalon-dev"
	@echo "  make cdk-deploy-kapsalon-staging"
	@echo "  make cdk-deploy-kapsalon-prod"
	@echo ""
	@echo "Deploy fish (per environment):"
	@echo "  make cdk-deploy-fish-dev"
	@echo "  make cdk-deploy-fish-staging"
	@echo "  make cdk-deploy-fish-prod"
	@echo ""
	@echo "App artifacts:"
	@echo "  make lambda-build           Build kapsalon lambda zip"
	@echo "  make fish-web-build         Build Flutter web for fish"
	@echo ""
	@echo "Requires $(DEPLOYMENT_CONFIG) (copy from Constants.Deployment.cs.example)."

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

cdk-deploy-shared: cdk-check-config cdk-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_DOMAIN) --require-approval $(CDK_APPROVAL)
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_AUTH) --require-approval $(CDK_APPROVAL)

cdk-deploy-domain: cdk-check-config cdk-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_DOMAIN) --require-approval $(CDK_APPROVAL)

cdk-deploy-auth: cdk-check-config cdk-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_AUTH) --require-approval $(CDK_APPROVAL)

cdk-deploy-brand: cdk-check-config cdk-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_MIKEPATTYN_BRAND_FRONTEND_PROD) --require-approval $(CDK_APPROVAL)
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_ALIENBUTNICE_BRAND_FRONTEND_PROD) --require-approval $(CDK_APPROVAL)

cdk-deploy-kapsalon-dev: cdk-check-config cdk-build lambda-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_KAPSALON_FRONTEND_DEV) --require-approval $(CDK_APPROVAL)
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_KAPSALON_BACKEND_DEV) --require-approval $(CDK_APPROVAL)

cdk-deploy-kapsalon-staging: cdk-check-config cdk-build lambda-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_KAPSALON_FRONTEND_STAGING) --require-approval $(CDK_APPROVAL)
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_KAPSALON_BACKEND_STAGING) --require-approval $(CDK_APPROVAL)

cdk-deploy-kapsalon-prod: cdk-check-config cdk-build lambda-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_KAPSALON_FRONTEND_PROD) --require-approval $(CDK_APPROVAL)
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_KAPSALON_BACKEND_PROD) --require-approval $(CDK_APPROVAL)

cdk-deploy-fish-dev: cdk-check-config cdk-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_FISH_DATA_DEV) --require-approval $(CDK_APPROVAL)
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_FISH_API_DEV) --require-approval $(CDK_APPROVAL)
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_FISH_FRONTEND_DEV) --require-approval $(CDK_APPROVAL)

cdk-deploy-fish-staging: cdk-check-config cdk-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_FISH_DATA_STAGING) --require-approval $(CDK_APPROVAL)
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_FISH_API_STAGING) --require-approval $(CDK_APPROVAL)
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_FISH_FRONTEND_STAGING) --require-approval $(CDK_APPROVAL)

cdk-deploy-fish-prod: cdk-check-config cdk-build
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_FISH_DATA_PROD) --require-approval $(CDK_APPROVAL)
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_FISH_API_PROD) --require-approval $(CDK_APPROVAL)
	cd $(CDK_DIR) && $(CDK) deploy $(STACK_FISH_FRONTEND_PROD) --require-approval $(CDK_APPROVAL)

lambda-build:
	./scripts/build-lambda.sh

fish-web-build:
	./scripts/build-fish-web.sh

test-cdk:
	$(PWSH) -NoProfile -ExecutionPolicy Bypass -File ./scripts/test-cdk.ps1
