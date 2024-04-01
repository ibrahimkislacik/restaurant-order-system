using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CFusionRestaurant.Entities.Common;

public class BaseMongoEntity
{
    [BsonId]
    public ObjectId Id { get; set; }

    public DateTime CreatedDateTime { get; set; }
}