using MongoDB.Bson;
using MongoDB.Driver;
using Booking.DAL.Interfaces;
using Booking.Domain.Models;

namespace Booking.DAL.Repositories.MongoDB
{
    public class UserDetailsRepository : IUserDetailsRepository
    {
        private readonly IMongoCollection<BsonDocument> _mongoCollection;

        public UserDetailsRepository(string connectionStrings)
        {
            if (connectionStrings == null)
            {
                throw new ArgumentNullException(nameof(connectionStrings));
            }

            var mongoClient = new MongoClient(connectionStrings);

            var mongoDatabase = mongoClient.GetDatabase("booking");

            _mongoCollection = mongoDatabase.GetCollection<BsonDocument>("users");
        }

        public async Task<bool> Create(UserDelails entity)
        {
            await ChangeTheDocument(entity);
            return true;
        }

        public async Task<bool> Delete(int userId)
        {
            var filter = new BsonDocument("_id", userId);
            var update = Builders<BsonDocument>.Update.Unset("FirstName");
            var update2 = Builders<BsonDocument>.Update.Unset("LastName");
            var update3 = Builders<BsonDocument>.Update.Unset("Patronymic");
            var update4 = Builders<BsonDocument>.Update.Unset("YearOfBirth");

            await _mongoCollection.UpdateOneAsync(filter, update);
            await _mongoCollection.UpdateOneAsync(filter, update2);
            await _mongoCollection.UpdateOneAsync(filter, update3);
            await _mongoCollection.UpdateOneAsync(filter, update4);

            return true;
        }

        public async Task<List<UserDelails>> GetAll()
        {
            var filter = new BsonDocument();
            var users = new List<UserDelails>();

            using (var cursor = await _mongoCollection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var user = cursor.Current;
                    foreach (var item in user)
                    {
                        users.Add(new UserDelails()
                        {
                            UserId = item.GetValue("_id").ToInt32(),
                            FirstName = item.GetValue("FirstName").ToString(),
                            LastName = item.GetValue("LastName").ToString(),
                            Patronymic = item.GetValue("Patronymic").ToString(),
                            YearOfBirth = item.GetValue("YearOfBirth").ToInt32(),
                        });
                    }
                }
            }

            return users;
        }

        public async Task<UserDelails> GetById(int userId)
        {
            UserDelails user = new UserDelails();

            var filter = new BsonDocument("_id", userId);

            using (var cursor = await _mongoCollection.FindAsync(filter))
            {
                if (await cursor.MoveNextAsync())
                {
                    if (cursor.Current.Count() == 0)
                        return null;

                    var elements = cursor.Current.ToList();

                    var item = elements[0];
                    user.UserId = item.GetValue("_id").ToInt32();
                    user.FirstName = item.GetValue("FirstName").ToString();
                    user.LastName = item.GetValue("LastName").ToString();
                    user.Patronymic = item.GetValue("Patronymic").ToString();
                    user.YearOfBirth = item.GetValue("YearOfBirth").ToInt32();
                }
            }
            return user;
        }

        public async Task<UserDelails> GetByUserName(string userName)
        {
            UserDelails user = new UserDelails();

            var filter = new BsonDocument("UserName", userName);

            using (var cursor = await _mongoCollection.FindAsync(filter))
            {
                if (await cursor.MoveNextAsync())
                {
                    if (cursor.Current.Count() == 0)
                        return null;

                    var elements = cursor.Current.ToList();

                    var item = elements[0];

                    user.UserId = item.GetValue("_id").ToInt32();
                    user.FirstName = item.GetValue("FirstName").ToString();
                    user.LastName = item.GetValue("LastName").ToString();
                    user.Patronymic = item.GetValue("Patronymic").ToString();
                    user.YearOfBirth = item.GetValue("YearOfBirth").ToInt32();

                }
            }
            return user;
        }

        public async Task<bool> Update(UserDelails entity)
        {
            await ChangeTheDocument(entity);
            return true;
        }

        public async Task<bool> UpdateFirstName(int userId, string firstName)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", userId);
            var update = Builders<BsonDocument>.Update.Set("FirstName", firstName);
            await _mongoCollection.UpdateOneAsync(filter, update);

            return true;
        }

        public async Task<bool> UpdateLastName(int userId, string lastName)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", userId);
            var update = Builders<BsonDocument>.Update.Set("LastName", lastName);
            await _mongoCollection.UpdateOneAsync(filter, update);

            return true;
        }

        public async Task<bool> UpdatePatronymic(int userId, string patronymic)
        {

            var filter = Builders<BsonDocument>.Filter.Eq("_id", userId);
            var update = Builders<BsonDocument>.Update.Set("Patronymic", patronymic);
            await _mongoCollection.UpdateOneAsync(filter, update);
            return true;
        }

        private async Task ChangeTheDocument(UserDelails entity)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", entity.UserId);

            var update = Builders<BsonDocument>.Update.Set("FirstName", entity.FirstName);
            var update2 = Builders<BsonDocument>.Update.Set("LastName", entity.LastName);
            var update3 = Builders<BsonDocument>.Update.Set("Patronymic", entity.Patronymic);
            var update4 = Builders<BsonDocument>.Update.Set("YearOfBirth", entity.YearOfBirth);

            await _mongoCollection.UpdateOneAsync(filter, update);
            await _mongoCollection.UpdateOneAsync(filter, update2);
            await _mongoCollection.UpdateOneAsync(filter, update3);
            await _mongoCollection.UpdateOneAsync(filter, update4);
        }

    }
}
