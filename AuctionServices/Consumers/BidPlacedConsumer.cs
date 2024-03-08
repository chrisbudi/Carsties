﻿using AuctionService.Data;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers
{
    public class BidPlacedConsumer : IConsumer<BidPlaced>
    {
        readonly AuctionDbContext _dbContext;
        public BidPlacedConsumer(AuctionDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task Consume(ConsumeContext<BidPlaced> context)
        {
            Console.WriteLine("--> consuming bid placed");
            var auction = await _dbContext.Auctions.FindAsync(context.Message.Id);

            if (auction.CurrentHighBid == null ||
                context.Message.BidStatus.Contains("Accepted") &&
                context.Message.Amount > auction.CurrentHighBid)
            {
                auction.CurrentHighBid = context.Message.Amount;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}