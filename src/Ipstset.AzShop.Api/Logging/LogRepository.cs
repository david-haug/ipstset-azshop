using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Ipstset.AzShop.Infrastructure.SqlDataAccess;
using Newtonsoft.Json;

namespace Ipstset.AzShop.Api.Logging
{
    public class LogRepository : ILogRepository
    {
        private DbSettings _db;

        public LogRepository(DbSettings settings)
        {
            _db = settings;
        }

        public async Task Save(RequestLog requestLog)
        {
            var sql = $"exec {_db.Schema}.request_log_insert @logDate,@parameters,@route";
            await using var sqlConnection = new SqlConnection(_db.Connection);
            await sqlConnection.ExecuteAsync(sql, new
            {
                logDate = requestLog.LogDate,
                parameters = JsonConvert.SerializeObject(requestLog.Parameters),
                route = requestLog.Route
            });
        }
    }
}
