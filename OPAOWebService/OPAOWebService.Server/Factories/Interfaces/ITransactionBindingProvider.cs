using System.ServiceModel.Channels;

namespace OPAOWebService.Server.Factories.Interfaces
{
    public interface ITransactionBindingProvider
    {
        Binding GetBinding();

    }
}
