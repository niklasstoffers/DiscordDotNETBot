APP_NAME = hainz

.DEFAULT_GOAL := help
.PHONY: *

help:
	@grep -E '^[a-zA-Z0-9_-]+:.*?## .*$$' $(MAKEFILE_LIST) \
	| sed -n 's/^\(.*\): \(.*\)\(##\)\(.*\)/\1\3-\4/p' \
	| column -t -s '##'

build-hainz: ## Builds bot docker image
	@docker build . -t $(APP_NAME)

rebuild-hainz: ## Builds bot docker image without cache
	@docker build . --no-cache -t $(APP_NAME)

run-hainz: ## Runs bot docker image
	@docker run -it --rm --name="$(APP_NAME)" $(APP_NAME)

build: ## Builds application
	@docker-compose build

rebuild: ## Rebuilds application
	@docker-compose build --no-cache

run: ## Runs application
	@docker-compose up

migration: ## Creates a new EF migration. Invoke with name=<migration name>.
	@dotnet ef migrations add $(name) --project src/Hainz.Persistence/Hainz.Persistence.csproj