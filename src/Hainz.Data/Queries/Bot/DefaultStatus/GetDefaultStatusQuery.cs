using MediatR;
using Discord;

namespace Hainz.Data.Queries.Bot.DefaultStatus;

public record GetDefaultStatusQuery() : IRequest<UserStatus>;