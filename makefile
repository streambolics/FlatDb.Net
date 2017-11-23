all: docker_build
check: dotnet_build

docker_run: docker_build
	sudo docker run -p80:80 streambolics/flatdb

dotnet_run: dotnet_build
	dotnet run

docker_build: dotnet_build
	sudo docker build -t streambolics/flatdb .
	sudo docker save streambolics/flatdb | gzip >bin/docker/flatdb.amd64.tgz
	sudo docker push streambolics/flatdb

dotnet_build:
	dotnet publish -o bin/amd64
	dotnet publish -o bin/arm32 -r linux-arm
	