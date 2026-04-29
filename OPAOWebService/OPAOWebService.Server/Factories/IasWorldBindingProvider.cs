using OPAOWebService.Server.Factories.Interfaces;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace OPAOWebService.Server.Factories
{
    public class IasWorldBindingProvider : ITransactionBindingProvider
    {
        public Binding GetBinding()
        {
            var binding = new WSHttpBinding(SecurityMode.TransportWithMessageCredential);

            // Security configuration
            binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
            binding.Security.Message.EstablishSecurityContext = false;
            binding.Security.Message.NegotiateServiceCredential = false;

            // Message Size / Performance configuration
            binding.MaxReceivedMessageSize = 2147483647;
            binding.MaxBufferPoolSize = 2147483647;

            binding.ReaderQuotas.MaxDepth = 32;
            binding.ReaderQuotas.MaxStringContentLength = 2147483647;
            binding.ReaderQuotas.MaxArrayLength = 2147483647;
            binding.ReaderQuotas.MaxBytesPerRead = 2147483647;
            binding.ReaderQuotas.MaxNameTableCharCount = 2147483647;

            return binding;
        }
    }
}