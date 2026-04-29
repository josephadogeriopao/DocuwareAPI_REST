using Serilog;
using Serilog.Events;
using Serilog.Expressions;
using Serilog.Formatting.Compact;

namespace OPAOWebService.Server.Infrastructure.Extensions
{
    public static class LoggingExtensions
    {
        public static void AddSerilogLogging(this ConfigureHostBuilder host, IConfiguration configuration, string contentRootPath)
        {
            host.UseSerilog((context, services, loggerConfiguration) =>
            {
                string logDirectory = Path.Combine(contentRootPath, "logs");

                if (!Directory.Exists(logDirectory))
                    Directory.CreateDirectory(logDirectory);

                loggerConfiguration
                    .MinimumLevel.Fatal()
                    .MinimumLevel.Override("OPAOWebService", Serilog.Events.LogEventLevel.Debug)
                    .Enrich.FromLogContext()
                                // FILE A: General Logs
                                //.WriteTo.Logger(lc => lc
                                //    .Filter.ByExcluding(oracleFilter)
                                //    .WriteTo.File(
                                //        path: Path.Combine(logDirectory, "general-exceptions-.txt"),
                                //        rollingInterval: RollingInterval.Day))
                                //// FILE B: Database Logs
                                //.WriteTo.Logger(lc => lc
                                //    .Filter.ByIncludingOnly(oracleFilter)
                                //    .WriteTo.File(
                                //        path: Path.Combine(logDirectory, "database-errors-.txt"),
                                //        rollingInterval: RollingInterval.Day))
                                //.WriteTo.File(
                                //    path: Path.Combine(logDirectory, "full-audit-.txt"),
                                //    rollingInterval: RollingInterval.Day,
                                //    fileSizeLimitBytes: 10 * 1024 * 1024,
                                //    retainedFileCountLimit: 14)
                        .WriteTo.Logger(lc => lc
                            .Filter.ByIncludingOnly("StartsWith(SourceContext, 'OPAOWebService')")
                                .WriteTo.File(
                                    new RenderedCompactJsonFormatter(), // Formats as JSON
                                    path: Path.Combine(logDirectory, "api-logs.json"),
                                    rollingInterval: RollingInterval.Day,
                                    fileSizeLimitBytes: 10 * 1024 * 1024,
                                    retainedFileCountLimit: 7); 

            });
        }
    }
}
