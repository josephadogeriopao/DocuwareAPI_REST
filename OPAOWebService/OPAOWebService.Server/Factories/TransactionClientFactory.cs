using IasworldTransactionService;
using OPAOWebService.Server.Factories.Interfaces;
using OPAOWebService.Server.Infrastructure.Security;
using OPAOWebService.Server.Infrastructure.Security.Interfaces;
using OPAOWebService.Server.Infrastructure.Validation;
using System.Configuration;
using System.Diagnostics;
using System.Linq; 
using System.ServiceModel;

namespace OPAOWebService.Server.Factories
{
    public class TransactionClientFactory : ITransactionClientFactory
    {
        private readonly ITransactionBindingProvider _bindingProvider;
        private readonly IConfiguration _configuration;
        private readonly IConfigProtector _configProtector;

        public TransactionClientFactory() { }

        public TransactionClientFactory(ITransactionBindingProvider bindingProvider, IConfiguration configuration, IConfigProtector configProtector)
        {
            _bindingProvider = bindingProvider;
            _configuration = configuration;
            _configProtector = configProtector;
        }

        public TransactionClient Create(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username cannot be null or empty.", nameof(username));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password cannot be null or empty.", nameof(password));

            try
            {
                // In ASP.NET Core, we must use the programmatic URL
                string serviceUrl = _configProtector.Decrypt(_configuration["IAS_ENDPOINT_ADDRESS"], "IAS_ENDPOINT_ADDRESS");

                var field = new Dictionary<string, string?>
                {
                    {"IAS_ENDPOINT_ADDRESS", serviceUrl}
                };

                ConfigurationValidator.ValidateFields(field);

                // 1. Define Binding (Matches your old WSHttpBinding_ITransaction)
                var binding = _bindingProvider.GetBinding();
                // 2. Define Endpoint Address
                var endpointAddress = new EndpointAddress(serviceUrl);

                // 3. Initialize Client using the objects, not a config name string
                var client = new TransactionClient(binding, endpointAddress);

                // 4. Set the standard credentials for good measure
                client.ClientCredentials.UserName.UserName = username;
                client.ClientCredentials.UserName.Password = password;

                Debug.WriteLine($"The .svc address is: {client.Endpoint.Address.Uri}");
                Console.WriteLine($"The .svc address is: {client.Endpoint.Address.Uri}");

                return client;
            }
            catch (Exception ex)
            {
                // We catch general exceptions because programmatic config failures 
                // are usually ArgumentExceptions or InvalidOperationExceptions.
                System.Diagnostics.Debug.WriteLine($"exception in transation client factory : Details: {ex.InnerException.Message}");
                throw new Exception($"Failed to initialize IasWorld Transaction Client: {ex.Message}", ex);
            }
        }
    }
}
