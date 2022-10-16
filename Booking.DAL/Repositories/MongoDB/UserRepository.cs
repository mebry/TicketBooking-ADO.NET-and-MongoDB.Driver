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
    public class UserRepository : IBaseRepository<User>
    {
        private readonly IMongoCollection<User> _mongoCollection;

        public UserRepository(IMongoClient mongoClient)
        {
            if (mongoClient == null)
            {
                throw new ArgumentNullException(nameof(mongoClient));
            }

            var mongoDatabase = mongoClient.GetDatabase("booking");
            _mongoCollection = mongoDatabase.GetCollection<User>("users");
        }

        public Task<bool> Create(User entity)
        {
            return null;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
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
                        users.Add(item);
                    }
                }
            }

            return users;
        }

        public Task<User> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(User entity)
        {
            throw new NotImplementedException();
        }

        private int MaxIndex()
        {
            var data = _mongoCollection.Find(new BsonDocument()).Sort("{_id:-1}").Limit(1).ToList();
            if(data!=null && data.Count > 0)
            {
                return data[0].Id;
            }

            return 0;
        }
    }
}
