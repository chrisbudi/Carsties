using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
    public class AuctionCreatedConsumer(IMapper _mapper) : IConsumer<AuctionCreated>
    {
        public async Task Consume(ConsumeContext<AuctionCreated> context)
        {
            Console.WriteLine(" Consume auction created : " + context.Message.Id);
            var item = _mapper.Map<Item>(context.Message);

            Console.WriteLine("Item created: " + item.ToString());

            if (item.Model == "Foo") throw new ArgumentException("Model is Foo");


            await item.SaveAsync();
        }
    }
}
