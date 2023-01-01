using Discord.Commands;
using Hainz.Commands.Metadata;

namespace Hainz.Commands.Modules;

[RequireOwner]
[CommandSection("bot")]
public class BotCommandBase : ModuleBase<SocketCommandContext> { }