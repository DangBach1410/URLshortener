using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace URLShortener_BackEnd.Models
{
    public class ShortUrl
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("originalUrl")]
        public string OriginalUrl { get; set; } = null!;

        [BsonElement("shortCode")]
        public string ShortCode { get; set; } = null!;

    }
}
