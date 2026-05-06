using Microsoft.AspNetCore.DataProtection;
using OPAOWebService.Shared.Constants;

namespace OPAOWebService.ConfigTool
{
    public static class ConsoleInterface
    {
        public static void RunMainLoop(IDataProtector protector)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- OPAO Encryption Utility ---");
                Console.WriteLine("1. Encrypt one-by-one (Manual)");
                Console.WriteLine("2. Bulk Encrypt from File (.txt)");
                Console.WriteLine("3. Decrypt (Verify)");
                Console.WriteLine("4. Exit");
                Console.Write("\nSelect an option: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        EncryptionService.RunManualMode(protector);
                        break;
                    case "2":
                        EncryptionService.RunBulkMode(protector);
                        break;
                    case "3":
                        EncryptionService.RunDecryptionMode(protector);
                        break;
                    case "4":
                        return;
                }
            }
        }

        public static string selectEnvironmentPane()
        {
            string env = "";
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== OPAO Config Tool ===");
                Console.WriteLine("1) Production");
                Console.WriteLine("2) Development");
                Console.WriteLine("X) Exit");
                Console.Write("\nSelection: ");

                string input = Console.ReadLine()?.ToUpper();

                if (input == "1")
                {
                    env = AppConfigConstants.ProductionEnvironment;
                    break;
                }
                if (input == "2")
                {
                    env = AppConfigConstants.DevelopmentionEnvironment;
                    break;
                }
                if (input == "X")
                {
                    return null; // Closes the application
                }

                Console.WriteLine("Invalid selection. Press any key to try again...");
                Console.ReadKey();
            }
            return env;


        }
    }
}