using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AgnosticDatabaseHelper
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((context, builder) => ConfigureApp(context, builder, args))
                .ConfigureServices(ConfigureServices)
                .Build();

            var dbConnections = new List<(string Name, IDbConnectionFactory Connection)>();

            var oracleConnectionFactory = hostBuilder.Services.GetService<OracleConnectionFactory>();
            dbConnections.Add(("Oracle", oracleConnectionFactory));

            var npgsqlConnectionFactory = hostBuilder.Services.GetService<NpgsqlConnectionFactory>();
            dbConnections.Add(("Postgres", npgsqlConnectionFactory));

            foreach (var dbConnection in dbConnections)
            {
                PrintInfoTable(dbConnection);
            }
        }

        private static void PrintInfoTable((string Name, IDbConnectionFactory ConnectionFactory) dbConnectionProperties)
        {
            Console.WriteLine($"Start - {dbConnectionProperties.Name} Section");

            var dbConnection = dbConnectionProperties.ConnectionFactory.GetConnection();
            dbConnection.Open();
            DataTable dataTable = dbConnection.GetSchema(DbMetaDataCollectionNames.DataSourceInformation);

            for (var j = 0; j < dataTable.Rows.Count; j++)
            {
                for (var i = 0; i < dataTable.Columns.Count; i++)
                {
                    Console.Write(dataTable.Columns[i].ColumnName + ": ");
                    Console.WriteLine(dataTable.Rows[j].ItemArray[i]);
                }
            }

            dbConnection.Close();

            Console.WriteLine($"End - {dbConnectionProperties.Name} Section");
            Console.WriteLine();
        }

        private static void ConfigureApp(HostBuilderContext hostContext, IConfigurationBuilder configuration, string[] args)
        {
            var environment = hostContext.HostingEnvironment.EnvironmentName;

            configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Program>(optional: true)
                .AddCommandLine(args);
        }

        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            var configuration = hostContext.Configuration;
            services
                .AddSingleton(new OracleConnectionFactory(configuration.GetConnectionString("oracle")))
                .AddSingleton(new NpgsqlConnectionFactory(configuration.GetConnectionString("postgres")))
                ;
        }
    }
}
