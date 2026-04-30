

using IasworldTransactionService;
using OPAOWebService.Server.Business.Interfaces;
using OPAOWebService.Server.Business.Validators;
using OPAOWebService.Server.Business.Validators.Interfaces;
using OPAOWebService.Server.Data.Repositories;
using OPAOWebService.Server.Data.Repositories.Interfaces;
using OPAOWebService.Server.Factories;
using OPAOWebService.Server.Factories.Interfaces;
using OPAOWebService.Server.Infrastructure.Helpers;
using OPAOWebService.Server.Infrastructure.Security;
using OPAOWebService.Server.Infrastructure.Security.Interfaces;
using OPAOWebService.Server.Infrastructure.TransactionServiceProxy;
using OPAOWebService.Server.Models.DTOs;
using OPAOWebService.Server.Models.DTOs.Requests;
using OPAOWebService.Server.Models.Entities;
using OPAOWebService.Server.Models.Exceptions;
using OPAOWebService.Server.Utils;
using System.Diagnostics;
using System.Text.Json;
using System.Xml.Linq;

namespace OPAOWebService.Server.Business
{
    public class LogService : ILogService
    {
        private readonly string _logPath;

        public LogService(IWebHostEnvironment env)
        {
            // Path to your daily rolling log file
            _logPath = Path.Combine(env.ContentRootPath, "logs", $"api-logs-{DateTime.Now:yyyyMMdd}.json");
        }

        public async Task<List<LogEntry>> GetApiLogsAsync()
        {
            var logs = new List<LogEntry>();

            // 1. Get the directory path (Assuming _logPath was just a file, get its folder)
            string logDirectory = Path.GetDirectoryName(_logPath);
            if (!Directory.Exists(logDirectory)) return logs;

            // 2. Find all files matching the pattern (e.g., api-logs-*.json)
            // Adjust "api-logs-*.json" to match your actual filename prefix
            var logFiles = Directory.GetFiles(logDirectory, "api-logs-*.json");

            foreach (var file in logFiles)
            {
                // Use FileShare.ReadWrite so we don't crash if Serilog is currently writing to the file
                using var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var reader = new StreamReader(stream);

                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    try
                    {
                        var entry = JsonSerializer.Deserialize<LogEntry>(line, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        if (entry != null) logs.Add(entry);
                    }
                    catch { /* Skip malformed lines in older or corrupted files */ }
                }
            }

            // 3. Sort all combined logs by date (newest first)
            return logs.OrderByDescending(l => l.Datetime).ToList();
        }

    }


}
