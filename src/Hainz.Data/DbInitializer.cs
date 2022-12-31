using AutoMapper;
using Discord;
using Hainz.Data.Entities;
using static Hainz.Data.DTOs.ApplicationSettingName;
using Microsoft.EntityFrameworkCore;

namespace Hainz.Data;

public class DbInitializer
{
    private readonly IMapper _mapper;

    public DbInitializer(IMapper mapper)
    {
        _mapper = mapper;
    }

    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationSetting>().HasData(
            new ApplicationSetting() { Id = 1, Name = DefaultActivityType, Value = _mapper.Map<string>(ActivityType.Playing) },
            new ApplicationSetting() { Id = 2, Name = DefaultActivityName, Value = "Development" },
            new ApplicationSetting() { Id = 3, Name = DefaultStatus, Value = _mapper.Map<string>(UserStatus.AFK) }
        );
    }
}