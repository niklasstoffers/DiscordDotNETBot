using AutoMapper;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Hainz.Commands.TypeReaders;

public sealed class UserStatusTypeReader : TypeReaderBase
{
    public sealed override Type ForType => typeof(UserStatus);

    public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
    {
        var mapper = services.GetRequiredService<IMapper>();
        var userStatus = mapper.Map<UserStatus?>(input);

        if (userStatus == null)
            return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, "Input could not be parsed as a user status"));
            
        return Task.FromResult(TypeReaderResult.FromSuccess(userStatus));
    }
}