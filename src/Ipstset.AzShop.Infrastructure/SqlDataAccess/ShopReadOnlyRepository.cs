using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Ipstset.AzShop.Application.Shops;
using Ipstset.AzShop.Infrastructure.Extensions;
using Ipstset.AzShop.Infrastructure.Models;

namespace Ipstset.AzShop.Infrastructure.SqlDataAccess
{
    public class ShopReadOnlyRepository : IShopReadOnlyRepository
    {
        private DbSettings _db;
        public ShopReadOnlyRepository(DbSettings settings)
        {
            _db = settings;
        }

        public async Task<ShopResponse> GetByIdAsync(string id)
        {
            ShopResponse response;
            var sql = $"exec {_db.Schema}.get_json @table,@id";
            using (var sqlConnection = new SqlConnection(_db.Connection))
            {
                var document = await sqlConnection.QueryFirstOrDefaultAsync<SqlDocument>(sql, new { table = SqlDataConstants.Tables.SHOP, id });
                response = document.ToShopResponse();
            }

            return response;
        }
    }
}
