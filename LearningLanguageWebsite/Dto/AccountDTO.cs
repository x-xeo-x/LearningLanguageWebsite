using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LearningLanguageWebsite.Dto
{
    public class AccountDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public long CreationTime { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		public List<string> LangueageId { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		public List<string> CardsId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> ArticlesId { get; set; }
	}
}
