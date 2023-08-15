using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LearningLanguageWebsite.Dto
{
    public class LanguageDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Language { get; set; }
    }
}
