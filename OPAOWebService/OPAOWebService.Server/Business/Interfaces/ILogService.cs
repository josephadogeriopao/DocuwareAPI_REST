using OPAOWebService.Server.Models.DTOs;

namespace OPAOWebService.Server.Business.Interfaces
{
    /// <summary>
    /// Defines the contract for retrieving and processing application log data.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 30-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> ILogService.cs</para>
    /// </remarks>
    public interface ILogService
    {
        /// <summary>
        /// Asynchronously retrieves a list of parsed log entries from the NDJSON source.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing a list of <see cref="LogEntry"/>.</returns>
        Task<List<LogEntry>> GetApiLogsAsync();
    }
}
