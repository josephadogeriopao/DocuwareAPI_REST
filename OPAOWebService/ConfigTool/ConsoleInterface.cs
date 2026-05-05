using Microsoft.AspNetCore.DataProtection;

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
    }
}