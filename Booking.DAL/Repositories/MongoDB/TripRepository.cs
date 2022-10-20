using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Booking.DAL.Interfaces;
using Booking.Domain.Models;
using Booking.Domain.ViewModels.Trip;

namespace Booking.DAL.Repositories.MongoDB
{
    public class TripRepository : ITripRepository
    {
        private readonly IMongoCollection<BsonDocument> _mongoCollection;
        private ICountryRepository _countryRepository;
        private ICityRepository _cityRepository;
        private IPlaneRepository _planeRepository;

        public TripRepository(string connectionStrings)
        {
            if (connectionStrings == null)
            {
                throw new ArgumentNullException(nameof(connectionStrings));
            }
            _countryRepository = new CountryRepository(connectionStrings);
            _cityRepository = new CityRepository(connectionStrings);
            _planeRepository = new PlaneRepository(connectionStrings);
            var mongoClient = new MongoClient(connectionStrings);
            var mongoDatabase = mongoClient.GetDatabase("booking");

            _mongoCollection = mongoDatabase.GetCollection<BsonDocument>("trips");
        }

        public async Task<bool> Create(Trip entity)
        {
            entity.Id = Service.MaxIndex(_mongoCollection) + 1;
            var plane = await _planeRepository.GetById(entity.PlaneId);

            var startCity = await _cityRepository.GetById(entity.StartCityId);
            var endCity = await _cityRepository.GetById(entity.EndCityId);
            var startCountry = await _countryRepository.GetById(startCity.CountryId);
            var endCountry = await _countryRepository.GetById(endCity.CountryId);
            var document = new BsonDocument
            {
                { "_id", entity.Id },
                {"PlaneId",entity.PlaneId},
                {"PlaneName",plane.Name },
                {"StartCityId",entity.StartCityId },
                {"StartCityName",startCity.Name},
                {"StartCountryId",startCity.CountryId},
                {"StartCountryName",startCountry.Name},
                {"EndCityId",entity.EndCityId},
                {"EndCityName",endCity.Name},
                {"EndCountryId",endCity.CountryId},
                {"EndCountryName",endCountry.Name},
                {"Capacity",entity.Capacity},
                {"Price",entity.Price},
                {"StartDate",entity.StartDate},
                {"EndDate",entity.EndDate},
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

        public async Task<List<Trip>> GetAll()
        {
            var filter = new BsonDocument();
            var trips = new List<Trip>();

            using (var cursor = await _mongoCollection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var user = cursor.Current;
                    foreach (var item in user)
                    {
                        trips.Add(new Trip()
                        {
                            Id = item.GetValue("_id").ToInt32(),
                            PlaneId = item.GetValue("PlaneId").ToInt32(),
                            StartCityId = item.GetValue("StartCityId").ToInt32(),
                            EndCityId = item.GetValue("EndCityId").ToInt32(),
                            Capacity = item.GetValue("Capacity").ToInt32(),
                            Price = item.GetValue("Price").ToInt32(),
                            StartDate = item.GetValue("StartDate").ToLocalTime(),
                            EndDate = item.GetValue("EndDate").ToLocalTime(),
                        });
                    }
                }
            }

            return trips;
        }

        public async Task<Trip> GetById(int id)
        {
            Trip trip = new Trip();

            var filter = new BsonDocument("_id", id);

            using (var cursor = await _mongoCollection.FindAsync(filter))
            {
                if (await cursor.MoveNextAsync())
                {
                    if (cursor.Current.Count() == 0)
                        return null;

                    var elements = cursor.Current.ToList();

                    var item = elements[0];

                    trip.Id = item.GetValue("_id").ToInt32();
                    trip.PlaneId = item.GetValue("PlaneId").ToInt32();
                    trip.StartCityId = item.GetValue("StartCityId").ToInt32();
                    trip.EndCityId = item.GetValue("EndCityId").ToInt32();
                    trip.Capacity = item.GetValue("Capacity").ToInt32();
                    trip.Price = item.GetValue("Price").ToInt32();
                    trip.StartDate = item.GetValue("StartDate").ToLocalTime();
                    trip.EndDate = item.GetValue("EndDate").ToLocalTime();

                }
            }
            return trip;
        }
        
        public async Task<bool> Update(Trip entity)
        {
            var plane = await _planeRepository.GetById(entity.PlaneId);
            var startCity = await _cityRepository.GetById(entity.StartCityId);
            var endCity = await _cityRepository.GetById(entity.EndCityId);
            var startCountry = await _countryRepository.GetById(startCity.CountryId);
            var endCountry = await _countryRepository.GetById(endCity.CountryId);

            var filter = Builders<BsonDocument>.Filter.Eq("_id", entity.Id);

            var update = Builders<BsonDocument>.Update.Set("PlaneId", entity.PlaneId);
            var update2 = Builders<BsonDocument>.Update.Set("PlaneName", plane.Name);
            var update3 = Builders<BsonDocument>.Update.Set("StartCityId", entity.StartCityId);
            var update4 = Builders<BsonDocument>.Update.Set("StartCityName", startCity.Name);
            var update5 = Builders<BsonDocument>.Update.Set("StartCountryId", startCity.CountryId);
            var update6 = Builders<BsonDocument>.Update.Set("StartCountryName", startCountry.Name);
            var update7 = Builders<BsonDocument>.Update.Set("EndCityId", entity.EndCityId);
            var update8 = Builders<BsonDocument>.Update.Set("EndCityName", endCity.Name);
            var update9 = Builders<BsonDocument>.Update.Set("EndCountryId", endCity.CountryId);
            var update10 = Builders<BsonDocument>.Update.Set("EndCountryName", endCountry.Name);
            var update11 = Builders<BsonDocument>.Update.Set("Capacity", entity.Capacity);
            var update12 = Builders<BsonDocument>.Update.Set("Price", entity.Price);
            var update13 = Builders<BsonDocument>.Update.Set("StartDate", entity.StartDate);
            var update14 = Builders<BsonDocument>.Update.Set("EndDate", entity.EndDate);

            var updates = new UpdateDefinition<BsonDocument>[] { update , update2, update3, update4, update5, update6,
            update7,update8,update9,update10,update11,update12,update13,update14};

            foreach (var item in updates)
            {
                await _mongoCollection.UpdateOneAsync(filter, item);
            }

            return true;
        }

        
    }
}
