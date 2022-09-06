namespace Customers.API.Options;

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = default!;

    public string DatabaseName { get; set; } = default!;

    public string CollectionName { get; set; } = default!;
}
