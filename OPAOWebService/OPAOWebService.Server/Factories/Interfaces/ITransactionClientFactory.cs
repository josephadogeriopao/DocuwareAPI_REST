using IasworldTransactionService;

namespace OPAOWebService.Server.Factories.Interfaces
{
    /// <summary>
    /// Defines a factory for creating authenticated instances of the iasWorld TransactionClient.
    /// This abstracts the client creation and credential injection process.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 23-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> ITransactionClientFactory.cs</para>
    /// </remarks>
    public interface ITransactionClientFactory
    {
        /// <summary>
        /// Creates and configures a new TransactionClient with the provided credentials.
        /// </summary>
        /// <param name="username">The iasWorld service account username.</param>
        /// <param name="password">The iasWorld service account password.</param>
        /// <returns>An initialized TransactionClient ready for service calls.</returns>
        public TransactionClient Create(string username, string password);
    }

}