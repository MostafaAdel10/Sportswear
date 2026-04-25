using MassTransit;
using Serilog;
using Sportswear.Service.Abstract;
using Sportswear.Service.Messages;

namespace Sportswear.Service.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedMessage>
    {
        private readonly IEmailsService _emailsService;

        public OrderCreatedConsumer(IEmailsService emailsService)
        {
            _emailsService = emailsService;
        }

        public async Task Consume(ConsumeContext<OrderCreatedMessage> context)
        {
            var message = context.Message;
            try
            {
                Log.Information("Processing OrderCreated for Order #{OrderId}", message.OrderId);

                await _emailsService.SendEmailAsync(
                    message.CustomerEmail,
                    "Order Confirmed ✅",
                    $@"Dear {message.CustomerName},
                    
                    Your order #{message.OrderId} has been placed successfully!
                    Total Amount: {message.TotalAmount:C}
                    Date: {message.CreatedAt:yyyy-MM-dd HH:mm}
                    
                    Thank you for shopping with ABOUTRIKA!");

                Log.Information("Confirmation email sent for Order #{OrderId}", message.OrderId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to send confirmation email for Order #{OrderId}", message.OrderId);
                throw; // MassTransit هيعمل retry
            }
        }
    }
}
