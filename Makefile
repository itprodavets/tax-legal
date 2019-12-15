PROJECT_NAME := tl
TAG_PREFIX := registry.annium.com/$(PROJECT_NAME)

ref-books-db-up ref-books-api-up cbc-report-api-up client-up:
	@pwsh dev/scripts/net-up.ps1 -project $(PROJECT_NAME)
	@pwsh dev/scripts/up.ps1 -project $(PROJECT_NAME) -component $(subst -up,,$@)

ref-books-db-down ref-books-api-down cbc-report-api-down client-down:
	@pwsh dev/scripts/down.ps1 -project $(PROJECT_NAME) -component $(subst -down,,$@)
	@pwsh dev/scripts/net-down.ps1 -project $(PROJECT_NAME)

ref-books-db-log ref-books-api-log cbc-report-api-log client-log:
	@docker logs -f $(PROJECT_NAME)_$(subst -,_,$(subst -log,,$@))


ref-books-db-drop ref-books-db-update ref-books-migrations-add ref-books-migrations-list ref-books-migrations-remove:
	@cd server && pwsh tools/ef/$@.ps1 -startup src/Annium.Id.Api -project src/Annium.Id.Db -context Context

# https://github.com/aspnet/EntityFrameworkCore/issues/18292 - to cleanup migrations flow
publish-api:
	$(call publish,migrations,server,src/Annium.Id.Api/migrations.Dockerfile)
	$(call publish,api,server,src/Annium.Id.Api/api.Dockerfile)

publish-site:
	$(call publish,site,web/src/site,Dockerfile)


define publish
	@$(eval image := $(1))
	@$(eval context := $(2))
	@$(eval dockerfile := $(3))
	@docker build -t $(TAG_PREFIX)/$(image) -f $(context)/$(dockerfile) $(context)
	@docker push $(TAG_PREFIX)/$(image)
endef

.PHONY: $(MAKECMDGOALS)