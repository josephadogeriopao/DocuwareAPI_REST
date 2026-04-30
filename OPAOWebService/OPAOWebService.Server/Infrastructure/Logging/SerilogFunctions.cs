using Serilog.Events;

namespace OPAOWebService.Server.Infrastructure.Logging
{
    public static class SerilogFunctions
    {
        // Custom functions in Serilog Expressions must take and return LogEventPropertyValue
        public static LogEventPropertyValue? ToUnixMs(LogEventPropertyValue? value)
        {
            // Try to unwrap the value into a C# DateTimeOffset
            if (value is ScalarValue scalar && scalar.Value is DateTimeOffset dto)
            {
                // Return a new ScalarValue containing the milliseconds
                return new ScalarValue(dto.ToUnixTimeMilliseconds());
            }

            // If the input isn't a date, return null (undefined)
            return null;
        }
    }
}
