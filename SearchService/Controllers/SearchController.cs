using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] SearchParams search)
        {
            var query = DB.PagedSearch<Item, Item>();
            query.Sort(x => x.Ascending(a => a.Make));

            if (!string.IsNullOrEmpty(search.SearchTerm))
            {
                query.Match(Search.Full, search.SearchTerm).SortByTextScore();
            }
            // paging
            query = search.OrderBy switch
            {
                "make" => query.Sort(x => x.Ascending(a => a.Make)),
                "new" => query.Sort(x => x.Descending(a => a.CreatedAt)),
                _ => query.Sort(x => x.Ascending(a => a.AuctionEnd))
            };

            // filter
            query = search.FilterBy switch
            {
                "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
                "endingSoon" => query.Match(x =>
                x.AuctionEnd < DateTime.UtcNow.AddHours(6) &&
                x.AuctionEnd > DateTime.UtcNow),
                _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow)
            };

            if (!string.IsNullOrEmpty(search.Seller))
            {
                query.Match(x => x.Seller == search.Seller);
            }

            if (!string.IsNullOrEmpty(search.Winner))
            {
                query.Match(x => x.Winner == search.Winner);
            }

            query.PageNumber(search.PageNumber).PageSize(search.PageSize);

            var result = await query.ExecuteAsync();
            return Ok(new
            {
                results = result.Results,
                pageCount = result.PageCount,
                totalCount = result.TotalCount
            });

        }



    }
}
