using MongoDB.Bson.Serialization.Attributes;

namespace SimpleBot
{
    [BsonIgnoreExtraElements]
    public class UserProfile
    {
        public string Id { get; set; }
        public int Visitas { get; set; }
    }
}