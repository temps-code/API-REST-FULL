using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace prueba1.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public required string Name { get; set; }

        [BsonElement("email")]
        public required string Email { get; set; }

        [BsonElement("pass")]
        public required string Pass { get; set; }

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;
    }
}
