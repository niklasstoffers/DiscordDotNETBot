using Hainz.Data.DTOs.Discord;
using MediatR;

namespace Hainz.Data.Queries.Channels.LogChannels.GetLogChannels;

public record GetLogChannelsQuery() : IRequest<IEnumerable<ChannelDTO>>;