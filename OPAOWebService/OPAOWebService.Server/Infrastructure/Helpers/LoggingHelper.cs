using OPAOWebService.Server.Data.Constants;
using OPAOWebService.Server.Models.DTOs.Responses;
using OPAOWebService.Server.Models.Exceptions;
using Oracle.ManagedDataAccess.Client;
using System.ServiceModel;
using System.Xml;

namespace OPAOWebService.Server.Infrastructure.Helpers
{
    public class LoggingHelper
    {
        /// <summary>
        /// Maps a specific Child Exception to its Parent LogTier.
        /// </summary>
        public static string GetTier(Exception ex)
        {
            return ex switch
            {
                // [Presentation]
                ValidationException => LogTiers.Presentation,

                // [Business Logic]
                EntityNotFoundException or
                ParcelLockedException or
                TransactionNotFoundException or
                TransactionFailedException or
                TaxCalculationException or
                System.Xml.XmlException => LogTiers.Business,

                // [Infrastructure (WCF)]
                System.ServiceModel.CommunicationException or
                System.TimeoutException => LogTiers.Infrastructure,

                // [Data Access]
                OracleException => LogTiers.Data,

                // [Domain]
                ArgumentException or
                ArgumentNullException or
                InvalidOperationException or
                ArithmeticException => LogTiers.Domain,

                // [Domain / Default]
                _ => LogTiers.Business
            };
        }
        public static AssessmentStatusResponse HandleException(Exception ex, string parcelId, ILogger logger)
        {
            string tier = GetTier(ex);

            // 1. Log the error to your api-logs.json
            logger.LogError(ex, "Error in {tier} for {parcelId}. Type: {exceptionType}",
                tier, parcelId, ex.GetType().Name);

            // 2. Return the appropriate response based on the exception type
            return ex switch
            {
                ParcelLockedException => new AssessmentStatusResponse
                {
                    StatusCode = 0,
                    Message = $"{parcelId} is currently locked by another user."
                },
                ValidationException vex => new AssessmentStatusResponse
                {
                    StatusCode = 0,
                    Message = vex.Message
                },
                EntityNotFoundException enf => new AssessmentStatusResponse
                {
                    StatusCode = 0,
                    Message = enf.Message
                },
                TransactionFailedException => new AssessmentStatusResponse
                {
                    StatusCode = 0,
                    Message = "The update was rejected by the external service."
                },
                CommunicationException or TimeoutException => new AssessmentStatusResponse
                {
                    StatusCode = 0,
                    Message = "External service (IASWorld) is currently unavailable."
                },
                XmlException => new AssessmentStatusResponse
                {
                    StatusCode = 0,
                    Message = "A data processing error occurred (Invalid XML)."
                },
                _ => new AssessmentStatusResponse
                {
                    StatusCode = 0,
                    Message = "An unexpected system error occurred."
                }
            };
        }
    }
}
