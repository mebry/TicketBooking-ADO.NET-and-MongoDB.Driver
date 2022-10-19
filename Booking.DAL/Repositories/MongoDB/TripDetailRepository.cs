using System.Data.SqlClient;
using System.Data;
using Booking.DAL.Interfaces;
using Booking.Domain.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Booking.DAL.Repositories.MongoDB
{
    public class TripDetailRepository : ITripDetailRepository
    {
        private readonly IMongoCollection<BsonDocument> _mongoCollection;
        private IUserRepository _userRepository;
        public TripDetailRepository(string connectionStrings)
        {
            if (connectionStrings == null)
            {
                throw new ArgumentNullException(nameof(connectionStrings));
            }

            var mongoClient = new MongoClient(connectionStrings);
            var mongoDatabase = mongoClient.GetDatabase("booking");
            _userRepository = new UserRepository(connectionStrings);
            _mongoCollection = mongoDatabase.GetCollection<BsonDocument>("trips");
        }

        public async Task<bool> Create(TripDetails entity)
        {
            var pipeline = new BsonDocument
            {
                {"$unwind", "$Details"}
            };

            var pipeline2 = new BsonDocument
            {
                { "$project", new BsonDocument
                    {
                    { "_id", "$_id"},
                    { "UserId", "$Details._id"}
                } }
            };
            BsonDocument[] pipelines = new BsonDocument[] { pipeline, pipeline2 };
            List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

            List<int> arr = new List<int>();
            foreach (var item in results)
            {
                arr.Add(item.GetValue("UserId").ToInt32());
            }

            if (arr.Count > 0)
            {
                entity.Id = arr.Max() + 1;
            }
            else
            {
                entity.Id = 1;
            }

            var filter = Builders<BsonDocument>.Filter.Eq("_id", entity.TripId);
            var user = await _userRepository.GetById(entity.UserId);
            BsonDocument bsonElements = new BsonDocument()
            {
                {"_id", entity.Id},
                {"UserId", entity.UserId},
                {"UserName", user.UserName},
                {"Place", entity.Place}
            };
            var arrayUpdate = Builders<BsonDocument>.Update.Push("Details", bsonElements);

            await _mongoCollection.UpdateOneAsync(filter, arrayUpdate);

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var data = await GetById(id);

            if (data == null)
                return false;

            var filter = new BsonDocument("_id", data.TripId);
            var update = Builders<BsonDocument>.Update.Pull("Details",
                new BsonDocument() { { "_id", id } });

            await _mongoCollection.FindOneAndUpdateAsync(filter, update);

            return true;
        }

        public async Task<bool> DeleteByPlace(int tripId, int place)
        {
            var data = await GetById(tripId);

            if (data == null)
                return false;

            var filter = new BsonDocument("_id", data.TripId);
            var update = Builders<BsonDocument>.Update.Pull("Details",
                new BsonDocument() { { "Place", place } });

            await _mongoCollection.FindOneAndUpdateAsync(filter, update);

            return true;
        }

        public async Task<List<TripDetails>> GetAll()
        {
            var pipeline = new BsonDocument
            {
                {"$unwind", "Details"}
            };

            var pipeline2 = new BsonDocument
             {
                { "$project", new BsonDocument
                    {
                    { "_id", "$_id"},
                    { "UserId", "$Details.UserId"},
                    { "DetailId", "$Details._id"},
                    { "Place", "$Details.Place"}

                } }
            };

            BsonDocument[] pipelines = new BsonDocument[] { pipeline, pipeline2 };
            List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

            List<TripDetails> tripDetails = new();

            foreach (BsonDocument item in results)
            {
                tripDetails.Add(new TripDetails()
                {
                    UserId = item.GetValue("UserId").ToInt32(),
                    Id = item.GetValue("DetailId").ToInt32(),
                    Place = item.GetValue("Place").ToInt32(),
                    TripId = item.GetValue("_id").ToInt32()
                });
            }

            return tripDetails.Count > 0 ? tripDetails : null;
        }

        public async Task<TripDetails> GetById(int id)
        {
            var pipeline = new BsonDocument
            {
                {"$unwind", "$Details"}
            };

            var pipeline2 = new BsonDocument
            {
                {"$match", new BsonDocument{
                    {"Details._id", id }
                }}
            };

            var pipeline3 = new BsonDocument
            {
                { "$project", new BsonDocument
                    {
                    { "_id", "$_id"},
                    { "UserId", "$Details.UserId"},
                    { "DetailId", "$Details._id"},
                    { "Place", "$Details.Place"}

                } }
            };

            BsonDocument[] pipelines = new BsonDocument[] { pipeline, pipeline2, pipeline3 };
            List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

            TripDetails tripDetails = new TripDetails();

            if (results.Count == 0)
            {
                return null;
            }

            var item = results[0];

            tripDetails.UserId = item.GetValue("UserId").ToInt32();
            tripDetails.Id = item.GetValue("DetailId").ToInt32();
            tripDetails.Place = item.GetValue("Place").ToInt32();
            tripDetails.TripId = item.GetValue("_id").ToInt32();

            return tripDetails;
        }

        public async Task<List<TripDetails>> GetDetailsByTripId(int tripId)
        {
            var pipeline = new BsonDocument
            {
                {"$unwind", "$Details"}
            };

            var pipeline2 = new BsonDocument
            {
                {"$match", new BsonDocument{
                    {"_id", tripId }
                }}
            };

            var pipeline3 = new BsonDocument
            {
                { "$project", new BsonDocument
                    {
                    { "_id", "$_id"},
                    { "UserId", "$Details.UserId"},
                    { "DetailId", "$Details._id"},
                    { "Place", "$Details.Place"}
                } }
            };

            BsonDocument[] pipelines = new BsonDocument[] { pipeline, pipeline2,pipeline3 };
            List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

            List<TripDetails> tripDetails = new();

            foreach (BsonDocument item in results)
            {
                tripDetails.Add(new TripDetails()
                {
                    UserId = item.GetValue("UserId").ToInt32(),
                    Id = item.GetValue("DetailId").ToInt32(),
                    Place = item.GetValue("Place").ToInt32(),
                    TripId = item.GetValue("_id").ToInt32()
                });
            }

            return tripDetails.Count > 0 ? tripDetails : null;
        }

        public async Task<bool> Update(TripDetails entity)
        {
            var arrayFilter = Builders<BsonDocument>.Filter.Eq("_id", entity.TripId) & Builders<BsonDocument>
                  .Filter.Eq("Details._id", entity.Id);

            var arrayUpdate = Builders<BsonDocument>.Update.Set("Details.$.UserId", entity.UserId);
            var arrayUpdate2 = Builders<BsonDocument>.Update.Set("Details.$.Place", entity.Place);
            _mongoCollection.UpdateOne(arrayFilter, arrayUpdate);
            _mongoCollection.UpdateOne(arrayFilter, arrayUpdate2);

            return true;
        }
    }
}
