using MediatR;

namespace FF.Core.Features.GamesPlayed.Notify
{
    public class GamePlayedNotify
    {
        public GamePlayedNotify(string gameId, string title, string userName)
        {
            GameId = gameId;
            Title = title;
            UserName = userName;
        }

        public string GameId { get; }
        public string Title { get; }
        public string UserName { get; }
    }

    public class GamePlayedNotification : INotification
    {
        public GamePlayedNotification(GamePlayedNotify gamePlayed)
        {
            GamePlayed = gamePlayed;
        }

        public GamePlayedNotify GamePlayed { get; }
    }
}
