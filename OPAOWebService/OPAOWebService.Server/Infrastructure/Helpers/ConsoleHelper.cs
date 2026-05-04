using Microsoft.AspNetCore.DataProtection;
using OPAOWebService.Server.Infrastructure.Extensions;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;




namespace OPAOWebService.Server.Infrastructure.Helpers
{
    public static class ConsoleHelper
    {
        public static void DisplayApplicationLogo()
        {
            // 1. Load the image and resize it (Console width is limited)
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "assets", "seal.jpg"); 
            if (!File.Exists(path))
            {
                Console.WriteLine("Logo file not found at: " + path);
                return;
            }
            Bitmap bmp = new Bitmap(path);
            int width = 60; // Standard console width
            int height = (bmp.Height * width) / bmp.Width / 2; // Compensate for text height
            Bitmap resizedBmp = new Bitmap(bmp, new Size(width, height));

            // 2. Define characters from darkest to lightest
            string asciiChars = "@#S%?*+;:,. ";

            StringBuilder asciiArt = new StringBuilder();

            for (int y = 0; y < resizedBmp.Height; y++)
            {
                for (int x = 0; x < resizedBmp.Width; x++)
                {
                    // 3. Get pixel brightness (0 to 1)
                    Color pixel = resizedBmp.GetPixel(x, y);
                    float brightness = pixel.GetBrightness();

                    // 4. Map brightness to our string index
                    int index = (int)(brightness * (asciiChars.Length - 1));
                    asciiArt.Append(asciiChars[index]);
                }
                asciiArt.AppendLine();
            }

            // 5. Print the "string image"
            Console.Write(asciiArt.ToString());
            // Use UTF-8 to ensure symbols and spacing render correctly
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.Cyan;



            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("==============================================");
            Console.WriteLine("       ORLEANS PARISH ASSESSOR'S OFFICE       ");
            Console.WriteLine("           City of New Orleans, LA            ");
            Console.WriteLine("==============================================");

            Console.ResetColor();


            // The equivalent of your second console.log
            Debug.WriteLine("DEBUG: Orleans Parish Assessor's Office Logo Loaded");

        }
    }
}
