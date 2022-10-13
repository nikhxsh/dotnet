using MongoConsumer.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoConsumer.MongoService
{
	public interface IMongoRepository
	{
		Task Save(DbSettings dbSettings, List<BsonDocument> payload);
	}

	public class MongoRepository : IMongoRepository
	{
		public async Task Save(DbSettings dbSettings, List<BsonDocument> payload)
		{
			var settings = MongoClientSettings.FromConnectionString(dbSettings.connection);
			var client = new MongoClient(settings);
			IMongoDatabase _mongoDatabase = client.GetDatabase(dbSettings.database);

			var _collection = _mongoDatabase.GetCollection<BsonDocument>(dbSettings.collection, new MongoCollectionSettings
			{
				AssignIdOnInsert = true
			});
			try
			{
				await _collection.InsertManyAsync(payload);
			}
			catch (MongoWriteException ex)
			{
				throw ex;
			}
		}
	}
}
