using AutoMapper;
using Discord;
using Hainz.Infrastructure.Mapping.TypeConverters;

namespace Hainz.Infrastructure.Mapping.Profiles;

public sealed class ConfigurationMappingProfile : Profile
{
    public ConfigurationMappingProfile()
    {
        CreateMap<string, UserStatus?>().ConvertUsing<UserStatusTypeConverter>();
        CreateMap<string, ActivityType?>().ConvertUsing<ActivityTypeTypeConverter>();
    }
}