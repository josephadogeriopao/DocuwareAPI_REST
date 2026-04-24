using IasworldTransactionService;
using OPAOWebService.Server.Factories.Interfaces;
using OPAOWebService.Server.Infrastructure.Security;
using System.Diagnostics;

namespace OPAOWebService.Server.Factories
{
    /// <summary>
    /// Factory class responsible for instantiating and configuring the iasWorld TransactionClient.
    /// It specifically handles the injection of custom SOAP security credentials.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 23-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> TransactionClientFactory.cs</para>
    /// </remarks>
    public class TransactionClientFactory : ITransactionClientFactory
    {
        /// <summary>
        /// Creates a TransactionClient and replaces standard credentials with <see cref="CustomCredentials"/>.
        /// </summary>
        /// <param name="username">The service account username.</param>
        /// <param name="password">The service account password.</param>
        /// <returns>A configured <see cref="TransactionClient"/>.</returns>
        /// <exception cref="ArgumentException">Thrown if credentials are missing.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the endpoint configuration is missing in Web.config.</exception>
        public TransactionClient Create(string username, string password)
        {
            // 1. Check for Argument Errors first
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username cannot be null or empty.", nameof(username));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password cannot be null or empty.", nameof(password));
            try
            {
                //IasworldTransactionService.TransactionClient transactionClient = new IasworldTransactionService.TransactionClient("WSHttpBinding_ITransaction");
                //Debug.WriteLine("transaction client in factory", transactionClient, transactionClient.ToString());
                ////transactionClient.ChannelFactory.Endpoint.Behaviors.Remove<System.ServiceModel.Description.ClientCredentials>();
                ////CustomCredentials customCredentials = new CustomCredentials(username, password);
                ////transactionClient.ChannelFactory.Endpoint.Behaviors.Add(customCredentials);
                //return transactionClient;
                var client = new TransactionClient(TransactionClient.EndpointConfiguration.WSHttpBinding_ITransaction);

                // 2. Add the custom behavior that injects your specific XML security header
                client.Endpoint.EndpointBehaviors.Add(new SecurityBehavior(username, password));

                return client;
            }
            // 2. Catch Configuration Errors (e.g., endpoint name is wrong in web.config)
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Could not find the endpoint 'WSHttpBinding_ITransaction' in the configuration file.", ex);
            }
            // 3. Catch generic WCF setup errors
            catch (System.ServiceModel.CommunicationException ex)
            {
                throw new Exception("WCF communication channel could not be initialized.", ex);
            }
        }
    }
}