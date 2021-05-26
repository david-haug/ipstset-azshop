using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Ipstset.AzShop.Application.EventHandling;
using Ipstset.AzShop.Domain.Shops;
using Ipstset.AzShop.Infrastructure.Models;
using Ipstset.AzShop.Infrastructure.SqlDataAccess.Models;
using Newtonsoft.Json;

namespace Ipstset.AzShop.Infrastructure.SqlDataAccess
{
    public class ShopRepository: IShopRepository
    {
        private DbSettings _db;
        private IEventDispatcher _eventDispatcher;
        public ShopRepository(DbSettings settings, IEventDispatcher eventDispatcher)
        {
            _db = settings;
            _eventDispatcher = eventDispatcher;
        }

        public async Task<Shop> GetAsync(Guid id)
        {
            Shop shop = null;
            var sql = $"exec {_db.Schema}.get_json @table,@id";
            using (var sqlConnection = new SqlConnection(_db.Connection))
            {
                var document = await sqlConnection.QueryFirstOrDefaultAsync<SqlDocument>(sql, new { table = SqlDataConstants.Tables.SHOP, id });

                if (document != null)
                {
                    var data = JsonConvert.DeserializeObject<ShopDocument>(document.Data);
                    if (data != null)
                    {
                        //parse data
                        shop = Shop.Load(Guid.Parse(data.Id), data.Name);
                    }
                }
            }

            return shop;
        }

        public async Task SaveAsync(Shop shop)
        {
            var sql = $"exec {_db.Schema}.save_json @table,@id,@data";

            var document = new ShopDocument
            {
                Id = shop.Id.ToString(),
                Name = shop.Name
            };

            using (var sqlConnection = new SqlConnection(_db.Connection))
            {
                await sqlConnection.ExecuteAsync(sql, new { table = SqlDataConstants.Tables.SHOP, id = shop.Id.ToString(), data = JsonHelper.Serialize(document) });
            }

            await _eventDispatcher.DispatchAsync(shop.DequeueEvents().ToArray());
        }

        public Task DeleteAsync(Shop shop)
        {
            throw new NotImplementedException();
        }
    }
}
