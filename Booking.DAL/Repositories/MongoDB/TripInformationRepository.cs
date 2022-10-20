using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Booking.DAL.Interfaces;
using Booking.Domain.Models;
using Booking.Domain.ViewModels.Trip;

namespace Booking.DAL.Repositories.MongoDB
{
    public class TripInformationRepository : ITripInformationRepository
    {
        private readonly IMongoCollection<BsonDocument> _mongoCollection;

        public TripInformationRepository(string connectionStrings)
        {
            if (connectionStrings == null)
            {
                throw new ArgumentNullException(nameof(connectionStrings));
            }

            var mongoClient = new MongoClient(connectionStrings);
            var mongoDatabase = mongoClient.GetDatabase("booking");

            _mongoCollection = mongoDatabase.GetCollection<BsonDocument>("trips");
        }
        public async Task<List<UserTripViewModel>> GetExecutedTripsByUser(int userId)
        {
            var pipeline = new BsonDocument
            {
                {"$unwind", "$Details"}
            };

            var pipeline2 = new BsonDocument
            {
                {"$match", new BsonDocument{
                    {"Details.UserId", userId },
                    {"EndDate", new BsonDocument{{"$lt",DateTime.Now} } }
                }}
            };

            var pipeline3 = GetBsonProjectUserTripViewModel();
            BsonDocument[] pipelines = new BsonDocument[] { pipeline, pipeline2, pipeline3 };
            List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

            return GetDataUserTripViewModel(results);
        }

        public async Task<List<UserTripViewModel>> GetPlannedTripsByUser(int userId)
        {
            var pipeline = new BsonDocument
            {
                {"$unwind", "$Details"}
            };

            var pipeline2 = new BsonDocument
            {
                {"$match", new BsonDocument{
                    {"Details.UserId", userId },
                    {"StartDate", new BsonDocument{{"$gt",DateTime.Now} } }
                }}
            };

            var pipeline3 = GetBsonProjectUserTripViewModel();

            BsonDocument[] pipelines = new BsonDocument[] { pipeline, pipeline2, pipeline3 };
            List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

            return GetDataUserTripViewModel(results);
        }

        public async Task<List<UserTripViewModel>> GetTripsByUserId(int id)
        {
            var pipeline = new BsonDocument
            {
                {"$unwind", "$Details"}
            };

            var pipeline2 = new BsonDocument
            {
                {"$match", new BsonDocument{
                    {"Details.UserId", id }
                }}
            };

            var pipeline3 = GetBsonProjectUserTripViewModel();

            BsonDocument[] pipelines = new BsonDocument[] { pipeline, pipeline2, pipeline3 };
            List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

            return GetDataUserTripViewModel(results);
        }

        private List<UserTripViewModel> GetDataUserTripViewModel(List<BsonDocument> results)
        {
            List<UserTripViewModel> userTripViewModel = new();

            foreach (var item in results)
            {
                userTripViewModel.Add(
                    new UserTripViewModel()
                    {
                        TripId = item.GetValue("_id").ToInt32(),
                        Place = item.GetValue("Place").ToInt32(),
                        StartCity = item.GetValue("StartCityName").ToString(),
                        StartCountry = item.GetValue("StartCountryName").ToString(),
                        EndCity = item.GetValue("EndCityName").ToString(),
                        EndCountry = item.GetValue("EndCountryName").ToString(),
                        StartDate = item.GetValue("StartDate").ToLocalTime(),
                        EndDate = item.GetValue("EndDate").ToLocalTime(),
                    });
            }

            return userTripViewModel.Count > 0 ? userTripViewModel : null;
        }

        private List<TripInfo> GetDataTripInfo(List<BsonDocument> results)
        {
            List<TripInfo> tripInfos = new();

            foreach (var item in results)
            {
                int price = item.GetValue("Price").ToInt32();
                int count = item.GetValue("CountBoughtTickets").ToInt32();

                tripInfos.Add(
                    new TripInfo()
                    {
                        TripId = item.GetValue("_id").ToInt32(),
                        AllTictets = item.GetValue("Capacity").ToInt32(),
                        StartCity = item.GetValue("StartCityName").ToString(),
                        StartCountry = item.GetValue("StartCountryName").ToString(),
                        EndCity = item.GetValue("EndCityName").ToString(),
                        EndCountry = item.GetValue("EndCountryName").ToString(),
                        StartDate = item.GetValue("StartDate").ToLocalTime(),
                        EndDate = item.GetValue("EndDate").ToLocalTime(),
                        NumberOfOrderedTickets = count,
                        Profit = price * count
                    });
            }

            return tripInfos.Count > 0 ? tripInfos : null;
        }

        private BsonDocument GetBsonProjectUserTripViewModel()
        {
            return new BsonDocument
            {
                { "$project", new BsonDocument
                    {
                    { "_id", "$_id"},
                    { "StartCityName", "$StartCityName"},
                    { "StartCountryName", "$StartCountryName"},
                    { "EndCityName", "$EndCityName"},
                    { "EndCountryName", "$EndCountryName"},
                    { "Place", "$Details.Place"},
                    { "StartDate", "$StartDate"},
                    { "EndDate", "$EndDate"}
                } }
            };
        }

        private BsonDocument GetBsonProjectTripInfo()
        {
            return new BsonDocument
            {
                { "$project", new BsonDocument
                    {
                    { "_id", "$_id"},
                    { "StartCityName", "$StartCityName"},
                    { "StartCountryName", "$StartCountryName"},
                    { "EndCityName", "$EndCityName"},
                    { "EndCountryName", "$EndCountryName"},
                    { "CountBoughtTickets",
                        new BsonDocument {
                            { "$cond", new BsonDocument{
                                {"if",
                                    new BsonDocument{
                                        { "$isArray", "$Details" } }
                                },
                                { "then",
                                    new BsonDocument {
                                    { "$size", "$Details" } }},
                                { "else", 0 }
                                    }}}},
                    {"Capacity","$Capacity" },
                    { "Price","$Price"},
                    { "StartDate", "$StartDate"},
                    { "EndDate", "$EndDate"}
                } }};
        }



        public async Task<List<TripInfo>> GetInformationByTrips()
        {
            var pipeline = GetBsonProjectTripInfo();

            BsonDocument[] pipelines = new BsonDocument[] { pipeline };
            List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

            return GetDataTripInfo(results);
        }

        public async Task<TripInfo> GetInformationByTrip(int tripId)
        {
            var pipeline = new BsonDocument
            {
                {"$match", new BsonDocument{
                    {"_id", tripId }
                }}
            };

            var pipeline2 = GetBsonProjectTripInfo();

            BsonDocument[] pipelines = new BsonDocument[] { pipeline, pipeline2 };
            List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

            return GetDataTripInfo(results)?.First();
        }

        public async Task<List<TripInfo>> GetInformationByDate(int countDays)
        {
            var pipeline = new BsonDocument
            {
                {"$match", new BsonDocument{

                    {"StartDate", new BsonDocument{{"$lte",DateTime.Now} } }
                }}
            };

            var pipeline2 = new BsonDocument
            {
                {"$match", new BsonDocument{
                    {"StartDate", new BsonDocument{{"$gte",DateTime.Now.AddDays(-countDays)} } },
                }}
            };

            var pipeline3 = GetBsonProjectTripInfo();

            BsonDocument[] pipelines = new BsonDocument[] { pipeline, pipeline2,pipeline3 };
            List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

            return GetDataTripInfo(results);
        }

        public async Task<List<TripInfo>> GetInformationByTripsInside()
        {
            var pipeline = new BsonDocument
            {
                {"$match", new BsonDocument{
                    {"StartCountryName", new BsonDocument{
                        { "$eq", "Belarus" }} },
                }}
            };

            var pipeline2 = new BsonDocument
            {
                {"$match", new BsonDocument{
                    {"EndCountryName", new BsonDocument{ 
                        { "$eq", "Belarus" }} },
                }}
            };

            var pipeline3 = GetBsonProjectTripInfo();

            BsonDocument[] pipelines = new BsonDocument[] { pipeline, pipeline2, pipeline3 };
            List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

            return GetDataTripInfo(results);
        }

        public async Task<List<TripInfo>> GetInformationByTripsOutside()
        {
            var pipeline = new BsonDocument
            {
                {"$match", new BsonDocument{
                    {"StartCountryName", new BsonDocument{
                        { "$ne", "Belarus" }} },
                }}
            };

            var pipeline2 = new BsonDocument
            {
                {"$match", new BsonDocument{
                    {"EndCountryName", new BsonDocument{
                        { "$ne", "Belarus" }} },
                }}
            };

            var pipeline3 = GetBsonProjectTripInfo();

            BsonDocument[] pipelines = new BsonDocument[] { pipeline, pipeline2, pipeline3 };
            List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

            return GetDataTripInfo(results);
        }
    }
}

