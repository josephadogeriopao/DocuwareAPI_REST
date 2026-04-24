
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace OPAOWebService.Server.Infrastructure.Security
{
    public class SecurityHeaderInspector : IClientMessageInspector
    {
        private readonly string _username;
        private readonly string _password;

        public SecurityHeaderInspector(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            // Add the custom security header to the outgoing request
            request.Headers.Add(new CustomSecurityHeader(_username, _password));
            return null;
        }
        public void AfterReceiveReply(ref Message reply, object correlationState) { }
    }
}
