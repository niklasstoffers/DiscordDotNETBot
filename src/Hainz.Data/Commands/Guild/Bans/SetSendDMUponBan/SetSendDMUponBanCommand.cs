using MediatR;

namespace Hainz.Data.Commands.Guild.Bans.SetSendDMUponBan;

public record SetSendDMUponBanCommand(ulong GuildId, bool IsEnabled) : IRequest;