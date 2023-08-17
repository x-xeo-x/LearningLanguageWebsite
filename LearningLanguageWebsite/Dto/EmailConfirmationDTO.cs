using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LearningLanguageWebsite.Dto
{
    public class EmailConfirmationDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string AccountId { get; set; }
        public string Email { get; set; }
        public string Key { get; set; }
        public long CreationTime { get; set; }
        public bool Used { get; set; }
    }
}
