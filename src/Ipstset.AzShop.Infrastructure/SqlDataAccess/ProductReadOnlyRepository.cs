using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Ipstset.AzShop.Application;
using Ipstset.AzShop.Application.Products;
using Ipstset.AzShop.Application.Products.GetProducts;
using Ipstset.AzShop.Infrastructure.Extensions;
using Ipstset.AzShop.Infrastructure.Models;
using Ipstset.AzShop.Infrastructure.SqlData;

namespace Ipstset.AzShop.Infrastructure.SqlDataAccess
{
    public class ProductReadOnlyRepository : IProductReadOnlyRepository
    {
        private DbSettings _db;
        public ProductReadOnlyRepository(DbSettings settings)
        {
            _db = settings;
        }

        public async Task<ProductResponse> GetByIdAsync(string id)
        {
            ProductResponse response;
            var sql = $"exec {_db.Schema}.get_json @table,@id";
            using (var sqlConnection = new SqlConnection(_db.Connection))
            {
                var document = await sqlConnection.QueryFirstOrDefaultAsync<SqlDocument>(sql, new { table = SqlDataConstants.Tables.PRODUCT, id });
                response = document.ToProductResponse();
            }

            return response;
        }

        public async Task<QueryResult<ProductResponse>> GetProductsAsync(GetProductsRequest request)
        {
            //query process:
            //1. get all json from starting point
            //2. filter unauthorized records
            //3. apply remaining request filters
            //4. take # of records based on limit
            var products = new List<ProductResponse>();
            var sql = $"exec {_db.Schema}.get_json_all @table,@startAfter";
            using (var sqlConnection = new SqlConnection(_db.Connection))
            {
                var documents = await sqlConnection.QueryAsync<SqlDocument>(sql, new { table = SqlDataConstants.Tables.PRODUCT, startAfter = request.StartAfter });
                foreach (var document in documents)
                {
                    var response = document.ToProductResponse();
                    if (response != null)
                        products.Add(response);
                }
            }

            //filter
            if (!string.IsNullOrWhiteSpace(request.ShopId))
                products = products.Where(p => p.ShopId == request.ShopId).ToList();
            if (!string.IsNullOrWhiteSpace(request.Type))
                products = products.Where(p => p.Type == request.Type).ToList();
            if (request.IsActive.HasValue)
                products = products.Where(p => p.IsActive == request.IsActive).ToList();

            //sort
            var sorter = new Sorter<ProductResponse>();
            products = sorter.Sort(products, request.Sort?.ToArray()).ToList();

            return new QueryResult<ProductResponse> { Items = products.Take(request.Limit), TotalRecords = products.Count, Limit = request.Limit, StartAfter = request.StartAfter };
        }

        public async Task<ProductResponse> GetByProductCodeAsync(string productCode, string shopId)
        {
            var sql = $"exec {_db.Schema}.get_json_all @table,@startAfter";
            using (var sqlConnection = new SqlConnection(_db.Connection))
            {
                var documents = await sqlConnection.QueryAsync<SqlDocument>(sql, new { table = SqlDataConstants.Tables.PRODUCT, startAfter = 0});
                foreach (var document in documents)
                {
                    var response = document.ToProductResponse();
                    if (response != null && 
                        response.ShopId == shopId &&
                        response.ProductCode == productCode)
                        return response;
                }
            }

            return null;
        }
    }
}
