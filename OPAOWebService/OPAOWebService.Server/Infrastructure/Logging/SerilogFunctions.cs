using Serilog.Events;

namespace OPAOWebService.Server.Infrastructure.Logging
{
    /// <summary>
    /// Provides custom functions for Serilog Expression Templates.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 30-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> SerilogFunctions.cs</para>
    /// </remarks>
    public static class SerilogFunctions
    {
        /// <summary>
        /// Converts a Serilog LogEventPropertyValue (DateTimeOffset) into Unix Milliseconds.
        /// Used by the ExpressionTemplate to generate numeric IDs for the React frontend.
        /// </summary>
        /// <param name="value">The raw property value from the Serilog event (usually @t).</param>
        /// <returns>A ScalarValue containing the long millisecond timestamp, or null if invalid.</returns>
        public static LogEventPropertyValue? ToUnixMs(LogEventPropertyValue? value)
        {
            // Try to unwrap the value into a C# DateTimeOffset
            if (value is ScalarValue scalar && scalar.Value is DateTimeOffset dto)
            {
                // Return a new ScalarValue containing the total milliseconds since Unix Epoch
                return new ScalarValue(dto.Ticks);
            }

            // If the input isn't a date, return null (undefined) to the template
            return null;
        }
    }
}
