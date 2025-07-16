using Application.Common.Services;
using MassTransit;

namespace Infrastructure.Services;

public class EventPublisher(IPublishEndpoint publishEndpoint) : IEventPublisher
{
    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class
    {
        await publishEndpoint.Publish(@event, cancellationToken);
    }
}
