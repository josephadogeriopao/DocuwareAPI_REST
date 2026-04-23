namespace OPAOWebService.Server.Data.Constants
{
    /// <summary>
    /// Central repository for SQL queries used by the web service.
    /// Includes both string-interpolated and parameterized query versions.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 23-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> SqlCommands.cs</para>
    /// </remarks>
    public static class SqlCommands
    {
        /// <summary>
        /// Retrieves the current tax year for jurisdiction '9' using Environment.NewLine formatting.
        /// Clean, verbatim string version of the tax year lookup.
        /// </summary>
        public const string GetCurrentTaxYear = """
        SELECT THISYEAR 
        FROM AASYSJUR A 
        WHERE A.JUR = '9'
        """;

        /// <summary>
        /// Checks if a Parcel ID exists for a specific tax year using string interpolation.
        /// Secured version of the Parcel ID check using Oracle/Standard bind variables (:parid, :taxyr).
        /// Recommended for preventing SQL injection.
        /// </summary>
        public const string IsValidParcelId = """
        SELECT 
            CASE 
                WHEN EXISTS (
                    SELECT 1 FROM ASMT A 
                    WHERE A.parid = :parid 
                    AND A.taxyr = :taxyr 
                    AND A.cur = 'Y'
                ) THEN 'Y' 
                ELSE 'N' 
            END 
        FROM DUAL
        """;
    }
}