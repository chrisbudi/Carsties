using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.Services;

namespace SearchService.Data
{
    public class DBInitializer
    {
        public static async Task InitializeAsync(WebApplication app)
        {
            await DB.InitAsync("SearchDb",
                MongoClientSettings.FromConnectionString(
                    app.Configuration.GetConnectionString("MongoDbConnection")));


            await DB.Index<Item>().
                Key(x => x.Make, KeyType.Text).
                Key(x => x.Model, KeyType.Text).
                Key(x => x.Color, KeyType.Text).CreateAsync();


            var count = await DB.CountAsync<Item>();

            //if (count == 0)
            //{
            //    Console.WriteLine("No data to seed");
            //    var itemData = await File.ReadAllTextAsync("Data/auction.json");

            //    var options = new JsonSerializerOptions
            //    {
            //        PropertyNameCaseInsensitive = true,
            //    };

            //    var items = JsonSerializer.Deserialize<List<Item>>(itemData, options);

            //    await DB.SaveAsync(items);
            //}


            using var scope = app.Services.CreateScope();
            var httpClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();

            //var items = await httpClient.GetItemForSearchDB();

            //Console.WriteLine(items.Count + " items return from auction service");

            //if (items.Count > 0)
            //{
            //    await DB.SaveAsync(items);
            //}
            //else
            //{
            //    Console.WriteLine("No data to seed");
            //}

        }
    }
}
