APP_NAME = hainz

.DEFAULT_GOAL := help
.PHONY: *

help:
	@grep -E '^[a-zA-Z0-9_-]+:.*?## .*$$' $(MAKEFILE_LIST) \
	| sed -n 's/^\(.*\): \(.*\)\(##\)\(.*\)/\1\3-\4/p' \
	| column -t -s '##'

build: ## Builds docker image
	@docker build . -t $(APP_NAME)

rebuild: ## Builds docker image without cache
	@docker build . --no-cache -t $(APP_NAME)

run: ## Runs docker image
	@docker run -it --rm --name="$(APP_NAME)" $(APP_NAME)