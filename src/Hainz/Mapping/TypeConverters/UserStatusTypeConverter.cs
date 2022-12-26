using AutoMapper;
using Discord;
using Hainz.Common.Collections;

namespace Hainz.Mapping.TypeConverters;

public sealed class UserStatusTypeConverter : ITypeConverter<string, UserStatus?>, ITypeConverter<UserStatus, string?>
{
    private static readonly BidirectionalMap<string, UserStatus> _map = new() 
    {
        { "online", UserStatus.Online },
        { "offline", UserStatus.Offline },
        { "afk", UserStatus.AFK },
        { "idle", UserStatus.Idle },
        { "donotdisturb", UserStatus.DoNotDisturb },
        { "invisible", UserStatus.Invisible }
    };

    public UserStatus? Convert(string source, UserStatus? destination, ResolutionContext context)
    {
        source = source.ToLower();

        if (_map.Forward.ContainsKey(source))
            return _map.Forward[source];
            
        return null;
    }

    public string? Convert(UserStatus source, string? destination, ResolutionContext context)
    {
        if (_map.Reverse.ContainsKey(source))
            return _map.Reverse[source];
        
        return null;
    }
}