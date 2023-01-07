using Discord.Commands;
using Hainz.Commands.Metadata;

namespace Hainz.Commands.Modules;

[RequireOwner]
[CommandSection("bot", "bot administration related commands")]
public class BotCommandBase : SocketCommandModuleBase { }