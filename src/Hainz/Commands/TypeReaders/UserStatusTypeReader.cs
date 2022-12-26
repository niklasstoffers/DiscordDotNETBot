using AutoMapper;
using Discord;
using Discord.Commands;

namespace Hainz.Commands.TypeReaders;

public sealed class UserStatusTypeReader : TypeReader
{
    private readonly IMapper _mapper;

    public UserStatusTypeReader(IMapper mapper)
    {
        _mapper = mapper;
    }

    public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
    {
        var userStatus = _mapper.Map<UserStatus?>(input);

        if (userStatus == null)
            return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, "Input could not be parsed as a user status"));
            
        return Task.FromResult(TypeReaderResult.FromSuccess(userStatus));
    }
}