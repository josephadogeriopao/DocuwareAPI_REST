using IasworldTransactionService;
using OPAOWebService.Server.Factories.Interfaces;
using System.Diagnostics;

namespace OPAOWebService.Server.Factories
{
    /// <summary>
    /// Factory class responsible for building <see cref="TransactionGetRequest"/> objects.
    /// Maps input parameters and Web.config settings into the iasWorld request structure.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 23-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> TransactionGetRequestFactory.cs</para>
    /// </remarks>
    public class TransactionGetRequestFactory : ITransactionGetRequestFactory
    {

        private readonly IConfiguration _configuration;

        // Traditional constructor injection
        public TransactionGetRequestFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// Creates and populates a new TransactionGetRequest for a specific parcel and tax year.
        /// </summary>
        /// <param name="parcelId">The Parcel Identification Number to query.</param>
        /// <param name="taxYear">The tax year associated with the request.</param>
        /// <returns>A fully initialized <see cref="TransactionGetRequest"/> object.</returns>
        public TransactionGetRequest Create(string parcelId, int taxYear)
        {
            Debug.WriteLine("values to create new object " + parcelId + " , " + taxYear);

            TransactionGetRequest req = new TransactionGetRequest
            {
                TaxYear = taxYear,
                Jurisdiction = "9",
                TransactionName = _configuration["TRANS_NAME"]?.ToString() ?? "DefaultValue",
                IncludeDeactivatedRecords = true,
                IncludeHistoryRecords = false,
                SubjectId = parcelId
            };

            return req;
        }

    }

}