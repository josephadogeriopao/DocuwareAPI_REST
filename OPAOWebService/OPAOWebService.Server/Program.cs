using Microsoft.OpenApi;
using OPAOWebService.Server.Business;
using OPAOWebService.Server.Business.Interfaces;
using OPAOWebService.Server.Data.Providers;
using OPAOWebService.Server.Data.Providers.Interfaces;
using OPAOWebService.Server.Data.Repositories;
using OPAOWebService.Server.Data.Repositories.Interfaces;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Load .env into the system environment so IConfiguration can see them
// This looks for the .env file in the actual project directory
//DotNetEnv.Env.Load(Path.Combine(Directory.GetCurrentDirectory(), ".env"));
DotNetEnv.Env.Load();
builder.Configuration.AddEnvironmentVariables();

// Dependency Injection
// Add services to the container.
// Register the connection provider as a Singleton or Scoped

builder.Services.AddSingleton<IDatabaseConnection, DatabaseConnection>();

builder.Services.AddScoped<ITaxService, TaxService>();
builder.Services.AddScoped<ITaxRepository, TaxRepository>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
         {
             // Rejects the request if the client sends properties not in the DTO
             options.JsonSerializerOptions.UnmappedMemberHandling =
                 System.Text.Json.Serialization.JsonUnmappedMemberHandling.Disallow;
         });

// 1. Register SwaggerGen (Swashbuckle) instead of AddOpenApi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OPAOWebService API",
        Version = "v1.0.0",
        Description = "A comprehensive web service for OPAO operations, providing endpoints for data management and integration.",
        Contact = new OpenApiContact
        {
            Name = "Joseph Adogeri",
            Email = "support@opao.com",
            Url = new Uri("https://opao.example.com")
        },
        License = new OpenApiLicense
        {
            Name = "Apache 2.0 (License)",
            Url = new Uri("http://apache.org")
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // 2. Enable the classic Swashbuckle JSON generator
    app.UseSwagger();

    //// 3. Point UI to the Swashbuckle JSON endpoint (standard path)
    app.UseSwaggerUI(options =>
    {
        // Points to the definition we just created
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "OPAOWebService v1");

        // Behavior matching the Pet Store API
        options.DocumentTitle = "OPAOWebService Documentation";
        options.DocExpansion(DocExpansion.List); // Collapses operations by default
        options.EnableDeepLinking();             // URLs update as you click methods
        //options.EnableFilter();                  // Adds the search/filter bar
        options.DisplayRequestDuration();        // Shows timing for "Try it out"
    });

}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

