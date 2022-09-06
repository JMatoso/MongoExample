using Serilog;
using Customers.API.Options;
using Customers.API.Entities;
using Customers.API.Extensions;
using Customers.API.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    EnvironmentName = Environments.Development
});

builder.Host.UseSerilog(Logging.ConfigureLogger);

// Add services to the container.
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ValidationFilterAttribute>();

builder.Services.Configure<ApiBehaviorOptions>(opt
    => opt.SuppressModelStateInvalidFilter = true);

builder.Services.AddSingleton<IMockEntity<Customer>, MockEntity<Customer>>();

builder.Services.AddControllers(opt =>
{
    opt.RespectBrowserAcceptHeader = true;
    opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc($"v1", new OpenApiInfo
    {
        Title = "Customers API",
        Version = $"v1",
        Description = "Customers api example using MongoDB.",
        Contact = new OpenApiContact
        {
            Name = "José Matoso",
            Email = "jos3matosoj@gmail.com",
            Url = new Uri("https://github.com/JMatoso/Booking")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://github.com/JMatoso/MongoExample/blob/master/LICENSE.txt")
        },
        TermsOfService = new Uri("https://github.com/JMatoso/MongoExample")
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(opt => {
    opt.AllowAnyHeader();
    opt.AllowAnyHeader();
    opt.SetIsOriginAllowed((host) => true);
    opt.AllowCredentials();
});

app.MapControllers();

app.Run();