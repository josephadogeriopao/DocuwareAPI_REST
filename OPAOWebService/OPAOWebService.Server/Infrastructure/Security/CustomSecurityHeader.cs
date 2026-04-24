using System.ServiceModel.Channels;
using System.Xml;

namespace OPAOWebService.Server.Infrastructure.Security
{
    public class CustomSecurityHeader : MessageHeader
    {
        private readonly string _username;
        private readonly string _password;

        public CustomSecurityHeader(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public override string Name => "Security";
        public override string Namespace => "http://oasis-open.org";
        public override bool MustUnderstand => true;

        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            string tokennamespace = "o";
            string createdStr = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

            // Generate Nonce
            string phrase = Guid.NewGuid().ToString();
            string nonce = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(phrase));

            // Format the exact XML structure you requested
            string xml = string.Format(
                "<{0}:UsernameToken u:Id=\"UsernameToken-{1}\" xmlns:u=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\">" +
                "<{0}:Username>{2}</{0}:Username>" +
                "<{0}:Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText\">{3}</{0}:Password>" +
                "<{0}:Nonce EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\">{4}</{0}:Nonce>" +
                "<u:Created>{5}</u:Created></{0}:UsernameToken>",
                tokennamespace, Guid.NewGuid(), _username, _password, nonce, createdStr);

            writer.WriteRaw(xml);
        }
    }
}
