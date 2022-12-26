using FF.Core.Interface;
using FF.Core.Models;
using FF.Domain.Models;
using MediatR;

namespace FF.Core.Features.Players.Cmd
{
    public class CreatePlayerCmd : IRequest<int>
    {
        public CreatePlayerCmd(string userId, CreatePlayerOption playerOption)
        {
            UserId = userId;
            PlayerOption = playerOption;
        }

        public string UserId { get; }
        public CreatePlayerOption PlayerOption { get; }
    }

    public class CreatePlayerCmdHandler : IRequestHandler<CreatePlayerCmd, int>
    {
        private readonly IRepository repository;

        public CreatePlayerCmdHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<int> Handle(CreatePlayerCmd request, CancellationToken cancellationToken)
        {
            var p = new Player()
            {
                Initials = request.PlayerOption.Initials,
                Created = DateTime.Now,
                ApplicationUserId = request.UserId,
                Name = request.PlayerOption.Name,
                MachineDefault = request.PlayerOption.IsDefault
            };

            await repository.Players.AddAsync(p);
            await repository.SaveChangesAsync(cancellationToken);

            return p.Id;
        }
    }
}
