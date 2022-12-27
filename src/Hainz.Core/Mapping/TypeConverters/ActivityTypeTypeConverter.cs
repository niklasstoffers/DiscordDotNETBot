using AutoMapper;
using Discord;
using Hainz.Common.Collections;

namespace Hainz.Core.Mapping.TypeConverters;

public sealed class ActivityTypeTypeConverter : ITypeConverter<string, ActivityType?>, ITypeConverter<ActivityType, string?>
{
    private static readonly BidirectionalMap<string, ActivityType> _map = new()
    {
        { "playing", ActivityType.Playing },
        { "listening", ActivityType.Listening },
        { "streaming", ActivityType.Streaming },
        { "watching", ActivityType.Watching },
        { "competing", ActivityType.Competing },
    };

    public ActivityType? Convert(string source, ActivityType? destination, ResolutionContext context)
    {
        source = source.ToLower();

        if (_map.Forward.ContainsKey(source))
            return _map.Forward[source];
        
        return null;
    }

    public string? Convert(ActivityType source, string? destination, ResolutionContext context)
    {
        if (_map.Reverse.ContainsKey(source))
            return _map.Reverse[source];
        
        return null;
    }
}