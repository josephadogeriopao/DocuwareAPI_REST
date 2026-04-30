using System;

namespace OPAOWebService.Server.Data.Constants
{
    /// <summary>
    /// Defines centralized constants for the Serilog logging infrastructure.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 30-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> LoggingConstants.cs</para>
    /// </remarks>
    public static class LoggingConstants
    {
        /// <summary>
        /// The Serilog ExpressionTemplate used to format logs into the specific JSON 
        /// structure required by the React frontend dashboard.
        /// </summary>
        public static string ReactJsonTemplate =
        "{ { " +
        "id: ToUnixMs(@t), " +
        "datetime: @t, " +
        "parcelId: parcelId, " +
        "tier: tier, " +
        "exception: Coalesce(exceptionType, 'Error'), " +
        "description: @m, " +
        "stacktrace: @x " +
        "} }\n";

        /// <summary>
        /// A Serilog filter expression to exclude framework noise and only include 
        /// logs originating from the OPAOWebService namespace.
        /// </summary>
        public const string AppNamespaceFilter = "StartsWith(SourceContext, 'OPAOWebService')";

        /// <summary>
        /// A Serilog filter expression specifically designed to identify database-related 
        /// errors or messages containing Oracle-specific keywords.
        /// </summary>
        public const string OracleFilter = "SourceContext = 'Database' or @m like '%DB Error%' or (@x is not null and @x like '%Oracle%')";
    }
}
