using Oracle.ManagedDataAccess.Client;

namespace OPAOWebService.Server.Data.Providers.Interfaces
{
    /// <summary>
    /// Defines the contract for providing and managing Oracle database connections.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 23-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> IDatabaseConnection.cs</para>
    /// </remarks>
    public interface IDatabaseConnection
    {
        /// <summary>
        /// Retrieves the formatted connection string from the configuration source.
        /// </summary>
        /// <returns>A string containing data source, user ID, and password.</returns>
        string GetConnectionString();

        /// <summary>
        /// Initializes and returns a new OracleConnection object.
        /// </summary>
        /// <returns>An instance of OracleConnection ready for opening.</returns>
        OracleConnection CreateConnection();
    }
}