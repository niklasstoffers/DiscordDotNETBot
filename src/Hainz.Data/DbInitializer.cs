using AutoMapper;
using Hainz.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hainz.Data;

public class DbInitializer
{
    private readonly IMapper _mapper;

    public DbInitializer(IMapper mapper)
    {

    }

    public void Seed(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<ApplicationSetting>().HasData(
        //     new ApplicationSetting() { Id = 1, Name = "DefaultStatus", Value = "online" }
        // );
    }
}