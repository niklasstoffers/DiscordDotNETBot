using Discord.Commands;
using Hainz.Commands.Preconditions;

namespace Hainz.Commands.Modules;

[RequireOwner(Group = "Permission")]
[RequireBotAdminPermission(Group = "Permission")]
public class BotCommandBase : ModuleBase<SocketCommandContext> { }