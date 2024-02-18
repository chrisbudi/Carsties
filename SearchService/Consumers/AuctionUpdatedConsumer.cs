using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
    public class AuctionUpdatedConsumer(IMapper _mapper) : IConsumer<AuctionUpdated>
    {
        public async Task Consume(ConsumeContext<AuctionUpdated> context)
        {
            Console.WriteLine(" --> Consume auction updated : " + context.Message.Id);
            var item = _mapper.Map<Item>(context.Message);

            var result = await DB.Update<Item>()
                .Match(a => a.ID == context.Message.Id)
                .ModifyOnly(m => new
                {
                    m.Color,
                    m.Make,
                    m.Model,
                    m.Year
                }, item)
                .ExecuteAsync();

            if (!result.IsAcknowledged)
                throw new MessageException(typeof(AuctionUpdated), "problem updating item in search db");
        }
    }
}
