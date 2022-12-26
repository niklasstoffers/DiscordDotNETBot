using AutoMapper;
using Discord;
using Hainz.Mapping.TypeConverters;

namespace Hainz.Mapping.Profiles;

public sealed class ConfigurationMappingProfile : Profile
{
    public ConfigurationMappingProfile()
    {
        CreateMap<string, UserStatus?>().ConvertUsing<UserStatusTypeConverter>();
        CreateMap<string, ActivityType?>().ConvertUsing<ActivityTypeTypeConverter>();
    }
}