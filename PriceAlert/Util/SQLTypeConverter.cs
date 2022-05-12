using Microsoft.Data.SqlClient;

namespace PriceAlertAPI.Util
{
    public static class SQLTypeConverter
    {
        public static double? TryGetDouble(SqlDataReader reader, string columnName)
        {
            if (reader[columnName] == null)
            {
                return null;
            }

            return Convert.ToDouble(reader[columnName]);
        }

        public static decimal? TryGetDecimal(SqlDataReader reader, string columnName)
        {
            if (reader[columnName] == null)
            {
                return null;
            }

            return Convert.ToDecimal(reader[columnName]);
        }

        public static ulong? TryGetUlong(SqlDataReader reader, string columnName)
        {
            if (reader[columnName] == null)
            {
                return null;
            }

            return Convert.ToUInt64(reader[columnName]);
        }

        public static int? TryGetInt(SqlDataReader reader, string columnName)
        {
            if (reader[columnName] == null)
            {
                return null;
            }

            return Convert.ToInt32(reader[columnName]);
        }

        public static string? TryGetString(SqlDataReader reader, string columnName)
        {
            if (reader[columnName] == null)
            {
                return null;
            }

            return Convert.ToString(reader[columnName]);
        }

        public static bool? TryGetBool(SqlDataReader reader, string columnName)
        {
            if (reader[columnName] == null)
            {
                return null;
            }

            return Convert.ToBoolean(reader[columnName]);
        }
    }
}
