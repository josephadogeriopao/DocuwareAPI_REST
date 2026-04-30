using Microsoft.AspNetCore.Mvc;
using OPAOWebService.Server.Business;
using OPAOWebService.Server.Business.Interfaces;
using OPAOWebService.Server.Data.Constants;
using OPAOWebService.Server.Data.Repositories.Interfaces;
using OPAOWebService.Server.Infrastructure.Helpers;
using OPAOWebService.Server.Models.DTOs.Requests;
using OPAOWebService.Server.Models.DTOs.Responses;
using OPAOWebService.Server.Models.Exceptions;

namespace OPAOWebService.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaxController : ControllerBase
    {
        private readonly ITaxService _taxService;
        private readonly ILogger<TaxController> _logger;

        public TaxController(ITaxService taxService, ILogger<TaxController> logger)
        {
            _taxService = taxService;
            _logger = logger;
        }
 
        [HttpPost("AssessmentUpdateStatus", Name = "PostAssessmentUpdateStatus")]
        public AssessmentStatusResponse GetAssessmentUpdateStatus(
         string ParcelId, string ReasonCode, int RevisedLand, int RevisedBldg, int RevisedTot, string Notes, string ManualCode)
        {
            try
            {

                string cleanedParcelId = ValidationHelper.ParseRequiredString(ParcelId, "ParcelId");
                string cleanedManualCode = ValidationHelper.ParseRequiredString(ManualCode, "ManualCode");
                string cleanedReasonCode = ValidationHelper.ParseRequiredString(ReasonCode, "ReasonCode");
                string cleanedNotes = ValidationHelper.ParseRequiredString(Notes, "Notes");

                // Log successful request (Goes to general-exceptions and audit)
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
            // 1. Add the specific catch for ParcelLockedException
            catch (ParcelLockedException ex)
            {
                _logger.LogError(ex, "Error in {tier} for {parcelId}. Type: {exceptionType}",
                    LogTiers.Presentation, ParcelId, ex.GetType().Name);
                // Build a custom message using the LockedInformation object if it exists
                string detail = ex.LockedInformation != null ? $" (Locked by: {ex.LockedInformation.Owner})" : "";
                return new AssessmentStatusResponse
                {
                    StatusCode = 0, // Use a unique code for locking if needed
                    Message = $"{ex.Message}{detail}"
                };
            }
            catch (ValidationException ex)
            {
                // 3. Log user errors as Warnings (Goes to general-exceptions)
                //Log.Warning("Validation Failed for {ParcelId}: {Message}", ParcelId, ex.Message);
                return new AssessmentStatusResponse { StatusCode = 0, Message = ex.Message };
            }
            catch (Exception ex)
            {
                // 4. Log system crashes as Errors (Goes to general-exceptions)
                //Log.Error(ex, "Unexpected error calculating status for {ParcelId}", ParcelId);
                _logger.LogError(ex, "Unexpected error in {tier} during processing for {parcelId}. Description: {description}",
                    LogTiers.Presentation, ParcelId, ex.Message);
                return new AssessmentStatusResponse { StatusCode = 0, Message = ex.Message };
            }
        }

    }

}
