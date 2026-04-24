using OPAOWebService.Server.Infrastructure.Security;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;



namespace OPAOWebService.Server.Infrastructure.Security
{
    public class SecurityBehavior : IEndpointBehavior
    {
        private readonly string _username;
        private readonly string _password;

        public SecurityBehavior(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(new SecurityHeaderInspector(_username, _password));
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) { }
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher) { }
        public void Validate(ServiceEndpoint endpoint) { }
    }

}
