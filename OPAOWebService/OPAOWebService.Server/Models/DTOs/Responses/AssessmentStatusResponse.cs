using System.Runtime.Serialization;

namespace OPAOWebService.Server.Models.DTOs.Responses
{
    /// <summary>
    /// Data contract for the assessment update status service response.
    /// </summary>
    /// /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 22-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0</para>
    /// <para><strong>File:</strong> AssessmentStatusResponse.cs</para>
    /// </remarks>
    public class AssessmentStatusResponse
    {
        /// <summary>
        /// Gets or sets the result status of the operation (0 for failure, 1 for success).
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets a descriptive message providing more detail about the status.
        /// </summary>
        public required string Message { get; set; }
    }
}
