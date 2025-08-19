using System.Threading.Tasks;
using MassTransit;
using Contracts;

namespace References;

public class UsdEuroEventConsumer : IConsumer<UsdEuroEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public UsdEuroEventConsumer(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task Consume(ConsumeContext<UsdEuroEvent> context)
    {
        //TODO:Code
        _publishEndpoint.Publish(new ResponseUsdEuroEvent
            { StartDate = context.Message.StartDate, EndDate = context.Message.EndDate });
        return Task.CompletedTask;
    }
}