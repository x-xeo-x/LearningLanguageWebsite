using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LearningLanguageWebsite.Dto
{
    public class ArticleDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        public string CorrectAnswer { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public LanguageDTO LanguageId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Author { get; set; }
    }
}
