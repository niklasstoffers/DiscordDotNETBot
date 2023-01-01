using MediatR;

namespace Hainz.Data.Commands.Channel.AddLogChannel;

public record AddLogChannelCommand(ulong ChannelId) : IRequest<AddLogChannelResult>;