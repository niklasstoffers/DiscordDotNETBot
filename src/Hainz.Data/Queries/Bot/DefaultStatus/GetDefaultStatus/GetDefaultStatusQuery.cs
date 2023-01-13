using MediatR;
using Discord;

namespace Hainz.Data.Queries.Bot.DefaultStatus.GetDefaultStatus;

public record GetDefaultStatusQuery() : IRequest<UserStatus>;