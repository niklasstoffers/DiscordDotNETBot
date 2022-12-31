using MediatR;
using Hainz.Data.DTOs.Discord;

namespace Hainz.Data.Queries.Bot.DefaultActivity;

public record GetDefaultActivityQuery() : IRequest<ActivityDTO>;