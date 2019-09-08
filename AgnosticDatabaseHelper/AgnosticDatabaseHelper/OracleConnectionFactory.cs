using System.Data.Common;
using Light.GuardClauses;
using Oracle.ManagedDataAccess.Client;

namespace AgnosticDatabaseHelper
{
    public class OracleConnectionFactory : IDbConnectionFactory
    {
        private readonly string _oracleConnectionString;

        public OracleConnectionFactory(string oracleConnectionString)
        {
            oracleConnectionString.MustNotBeNullOrWhiteSpace();
            _oracleConnectionString = oracleConnectionString;
        }

        /// <summary>
        /// Returns OracleConnection from the Oracle Managed driver.
        /// It does NOT manage the disposal of said connections.
        /// </summary>
        /// <returns></returns>
        public DbConnection GetConnection() => new OracleConnection(_oracleConnectionString);
    }
}
