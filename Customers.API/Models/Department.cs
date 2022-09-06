using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Customers.API.Models;

[JsonConverter(typeof(StringEnumConverter))]
public enum Department
{
    HumanResources = 1,
    InformationTechnology = 2,
    Finance = 3,
    Maintenence = 4,
    Administration = 5
}
