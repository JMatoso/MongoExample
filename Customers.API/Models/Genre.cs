using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Customers.API.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Genre
    {
        Male = 1,
        Female = 2
    }
}
