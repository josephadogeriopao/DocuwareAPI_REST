using Microsoft.AspNetCore.DataProtection;
using OPAOWebService.Server.Data.Providers.Interfaces;
using OPAOWebService.Server.Infrastructure.Security.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System.Diagnostics;

namespace OPAOWebService.Server.Data.Providers
{
    /// <summary>
    /// Implementation of IDatabaseConnection for Oracle using Managed Data Access.
    /// Handles dynamic connection string construction from Web.config settings.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 24-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> DatabaseConnection.cs</para>
    /// </remarks>
    public class DatabaseConnection : IDatabaseConnection
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigProtector _configProtector;
        public DatabaseConnection(IConfiguration configuration, IConfigProtector configProtector)
        {
            _configuration = configuration;
            _configProtector = configProtector;
        }

        /// <summary>
        /// Retrieves the connection template and injects credentials from AppSettings.
        /// </summary>
        /// <returns>A fully formatted Oracle connection string.</returns>
        /// <exception cref="Exception">Thrown if the 'OracleIAS' connection string is missing.</exception>
        /// <exception cref="InvalidOperationException">Thrown if One or more required AppSettings are missing.</exception>
        public string GetConnectionString()
        {
            try
            {
                // 1. Get the template
                string? rawTemplate = _configuration.GetConnectionString("OracleDbConnection");
                if (string.IsNullOrEmpty(rawTemplate))
                    throw new Exception("Connection string template 'OracleDbConnection' is missing from config.");

                // 2. Pull values (automatically checks .env/IIS first because of AddEnvironmentVariables)
                // Directly decrypt as we pull from config
                Debug.WriteLine("d raw template ==> " + rawTemplate);
                Console.WriteLine("c raw template ==> " + rawTemplate);


                Debug.WriteLine("encrypted host==> " + _configuration["ORACLE_HOST"]);
                Console.WriteLine("encrypted host==> " + _configuration["ORACLE_HOST"]);
                string host = _configProtector.Decrypt(_configuration["ORACLE_HOST"], "ORACLE_HOST");
                string port = _configProtector.Decrypt(_configuration["ORACLE_PORT"], "ORACLE_PORT");
                string sid = _configProtector.Decrypt(_configuration["ORACLE_SID"], "ORACLE_SID");
                string user = _configProtector.Decrypt(_configuration["ORACLE_USERNAME"], "ORACLE_USERNAME");
                string pass = _configProtector.Decrypt(_configuration["ORACLE_PASSWORD"], "ORACLE_PASSWORD");

                Debug.WriteLine("decrypted host==> " + host);
                Console.WriteLine("decrypted host==> " + host);

                Debug.WriteLine(" oracle string done ==> " + string.Format(rawTemplate, host, port, sid, user, pass));
                Console.WriteLine("oracle string done ==> " + string.Format(rawTemplate, host, port, sid, user, pass));




                // 3. SECURE CHECK: Ensure we didn't get "placeholder" or null
                var fields = new Dictionary<string, string?>
                {
                    {"HOST", host}, {"SID", sid}, {"USER", user}, {"PASS", pass}
                };
                Console.WriteLine("fields ==> " + fields.ToString());
                Debug.WriteLine("fields ==> " + fields.ToString());

                

                ValidateFields(fields);

                // 4. Return the formatted string
                return string.Format(rawTemplate, host, port, sid, user, pass);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetConnectionString Error]: {ex.Message}");
                throw; // Rethrow to stop the app from running with bad config
            }
        }

        /// <summary>
        /// Instantiates a new OracleConnection using the generated connection string.
        /// </summary>
        /// <returns>An un-opened OracleConnection object.</returns>
        /// <inheritdoc/>
        public OracleConnection CreateConnection()
        {
            OracleConnection.ClearPool(new OracleConnection(GetConnectionString()));

            return new OracleConnection(GetConnectionString());
        }

        /// <summary>
        /// Validates that the provided configuration values are not null, empty, or set to "placeholder".
        /// </summary>
        /// <param name="fieldss">A collection of string values to validate.</param>
        /// <exception cref="Exception">Thrown when a value is missing or contains default placeholder text.</exception>
        private void ValidateFields(Dictionary<string, string?> fields)
        {
            foreach (var field in fields)
            {
                if (string.IsNullOrEmpty(field.Value) || field.Value.Equals("placeholder", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException($"Database Setting '{field.Key}' is missing or still set to 'placeholder'. Check your .env file or IIS settings.");
                }
            }
        }
    }
}
