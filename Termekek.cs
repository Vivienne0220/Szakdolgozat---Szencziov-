using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace szakdolgozat
{
    [Serializable]

    public enum CigarettaType
    {
        Mäkké,
        Tvrdé,
        Krátké,
        Dlhé
    }

    public enum KategoriaType
    {
        Sud,
        Flasa,
        Pleh,
        Sklenené,
        Nič
    }

    public static class KategoriaHelper
    {
        private static readonly Dictionary<KategoriaType, decimal> KategoriaErtekek = new Dictionary<KategoriaType, decimal>
        {
            { KategoriaType.Sud, 10.00m },
            { KategoriaType.Flasa, 0.15m },
            { KategoriaType.Pleh, 0.15m },
            { KategoriaType.Sklenené, 0.13m },
            { KategoriaType.Nič, 0.00m }
        };

        public static decimal GetFixErtek(KategoriaType kategoria)
        {
            return KategoriaErtekek.ContainsKey(kategoria) ? KategoriaErtekek[kategoria] : 0.00m;
        }
    }
    
    public class Termekek
    {
   

        [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }

        [BsonElement("product_code"), BsonRepresentation(BsonType.String)]
        public string ProductCode { get; set; }

        [BsonElement("product_name"), BsonRepresentation(BsonType.String)]
        public string ProductName { get; set; }

        [BsonElement("price"), BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }

        [BsonElement("zosPred"), BsonRepresentation(BsonType.Decimal128)]
        public decimal ZosPred { get; set; }

        [BsonElement("prijem"), BsonRepresentation(BsonType.Decimal128)]
        public decimal Prijem { get; set; }

        [BsonElement("uzavZos"), BsonRepresentation(BsonType.Decimal128)]
        public decimal UzavZos { get; set; }

        [BsonElement("kategoria"), BsonRepresentation(BsonType.String)]
        public KategoriaType Kategoria { get; set; }

        [BsonElement("kategoriacigi"), BsonRepresentation(BsonType.String)]
        public CigarettaType KategoriaCigi { get; set; }


    }
}
