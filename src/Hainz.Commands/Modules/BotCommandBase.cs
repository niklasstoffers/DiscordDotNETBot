using Discord.Commands;
using Hainz.Commands.Metadata;
using Hainz.Commands.Preconditions;

namespace Hainz.Commands.Modules;

[RequireOwner(Group = "Permission")]
[RequireBotAdminPermission(Group = "Permission")]
[CommandSection("bot")]
public class BotCommandBase : ModuleBase<SocketCommandContext> { }