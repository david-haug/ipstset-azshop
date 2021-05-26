using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Ipstset.AzShop.Application;
using Ipstset.AzShop.Application.EventHandling;
using Ipstset.AzShop.Application.Events;
using Ipstset.AzShop.Infrastructure.Models;
using Ipstset.AzShop.Infrastructure.SqlData;
using Newtonsoft.Json;

namespace Ipstset.AzShop.Infrastructure.SqlDataAccess
{
    public class EventRepository : IEventRepository
    {
        private DbSettings _db;
        public EventRepository(DbSettings settings)
        {
            _db = settings;
        }

        public async Task<QueryResult<ApplicationEvent>> GetEventsAsync(GetEventsRequest request)
        {
            var events = new List<ApplicationEvent>();
            var sql = $"exec {_db.Schema}.get_json_all @table,@startAfter";
            using (var sqlConnection = new SqlConnection(_db.Connection))
            {
                var documents = await sqlConnection.QueryAsync<SqlDocument>(sql, new { table = "event", startAfter = request.StartAfter });
                foreach (var document in documents)
                {
                    var @event = JsonConvert.DeserializeObject<ApplicationEvent>(document.Data);
                    events.Add(@event);
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
                events = events.Where(r => r.Name == request.Name).ToList();
            if (request.StartDate.HasValue)
                events = events.Where(r => r.DateOccurred >= request.StartDate.Value).ToList();
            if (request.EndDate.HasValue)
                events = events.Where(r => r.DateOccurred <= request.EndDate.Value).ToList();

            var sorter = new Sorter<ApplicationEvent>();
            events = sorter.Sort(events, request.Sort?.ToArray()).ToList();

            return new QueryResult<ApplicationEvent> { Items = events.Take(request.Limit), TotalRecords = events.Count, Limit = request.Limit, StartAfter = request.StartAfter };
        }

        public async Task SaveAsync(ApplicationEvent @event)
        {
            var sql = $"exec {_db.Schema}.save_json @table,@id,@data";
            using (var sqlConnection = new SqlConnection(_db.Connection))
            {
                await sqlConnection.ExecuteAsync(sql, new { table = "event", id = @event.Id, data = JsonHelper.Serialize(@event) });
            }

        }
    }
}
