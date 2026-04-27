/// <summary>
/// Provides methods for decrypting configuration settings at runtime.
/// </summary>
/// <remarks>
/// <para><strong>Author:</strong> Joseph Adogeri</para>
/// <para><strong>Since:</strong> 27-APR-2026</para>
/// <para><strong>Version:</strong> 1.0.0</para>
/// <para><strong>File:</strong> IConfigProtector.cs</para>
/// </remarks>
namespace OPAOWebService.Server.Infrastructure.Security.Interfaces
{
    public interface IConfigProtector
    {
        /// <summary>
        /// Decrypts a specific configuration value and validates against placeholders.
        /// </summary>
        /// <param name="encryptedValue">The protected string from config.</param>
        /// <param name="fieldName">The name of the field (for logging/errors).</param>
        /// <returns>A plain-text decrypted string.</returns>
        string Decrypt(string? encryptedValue, string fieldName);
    }
}
