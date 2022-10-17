using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Booking.DAL.Interfaces;
using Booking.Domain.Models;

namespace Booking.DAL.Repositories.MongoDB
{
    public class PlaneRepository : IPlaneRepository
    {
        private readonly IMongoCollection<Plane> _mongoCollection;

        public PlaneRepository(string connectionStrings)
        {
            if (connectionStrings == null)
            {
                throw new ArgumentNullException(nameof(connectionStrings));
            }

            var mongoClient = new MongoClient(connectionStrings);
            var mongoDatabase = mongoClient.GetDatabase("booking");

            _mongoCollection = mongoDatabase.GetCollection<Plane>("planes");
        }
        public async Task<bool> Create(Plane entity)
        {
            var data = _mongoCollection.Find(new BsonDocument()).Sort("{_id:-1}").Limit(1).ToList();

            int maxValue = 0;

            if (data != null && data.Count > 0)
            {
                maxValue=data[0].Id;
            }

            entity.Id = maxValue+1;

            await _mongoCollection.InsertOneAsync(entity);

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var deleteFilter = Builders<Plane>.Filter.Eq("_id", id);
            await _mongoCollection.DeleteOneAsync(deleteFilter);

            return true;
        }

        public async Task<List<Plane>> GetAll()
        {
            List<Plane> planes = new List<Plane>();

            var filter = new BsonDocument();

            using (var cursor = await _mongoCollection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var data = cursor.Current;

                    foreach (Plane doc in data)
                    {
                        planes.Add(doc);
                    }
                }
            }

            return planes;
        }

        public async Task<Plane> GetById(int id)
        {
            Plane plane = new Plane();

            var filter = new BsonDocument("_id", id);

            using (var cursor = await _mongoCollection.FindAsync(filter))
            {
                if (await cursor.MoveNextAsync())
                {
                    if (cursor.Current.Count() == 0)
                        return null;

                    var elements = cursor.Current.ToList();

                    plane = elements[0];
                }
            }
            return plane;
        }

        public async Task<Plane> GetByPlaneName(string planeName)
        {
            Plane plane = new Plane();
            var filter = new BsonDocument("Name", planeName);

            using (var cursor = await _mongoCollection.FindAsync(filter))
            {
                if (await cursor.MoveNextAsync())
                {
                    if (cursor.Current.Count() == 0)
                        return null;

                    var elements = cursor.Current.ToList();
                    plane = elements[0];
                }

            }
            return plane;
        }

        public async Task<bool> Update(Plane entity)
        {
            var filter = Builders<Plane>.Filter.Eq("_id", entity.Id);

            var update = Builders<Plane>.Update.Set("Name", entity.Name);
            var update2 = Builders<Plane>.Update.Set("Capacity", entity.Capacity);

            await _mongoCollection.UpdateOneAsync(filter, update);
            await _mongoCollection.UpdateOneAsync(filter, update2);

            return true;
        }
    }
}
