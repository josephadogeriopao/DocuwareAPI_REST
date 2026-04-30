namespace OPAOWebService.Server.Data.Constants
{
    public static class LoggingConstants
    {
        // Your specific React JSON format
        public static string ReactJsonTemplate =
        "{ { " +
        "id: ToUnixMs(@t), " +
        "datetime: @t, " +
        "parcelId: parcelId, " +
        "tier: tier, " +
        // This will grab the 'exceptionType' property you sent from the Controller
        // If it's missing, it will just show 'Error'
        "exception: Coalesce(exceptionType, 'Error'), " +
        "description: @m, " +
        "stacktrace: @x " +
        "} }\n";

        // The filter that keeps framework noise out
        public const string AppNamespaceFilter = "StartsWith(SourceContext, 'OPAOWebService')";
        // The Oracle error filter
        public const string OracleFilter = "SourceContext = 'Database' or @m like '%DB Error%' or (@x is not null and @x like '%Oracle%')";
    }
}
