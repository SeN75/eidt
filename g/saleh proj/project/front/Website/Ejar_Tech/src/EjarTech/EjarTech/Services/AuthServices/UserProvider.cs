using EjarTech.Services.AuthServices.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EjarTech.Models.DatabaseModels;
using EjarTech.Services.DatabaseServices.Connection.Helpers;
using Microsoft.Extensions.Options;
using EjarTech.Models.ConfigurationModel.Database;
using MongoDB.Driver;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;

namespace EjarTech.Services.AuthServices
{
    public class UserProvider : IUserProvider
    {
        private IMongoDatabase _database;
        private readonly HttpContext _context;
        private readonly IOptions<DatabaseOptions> _config;
        private readonly IDatabaseConnection _connection;
        public UserProvider(IDatabaseConnection connection, IOptions<DatabaseOptions> config, IHttpContextAccessor contextAccessor)
        {
            _connection = connection;
            _config = config;
            _context = contextAccessor.HttpContext;
        }

        public async Task<User> GetUserAsync()
        {
            _database = await _connection.GetDatabaseAsync(_config.Value.DatabaseName);
            if (!_context.Request.Cookies.ContainsKey("_uid"))
                return null;
            string key = _context.Request.Cookies["_uid"];
            if (string.IsNullOrWhiteSpace(key))
                return null;
            if (!ObjectId.TryParse(key, out ObjectId id))
                return null;
            return await _database.GetCollection<User>("cr_users").Find(usr => usr.TempCode == id).FirstOrDefaultAsync();
        }
    }
}
