using System;

namespace OPAOWebService.Server.Models.DTOs
{
    /// <summary>
    /// Represents a log entry record parsed from the Serilog NDJSON source.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 30-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> LogEntry.cs</para>
    /// </remarks>
    public class LogEntry
    {
        /// <summary>Gets or sets the Unique Millisecond Timestamp ID.</summary>
        public long Id { get; set; }

        /// <summary>Gets or sets the Date and Time of the log event.</summary>
        public DateTimeOffset Datetime { get; set; }

        /// <summary>Gets or sets the associated Parcel Identifier.</summary>
        public string? ParcelId { get; set; }

        /// <summary>Gets or sets the Application Tier (e.g., Presentation, Infrastructure).</summary>
        public string? Tier { get; set; }

        /// <summary>Gets or sets the Short Exception Name (e.g., ParcelLockedException).</summary>
        public string? Exception { get; set; }

        /// <summary>Gets or sets the Human-readable message description.</summary>
        public string? Description { get; set; }

        /// <summary>Gets or sets the Technical Stack Trace for debugging.</summary>
        public string? Stacktrace { get; set; }
    }
}
