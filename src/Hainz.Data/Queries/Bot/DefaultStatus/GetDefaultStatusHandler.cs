using AutoMapper;
using MediatR;
using Discord;
using Hainz.Data.DTOs;

namespace Hainz.Data.Queries.Bot.DefaultStatus;

public class GetDefaultStatusHandler : IRequestHandler<GetDefaultStatusQuery, UserStatus>
{
    private readonly HainzDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetDefaultStatusHandler(HainzDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public Task<UserStatus> Handle(GetDefaultStatusQuery request, CancellationToken cancellationToken)
    {
        var defaultStatus = _dbContext.ApplicationSettings
            .SingleOrDefault(setting => setting.Name == ApplicationSettingName.DefaultStatus);

        if (defaultStatus == null) throw new Exception("No application setting for default status found");

        return Task.FromResult(_mapper.Map<UserStatus>(defaultStatus));
    }
}