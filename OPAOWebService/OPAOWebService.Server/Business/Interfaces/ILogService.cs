using OPAOWebService.Server.Models.DTOs.Requests;

namespace OPAOWebService.Server.Business.Interfaces
{
    /// <summary>
    /// Defines business logic operations for processing tax-related property data.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 23-APR-2026</para>
    /// <para><strong>Version:</strong> 1.1.0</para>
    /// <para><strong>File:</strong> ITaxService.cs</para>
    /// </remarks>

    public interface ITaxService
    {

        /// <summary>
        /// Updates the valuation status of a property parcel.
        /// </summary>
        /// <param name="request">The assessment update details.</param>
        /// <returns>An integer status code (e.g., 1 for success, 0 for failure).</returns>
        int UpdatePropertyValuation(AssessmentStatusRequest request);
    }
}
