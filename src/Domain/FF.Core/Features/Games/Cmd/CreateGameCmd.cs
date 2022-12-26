using FF.Core.Features.Games.Notify;
using FF.Core.Features.Media.Cmd;
using FF.Core.Interface;
using FF.Core.Models.Dto.Games;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FF.Core.Features.Games.Cmd
{
    public class CreateGameCmd : IRequest<string>
    {
        public CreateGameCmd(CreateGameDto gameDto)
        {
            GameDto = gameDto;
        }

        public CreateGameDto GameDto { get; }
    }

    public class CreateGameCmdHandler : IRequestHandler<CreateGameCmd, string>
    {
        private readonly IRepository repository;
        private readonly IMediator mediator;
        private readonly IFileService fileService;
        private readonly ILogger<CreateGameCmdHandler> logger;

        public CreateGameCmdHandler(IRepository repository, IMediator mediator, IFileService fileService, ILogger<CreateGameCmdHandler> logger)
        {
            this.repository = repository;
            this.mediator = mediator;
            this.fileService = fileService;
            this.logger = logger;
        }

        public async Task<string> Handle(CreateGameCmd request, CancellationToken cancellationToken)
        {
            request.GameDto.Id = Guid.NewGuid().ToString();
            request.GameDto.Created = DateTime.Now;

            try
            {
                //save the TransLite (BackGlass) image for the game. TODO: Screenshot
                if(request.GameDto.TransliteImage?.Length > 0)
                {
                    var cmd = new SaveMediaCmd("translite.jpg", $"games\\{request.GameDto.Id}", null, request.GameDto.TransliteImage);
                    await mediator.Send(cmd);
                }

                //add and save game to database
                await repository.Games.AddAsync(request.GameDto);
                await repository.SaveChangesAsync(cancellationToken);

                //notify game created
                await mediator.Publish(new GameCreatedNotification(request.GameDto.Id));
                return request.GameDto.Id;
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}-{ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
