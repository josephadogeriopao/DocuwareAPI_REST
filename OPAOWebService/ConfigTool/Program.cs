using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OPAOWebService.ConfigTool;
using OPAOWebService.Shared.Constants;
using System.Diagnostics;


// 1. Determine the environment (default to Development for local work)
string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

Console.WriteLine($"environment ==> {env}");
Debug.WriteLine($"environment ==> {env}");


var builder = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true)
    // ADD THIS LINE to allow Development overrides
    .AddJsonFile($"appsettings.{env}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

var keyPath = builder[AppConfigConstants.DataProtectionConfigPath]
              ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ASP.NET", "OPAOWebService");

Console.WriteLine($"key path ==> {keyPath}");
Debug.WriteLine($"key path ==> {keyPath}");


if (!Directory.Exists(keyPath)) Directory.CreateDirectory(keyPath);

// 2. Setup the provider with the Shared Constants
var serviceCollection = new ServiceCollection();
serviceCollection.AddDataProtection()
    .SetApplicationName(AppConfigConstants.ApplicationName) // MUST match the API
    .PersistKeysToFileSystem(new DirectoryInfo(keyPath));

var serviceProvider = serviceCollection.BuildServiceProvider();
var protector = serviceProvider.GetDataProtectionProvider()
    .CreateProtector(AppConfigConstants.EncryptionPurpose); // MUST match the API


ConsoleInterface.RunMainLoop(protector);
