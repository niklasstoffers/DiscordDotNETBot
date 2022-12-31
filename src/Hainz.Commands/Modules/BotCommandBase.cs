using Discord.Commands;
using Hainz.Commands.Metadata;
using Hainz.Commands.Preconditions;

namespace Hainz.Commands.Modules;

[RequireOwner]
[CommandSection("bot")]
public class BotCommandBase : ModuleBase<SocketCommandContext> { }