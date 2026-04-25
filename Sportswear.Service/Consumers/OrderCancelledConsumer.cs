using MassTransit;
using Serilog;
using Sportswear.Service.Abstract;
using Sportswear.Service.Messages;

namespace Sportswear.Service.Consumers
{
    public class OrderCancelledConsumer : IConsumer<OrderCancelledMessage>
    {
        private readonly IEmailsService _emailsService;

        public OrderCancelledConsumer(IEmailsService emailsService)
        {
            _emailsService = emailsService;
        }

        public async Task Consume(ConsumeContext<OrderCancelledMessage> context)
        {
            var message = context.Message;
            try
            {
                Log.Information("Processing OrderCancelled for Order #{OrderId}", message.OrderId);

                await _emailsService.SendEmailAsync(
                    message.CustomerEmail,
                    "Order Cancelled ❌",
                    $@"Dear {message.CustomerName},

                        Your order #{message.OrderId} has been cancelled.
                        Date: {message.CancelledAt:yyyy-MM-dd HH:mm}
                        
                        If you have any questions, please contact us.
                        ABOUTRIKA Team");

                Log.Information("Cancellation email sent for Order #{OrderId}", message.OrderId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to send cancellation email for Order #{OrderId}", message.OrderId);
                throw;
            }
        }
    }
}
