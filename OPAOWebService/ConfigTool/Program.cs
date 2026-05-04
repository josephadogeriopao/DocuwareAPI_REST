using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
// 1. Define the shared key path
var keyPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DataProtectionKeys");

// 2. Ensure the directory exists
if (!Directory.Exists(keyPath)) Directory.CreateDirectory(keyPath);


// 1. Create the collection
var serviceCollection = new ServiceCollection();

// 2. Configure Data Protection (Notice we don't use .Services here)
serviceCollection.AddDataProtection()
    .SetApplicationName("OPAOWebService")
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DataProtectionKeys")));

// 3. Build the provider FROM the collection
var serviceProvider = serviceCollection.BuildServiceProvider();


var protector = serviceProvider.GetDataProtectionProvider()
    .CreateProtector("OPAODataProtectionEncryption");

while (true)
{
    Console.Clear();
    Console.WriteLine("--- OPAO Encryption Utility ---");
    Console.WriteLine("1. Encrypt one-by-one (Manual)");
    Console.WriteLine("2. Bulk Encrypt from File (.txt)");
    Console.WriteLine("3. Exit");
    Console.Write("\nSelect an option: ");

    string? choice = Console.ReadLine();

    if (choice == "1")
    {
        RunManualMode(protector);
    }
    else if (choice == "2")
    {
        RunBulkMode(protector);
    }
    else if (choice == "3")
    {
        break;
    }
}

static void RunManualMode(IDataProtector protector)
{
    Console.WriteLine("\n--- Manual Mode ---");
    Console.WriteLine("Enter value and press [Enter]. Press [ESC] to return to menu.");

    while (true)
    {
        string input = "";
        Console.Write("\nEnter value: ");

        // Listen for keys character by character
        while (true)
        {
            var keyInfo = Console.ReadKey(intercept: true);

            if (keyInfo.Key == ConsoleKey.Escape) return; // Exit back to main menu

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }

            if (keyInfo.Key == ConsoleKey.Backspace && input.Length > 0)
            {
                input = input[..^1];
                Console.Write("\b \b");
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                input += keyInfo.KeyChar;
                Console.Write(keyInfo.KeyChar);
            }
        }

        if (string.IsNullOrWhiteSpace(input)) continue;

        try
        {
            string encrypted = protector.Protect(input);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Encrypted: {encrypted}");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
        }
    }
}

static void RunBulkMode(IDataProtector protector)
{
    // 1. Navigate from bin\Debug\netX.X up to the Project Root (where Program.cs is)
    string executionDir = AppDomain.CurrentDomain.BaseDirectory;
    DirectoryInfo projectRootDir = Directory.GetParent(executionDir)?.Parent?.Parent?.Parent;

    if (projectRootDir == null || !projectRootDir.Exists)
    {
        Console.WriteLine("\n❌ Could not locate the project root directory.");
        Thread.Sleep(3000);
        return;
    }

    string projectPath = projectRootDir.FullName + "\\..\\";

    // 2. Scan for .txt files in the project root, excluding results we already generated
    var files = Directory.GetFiles(projectPath, "*.txt")
        .Where(f => !Path.GetFileName(f).StartsWith("encrypted_"))
        .ToList();

    if (files.Count == 0)
    {
        Console.WriteLine($"\n❌ No .txt files found in: {projectPath}");
        Thread.Sleep(3000);
        return;
    }

    // 3. Display files for choice
    Console.WriteLine($"\n--- Project Files ({projectPath}) ---");
    for (int i = 0; i < files.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");
    }
    Console.WriteLine($"{files.Count + 1}. [Cancel]");

    Console.Write("\nEnter choice number: ");
    if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > files.Count) return;

    string selectedPath = files[choice - 1];
    string fileName = Path.GetFileName(selectedPath);

    try
    {
        string[] lines = File.ReadAllLines(selectedPath);
        string outputFileName = $"encrypted_{fileName}";
        string outputPath = Path.Combine(projectPath, outputFileName);

        int processedCount = 0;
        using (StreamWriter writer = new StreamWriter(outputPath))
        {
            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();

                if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith("#") || !trimmedLine.Contains('='))
                {
                    writer.WriteLine(line);
                    continue;
                }

                int separatorIndex = trimmedLine.IndexOf('=');
                string key = trimmedLine.Substring(0, separatorIndex).Trim();
                string value = trimmedLine.Substring(separatorIndex + 1).Trim();

                if (!string.IsNullOrEmpty(value))
                {
                    writer.WriteLine($"{key}={protector.Protect(value)}");
                    processedCount++;
                }
                else
                {
                    writer.WriteLine($"{key}=");
                }
            }
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n✅ Success! {processedCount} keys encrypted.");
        Console.WriteLine($"Generated: {outputFileName} in project root.");
        Console.ResetColor();
        Console.WriteLine("\nPress any key to return to menu...");
        Console.ReadKey();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Bulk Error: {ex.Message}");
        Thread.Sleep(3000);
    }
}
