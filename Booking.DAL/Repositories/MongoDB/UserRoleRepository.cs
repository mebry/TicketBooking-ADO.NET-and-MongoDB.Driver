using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Booking.DAL.Interfaces;
using Booking.Domain.Models;

namespace Booking.DAL.Repositories.MongoDB
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly IMongoCollection<BsonDocument> _mongoCollection;
        private readonly IRoleRepository _roleRepository;

        public UserRoleRepository(string connectionStrings)
        {
            if (connectionStrings == null)
            {
                throw new ArgumentNullException(nameof(connectionStrings));
            }

            _roleRepository = new RoleRepository(connectionStrings);
            var mongoClient = new MongoClient(connectionStrings);

            var mongoDatabase = mongoClient.GetDatabase("booking");

            _mongoCollection = mongoDatabase.GetCollection<BsonDocument>("users");
        }

        public async Task<bool> Create(UserRole entity)
        {
            var pipeline = new BsonDocument
            {
                {"$unwind", "$Roles"}
            };

            var pipeline2 = new BsonDocument
            {
                { "$project", new BsonDocument
                    {
                    { "_id", "$_id"},
                    { "LocalRoleId", "$Roles._id"}
                } }
            };
            BsonDocument[] pipelines = new BsonDocument[] { pipeline, pipeline2 };
            List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

            List<int> arr = new List<int>();
            foreach (var item in results)
            {
                arr.Add(item.GetValue("LocalRoleId").ToInt32());
            }

            if (arr.Count > 0)
            {
                entity.Id = arr.Max() + 1;
            }
            else
            {
                entity.Id = 1;
            }


            var filter = Builders<BsonDocument>.Filter.Eq("_id", entity.UserId);
            var str = await _roleRepository.GetById(entity.RoleId);

            BsonDocument bsonElements = new BsonDocument()
            {
                {"_id", entity.Id},
                {"RoleId", entity.RoleId},
                {"RoleName", str.Name}
            };

            var arrayUpdate = Builders<BsonDocument>.Update.Push("Roles", bsonElements);

            await _mongoCollection.UpdateOneAsync(filter, arrayUpdate);

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var data = await GetById(id);

            if (data == null)
                return false;

            var filter = new BsonDocument("_id", data.UserId);
            var update = Builders<BsonDocument>.Update.Pull("Roles",
                new BsonDocument() { { "_id", id } });
            await _mongoCollection.FindOneAndUpdateAsync(filter, update);

            return true;
        }

        public async Task<List<UserRole>> GetAll()
        {
            var pipeline = new BsonDocument
            {
                {"$unwind", "$Roles"}
            };

            var pipeline2 = new BsonDocument
            {
                { "$project", new BsonDocument
                    {
                    { "_id", "$_id"},
                    { "RoleId", "$Roles.RoleId"},
                    { "InfoId", "$Roles._id"}
                } }
            };
            BsonDocument[] pipelines = new BsonDocument[] { pipeline, pipeline2 };
            List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

            List<UserRole> userRoles = new();

            foreach (BsonDocument item in results)
            {
                userRoles.Add(new UserRole()
                {
                    Id = item.GetValue("InfoId").ToInt32(),
                    UserId = item.GetValue("_id").ToInt32(),
                    RoleId = item.GetValue("RoleId").ToInt32()
                });
            }

            return userRoles;
        }

        public async Task<UserRole> GetById(int id)
        {
            var pipeline = new BsonDocument
            {
                {"$unwind", "$Roles"}
            };

            var pipeline2 = new BsonDocument
            {
                {"$match", new BsonDocument{
                    {"Roles._id", id }

                }}
            };

            var pipeline3 = new BsonDocument
            {
                { "$project", new BsonDocument
                    {
                    { "_id", "$_id"},
                    { "RoleId", "$Roles.RoleId"},
                    { "InfoId", "$Roles._id"}
                } }
            };
            BsonDocument[] pipelines = new BsonDocument[] { pipeline, pipeline2, pipeline3 };
            List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

            UserRole userRole = new UserRole();

            if (results.Count == 0)
            {
                return null;
            }

            var item = results[0];

            userRole.Id = item.GetValue("InfoId").ToInt32();
            userRole.UserId = item.GetValue("_id").ToInt32();
            userRole.RoleId = item.GetValue("RoleId").ToInt32();

            return userRole;
        }

        public async Task<List<UserRole>> GetByUserName(string username)
        {
            var pipeline = new BsonDocument
            {
                {"$unwind", "$Roles"}
            };

            var pipeline2 = new BsonDocument
            {
                {"$match", new BsonDocument{
                    {"UserName", username }

                }}
            };

            var pipeline3 = new BsonDocument
            {
                { "$project", new BsonDocument
                    {
                    { "_id", "$_id"},
                    { "RoleId", "$Roles.RoleId"},
                    { "InfoId", "$Roles._id"}
                } }
            };
            BsonDocument[] pipelines = new BsonDocument[] { pipeline, pipeline2, pipeline3 };
            List<BsonDocument> results = await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync();

            List<UserRole> userRoles = new();

            if (results.Count == 0)
            {
                return null;
            }


            foreach (var item in results)
            {
                userRoles.Add(new UserRole()
                {
                    RoleId = item.GetValue("RoleId").ToInt32(),
                    UserId = item.GetValue("_id").ToInt32(),
                    Id = item.GetValue("InfoId").ToInt32()
                });
            }


            return userRoles;
        }

        public async Task<bool> Update(UserRole entity)
        {
            var arrayFilter = Builders<BsonDocument>.Filter.Eq("_id", entity.UserId) & Builders<BsonDocument>
                  .Filter.Eq("Roles._id", entity.Id);
            var str = await _roleRepository.GetById(entity.RoleId);
            var arrayUpdate = Builders<BsonDocument>.Update.Set("Roles.$.RoleId", entity.RoleId);
            var arrayUpdate2 = Builders<BsonDocument>.Update.Set("Roles.$.RoleName", str.Name);

            _mongoCollection.UpdateOne(arrayFilter, arrayUpdate);
            _mongoCollection.UpdateOne(arrayFilter, arrayUpdate2);

            return true;
        }
    }
}
