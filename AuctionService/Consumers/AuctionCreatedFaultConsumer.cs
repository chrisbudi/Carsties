using Contracts;
using MassTransit;

namespace AuctionService.Consumers
{
    public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
    {
        public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
        {

            Console.WriteLine(" --> consume fault auction created : " + context.Message.Message.Id);
            var exception = context.Message.Exceptions.First();

            if (exception.ExceptionType == "System.ArgumentException")
            {
                context.Message.Message.Model = "FooBar";
                Console.WriteLine(exception.Message, "Exception Message");
                await context.Publish(context.Message.Message);
            }
            else
            {
                Console.WriteLine("Unknown Exception");
            }
        }
    }
}
