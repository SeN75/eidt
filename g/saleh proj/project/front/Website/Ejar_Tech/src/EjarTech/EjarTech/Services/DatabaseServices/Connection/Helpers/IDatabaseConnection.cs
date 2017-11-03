using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EjarTech.Services.DatabaseServices.Connection.Helpers
{
    public interface IDatabaseConnection
    {
        Task<IMongoDatabase> GetDatabaseAsync(string databaseName);
    }
}
