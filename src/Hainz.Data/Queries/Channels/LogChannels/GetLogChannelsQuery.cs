using Hainz.Data.DTOs.Discord;
using MediatR;

namespace Hainz.Data.Queries.Channels.LogChannels;

public record GetLogChannelsQuery() : IRequest<IEnumerable<ChannelDTO>>;