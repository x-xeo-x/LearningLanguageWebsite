using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LearningLanguageWebsite.Dto
{
    public class CardDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        public string CorrectAnswer { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string LanguageId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Author { get; set; }
    }
}
