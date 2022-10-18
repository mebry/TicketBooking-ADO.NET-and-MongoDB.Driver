using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Booking.DAL.Interfaces;
using Booking.Domain.Models;

namespace Booking.DAL.Repositories.MongoDB
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IMongoCollection<BsonDocument> _mongoCollection;

        public CountryRepository(string connectionStrings)
        {
            if (connectionStrings == null)
            {
                throw new ArgumentNullException(nameof(connectionStrings));
            }

            var mongoClient = new MongoClient(connectionStrings);
            var mongoDatabase = mongoClient.GetDatabase("booking");

            _mongoCollection = mongoDatabase.GetCollection<BsonDocument>("countries");
        }

        public async Task<bool> Create(Country entity)
        {
            entity.Id = Service.MaxIndex(_mongoCollection) + 1;

            var document = new BsonDocument
            {
                { "_id", entity.Id },
                {"Name",entity.Name },
            };

            await _mongoCollection.InsertOneAsync(document);

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var deleteFilter = Builders<BsonDocument>.Filter.Eq("_id", id);
            await _mongoCollection.DeleteOneAsync(deleteFilter);

            return true;
        }

        public async Task<List<Country>> GetAll()
        {
            var filter = new BsonDocument();
            var countries = new List<Country>();

            using (var cursor = await _mongoCollection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var user = cursor.Current;
                    foreach (var item in user)
                    {
                        countries.Add(new Country()
                        {
                            Id = item.GetValue("_id").ToInt32(),
                            Name = item.GetValue("Name").ToString()
                        });
                    }
                }
            }

            return countries;
        }

        public async Task<List<City>> GetAllCities(int countryId)
        {
            var pipeline = new BsonDocument
            {
                {"$unwind", "$Cities"}
            };

            var pipeline2 = new BsonDocument
            {
                {"$match", new BsonDocument{
                    {"_id", countryId }
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

        public async Task<Country> GetById(int id)
        {
            Country country = new Country();

            var filter = new BsonDocument("_id", id);

            using (var cursor = await _mongoCollection.FindAsync(filter))
            {
                if (await cursor.MoveNextAsync())
                {
                    if (cursor.Current.Count() == 0)
                        return null;

                    var elements = cursor.Current.ToList();

                    var item = elements[0];

                    country.Id = item.GetValue("_id").ToInt32();
                    country.Name = item.GetValue("Name").ToString();
                }
            }
            return country;
        }

        public async Task<Country> GetByName(string name)
        {
            Country country = new Country();

            var filter = new BsonDocument("Name", name);

            using (var cursor = await _mongoCollection.FindAsync(filter))
            {
                if (await cursor.MoveNextAsync())
                {
                    if (cursor.Current.Count() == 0)
                        return null;

                    var elements = cursor.Current.ToList();

                    var item = elements[0];

                    country.Id = item.GetValue("_id").ToInt32();
                    country.Name = item.GetValue("Name").ToString();
                }
            }
            return country;
        }

        public async Task<bool> Update(Country entity)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", entity.Id);

            var update = Builders<BsonDocument>.Update.Set("Name", entity.Name);

            await _mongoCollection.UpdateOneAsync(filter, update);

            return true;
        }
    }
}
