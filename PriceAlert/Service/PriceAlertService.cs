using System.Data;
using Microsoft.Data.SqlClient;

using PriceAlertAPI.Models;
using PriceAlertAPI.Util;

namespace PriceAlertAPI.Service
{
    public class PriceAlertService : IPriceAlertService
    {
        private IConfiguration _configuration { get; }
        public PriceAlertService(IConfiguration configuration) {
            _configuration = configuration;
        }
        public async Task SetLowPriceAlertAsync(string username, string ticker, decimal priceLimit) {
            
            string query = @$"INSERT INTO [GTAM].[PriceAlert] ([username], [ticker], [price_limit], [updated])
                              VALUES ('{username}', '{ticker}', '{priceLimit}', @datetime)";

            using (var conn = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@datetime", DateTime.Now);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<IList<PriceAlert>> GetAlertsAsync() {
            IList<PriceAlert> alerts = new List<PriceAlert>();
            string query = "SELECT [id], [username], [ticker], [price_limit], [updated] FROM [GTAM].[PriceAlert]";

            using (var conn = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand(query, conn);
                var reader = await cmd.ExecuteReaderAsync();
                await ReadAlert(reader, alerts);
                reader.Close();
            }
            return alerts;
        }

        public async Task<IList<PriceAlert>> CheckAlertsAsync(string ticker, decimal price) {
            IList<PriceAlert> alerts = new List<PriceAlert>();
            using (var conn = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand("GTAM.CheckAlerts_V1", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                var reader = await cmd.ExecuteReaderAsync();
                cmd.Parameters.Add(new SqlParameter("@ticker", ticker));
                cmd.Parameters.Add(new SqlParameter("@price", price));
                await ReadAlert(reader, alerts);
                reader.Close();
            }
            return alerts;
        }

        // Delete all alerts for a ticker and a user
        public async Task RemoveAlertAsync(string username, string ticker) {
            string query = $"DELETE FROM [GTAM].[PriceAlert] WHERE [username] = '{username}' AND [ticker] = '{ticker}'";

            using (var conn = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                await conn.OpenAsync();
                var cmd = new SqlCommand(query, conn);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        private async Task ReadAlert(SqlDataReader reader, IList<PriceAlert> alerts) {
            while (await reader.ReadAsync())
            {
                var alert = new PriceAlert()
                {
                    Id = SQLTypeConverter.TryGetUlong(reader, "id") ?? 0,
                    Username = SQLTypeConverter.TryGetString(reader, "username") ?? "",
                    Ticker = SQLTypeConverter.TryGetString(reader, "ticker") ?? "",
                    PriceLimit = SQLTypeConverter.TryGetDecimal(reader, "price_limit") ?? 0.00M
                };
                alerts.Add(alert);
            }
        }
    }
}
