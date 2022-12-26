using FF.Core.Interface;
using FF.Shared.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FF.Core.Features.Machines.Query
{
    /// <summary>
    /// Creates a user machine based on their user and players
    /// </summary>
    public class GetUserMachinesQuery : IRequest<UserMachinesResult>
    {
        public GetUserMachinesQuery(UserMachinesQueryDto queryDto)
        {
            QueryDto = queryDto;
        }

        public UserMachinesQueryDto QueryDto { get; }
    }

    public class GetUserMachinesQueryHandler : IRequestHandler<GetUserMachinesQuery, UserMachinesResult>
    {
        private readonly IRepository repository;

        public GetUserMachinesQueryHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<UserMachinesResult> Handle(GetUserMachinesQuery request, CancellationToken cancellationToken)
        {
            var result = new UserMachinesResult();

            IQueryable<UserMachine> query = repository.Users
                .AsNoTracking()
                .Include(p => p.Players)
                .Select(u => new UserMachine
                {
                    Birthday = u.MachBirthday,
                    Country = u.Country,
                    MachineDescription = u.MachDesc,
                    Username = u.UserName,
                    Created = u.Created,
                    MachineName = u.MachName,
                    PlayerCnt = u.Players.Count,
                    Players = request.QueryDto.IncludePlayers ?
                        u.Players.Select(p => new Player
                        {
                            Created = p.Created,
                            Id = p.Id,
                            Initials = p.Initials,
                            MachineDefault = p.MachineDefault,
                            Name = p.Name
                        }) : null
                });

            if (request.QueryDto.UserId != null)
            {
                //should just return a single user, so include ApiKey. No other user should have access to this
                query = repository.Users
                .AsNoTracking()
                .Include(p => p.Players)
                .Select(u => new UserMachine
                {
                    ApiKey = u.ApiKey,
                    Birthday = u.MachBirthday,
                    Country = u.Country,
                    MachineDescription = u.MachDesc,
                    Username = u.UserName,
                    Created = u.Created,
                    MachineName = u.MachName,
                    PlayerCnt = u.Players.Count,
                    UserId = u.Id,
                    Players = request.QueryDto.IncludePlayers ?
                        u.Players.Select(p => new Player
                        {
                            Created = p.Created,
                            Id = p.Id,
                            Initials = p.Initials,
                            MachineDefault = p.MachineDefault,
                            Name = p.Name
                        }) : null
                }).Where(x=>x.UserId == request.QueryDto.UserId);

            }
            else if (request.QueryDto.UserName != null)
            {
                query = query.Where(x => x.Username.ToLower().Contains(request.QueryDto.UserName.ToLower()));
            }

            result.Results = await query.CountAsync();
            result.UserMachines = await query.Take(request.QueryDto.Limit).ToListAsync();

            return result;
        }
    }
}
