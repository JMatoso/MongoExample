using Customers.API.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Customers.API.Entities
{
    public class Customer
    {
        [BsonId]
        [BsonElement("Id")]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; }

        [StringLength(100)]
        [BsonElement("Name")]
        [Required, BsonRequired]
        public string Name { get; set; } = default!;

        [BsonElement("Salary")]
        [DataType(DataType.Currency)]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Salary { get; set; }

        [BsonElement("Department")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Department Department { get; set; }

        [BsonElement("Genre")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Genre Genre { get; set; }

        [BsonElement("Created")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Created { get; set; }

        public Customer()
        {
            Id = Guid.NewGuid();
            Genre = Genre.Male;
            Created = DateTime.Now;
        }
    }
}
