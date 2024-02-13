using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers
{
    public class AuctionController(AuctionDbContext context, IMapper mapper) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuctionDto>>> GetAuctions()
        {
            var auctions = await context.Auctions
                .Include(x => x.Item)
                .OrderBy(x => x.Item.Make)
                .ToListAsync();

            return Ok(mapper.Map<IEnumerable<AuctionDto>>(auctions));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
        {

            var auction = await context.Auctions
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (auction == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<AuctionDto>(auction));
        }

        [HttpPost]
        public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto createAuctionDto)
        {
            var auction = mapper.Map<Auction>(createAuctionDto);
            // TODO: add current user
            auction.Seller = "test";

            context.Auctions.Add(auction);
            var result = await context.SaveChangesAsync() > 0;
            if (!result) return BadRequest("Failed to create auction");
            return CreatedAtAction(nameof(GetAuctionById),
                new { auction.Id }, mapper.Map<AuctionDto>(auction));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
        {
            var auction = await context.Auctions.Include(m => m.Item).FirstOrDefaultAsync(x => x.Id == id);
            if (auction == null) return NotFound();

            // TODO: check seller is current user


            auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
            auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
            auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
            auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
            auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;

            var result = await context.SaveChangesAsync() > 0;
            if (!result) return BadRequest("Failed to update auction");
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuction(Guid id)
        {
            var auction = await context.Auctions.FirstOrDefaultAsync(x => x.Id == id);
            if (auction == null) return NotFound();
            context.Remove(auction);
            var result = await context.SaveChangesAsync() > 0;
            if (!result) return BadRequest("Failed to delete auction");
            return Ok();
        }

    }
}
