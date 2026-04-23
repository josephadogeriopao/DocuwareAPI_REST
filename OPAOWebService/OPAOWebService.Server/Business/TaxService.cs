using OPAOWebService.Server.Business.Interfaces;
using OPAOWebService.Server.Data.Repositories.Interfaces;
using OPAOWebService.Server.Models.DTOs.Requests;

namespace OPAOWebService.Server.Business
{
    public class TaxService : ITaxService
    {
        private readonly ITaxRepository _taxRepository;

        public TaxService(ITaxRepository taxRepository)
        { 
            _taxRepository = taxRepository;
        }

        public int UpdatePropertyValuation(AssessmentStatusRequest request)
        {
            bool answer = _taxRepository.IsValidParcelId("5641-CHRISTIANLN",2026);


            return  answer ? 1 : 0;
        }
    }
}
