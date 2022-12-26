using FF.Core.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FF.Core.Features.Users.Cmd
{
    /// <summary>
    /// Creates and returns a 10 char ApiKey for the user
    /// </summary>
    public class GenerateUserApiKeyCmd : IRequest<string>
    {
        public GenerateUserApiKeyCmd(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
    }

    internal class GenerateUserApiKeyCmdHandler : IRequestHandler<GenerateUserApiKeyCmd, string>
    {
        private readonly IRepository repository;

        public GenerateUserApiKeyCmdHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<string> Handle(GenerateUserApiKeyCmd request, CancellationToken cancellationToken)
        {
            var id = request.UserId;
            if (id == null)
                throw new NullReferenceException("No user id supplied with request");

            var machine = await repository.Users
                    .FirstOrDefaultAsync(x => x.Id == id);

            if (machine == null)
                throw new NullReferenceException("No user found.");

            if (id != machine.Id)
                throw new Exception("Only users can generate keys for themselves");

            //create key
            var apiKey = Guid.NewGuid().ToString("N").Substring(0, 10);
            machine.ApiKey = apiKey;

            //save and return it
            repository.Update(machine);
            await repository.SaveChangesAsync(cancellationToken);
            return apiKey;
        }
    }
}
