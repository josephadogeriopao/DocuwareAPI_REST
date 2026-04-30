using OPAOWebService.Server.Data.Constants;
using Serilog;
using Serilog.Events;
using Serilog.Expressions;
using Serilog.Formatting.Compact;
using Serilog.Templates;
using OPAOWebService.Server.Infrastructure.Logging;

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

                string oracleFilter = "SourceContext = 'Database' or @m like '%DB Error%' or (@x is not null and @x like '%Oracle%')";

                loggerConfiguration
                    // 1. Mute EVERYTHING by default (even Fatal startup errors)
                    .MinimumLevel.Fatal()

                    // 2. Kill specific framework noise
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
                    .MinimumLevel.Override("System", LogEventLevel.Fatal)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Fatal)

                    // 3. WHITELIST your project (Only your code can speak)
                    .MinimumLevel.Override("OPAOWebService", LogEventLevel.Debug)

                    .Enrich.FromLogContext()

                    // General Log Sink
                    .WriteTo.Logger(lc => lc
                        .Filter.ByIncludingOnly("StartsWith(SourceContext, 'OPAOWebService')")
                        .WriteTo.File(
                            path: Path.Combine(logDirectory, "general-.txt"),
                            rollingInterval: RollingInterval.Day))

                    // JSON Log Sink for React
                    .WriteTo.Logger(lc => lc
                        .Filter.ByIncludingOnly(LoggingConstants.AppNamespaceFilter)
                        .WriteTo.File(
                        new ExpressionTemplate(LoggingConstants.ReactJsonTemplate, nameResolver: new StaticMemberNameResolver(typeof(SerilogFunctions))),
                            // new RenderedCompactJsonFormatter(),
                            path: Path.Combine(logDirectory, "api-logs-.json"),
                            rollingInterval: RollingInterval.Day));
            });
        }
    }

}
