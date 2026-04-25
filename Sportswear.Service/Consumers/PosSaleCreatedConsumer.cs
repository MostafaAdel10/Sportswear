using MassTransit;
using Serilog;
using Sportswear.Service.Messages;

namespace Sportswear.Service.Consumers
{
    public class PosSaleCreatedConsumer : IConsumer<PosSaleCreatedMessage>
    {
        public Task Consume(ConsumeContext<PosSaleCreatedMessage> context)
        {
            var message = context.Message;

            Log.Information(
                "POS Sale created: #{SaleNumber} | Amount: {Amount} | By: {CreatedBy}",
                message.SaleNumber,
                message.FinalAmount,
                message.CreatedBy);

            return Task.CompletedTask;
        }
    }
}
