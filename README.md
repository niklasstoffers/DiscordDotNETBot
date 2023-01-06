<p align="center">
    <img src="./Hainz.png" alt"Hainz" height="200px">
</p>

<h3 align="center">Hainz</h3>
<br>

<p align="center">
    <a href="https://github.com/niklasstoffers/Hainz/blob/main/LICENSE"><img src="https://img.shields.io/github/license/niklasstoffers/Hainz?color=informational" alt="license"></a>
    <img src="https://img.shields.io/github/actions/workflow/status/niklasstoffers/Hainz/build.yml" alt="build status">
    <img src="https://img.shields.io/github/issues/niklasstoffers/Hainz" alt="open issues">
    <img src="https://img.shields.io/github/last-commit/niklasstoffers/Hainz" alt="last commit">
    <br>
    <img src="https://img.shields.io/github/languages/top/niklasstoffers/Hainz?color=blueviolet" alt="top language">
    <img src="https://img.shields.io/tokei/lines/github/niklasstoffers/Hainz" alt="lines of code">
    <a href="https://www.codefactor.io/repository/github/niklasstoffers/hainz"><img src="https://www.codefactor.io/repository/github/niklasstoffers/hainz/badge" alt="code quality" /></a>
    <hr>
</p>

**Hainz** is a Discord bot for server administration, music, entertainment and more. It's built on top of the [Discord.NET](https://github.com/discord-net/Discord.Net) framework. 

*This project is currently under development*

## Configuring the bot

In order for the bot to work you need to provide configuration for it. To do this open the *appsettings.json* file under *src/Hainz* and set the configuration options to your liking. The most important setting here is the `Bot.Token` option. You need to set this to your [Discord application bot token](https://discord.com/developers/applications).

### Configuration options

| Option      | Description |
| ----------- | ----------- |
| `Bot.Token` | Your applications bot token. This is required in order for the bot to work.       |
| `Commands.FallbackPrefix` | Fallback command prefix that's used if retrieving prefix from database fails. |
| `Persistence.Host`   | Hostname for PostgreSQL server.       |
| `Persistence.Port`   | Port for PostgreSQL server.        |
| `Persistence.Username`   | Login username for PostgreSQL server.        |
| `Persistence.Password`   | Login password for PostgreSQL server.        |
| `Persistence.Database`   | Name of the PostgreSQL database to use. Note that this database doesn't have to exist yet and is created automatically by the application if your PostgreSQL login role has the permissions to do so.        |
| `Caching` | Caching configuration options. You'll likely dont need to worry about these settings and should only change them if you know what you're doing. |
| `Caching.Redis` | Caching configuration options related to the Redis server. |
| `Caching.Redis.Hostname` | Hostname for Redis cache server. |
| `Caching.Redis.Port` | Port for Redis cache server. |

> **Note about configuration options:**
  Configuration options marked with `?` are optional and can be omitted if unnecessary.

> **Note about persistence and caching configuration:**  
  Most of the persistence configuration in the *appsettings.json* file is only relevant if you host the PostgreSQL DB server by yourself. The same applies to the Redis hostname and port settings. If you're using the Docker PostgreSQL and Redis services coming with the repository, the respective configuration will be overriden by environment variables defined in the *.env.docker* file.

## Self hosting

The recommended way to self-host this bot is to build and run the Docker image coming with the repository. However if you don't want to use the Docker image you can also build and run the project by yourself.

### Using Docker

Make sure that both [Docker](https://docs.docker.com/get-docker/) and [Docker Compose](https://docs.docker.com/compose/install/) are installed for this step.
To build and run the application, database and cache server Docker images you have to run these commands:

```
make build
make run
```

If you only want to build and run the application Docker image you have to run these commands:

```
make build-app
make run-app
```

### Building it by yourself

Make sure you have a PostgreSQL and Redis server running for this step. Open a terminal and run these commands:

```
dotnet build .
dotnet run --project src/Hainz/Hainz.csproj
```

## PostgreSQL server

The bot uses a PostgreSQL database to store data needed for its operation. This data includes information about Discord Guilds, Users, Channels, Configuration etc. We only store data that's crucial to the bots operation and thus try to keep it as minimal as possible. We do not store any personal information, message content, etc.

### Setting up the PostgreSQL server

If you're building and running the application using `make build && make run` you can skip this step. Otherwise you'll need to install and setup a local PostgreSQL server. Please refer to [this documentation](https://www.postgresql.org/docs/current/tutorial-install.html) on how to do so.

## Redis server

The bot uses a Redis server to cache database queries for faster response times. To configure caching options take a look at the `Caching` section in the *appsettings.json* file.

### Setting up the Redis server

Like with the PostgreSQL server, the repository comes with a Docker service for the Redis cache server. Thus if you use `make build && make run` to build and run the application you dont need to worry about setting up a local Redis instance. Otherwise please refer to [this documentation](https://redis.io/docs/getting-started/installation/) on how to install and setup Redis.