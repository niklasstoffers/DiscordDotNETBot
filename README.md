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
    <img src="https://www.aschey.tech/tokei/github/niklasstoffers/Hainz" alt="lines of code">
    <a href="https://www.codefactor.io/repository/github/niklasstoffers/hainz"><img src="https://www.codefactor.io/repository/github/niklasstoffers/hainz/badge" alt="code quality" /></a>
    <hr>
</p>

**Hainz** is a Discord bot for server administration, music, entertainment and more. It's built on top of the [Discord.NET](https://github.com/discord-net/Discord.Net) framework. 

*This project is currently still under development*

## Configuring the bot

In order for the bot to work you need to provide proper configuration for it. To do this open the *appsettings.json* file under *src/Hainz* and set the configuration options to your liking. The most important option here is the `Bot.Token` option. You need to set this to your [Discord application bot token](https://discord.com/developers/applications).

### Configuration options

| Option      | Description |
| ----------- | ----------- |
| `Bot.Token` | Your applications bot token. This is required in order for the bot to work.       |
| `Bot.DefaultStatus?`   | The bots default status. Can be set to `online`, `offline`, `afk`, `idle`, `donotdisturb` and `invisible`.        |
| `Bot.DefaultActivity?`   | The bots default activity.        |
| `Bot.DefaultActivity?.Name`   | The default activity name.        |
| `Bot.DefaultActivity?.Type`   | The default activity type. Can be set to `playing`, `listening`, `streaming`, `watching` and `competing`.        |
| `Server.BotAdminRole?`   | Required role to invoke bot administration commands. If not set, nobody will be able to execute administration commands.        |
| `Server.Channels.LogChannel?`   | Configuration options for the servers log channel. If enabled the bot will post it's internal log messages here. Useful if you quickly want to find out if something went wrong (especially if the bot is hosted somewhere on a server).        |
| `Server.Channels.LogChannel?.ChannelId`   | The log channels discord channel id.       |
| `Server.Channels.LogChannel?.IsEnabled`   | Whether the logging channel feature is enabled or not. Note that per default the discord channel log target is only used if the application is run in the `Production` environment.       |

> **Note:** Options marked with `?` are optional and may be ommitted if unneeded.

### Note about discord channel logging

Although posting internal logs directly to a discord channel has it's usefulness in that you can quickly determine if something's wrong, it should be used with some caution. First and foremost especially on active servers log messages posted to a Discord channel may contribute to exceeding the Discord gateways rate limiting for your application. Furthermore logging to a discord channel will only work during the applications gateway uptime. Log events that take place during the gateways downtime will be added to a queue by default so they can be posted once the gateway is up again. 

## Self hosting

The recommended way to self-host this bot is to build and run the Docker image coming with the repository. However if you don't want to use the Docker image you can also build and run the project by yourself.

### Using Docker

Make sure that [Docker is installed and running](https://docs.docker.com/get-docker/) for this step.
To build and run the Docker image you have to run these commands:

```
make build
make run
```

### Building it by yourself

Open a terminal and run these commands:

```
dotnet build .
dotnet run --project src/Hainz/Hainz.csproj
```