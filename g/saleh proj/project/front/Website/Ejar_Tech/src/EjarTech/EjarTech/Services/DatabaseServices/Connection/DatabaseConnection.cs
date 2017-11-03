using EjarTech.Services.DatabaseServices.Connection.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using EjarTech.Models.ConfigurationModel.Database;
using Microsoft.Extensions.Options;

namespace EjarTech.Services.DatabaseServices.Connection
{
    public class DatabaseConnection : IDatabaseConnection
    {
        private readonly IOptions<DatabaseOptions> _connection;
        public DatabaseConnection(IOptions<DatabaseOptions> databaseConfig)
        {
            _connection = databaseConfig;
        }
        public async Task<IMongoDatabase> GetDatabaseAsync(string databaseName)
        {
            if(string.IsNullOrWhiteSpace(_connection.Value.ConnectionString))
                return new MongoClient().GetDatabase(databaseName);
            else
                return new MongoClient(_connection.Value.ConnectionString).GetDatabase(databaseName);
        }
    }
}
