APP_NAME = hainz
ENV_FILE = .env.docker

.DEFAULT_GOAL := help
.PHONY: *

help:
	@grep -E '^[a-zA-Z0-9_-]+:.*?## .*$$' $(MAKEFILE_LIST) \
	| sed -n 's/^\(.*\): \(.*\)\(##\)\(.*\)/\1\3-\4/p' \
	| column -t -s '##'

build-app: ## Builds application docker image
	@docker build . -t $(APP_NAME)

rebuild-app: ## Builds application docker image without cache
	@docker build . --no-cache -t $(APP_NAME)

run-app: ## Runs application docker image
	@docker run -it --rm --name="$(APP_NAME)" $(APP_NAME)

build: ## Builds docker application with database
	@docker-compose --env-file $(ENV_FILE) build

rebuild: ## Rebuilds docker application with database
	@docker-compose --env-file $(ENV_FILE) build --no-cache

run: ## Runs docker application with database
	@docker-compose --env-file $(ENV_FILE) up

migration: ## Creates a new EF migration. Invoke with name=<migration name>.
	@dotnet ef migrations add $(name) --project src/Hainz.Persistence/Hainz.Persistence.csproj