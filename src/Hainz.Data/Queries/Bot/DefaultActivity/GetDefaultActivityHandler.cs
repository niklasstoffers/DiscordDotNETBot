using AutoMapper;
using Hainz.Data.DTOs.Discord;
using Hainz.Data.DTOs;
using MediatR;
using Discord;

namespace Hainz.Data.Queries.Bot.DefaultActivity;

public class GetDefaultActivityHandler : IRequestHandler<GetDefaultActivityQuery, ActivityDTO>
{
    private readonly HainzDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetDefaultActivityHandler(HainzDbContext dbContext,
                                     IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public Task<ActivityDTO> Handle(GetDefaultActivityQuery request, CancellationToken cancellationToken)
    {
        var defaultActivityName = _dbContext.ApplicationSettings
            .SingleOrDefault(setting => setting.Name == ApplicationSettingName.DefaultActivityName);
        var defaultActivityType = _dbContext.ApplicationSettings
            .SingleOrDefault(setting => setting.Name == ApplicationSettingName.DefaultActivityType);

        if (defaultActivityName == null) throw new Exception("No application setting for default activity name found in database");
        if (defaultActivityType == null) throw new Exception("No application setting for default activity type found in database");

        var activityType = _mapper.Map<ActivityType>(defaultActivityType.Value);
        return Task.FromResult(new ActivityDTO(activityType, defaultActivityName.Value));
    }
}