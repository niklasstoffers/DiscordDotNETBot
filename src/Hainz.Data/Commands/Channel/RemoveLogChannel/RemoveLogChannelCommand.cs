using MediatR;

namespace Hainz.Data.Commands.Channel.RemoveLogChannel;

public record RemoveLogChannelCommand(ulong ChannelId) : IRequest<RemoveLogChannelResult>;