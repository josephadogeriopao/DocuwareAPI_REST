using Microsoft.AspNetCore.DataProtection;
using OPAOWebService.Server.Infrastructure.Security.Interfaces;
using System.Security.Cryptography;

namespace OPAOWebService.Server.Infrastructure.Security
{
    /// <summary>
    /// Implementation of IConfigProtector using the .NET Data Protection API (DPAPI).
    /// Provides secure decryption for sensitive configuration values stored in environment variables or .env files.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 27-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> ConfigProtector.cs</para>
    /// </remarks>
    public class ConfigProtector : IConfigProtector
    {
        private readonly IDataProtector _protector;

        /// <summary>
        /// Initializes a new instance of the ConfigProtector class.
        /// </summary>
        /// <param name="provider">The data protection provider injected by the DI container.</param>
        public ConfigProtector(IDataProtectionProvider provider)
        {
            // The "Purpose String" ensures that only this specific application 
            // can decrypt values encrypted with the same purpose.
            _protector = provider.CreateProtector("OPAODataProtectionEncryption");
        }

        public ConfigProtector() { }

        /// <summary>
        /// Decrypts a protected configuration value and validates that it is not a placeholder.
        /// </summary>
        /// <param name="encryptedValue">The encrypted string from the configuration source.</param>
        /// <param name="fieldName">The name of the field (used for detailed error reporting).</param>
        /// <returns>The plain-text decrypted value.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the value is missing or set to 'placeholder'.</exception>
        /// <exception cref="Exception">Thrown when decryption fails (e.g., if encrypted on a different machine).</exception>
        public string Decrypt(string? encryptedValue, string fieldName)
        {
            // 1. Validate the input is present and not a default placeholder
            if (string.IsNullOrEmpty(encryptedValue) || encryptedValue.Equals("placeholder", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Configuration setting '{fieldName}' is missing or still set to 'placeholder'. Please check your .env file or IIS environment variables.");
            }

            try
            {
                // 2. Perform the decryption (Unprotect)
                return _protector.Unprotect(encryptedValue);
            }
            catch (CryptographicException ex)
            {
                // 3. Provide context-specific error messages (crucial for troubleshooting machine-bound DPAPI)
                throw new CryptographicException(
                          $"Decryption failed for '{fieldName}'. Ensure the value was encrypted on this machine/account.", ex);
            }
        }
    }
}
