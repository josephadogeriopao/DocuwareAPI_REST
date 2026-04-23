using OPAOWebService.Server.Data.Providers.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System.Diagnostics;

namespace OPAOWebService.Server.Data.Providers
{
    public class DatabaseConnection : IDatabaseConnection
    {
        private readonly IConfiguration _configuration;

        private readonly string _connectionString;

        public DatabaseConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string GetConnectionString()
        {
            try
            {
                // 1. Get the template
                string? rawTemplate = _configuration.GetConnectionString("OracleDbConnection");
                if (string.IsNullOrEmpty(rawTemplate))
                    throw new Exception("Connection string template 'OracleDbConnection' is missing from config.");

                // 2. Pull values (automatically checks .env/IIS first because of AddEnvironmentVariables)
                string? host = _configuration["ORACLE_HOST"];
                string? port = _configuration["ORACLE_PORT"] ?? "1521";
                string? sid = _configuration["ORACLE_SID"];
                string? user = _configuration["ORACLE_USERNAME"];
                string? pass = _configuration["ORACLE_PASSWORD"];

                // 3. SECURE CHECK: Ensure we didn't get "placeholder" or null
                var fields = new Dictionary<string, string?>
                {
                    {"HOST", host}, {"SID", sid}, {"USER", user}, {"PASS", pass}
                };

                foreach (var field in fields)
                {
                    if (string.IsNullOrEmpty(field.Value) || field.Value.Equals("placeholder", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new InvalidOperationException($"Database Setting '{field.Key}' is missing or still set to 'placeholder'. Check your .env file or IIS settings.");
                    }
                }

                // 4. Return the formatted string
                return string.Format(rawTemplate, host, port, sid, user, pass);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetConnectionString Error]: {ex.Message}");
                throw; // Rethrow to stop the app from running with bad config
            }
        }

        public OracleConnection CreateConnection()
        {
            return new OracleConnection(GetConnectionString());
        }
    }
}
