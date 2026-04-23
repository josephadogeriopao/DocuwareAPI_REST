using OPAOWebService.Server.Data.Providers.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System.Diagnostics;

namespace OPAOWebService.Server.Data.Providers
{
    public class DatabaseConnection : IDatabaseConnection
    {
        private readonly IConfiguration _configuration;

        // Constructor Injection - Traditional Style
        public DatabaseConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString()
        {
            try
            {
                // In .NET Core, we get the section directly
                string? rawTemplate = _configuration.GetConnectionString("OracleIAS");

                if (string.IsNullOrEmpty(rawTemplate))
                    throw new Exception("Connection string 'OracleIAS' is missing.");

                // .NET Core looks at appsettings, then .env, then IIS automatically
                string? host = _configuration["ORACLE_HOSTNAME"];
                string? port = _configuration["ORACLE_PORT"];
                string? sid = _configuration["ORACLE_SID"];
                string? user = _configuration["ORACLE_USERNAME"];
                string? pass = _configuration["ORACLE_PASSWORD"];

                Console.WriteLine(host, port, sid, user, pass);
                Debug.WriteLine(host, sid, user, pass, port);

                if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port) || string.IsNullOrEmpty(sid) ||
                    string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
                {
                    throw new InvalidOperationException("One or more required settings (ORACLE_HOSTNAME, etc.) are missing.");
                }

                return string.Format(rawTemplate, host, port, sid, user, pass);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DatabaseConnection.GetConnectionString] Error: {ex.Message}");
                throw new Exception("Failed to initialize database connection. Check environment settings.", ex);
            }
        }

        public OracleConnection CreateConnection()
        {
            return new OracleConnection(GetConnectionString());
        }
    }
}