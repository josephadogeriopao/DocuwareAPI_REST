
using Microsoft.AspNetCore.DataProtection;
using OPAOWebService.Server.Infrastructure.Extensions;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.IO;


try { 
// Create a specific folder for keys (e.g., in your user profile)
var keyPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "OPAO-Keys");

// Ensure the directory exists
if (!Directory.Exists(keyPath)) Directory.CreateDirectory(keyPath);




var builder = WebApplication.CreateBuilder(args);

// Define your log path
var logPath = Path.Combine(builder.Environment.ContentRootPath, "logs");

// Force create the directory if it's missing
if (!Directory.Exists(logPath))
{
    Directory.CreateDirectory(logPath);
}

    // 0. Load Environment Variables
    // Load .env into the system environment so IConfiguration can see them
    // This looks for the .env file in the actual project directory
    //var envPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".env");
    //DotNetEnv.Env.Load(envPath);
DotNetEnv.Env.Load();
builder.Configuration.AddEnvironmentVariables();

// 1. Data Protection - Secure Sensitive Data
builder.Services.AddDataProtection()
    .SetApplicationName("OPAOWebService")
    .PersistKeysToFileSystem(new DirectoryInfo(keyPath)); // Force both to use this folder; 

// Dependency Injection
// Add services to the container.
// Register the connection provider as a Singleton or Scoped

// 2. Register Services via your Extensions
builder.Services.AddProjectServices(); // Registers DB, TaxService, TaxRepo
builder.Services.AddSwaggerDocumentation(); // Registers SwaggerGen

// Add the new logging extension
builder.Host.AddSerilogLogging(builder.Configuration, builder.Environment.ContentRootPath);



// 3. Controllers and JSON Strictness
builder.Services.AddControllers()
    .AddJsonOptions(options =>
         {
             // Rejects the request if the client sends properties not in the DTO
             options.JsonSerializerOptions.UnmappedMemberHandling =
                 System.Text.Json.Serialization.JsonUnmappedMemberHandling.Disallow;
         });

// Register SwaggerGen (Swashbuckle) instead of AddOpenApi
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// 4. Configure Middleware Pipeline
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
else
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
// 1. MUST ADD THIS to serve the React files (index.html, js, css)
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapFallbackToFile("index.html");


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
}
catch (Exception ex)
{
    // This creates a text file on your desktop with the REAL error
    File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CRASH_LOG.txt"), ex.ToString());
    Console.WriteLine(ex.Message);
    throw;
}

