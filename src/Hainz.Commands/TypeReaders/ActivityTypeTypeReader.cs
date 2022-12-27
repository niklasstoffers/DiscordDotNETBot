using AutoMapper;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Hainz.Commands.TypeReaders;

public sealed class ActivityTypeTypeReader : TypeReaderBase
{
    public override sealed Type ForType => typeof(ActivityType);

    public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
    {
        var mapper = services.GetRequiredService<IMapper>();
        var activityType = mapper.Map<ActivityType?>(input);

        if (activityType == null)
            return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, "Parameter could not be resolved to an activity type"));
        
        return Task.FromResult(TypeReaderResult.FromSuccess(activityType));
    }
}