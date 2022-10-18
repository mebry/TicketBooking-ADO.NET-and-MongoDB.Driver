using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Booking.DAL.Interfaces;
using Booking.Domain.Models;
using System;

namespace Booking.DAL.Repositories.MongoDB
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IMongoCollection<Role> _mongoCollection;

        public RoleRepository(string connectionStrings)
        {
            if (connectionStrings == null)
            {
                throw new ArgumentNullException(nameof(connectionStrings));
            }

            var mongoClient = new MongoClient(connectionStrings);
            var mongoDatabase = mongoClient.GetDatabase("booking");

            _mongoCollection = mongoDatabase.GetCollection<Role>("roles");
        }

        public async Task<bool> Create(Role entity)
        {
            var data = _mongoCollection.Find(new BsonDocument()).Sort("{_id:-1}").Limit(1).ToList();

            int maxValue = 0;

            if (data != null && data.Count > 0)
            {
                maxValue = data[0].Id;
            }

            entity.Id = maxValue + 1;

            await _mongoCollection.InsertOneAsync(entity);

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var deleteFilter = Builders<Role>.Filter.Eq("_id", id);
            await _mongoCollection.DeleteOneAsync(deleteFilter);

            return true;
        }

        public async Task<List<Role>> GetAll()
        {
            List<Role> roles = new List<Role>();

            var filter = new BsonDocument();

            using (var cursor = await _mongoCollection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var data = cursor.Current;

                    foreach (Role doc in data)
                    {
                        roles.Add(doc);
                    }
                }
            }

            return roles;
        }

        public async Task<Role> GetById(int id)
        {
            Role role = new Role();

            var filter = new BsonDocument("_id", id);

            using (var cursor = await _mongoCollection.FindAsync(filter))
            {
                if (await cursor.MoveNextAsync())
                {
                    if (cursor.Current.Count() == 0)
                        return null;

                    var elements = cursor.Current.ToList();

                    role = elements[0];
                }
            }
            return role;
        }

        public async Task<Role> GetByName(string name)
        {
            Role role = new Role();
            var filter = new BsonDocument("Name", name);

            using (var cursor = await _mongoCollection.FindAsync(filter))
            {
                if (await cursor.MoveNextAsync())
                {
                    if (cursor.Current.Count() == 0)
                        return null;

                    var elements = cursor.Current.ToList();
                    role = elements[0];
                }

            }
            return role;
        }

        public async Task<bool> Update(Role entity)
        {
            var filter = Builders<Role>.Filter.Eq("_id", entity.Id);

            var update = Builders<Role>.Update.Set("Name", entity.Name);

            await _mongoCollection.UpdateOneAsync(filter, update);

            return true;
        }
    }
}
