using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Booking.DAL.Interfaces;
using Booking.Domain.Models;

namespace Booking.DAL.Repositories.MongoDB
{
    public class CityRepository : ICityRepository
    {
        private readonly IMongoCollection<BsonDocument> _mongoCollection;

        public CityRepository(string connectionStrings)
        {
            if (connectionStrings == null)
            {
                throw new ArgumentNullException(nameof(connectionStrings));
            }

            var mongoClient = new MongoClient(connectionStrings);
            var mongoDatabase = mongoClient.GetDatabase("booking");

            _mongoCollection = mongoDatabase.GetCollection<BsonDocument>("countries");
        }

        public async Task<bool> Create(City entity)
        {
            var pipeline = new BsonDocument
            {
                {"$unwind", "$Cities"}
            };

            var pipeline2 = new BsonDocument
            {
                { "$project", new BsonDocument
                    {
                    { "_id", "$_id"},
                    { "CityId", "$Cities._id"}
                } }
            };
            BsonDocument[] pipelines = new BsonDocument[] { pipeline, pipeline2 };
            List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

            List<int> arr = new List<int>();
            foreach (var item in results)
            {
                arr.Add(item.GetValue("CityId").ToInt32());
            }

            if (arr.Count > 0)
            {
                entity.Id = arr.Max() + 1;
            }
            else
            {
                entity.Id = 1;
            }

            var filter = Builders<BsonDocument>.Filter.Eq("_id", entity.CountryId);
            BsonDocument bsonElements = new BsonDocument()
            {
                {"_id", entity.Id},
                {"CityName", entity.Name}
            };
            var arrayUpdate = Builders<BsonDocument>.Update.Push("Cities", bsonElements);

            await _mongoCollection.UpdateOneAsync(filter, arrayUpdate);

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var data = await GetById(id);

            if (data == null)
                return false;

            var filter = new BsonDocument("_id", data.CountryId);
            var update = Builders<BsonDocument>.Update.Pull("Cities",
                new BsonDocument() { { "_id", id } });
            await _mongoCollection.FindOneAndUpdateAsync(filter, update);

            return true;
        }

        public async Task<List<City>> GetAll()
        {
            var pipeline = new BsonDocument
            {
                {"$unwind", "$Cities"}
            };

            var pipeline2 = new BsonDocument
            {
                { "$project", new BsonDocument
                    {
                    { "_id", "$_id"},
                    { "CityName", "$Cities.CityName"},
                    { "CityId", "$Cities._id"}
                } }
            };
            BsonDocument[] pipelines = new BsonDocument[] { pipeline, pipeline2 };
            List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

            List<City> cities = new();

            foreach (BsonDocument item in results)
            {
                cities.Add(new City()
                {
                    Name = item.GetValue("CityName").ToString(),
                    Id = item.GetValue("CityId").ToInt32(),
                    CountryId = item.GetValue("_id").ToInt32()
                });
            }

            return cities;
        }

        public async Task<City> GetById(int id)
        {
            var pipeline = new BsonDocument
            {
                {"$unwind", "$Cities"}
            };

            var pipeline2 = new BsonDocument
            {
                {"$match", new BsonDocument{
                    {"Cities._id", id }

                }}
            };

            var pipeline3 = new BsonDocument
            {
                { "$project", new BsonDocument
                    {
                    { "_id", "$_id"},
                    { "CityName", "$Cities.CityName"},
                    { "CityId", "$Cities._id"}
                } }
            };
            BsonDocument[] pipelines = new BsonDocument[] { pipeline, pipeline2, pipeline3 };
            List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

            City city = new City();

            if (results.Count == 0)
            {
                return null;
            }

            var item = results[0];

            city.Name = item.GetValue("CityName").ToString();
            city.Id = item.GetValue("CityId").ToInt32();
            city.CountryId = item.GetValue("_id").ToInt32();


            return city;
        }

        public async Task<City> GetByName(string name)
        {
            var pipeline = new BsonDocument
            {
                {"$unwind", "$Cities"}
            };

            var pipeline2 = new BsonDocument
            {
                {"$match", new BsonDocument{
                    {"Cities.Cityname", name }

                }}
            };

            var pipeline3 = new BsonDocument
            {
                { "$project", new BsonDocument
                    {
                    { "_id", "$_id"},
                    { "CityName", "$Cities.CityName"},
                    { "CityId", "$Cities._id"}
                } }
            };
            BsonDocument[] pipelines = new BsonDocument[] { pipeline, pipeline2, pipeline3 };
            List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

            City city = new City();

            if (results.Count == 0)
            {
                return null;
            }

            var item = results[0];

            city.Name = item.GetValue("CityName").ToString();
            city.Id = item.GetValue("CityId").ToInt32();
            city.CountryId = item.GetValue("_id").ToInt32();


            return city;
        }

        public async Task<bool> Update(City entity)
        {
            var arrayFilter = Builders<BsonDocument>.Filter.Eq("_id", entity.CountryId) & Builders<BsonDocument>
                  .Filter.Eq("Cities._id", entity.Id);

            var arrayUpdate = Builders<BsonDocument>.Update.Set("Cities.$.CityName", entity.Name);
            _mongoCollection.UpdateOne(arrayFilter, arrayUpdate);

            return true;
        }
    }
}
