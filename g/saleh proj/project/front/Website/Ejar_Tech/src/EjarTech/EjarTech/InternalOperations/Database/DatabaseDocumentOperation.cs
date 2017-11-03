using EjarTech.Models.DatabaseModels;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EjarTech.InternalOperations.Database
{
    public class DatabaseDocumentOperation<CType>
    {
        IMongoCollection<CType> _collection;

        public DatabaseDocumentOperation(IMongoCollection<CType> collection) => _collection = collection;

        public async Task AddDocument(CType document) => await _collection.InsertOneAsync(document);

        public async Task AddManyDocuments(IEnumerable<CType> documents) => await _collection.InsertManyAsync(documents);

        public async Task<CType> GetUserById(FilterDefinition<CType> filter) => await (await _collection.FindAsync(filter)).FirstOrDefaultAsync();

        public async Task<IList<CType>> GetAllDocuments() => await (await _collection.FindAsync(item => true)).ToListAsync();

        public async Task DropDocument(FilterDefinition<CType> filter) => await _collection.DeleteOneAsync(filter);

        public async Task UpdataeDocument(FilterDefinition<CType> filter, UpdateDefinition<CType> update) => await _collection.UpdateOneAsync(filter, update);

        public async Task UpdateManyDocument(FilterDefinition<CType> filter, UpdateDefinition<CType> update) => await _collection.UpdateManyAsync(filter, update);
    }
}
