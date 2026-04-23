using System.Runtime.Serialization;

namespace OPAOWebService.Server.Models.DTOs.Requests
{
    /// <summary>
    /// Data contract for the assessment update status service request.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 22-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0</para>
    /// <para><strong>File:</strong> AssessmentStatusRequest.cs</para>
    /// </remarks>
    public class AssessmentStatusRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier for the property parcel.
        /// </summary>
        public required string ParcelId { get; set; }

        /// <summary>
        /// Gets or sets the code indicating the reason for the assessment update.
        /// </summary>
        public required string ReasonCode { get; set; }

        /// <summary>
        /// Gets or sets the updated assessment value for the land.
        /// </summary>
        public int RevisedLand { get; set; }

        /// <summary>
        /// Gets or sets the updated assessment value for the building/improvements.
        /// </summary>
        public int RevisedBldg { get; set; }

        /// <summary>
        /// Gets or sets the total updated assessment value (Land + Building).
        /// </summary>
        public int RevisedTot { get; set; }

        /// <summary>
        /// Gets or sets additional comments or justifications for the update.
        /// </summary>
        public required string Notes { get; set; }

        /// <summary>
        /// Gets or sets the specific manual override or process code.
        /// </summary>
        public required string ManualCode { get; set; }
    }
}
