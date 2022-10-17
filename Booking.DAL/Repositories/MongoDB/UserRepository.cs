using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Booking.DAL.Interfaces;
using Booking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.DAL.Repositories.MongoDB
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<BsonDocument> _mongoCollection;

        public UserRepository(string connectionStrings)
        {
            if (connectionStrings == null)
            {
                throw new ArgumentNullException(nameof(connectionStrings));
            }

            var mongoClient= new MongoClient(connectionStrings);

            var mongoDatabase = mongoClient.GetDatabase("booking");

            _mongoCollection = mongoDatabase.GetCollection<BsonDocument>("users");
        }

        public async Task<bool> Create(User entity)
        {
            entity.Id = Service.MaxIndex(_mongoCollection) + 1;

            var document = new BsonDocument
            {
                { "_id", entity.Id },
                {"UserName",entity.UserName },
                {"Password",entity.Password },
                {"Codeword",entity.Codeword }
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

        public async Task<List<User>> GetAll()
        {
            var filter = new BsonDocument();
            var users = new List<User>();

            using (var cursor = await _mongoCollection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var user = cursor.Current;
                    foreach (var item in user)
                    {
                        users.Add(new User()
                        {
                            Id = item.GetValue("_id").ToInt32(),
                            UserName = item.GetValue("Username").ToString(),
                            Password = item.GetValue("Password").ToString(),
                            Codeword = item.GetValue("Codeword").ToString(),
                        });
                    }
                }
            }

            return users;
        }

        public async Task<User> GetById(int id)
        {
            User user = new User();

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
                    user.UserName = item.GetValue("UserName").ToString();
                    user.Password = item.GetValue("Password").ToString();
                    user.Codeword = item.GetValue("Codeword").ToString();

                }
            }
            return user;
        }

        public async Task<User> GetByUserName(string userName)
        {
            User user = new User();
            var filter = new BsonDocument("UserName", userName);

            using (var cursor = await _mongoCollection.FindAsync(filter))
            {
                if (await cursor.MoveNextAsync())
                {
                    if (cursor.Current.Count() == 0)
                        return null;

                    var elements = cursor.Current.ToList();
                    var item = elements[0];

                    user.Id = item.GetValue("_id").ToInt32();
                    user.UserName = item.GetValue("UserName").ToString();
                    user.Password = item.GetValue("Password").ToString();
                    user.Codeword = item.GetValue("Codeword").ToString();
                }

            }
            return user;
        }

        public async Task<bool> Update(User entity)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", entity.Id);

            var update = Builders<BsonDocument>.Update.Set("UserName", entity.UserName);
            var update2 = Builders<BsonDocument>.Update.Set("Password", entity.Password);
            var update3 = Builders<BsonDocument>.Update.Set("Codeword", entity.Codeword);

            await _mongoCollection.UpdateOneAsync(filter, update);
            await _mongoCollection.UpdateOneAsync(filter, update2);
            await _mongoCollection.UpdateOneAsync(filter, update3);

            return true;
        }

        public async Task<bool> UpdatePassword(int userId, string newPassword)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", userId);
            
            var update = Builders<BsonDocument>.Update.Set("Password", newPassword);
            await _mongoCollection.UpdateOneAsync(filter, update);

            return true;
        }
    }
}
