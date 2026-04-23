namespace OPAOWebService.Server.Data.Repositories.Interfaces
{  
    /// <summary>
   /// Defines the contract for tax-related data operations and property validations.
   /// Handles interactions with assessment and jurisdiction data.
   /// </summary>
   /// <remarks>
   /// <para><strong>Author:</strong> Joseph Adogeri</para>
   /// <para><strong>Since:</strong> 23-APR-2026</para>
   /// <para><strong>Version:</strong> 1.0.0</para>
   /// <para><strong>File:</strong> ITaxRepository.cs</para>
   /// </remarks>
    public interface ITaxRepository
    {
        /// <summary>
        /// Verifies if a specific Parcel ID exists and is active for the given tax year.
        /// </summary>
        /// <param name="parcelId">The unique Parcel Identification number.</param>
        /// <param name="taxYear">The specific tax year to validate against.</param>
        /// <returns>True if the parcel is valid and current; otherwise, false.</returns>
        bool IsValidParcelId(string parcelId, int taxYear);

        /// <summary>
        /// Retrieves the current active tax year from the jurisdiction settings.
        /// </summary>
        /// <returns>The integer value of the current tax year (e.g., 2024).</returns>
        int GetCurrentTaxYear();

    }
}