using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace szakdolgozat
{
    internal class Accounts
    {
        [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string AccountID { get; set; }

        [BsonElement("username"), BsonRepresentation(BsonType.String)]
        public string Username { get; set; }

        [BsonElement("password"), BsonRepresentation(BsonType.String)]
        public string Password { get; set; }

        [BsonElement("isBarbi"), BsonRepresentation(BsonType.Boolean)]
        public bool IsBarbi { get; set; }
    }
}
