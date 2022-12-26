using FF.Core.Features.GamesInProgress.Notify;
using FF.Core.Interface;
using FF.Core.Models;
using FF.Domain.Exceptions;
using FF.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FF.Core.Features.GamesInProgress.Cmd
{
    /// <summary>
    /// Starts a new game in progress, returns gameInProgress Id (string)
    /// </summary>
    public class StartNewGameCmd : IRequest<string>
    {
        public StartNewGameCmd(string userId, StartGameOption startGameOption)
        {
            UserId = userId;
            StartGameOption = startGameOption;
        }

        public string UserId { get; }
        public StartGameOption StartGameOption { get; }
    }

    internal class StartNewGameCmdHandler : IRequestHandler<StartNewGameCmd, string>
    {
        private readonly IRepository repository;
        private readonly IMediator mediator;

        public StartNewGameCmdHandler(IRepository repository, IMediator mediator)
        {
            this.repository = repository;
            this.mediator = mediator;
        }

        public async Task<string> Handle(StartNewGameCmd request, CancellationToken cancellationToken)
        {
            var gameOption = request.StartGameOption;
            var userId = request.UserId;

            //check the game Id and player one
            if (!await repository.Games.AnyAsync(x => x.Id == gameOption.GameId)) throw new NoGameFoundException();
            if (gameOption.Player1Id <= 0) throw new PlayerException("Player one isn't valid");

            //check the user has this player id on their machine
            var usersPlayerIds = repository.Players.AsNoTracking().Where(x => x.ApplicationUserId == userId).Select(x=>x.Id);
            if (!usersPlayerIds.Any(x => x == gameOption.Player1Id)) throw new PlayerException("No player found for this machine under that id");

            //remove any games already in progress for player
            var userGamesInProgress = repository.GamesInProgress.Where(x => x.ApplicationUserId == userId);
            if (userGamesInProgress.Count()>0)
                repository.GamesInProgress.RemoveRange(userGamesInProgress);

            //set up new game model, setting valid ids for each player. TODO: Add more players, some games can have more than 4 players
            var gip = new GameInProgress();
            gip.ApplicationUserId = userId;
            gip.Player1Id = gameOption.Player1Id;
            gip.Player2Id = gameOption.Player2Id.HasValue && gameOption.Player2Id.Value > 0 ? gameOption.Player2Id.Value : null;
            gip.Player3Id = gameOption.Player3Id.HasValue && gameOption.Player3Id.Value > 0 ? gameOption.Player3Id.Value : null;
            gip.Player4Id = gameOption.Player4Id.HasValue && gameOption.Player4Id.Value > 0 ? gameOption.Player4Id.Value : null;

            //run checks on other player ids to stop posting player ids on machines that don't have the players
            bool playerCheckError = false;
            if (gip.Player2Id != null)
            {
                if (!usersPlayerIds.Any(x => x == gip.Player2Id))
                    playerCheckError = true;

                if (gip.Player3Id != null)
                {
                    if (!usersPlayerIds.Any(x => x == gip.Player3Id))
                        playerCheckError = true;

                    if (gip.Player4Id != null)
                    {
                        if (!usersPlayerIds.Any(x => x == gip.Player4Id))
                            playerCheckError = true;
                    }
                }
            }
            if (playerCheckError) throw new PlayerException("One of the players is an invalid id for that machine");

            //checks completed add game started
            gip.Id = Guid.NewGuid().ToString();
            gip.GameId = gameOption.GameId;
            gip.Desktop = gameOption.Desktop;
            gip.BallsPerGame = gameOption.BallsPerGame;
            gip.Created = DateTime.Now;            
            gip.SystemVersion = gameOption.SystemVersion;

            await repository.GamesInProgress.AddAsync(gip);
            await repository.SaveChangesAsync(cancellationToken);

            //publish notification
            var notify = new GameInProgressStartedNotification(gip.Id);
            await mediator.Publish(notify, cancellationToken);

            return gip.Id;
        }
    }
}
