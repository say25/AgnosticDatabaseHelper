using System.Data.Common;

namespace AgnosticDatabaseHelper
{
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// Returns NpgsqlConnection
        /// It does NOT manage the disposal of said connections.
        /// </summary>
        /// <returns></returns>
        DbConnection GetConnection();
    }
}