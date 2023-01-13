using MediatR;

namespace Hainz.Data.Commands.Channel.RemoveLogChannels;

public record RemoveLogChannelsCommand() : IRequest<int>;