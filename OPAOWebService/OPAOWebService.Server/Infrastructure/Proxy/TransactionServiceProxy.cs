using IasworldTransactionService;
using OPAOWebService.Server.Factories.Interfaces;
using OPAOWebService.Server.Infrastructure.Proxy.Interfaces;
using System.ServiceModel;

namespace OPAOWebService.Server.Infrastructure.TransactionServiceProxy
{
    /// <summary>
    /// Implementation of ITransactionServiceProxy that manages the lifecycle of the iasWorld WCF client.
    /// Implements IDisposable to ensure persistent connections are properly closed or aborted.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 23-APR-2026</para>
    /// <para><strong>Version:</strong> 1.1.0</para>
    /// <para><strong>File:</strong> TransactionServiceProxy.cs</para>
    /// </remarks>    
    public class TransactionServiceProxy : ITransactionServiceProxy, IDisposable
    {
        private readonly ITransactionClientFactory _clientFactory;
        private readonly string _serviceUser;
        private readonly string _servicePass;

        // This persistent instance allows for subsequent calls on the same connection
        private TransactionClient _client;

        /// <summary>
        /// Initializes the proxy with necessary factories and credentials for service authentication.
        /// </summary>
        public TransactionServiceProxy(ITransactionClientFactory clientFactory, string serviceUser, string servicePass)
        {
            _clientFactory = clientFactory;
            _serviceUser = serviceUser;
            _servicePass = servicePass;
        }

        /// <summary>
        /// Logic to retrieve the current client or instantiate a new one if the channel is faulted.
        /// </summary>
        /// <returns>A valid, non-faulted <see cref="TransactionClient"/>.</returns>
        private TransactionClient GetClient()
        {
            if (_client == null || _client.State == CommunicationState.Faulted)
            {
                // If the old client faulted, abort it before creating a new one
                if (_client?.State == CommunicationState.Faulted)
                {
                    _client.Abort();
                }

                _client = _clientFactory.Create(_serviceUser, _servicePass);
            }
            return _client;
        }

        /// <summary>
        /// Wrapper to execute WCF service calls with centralized error handling and channel state management.
        /// </summary>
        /// <typeparam name="TResult">The expected return type of the service operation.</typeparam>
        /// <param name="action">The specific service method to invoke.</param>
        /// <returns>The result of the service operation.</returns>
        /// <exception cref="Exception">Wrapped exceptions for communication or timeout failures.</exception>        
        private TResult Execute<TResult>(Func<TransactionClient, TResult> action)
        {
            var client = GetClient();
            try
            {
                return action(client);
            }
            catch (CommunicationException ex)
            {
                //Log.Error(ex, "Network/Communication error with External Transaction Service");
                client.Abort();
                _client = null; // Clear the instance so the next call gets a fresh one
                throw new Exception("The external transaction service is currently unreachable.");
            }
            catch (TimeoutException ex)
            {
                //Log.Error(ex, "Timeout occurred calling External Transaction Service");
                client.Abort();
                _client = null;
                throw new Exception("The request to the transaction service timed out.");
            }
            // Note: No 'finally' block disposing the client here; reuse is handled by Dispose()
        }


        /// <summary>
        /// Retrieves raw XML data for a specific transaction request.
        /// </summary>
        public string GetTransactionXml(TransactionGetRequest req)
        {
            //Log.Information("Fetching XML for Transaction: {TransactionName}", req.TransactionName);
            return Execute(client => client.GetTransactionXml(req));
        }

        /// <summary>
        /// Validates XML data against iasWorld business logic without committing changes.
        /// </summary>
        public TransactionSubmitResponse ValidateTransactionXml(string xmlData)
        {
            //Log.Information("Validating Transaction XML data...");
            return Execute(client => client.ValidateTransactionXml(xmlData));
        }
        /// <summary>
        /// Submits the transaction XML to the external service for processing.
        /// </summary>
        public TransactionSubmitResponse SubmitTransactionXml(string xml, TransactionSubmitMode mode)
        {
            //Log.Information("Submitting Transaction to External Service...");
            return Execute(client => client.SubmitTransactionXml(xml, mode));
        }
        /// <summary>
        /// Reassigns an existing transaction to a new user account.
        /// </summary>
        public bool ReassignTransaction(int transactionId, string toUsername)
        {
            //Log.Information("Reassigning Transaction {Id} to User {User}...", transactionId, toUsername);
            return Execute(client => client.ReassignTransaction(transactionId, toUsername));
        }
        /// <summary>
        /// Abandons/Cancels an active transaction in the external system.
        /// </summary>
        public bool AbandonTransaction(int transactionId)
        {
            //Log.Information("Abandoning Transaction {Id}...", transactionId);
            return Execute(client => client.AbandonTransaction(transactionId));
        }

        /// <summary>
        /// Explicitly cleans up the WCF channel. Closes gracefully if possible, or Aborts if faulted.
        /// </summary>
        public void Dispose()
        {
            if (_client != null)
            {
                try
                {
                    if (_client.State != CommunicationState.Faulted)
                    {
                        _client.Close();
                    }
                    else
                    {
                        _client.Abort();
                    }
                }
                catch (Exception ex)
                {
                    //Log.Warning(ex, "Error while closing the TransactionClient during Dispose");
                    _client.Abort();
                }
                finally
                {
                    ((IDisposable)_client).Dispose();
                    _client = null;
                }
            }
        }
    }
}