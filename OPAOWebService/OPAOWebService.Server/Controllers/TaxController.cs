using Microsoft.AspNetCore.Mvc;
using OPAOWebService.Server.Business.Interfaces;
using OPAOWebService.Server.Infrastructure.Helpers;
using OPAOWebService.Server.Models.DTOs.Requests;
using OPAOWebService.Server.Models.DTOs.Responses;
using OPAOWebService.Server.Models.Exceptions;

namespace OPAOWebService.Server.Controllers
{
    public class TaxController : ApiControllerBase
    {
        private readonly ITaxService _taxService;
        private readonly ILogger<TaxController> _logger;

        public TaxController(ITaxService taxService, ILogger<TaxController> logger)
        {
            _taxService = taxService;
            _logger = logger;
        }

        [HttpPost("AssessmentUpdateStatus", Name = "PostAssessmentUpdateStatus")]
        public AssessmentStatusResponse PostAssessmentUpdateStatus(
         string ParcelId, string ReasonCode, int RevisedLand, int RevisedBldg, int RevisedTot, string Notes, string ManualCode)
        {
            try
            {
                string cleanedParcelId = ValidationHelper.ParseRequiredString(ParcelId, "ParcelId");
                string cleanedManualCode = ValidationHelper.ParseRequiredString(ManualCode, "ManualCode");
                string cleanedReasonCode = ValidationHelper.ParseRequiredString(ReasonCode, "ReasonCode");
                string cleanedNotes = ValidationHelper.ParseRequiredString(Notes, "Notes");

                //Log.Information("Started property update for ParcelId: {ParcelId}", ParcelId);
                var request = new AssessmentStatusRequest
                {
                    ParcelId = cleanedParcelId,
                    ReasonCode = cleanedReasonCode,
                    RevisedLand = RevisedLand,
                    RevisedBldg = RevisedBldg,
                    RevisedTot = RevisedTot,
                    Notes = Notes,
                    ManualCode = cleanedManualCode
                };

                int status = this._taxService.UpdatePropertyValuation(request);
                //Log.Information("Successful property update for ParcelId: {ParcelId}", ParcelId);
                return new AssessmentStatusResponse { StatusCode = 1, Message = "Success" };

            }

            catch (Exception ex)
            {
                // One line handles all logging and response logic
                return LoggingHelper.HandleException(ex, ParcelId, _logger);
            }

        }

    }

}
