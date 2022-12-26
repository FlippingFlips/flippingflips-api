using MediatR;

namespace FF.Core.Features.Games.Notify
{
    public class GameCreatedNotification : INotification
    {
        public GameCreatedNotification(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }

    internal class GameCreatedNotificationHandler : INotificationHandler<GameCreatedNotification>
    {
        public async Task Handle(GameCreatedNotification notification, CancellationToken cancellationToken)
        {
            //todo: game created notifications
            await Task.CompletedTask;
        }
    }
}
