using OPAOWebService.Server.Data.Repositories.Interfaces;

namespace OPAOWebService.Server.Data.Repositories
{
    public class TaxRepository : ITaxRepository
    {
        public int GetCurrentTaxYear()
        {
            return 1;
        }

        public bool IsValidParcelId(string parcelId, int taxYear)
        {
            return true;
        }
    }
}
