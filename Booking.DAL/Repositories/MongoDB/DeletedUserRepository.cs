using MongoDB.Bson;
using MongoDB.Driver;
using Booking.DAL.Interfaces;
using Booking.Domain.Models;
namespace Booking.DAL.Repositories.MongoDB
{
    public class DeletedUserRepository : IDeletedUserRepository
    {
        private readonly IMongoCollection<BsonDocument> _mongoCollection;

        private readonly IUserRepository _userRepository;

        public DeletedUserRepository(string connectionStrings)
        {
            if (connectionStrings == null)
            {
                throw new ArgumentNullException(nameof(connectionStrings));
            }

            var mongoClient = new MongoClient(connectionStrings);

            var mongoDatabase = mongoClient.GetDatabase("booking");

            _userRepository = new UserRepository(connectionStrings);

            _mongoCollection = mongoDatabase.GetCollection<BsonDocument>("deletedUsers");
        }

        public async Task<bool> Create(DeletedUser entity)
        {
            entity.Id = Service.MaxIndex(_mongoCollection) + 1;
            var user = await _userRepository.GetById(entity.UserId);

            if (user == null)
                return false;

            var document = new BsonDocument
            {
                { "_id", entity.Id },
                {"UserId",entity.UserId },
                {"UserName",user.UserName },
                {"Reason",entity.Reason },
                {"DateTime",entity.DateTime }
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

        public async Task<List<DeletedUser>> GetAll()
        {
            var filter = new BsonDocument();
            var users = new List<DeletedUser>();

            using (var cursor = await _mongoCollection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var user = cursor.Current;
                    foreach (var item in user)
                    {
                        users.Add(new DeletedUser()
                        {
                            Id = item.GetValue("_id").ToInt32(),
                            UserId = item.GetValue("UserId").ToInt32(),
                            Reason = item.GetValue("Reason").ToString(),
                            DateTime = item.GetValue("DateTime").ToLocalTime(),
                        });
                    }
                }
            }

            return users;
        }

        public async Task<DeletedUser> GetById(int id)
        {
            DeletedUser user = new DeletedUser();

            var filter = new BsonDocument("_id", id);

            using (var cursor = await _mongoCollection.FindAsync(filter))
            {
                if (await cursor.MoveNextAsync())
                {
                    if (cursor.Current.Count() == 0)
                        return null;

                    var elements = cursor.Current.ToList();

                    var item = elements[0];

                    user.Id = item.GetValue("_id").ToInt32();
                    user.UserId = item.GetValue("UserId").ToInt32();
                    user.Reason = item.GetValue("Reason").ToString();
                    user.DateTime = item.GetValue("DateTime").ToLocalTime();
                }
            }
            return user;
        }

        public async Task<bool> Update(DeletedUser entity)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", entity.Id);

            var update = Builders<BsonDocument>.Update.Set("UserId", entity.UserId);
            var update2 = Builders<BsonDocument>.Update.Set("Reason", entity.Reason);
            var update3 = Builders<BsonDocument>.Update.Set("DateTime", entity.DateTime);

            await _mongoCollection.UpdateOneAsync(filter, update);
            await _mongoCollection.UpdateOneAsync(filter, update2);
            await _mongoCollection.UpdateOneAsync(filter, update3);

            return true;
        }

        public async Task<bool> UpdateTheReason(int userId, string reason)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", userId);

            var update = Builders<BsonDocument>.Update.Set("Reason", reason);

            await _mongoCollection.UpdateOneAsync(filter, update);


            return true;
        }
    }
}
