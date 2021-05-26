using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Ipstset.AzShop.Application.EventHandling;
using Ipstset.AzShop.Domain.Products;
using Ipstset.AzShop.Infrastructure.Models;
using Ipstset.AzShop.Infrastructure.SqlDataAccess.Models;
using Newtonsoft.Json;

namespace Ipstset.AzShop.Infrastructure.SqlDataAccess
{
    public class ProductRepository : IProductRepository
    {
        private DbSettings _db;
        private IEventDispatcher _eventDispatcher;
        public ProductRepository(DbSettings settings, IEventDispatcher eventDispatcher)
        {
            _db = settings;
            _eventDispatcher = eventDispatcher;
        }

        public async Task<Product> GetAsync(Guid id)
        {
            Product product = null;
            var sql = $"exec {_db.Schema}.get_json @table,@id";
            using (var sqlConnection = new SqlConnection(_db.Connection))
            {
                var document = await sqlConnection.QueryFirstOrDefaultAsync<SqlDocument>(sql, new { table = SqlDataConstants.Tables.PRODUCT, id });

                if (document != null)
                {
                    var data = JsonConvert.DeserializeObject<ProductDocument>(document.Data);
                    if (data != null)
                    {
                        //parse data
                        product = Product.Load(
                            Guid.Parse(data.Id), 
                            Guid.Parse(data.ShopId),
                            data.Type,
                            data.ProductCode,
                            data.Copy,
                            data.IsActive,
                            data.Options,
                            data.Pricing);
                    }
                }
            }

            return product;
        }

        public async Task SaveAsync(Product product)
        {
            var sql = $"exec {_db.Schema}.save_json @table,@id,@data";

            var document = new ProductDocument
            {
                Id = product.Id.ToString(),
                ShopId = product.ShopId.ToString(),
                ProductCode = product.ProductCode,
                Type = product.Type,
                Copy = product.Copy,
                IsActive = product.IsActive,
                Options = product.Options,
                Pricing = product.Pricing
            };

            using (var sqlConnection = new SqlConnection(_db.Connection))
            {
                await sqlConnection.ExecuteAsync(sql, new { table = SqlDataConstants.Tables.PRODUCT, id = product.Id.ToString(), data = JsonHelper.Serialize(document) });
            }

            await _eventDispatcher.DispatchAsync(product.DequeueEvents().ToArray());
        }

        public Task DeleteAsync(Product shop)
        {
            throw new NotImplementedException();
        }
    }
}
