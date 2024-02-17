using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services
{
    public class AuctionSvcHttpClient(HttpClient client, IConfiguration config)
    {
        public async Task<List<Item>> GetItemForSearchDB()
        {
            var lastUpdate = await DB.Find<Item, string>()
                .Sort(x => x.Descending(m => m.UpdatedAt))
                .Project(x => x.UpdatedAt.ToString())
                .ExecuteFirstAsync();

            Console.WriteLine(config["AuctionServiceUrl"], "auction service");

            return await client.GetFromJsonAsync<List<Item>>($"{config["AuctionServiceUrl"]}/api/auctions?" +
                $"lastUpdate={lastUpdate}");
        }

    }
}
