using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace challenge.Domain.Entities
{
    public abstract class MongoEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    }
}
