
using IasworldTransactionService;
using OPAOWebService.Server.Business.Interfaces;
using OPAOWebService.Server.Business.Validators;
using OPAOWebService.Server.Business.Validators.Interfaces;
using OPAOWebService.Server.Data.Providers.Interfaces;
using OPAOWebService.Server.Data.Repositories;
using OPAOWebService.Server.Data.Repositories.Interfaces;
using OPAOWebService.Server.Factories;
using OPAOWebService.Server.Factories.Interfaces;
using OPAOWebService.Server.Infrastructure.Helpers;
using OPAOWebService.Server.Infrastructure.Security;
using OPAOWebService.Server.Infrastructure.Security.Interfaces;
using OPAOWebService.Server.Infrastructure.TransactionServiceProxy;
using OPAOWebService.Server.Models.DTOs;
using OPAOWebService.Server.Models.DTOs.Requests;
using OPAOWebService.Server.Models.Entities;
using OPAOWebService.Server.Models.Exceptions;
using OPAOWebService.Server.Utils;
using System.Configuration;
using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection;
using Oracle.ManagedDataAccess.Client;
using System.Diagnostics;

namespace OPAOWebService.Server.Business
{
    /// <summary>
    /// Implements business logic for property tax valuation updates.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 23-MAR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> TaxService.cs</para>
    /// </remarks>
    public class TaxService : ITaxService
    {
        private readonly ITaxRepository _taxRepository;
        private readonly ITransactionClientFactory _transactionClientFactory;
        private readonly ITransactionGetRequestFactory _transactionGetRequestFactory;
        private readonly IConfiguration _configuration;
        private readonly IConfigProtector _configProtector;

        public TaxService(ITaxRepository taxRepository, ITransactionClientFactory transactionClientFactory, ITransactionGetRequestFactory transactionGetRequestFactory,
            IConfiguration configuration, IConfigProtector configProtector)
        {
            this._taxRepository = taxRepository;
            this._transactionClientFactory = transactionClientFactory;
            this._transactionGetRequestFactory = transactionGetRequestFactory;
            this._configuration = configuration;
            this._configProtector = configProtector;
        }
        public TaxService() : this(
            new TaxRepository(),
            new TransactionClientFactory(),
            new TransactionGetRequestFactory(),
            new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> { // Added '?' here
            { "SUBMIT_MODE", "TEST_MODE" },
            { "SomeSetting", "SomeValue" }
                })
                .Build(),
            new ConfigProtector())
        { } // Standard chaining


        /// <inheritdoc />
        /// <remarks>
        /// This implementation updates the Appraisal, Assessment, and Note tables 
        /// within a single database transaction.
        /// </remarks>        
        public int UpdatePropertyValuation(AssessmentStatusRequest request)
        {
            TransactionSubmitResponse transactionSubmitResponse = null;
            // initializing local variables
            int TaxYear = this._taxRepository.GetCurrentTaxYear();
            string ParcelId = request.ParcelId;
            string ReasonCode = request.ReasonCode;
            string Notes = request.Notes;
            string ManualCode = request.ManualCode;
            int RevisedLand = request.RevisedLand;
            int RevisedBldg = request.RevisedBldg;
            int RevisedTot = request.RevisedTot;

            // initializing validator
            var validators = new List<IValidator> {
                new TaxYearValidator(TaxYear), new MetaValidator(ParcelId, ReasonCode, Notes, ManualCode), new AppraisedValidator(RevisedLand, RevisedBldg, RevisedTot)
            };

            ValidationHelper.EnsureValid(validators);

            Boolean isValid = this._taxRepository.IsValidParcelId(ParcelId, TaxYear);

            if (!isValid)
            {
                //dbLog.Error($"The parcel ID '{ParcelId}' does not exist in our records for tax year {TaxYear}.");
                //Log.Error($"The parcel ID '{ParcelId}' does not exist in our records for tax year {TaxYear}.");

                throw new EntityNotFoundException(
                    message: $"The parcel with ID '{ParcelId}' does not exist in our records.", entityName: "ASSESSMENT", entityKey: ParcelId
                 );
            }
            string username = _configProtector.Decrypt(_configuration["IAS_USERNAME"], "IAS_USERNAME");
            string password = _configProtector.Decrypt(_configuration["IAS_PASSWORD"], "IAS_PASSWORD");

            var proxy = new TransactionServiceProxy(this._transactionClientFactory, username, password);

            TransactionGetRequest transactionGetRequest = this._transactionGetRequestFactory.Create(ParcelId, TaxYear);

            // Get transaction xml response from iasworld in string format
            Console.WriteLine($"transactiongetrequest data ==> {transactionGetRequest.TaxYear}," +
                $" {transactionGetRequest.TransactionName}, {transactionGetRequest.SubjectId}," +
                $" {transactionGetRequest.Jurisdiction}");
            Debug.WriteLine($"transactiongetrequest data ==> {transactionGetRequest.TaxYear}," +
                $" {transactionGetRequest.TransactionName}, {transactionGetRequest.SubjectId}," +
                $" {transactionGetRequest.Jurisdiction}");
            string returnXml = proxy.GetTransactionXml(transactionGetRequest);
            Console.WriteLine("return Xml = " + returnXml);
            Debug.WriteLine("return Xml = " + returnXml);

            if (string.IsNullOrEmpty(returnXml))
            {
                throw new TransactionNotFoundException($"The requested tax record does not exist in our database for Parcel {ParcelId} in tax year {TaxYear}.", ParcelId, Convert.ToString(TaxYear));
            }
            // check if xml received from iasworld is valid
            XmlValidator xmlValidator = new XmlValidator();
            xmlValidator.SetXmlString(returnXml);
            if (!xmlValidator.IsValid())
            {
                //dbLog.Information($"The XML content for Parcel {ParcelId} ( Tax Year {TaxYear}) retrieved from Iasworld server contains malformed or invalid XML data.");
                throw new System.Xml.XmlException($"The XML content for Parcel {ParcelId} ( Tax Year {TaxYear}) retrieved from Iasworld server contains malformed or invalid XML data.");
            }
            XElement xelement = XElement.Parse(returnXml);

            // Note!! Root Node has 2 children
            // Use .Element() to find the first child with that name
            XElement informationNode = xelement.Element("INFO");
            XElement propertyNode = xelement.Element("PROPERTY");

            if (informationNode == null)
            {
                //Log.Error("System Error: Mandatory <INFO> node is missing for Parcel {ParcelId}.", ParcelId);
                throw new System.Xml.XmlException($"Mandatory <INFO> node is missing for Parcel {ParcelId}.");
            }

            if (propertyNode == null)
            {
                //Log.Error("System Error: Mandatory <PROPERTY> node is missing for Parcel {ParcelId}.", ParcelId);
                throw new System.Xml.XmlException($"Mandatory <PROPERTY> node is missing for Parcel {ParcelId}.");
            }
            Information information = new Information(informationNode);
            Property property = new Property(propertyNode);
            // checks status i.e transation id != 0 means record is locked by another user
            if (information.IsLocked())
            {
                // Log to DB: This is a business event, not a system crash.
                //dbLog.Warning("Access Denied: Parcel {ParcelId} is locked by Transaction {TransID}.",
                //              information.ParcelId, information.TransactionId);

                throw new ParcelLockedException(information, "TaxService.cs");
            }
            // if LAND USE CODE == 488, THEN REVREAS = "CMX" // else REVREAS = "Y"
            string RevReas = property.LandUseCode == "488" ? "CMX" : "Y";

            // instatiate Appraisal, Assessment and Note objects
            Appraisal appraisal = new Appraisal(ParcelId, ReasonCode, RevReas, RevisedLand, RevisedBldg, RevisedTot, TaxYear, property.AppraisalId);

            //int Id = 1;
            Assessment assessment = new Assessment(ManualCode, property.AssessmentId);

            Note note = new Note(ParcelId, Notes, property.CommentNumber, property.NoteId);

            TransactionFormat transactionFormat = new TransactionFormat(ParcelId, TaxYear);
            string transactionString = transactionFormat.ToXmlString(appraisal, assessment, note);

            transactionSubmitResponse = proxy.ValidateTransactionXml(transactionString);
            if (transactionSubmitResponse?.Errors?.Length != 0)
            {
                //dbLog.Error(
                //    $"Iasworld server returned errors in Transaction Submit Response object while validating xml transaction string. " +
                //    $"{Environment.NewLine} Parcel: {ParcelId}, Tax Year: {TaxYear}", transactionSubmitResponse.Errors);
                throw new TransactionFailedException(
                    $"Iasworld server returned errors in Transaction Submit Response object while validating xml transaction string. " +
                    $"{Environment.NewLine} Parcel: {ParcelId}, Tax Year: {TaxYear}", transactionSubmitResponse.Errors);
            }


            //post changes to IAS if both submit mode in web.config match modes in array
            // Safely handles cases where the key might be missing
            string webConfigSubmitMode = _configuration["SUBMIT_MODE"] ?? "NOT_SET";

            Debug.WriteLine($"web config submit mode: {webConfigSubmitMode}"); Debug.WriteLine("web config submit mode " + webConfigSubmitMode);
            Console.WriteLine("web config submit mode " + webConfigSubmitMode);

            TransactionSubmitMode transactionSubmitMode = SubmitModeUtil.GetMode(webConfigSubmitMode);
            transactionSubmitResponse = proxy.SubmitTransactionXml(transactionString, transactionSubmitMode);

            if (transactionSubmitResponse?.Errors?.Length != 0)
            {
                //dbLog.Error($"Iasworld server returned errors during final submission. " +
                //    $"{Environment.NewLine} Parcel: {ParcelId}, Tax Year: {TaxYear}, Submit Mode: {SubmitModeUtil.GetSubmitText(transactionSubmitMode)}");

                throw new TransactionFailedException(
                    $"Iasworld server returned errors during final submission. " +
                    $"{Environment.NewLine} Parcel: {ParcelId}, Tax Year: {TaxYear}, Submit Mode: {SubmitModeUtil.GetSubmitText(transactionSubmitMode)}", transactionSubmitResponse.Errors);
            }
            return 1;
        }

    }
}