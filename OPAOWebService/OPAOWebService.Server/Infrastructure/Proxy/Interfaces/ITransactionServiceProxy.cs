
using IasworldTransactionService;

namespace OPAOWebService.Server.Infrastructure.Proxy.Interfaces
{
    /// <summary>
    /// Defines a high-level proxy for interacting with the iasWorld Transaction Service.
    /// Abstracts the complexities of the underlying WCF client, handling XML retrieval,
    /// validation, and lifecycle management of transactions.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 23-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> ITransactionServiceProxy.cs</para>
    /// </remarks>
    public interface ITransactionServiceProxy
    {
        /// <summary>
        /// Retrieves the XML representation of a record from iasWorld based on the provided request parameters.
        /// </summary>
        /// <param name="req">The configured <see cref="TransactionGetRequest"/> containing the Subject ID and Tax Year.</param>
        /// <returns>A string containing the raw XML data of the requested transaction.</returns>
        string GetTransactionXml(TransactionGetRequest req);

        /// <summary>
        /// Performs a validation-only check on the provided XML string against iasWorld business rules
        /// without committing changes to the database.
        /// </summary>
        /// <param name="xml">The transaction XML to be validated.</param>
        /// <returns>A <see cref="TransactionSubmitResponse"/> containing success status and any validation messages.</returns>
        TransactionSubmitResponse ValidateTransactionXml(string xml);

        /// <summary>
        /// Submits the transaction XML to iasWorld to be processed/committed based on the specified submission mode.
        /// </summary>
        /// <param name="xml">The transaction XML to submit.</param>
        /// <param name="mode">The submission mode (e.g., Save, Post, or Close).</param>
        /// <returns>A <see cref="TransactionSubmitResponse"/> indicating the result of the submission.</returns>
        TransactionSubmitResponse SubmitTransactionXml(string xml, TransactionSubmitMode mode);

        /// <summary>
        /// Transfers an existing transaction's ownership to a different iasWorld user.
        /// </summary>
        /// <param name="transactionId">The unique ID of the transaction to reassign.</param>
        /// <param name="toUsername">The username of the new owner.</param>
        /// <returns>True if the reassignment was successful; otherwise, false.</returns>
        bool ReassignTransaction(int transactionId, string toUsername);

        /// <summary>
        /// Cancels and removes an active transaction from the iasWorld system.
        /// </summary>
        /// <param name="transactionId">The unique ID of the transaction to abandon.</param>
        /// <returns>True if the transaction was successfully abandoned; otherwise, false.</returns>
        bool AbandonTransaction(int transactionId);
    }
}