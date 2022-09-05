using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Customers.API.Models
{
    public class CustomerRequest
    {
        [BsonRequired]
        [StringLength(100)]
        [BsonElement("Name")]
        public string Name { get; set; } = default!;

        [BsonElement("Salary")]
        [DataType(DataType.Currency)]
        [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)]
        public decimal Salary { get; set; }

        [BsonElement("Department")]
        public Department Department { get; set; }

        [BsonElement("Genre")]
        public Genre Genre { get; set; }
    }
}
