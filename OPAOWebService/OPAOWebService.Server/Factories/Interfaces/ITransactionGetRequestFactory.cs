using IasworldTransactionService;

namespace OPAOWebService.Server.Factories.Interfaces
{
    /// <summary>
    /// Defines a factory for creating <see cref="TransactionGetRequest"/> objects.
    /// Abstracts the mapping of parcel data into the service request structure.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 23-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> ITransactionGetRequestFactory.cs</para>
    /// </remarks>
    public interface ITransactionGetRequestFactory
    {
        /// <summary>
        /// Creates a new instance of <see cref="TransactionGetRequest"/> based on the provided parameters.
        /// </summary>
        /// <param name="parcelId">The Parcel Identification Number (PARID) to query.</param>
        /// <param name="taxYear">The specific tax year for the property assessment.</param>
        /// <returns>A configured <see cref="TransactionGetRequest"/> object ready for the Web Service call.</returns>
        TransactionGetRequest Create(string parcelId, int taxYear);
    }
}