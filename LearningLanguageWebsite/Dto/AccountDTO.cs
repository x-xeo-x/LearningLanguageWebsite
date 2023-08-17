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
        public bool EmailConfirmed { get; set; }
        public bool IsAdmin { get; set; }
        public long CreationTime { get; set; }
        public long LastPasswordChange { get; set; }
        public long LastEmailPasswordSend { get; set; }
        public long LastEmailConfirmSend { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
		public List<string> LangueageId { get; set; }
	}
}
