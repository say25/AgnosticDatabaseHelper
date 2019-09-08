using System.Data.Common;
using Light.GuardClauses;
using Npgsql;

namespace AgnosticDatabaseHelper
{
    public class NpgsqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _npgsqlConnectionString;

        public NpgsqlConnectionFactory(string npgsqlConnectionString)
        {
            npgsqlConnectionString.MustNotBeNullOrWhiteSpace();
            _npgsqlConnectionString = npgsqlConnectionString;
        }

        /// <summary>
        /// Returns NpgsqlConnection
        /// It does NOT manage the disposal of said connections.
        /// </summary>
        /// <returns></returns>
        public DbConnection GetConnection() => new NpgsqlConnection(_npgsqlConnectionString);
    }
}
