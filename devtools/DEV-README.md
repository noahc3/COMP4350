## Building

### Run local PostgresDB

1. Windows only: [Setup WSL](https://learn.microsoft.com/en-us/windows/wsl/install) (default install of Ubuntu is fine).
2. [Download](https://www.docker.com/products/docker-desktop/) and install Docker Desktop.
    - Windows only: make sure "Use WSL" is checked during install.
3. Open Docker Desktop.
4. Open command prompt or terminal in `devtools/postgres-docker` folder.
5. Run the command 'docker-compose up -d`.
    - PostgresDB docker container will install and start automatically from now on. You can view it's status in Docker Desktop. 

### threadit-api

1. [Download](https://dotnet.microsoft.com/en-us/download) and install the .NET 7.0 SDK.
2. Open a terminal in the `threadit-api` folder.
3. Run `dotnet restore` to install NuGet packages.
4. Run `dotnet run` to launch the API server.
    - By default, threadit-api will try to connect to the local PostgresDB docker container setup above. You can configure the target database in `threadit-api/Properties/launchSettings.json`.

### threadit-web

1. [Download](https://nodejs.org/en/) and install NodeJS 18.
2. Open a terminal in the `threadit-web` folder.
3. Run `npm install -g yarn` to install the yarn package manager.
4. Run `yarn install` to install NPM packages.
5. Run `yarn start` to launch the frontend web server.