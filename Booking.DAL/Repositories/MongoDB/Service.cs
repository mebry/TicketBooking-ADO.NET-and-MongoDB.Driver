using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.DAL.Repositories.MongoDB
{
    public class Service
    {
        public static int MaxIndex(IMongoCollection<BsonDocument> mongoClient)
        {
            int maxValue = 0;

            var data = mongoClient.Find(new BsonDocument()).Sort("{_id:-1}").Limit(1).ToList();
            foreach (var item in data)
            {
                maxValue = item.GetValue("_id").ToInt32();
            }

            return maxValue;
        }
    }

}
