ARTIFACTS 	:= $(shell pwd)/artifacts
RESUME_JSON	:= $(shell pwd)/src/resume.json

.PHONY: restore
restore:
	@dotnet tool restore

.PHONY: clean
clean:
	@rm -rf $(ARTIFACTS)

.PHONY: validate
validate:
	@echo ""
	@echo "\033[0;32mValidating \033[0m"
	@echo "\033[0;32m------------------- \033[0m"
	@dotnet resume validate -f $(RESUME_JSON)

.PHONY: build
build: restore clean validate
	@echo ""
	@echo "\033[0;32mGenerating \033[0m"
	@echo "\033[0;32m------------------- \033[0m"
	@dotnet resume build -f $(RESUME_JSON) -o $(ARTIFACTS)
