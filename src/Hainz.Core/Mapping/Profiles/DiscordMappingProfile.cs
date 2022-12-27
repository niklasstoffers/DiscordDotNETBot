using AutoMapper;
using Discord;
using Hainz.Core.Mapping.TypeConverters;

namespace Hainz.Core.Mapping.Profiles;

public sealed class ConfigurationMappingProfile : Profile
{
    public ConfigurationMappingProfile()
    {
        CreateMap<string, UserStatus?>().ConvertUsing<UserStatusTypeConverter>();
        CreateMap<string, ActivityType?>().ConvertUsing<ActivityTypeTypeConverter>();
    }
}