
using Microsoft.AspNetCore.DataProtection;
using OPAOWebService.Server.Data.Constants;
using OPAOWebService.Server.Infrastructure.Extensions;
using OPAOWebService.Server.Infrastructure.Helpers;
using OPAOWebService.Shared.Constants;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;


try
{
    ConsoleHelper.DisplayApplicationLogo();

    var builder = WebApplication.CreateBuilder(args);

    var logPath = Path.Combine(builder.Environment.ContentRootPath, "logs");


    DotNetEnv.Env.Load();
    builder.Configuration.AddEnvironmentVariables();

    // 1. Resolve the path
    var keyPath = builder.Configuration[AppConfigConstants.DataProtectionConfigPath];
                  // FIX: Use DefaultFolderName here to match the ConfigTool
                  //?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppConfigConstants.DefaultFolderName);
    Console.WriteLine($"key path ==> {keyPath}");
    Debug.WriteLine($"key path ==> {keyPath}");

    // 2. Ensure the directory exists
    if (!Directory.Exists(keyPath)) Directory.CreateDirectory(keyPath);

    // 3. Configure Data Protection
    builder.Services.AddDataProtection()
        // FIX: Use the shared constant for ApplicationName
        .SetApplicationName(AppConfigConstants.ApplicationName)
        .PersistKeysToFileSystem(new DirectoryInfo(keyPath));


    // Dependency Injection
    builder.Services.AddProjectServices(); // Registers DB, TaxService, TaxRepo
    builder.Services.AddSwaggerDocumentation(); // Registers SwaggerGen

    // Add the new logging extension
    builder.Host.AddSerilogLogging(builder.Configuration, builder.Environment.ContentRootPath);

    // Controllers and JSON Strictness
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

