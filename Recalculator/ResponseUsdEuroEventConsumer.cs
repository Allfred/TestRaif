using Contracts;
using MassTransit;

namespace Recalculator;

public class ResponseUsdEuroEventConsumer : IConsumer<ResponseUsdEuroEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public ResponseUsdEuroEventConsumer(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task Consume(ConsumeContext<ResponseUsdEuroEvent> context)
    {
        //TODO:
        return Task.CompletedTask;
    }
}