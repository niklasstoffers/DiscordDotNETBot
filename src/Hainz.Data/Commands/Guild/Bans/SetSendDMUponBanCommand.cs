using MediatR;

namespace Hainz.Data.Commands.Guild.Bans;

public record SetSendDMUponBanCommand(ulong GuildId, bool IsEnabled) : IRequest;