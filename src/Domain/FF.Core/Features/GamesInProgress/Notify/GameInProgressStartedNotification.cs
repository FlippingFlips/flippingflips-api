using MediatR;

namespace FF.Core.Features.GamesInProgress.Notify
{
    public class GameInProgressStartedNotification : INotification
    {
        public GameInProgressStartedNotification(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }

    internal class GameInProgressStartedNotificationHandler : INotificationHandler<GameInProgressStartedNotification>
    {
        public Task Handle(GameInProgressStartedNotification notification, CancellationToken cancellationToken)
        {
            //TODO: notify game started
            return Task.CompletedTask;
        }
    }
}
