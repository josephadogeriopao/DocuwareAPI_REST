using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OPAOWebService.ConfigTool;
using OPAOWebService.Shared.Constants;
using System.Diagnostics;


string env = ConsoleInterface.selectEnvironmentPane();

Console.WriteLine($"environment ==> {env}");
Debug.WriteLine($"environment ==> {env}");
Console.WriteLine($"\nWorking in environment ==> {env}");
Thread.Sleep(4000);

if (env != null)
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: true)
        // ADD THIS LINE to allow Development overrides
        .AddJsonFile($"appsettings.{env}.json", optional: true)
        .AddEnvironmentVariables()
        .Build();

    // Logic for keyPath: 
    // 1. Check if it's in appsettings (highest priority)
    // 2. If missing AND Prod: Use a fixed Server path
    // 3. If missing AND Dev: Use LocalAppData
    //var keyPath = builder[AppConfigConstants.DataProtectionConfigPath]
    //              ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ASP.NET", "OPAOWebService");
    var keyPath = builder[AppConfigConstants.DataProtectionConfigPath];

    if (string.IsNullOrEmpty(keyPath))
    {
        if (env == AppConfigConstants.ProductionEnvironment)        // Change this to wherever your IIS server stores its keys!
            keyPath = @"C:\Keys\OPAOWebService";
        else
            keyPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ASP.NET", "OPAOWebService");
    }

    Console.WriteLine($"Target key path ==> {keyPath}");
    Thread.Sleep(4000);

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
}
