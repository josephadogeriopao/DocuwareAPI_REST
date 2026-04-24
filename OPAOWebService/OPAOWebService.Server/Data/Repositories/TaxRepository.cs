using OPAOWebService.Server.Data.Constants;
using OPAOWebService.Server.Data.Providers;
using OPAOWebService.Server.Data.Providers.Interfaces;
using OPAOWebService.Server.Data.Repositories.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System.Diagnostics;

namespace OPAOWebService.Server.Data.Repositories
{
    /// <summary>
    /// Repository implementation for tax data operations.
    /// Handles database interactions for tax years and parcel validations using Oracle.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 24-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> TaxRepository.cs</para>
    /// </remarks>
    public class TaxRepository : ITaxRepository
    {
        private readonly IDatabaseConnection _databaseConnection;

        public TaxRepository(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        /// <summary>
        /// Queries the database to retrieve the active tax year from the AASYSJUR table.
        /// </summary>
        /// <returns>The current tax year as an integer; returns -1 if not found.</returns>
        /// <exception cref="Exception">Re-throws database exceptions for WCF fault handling.</exception>
        public int GetCurrentTaxYear()
        {
            int currentTaxYear = -1;

            // 'using' ensures the connection is CLOSED and DISPOSED automatically
            using (OracleConnection conn = _databaseConnection.CreateConnection())
            {
                try
                {
                    conn.Open();
                    String sqlQuery = SqlCommands.GetCurrentTaxYear;
                    Debug.WriteLine("sql query get tax year: " + sqlQuery);

                    using (OracleCommand cmd = new OracleCommand(sqlQuery, conn))
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            currentTaxYear = reader.GetInt32(reader.GetOrdinal("THISYEAR"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Database Error: " + ex.ToString());
                    throw; // Important for WCF to report the fault
                }
            } // <--- Connection closes HERE automatically

            return currentTaxYear;

        }
        /// <summary>
        /// Checks if a parcel is valid for a specific tax year using secure bind variables.
        /// </summary>
        /// <param name="parcelId">The parcel identification string.</param>
        /// <param name="taxYear">The numeric tax year.</param>
        /// <returns>True if the database returns 'true', otherwise false.</returns>
        public bool IsValidParcelId(string parcelId, int taxYear)
        {
            // 'using' ensures the connection is CLOSED and DISPOSED automatically
            using (OracleConnection conn = _databaseConnection.CreateConnection())
            {
                try
                {
                    conn.Open();
                    string sqlQuery = SqlCommands.IsValidParcelId;
                    Debug.WriteLine("sql query is valid parid : " + sqlQuery);

                    using (var cmd = new OracleCommand(sqlQuery, conn))
                    {
                        // ADD PARAMETERS HERE - Safe and Secure
                        cmd.Parameters.Add(new OracleParameter("parid", parcelId));
                        cmd.Parameters.Add(new OracleParameter("taxyr", taxYear));

                        var result = cmd.ExecuteScalar()?.ToString();
                        return result == "true";
                    }
                }
                catch (OracleException ex)
                {
                    ////Log.ForContext("SourceContext", "Database")
                    //   .Error(ex, "Oracle Error validating Parid: {Parid}", parcelId);
                    throw;
                }
            }
        }

    }
}