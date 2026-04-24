using OPAOWebService.Server.Business.Validators.Interfaces;

namespace OPAOWebService.Server.Business.Validators
{
    /// <summary>
    /// Validates metadata fields for property transactions to ensure required strings are present and non-empty.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 24-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> MetaValidator.cs</para>
    /// </remarks>
    public class MetaValidator : BaseValidator, IValidationError
    {

        private readonly string ParcelId;
        private readonly string ReasonCode;
        private readonly string Notes;
        private readonly string ManualCode;
        /// <inheritdoc />
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetaValidator"/> class with parcel metadata.
        /// </summary>
        /// <param name="ParcelId">The unique identifier for the parcel.</param>
        /// <param name="ReasonCode">The reason code for the transaction.</param>
        /// <param name="Notes">Primary comment or note text.</param>
        /// <param name="ManualCode">Status or categorization flag of manual codes.</param>
        public MetaValidator(string ParcelId, string ReasonCode, string Notes, string ManualCode)
        {
            this.ParcelId = ParcelId;
            this.ReasonCode = ReasonCode;
            this.Notes = Notes;
            this.ManualCode = ManualCode;

        }

        /// <summary>
        /// Validates that all metadata fields are provided and contain at least one character.
        /// </summary>
        /// <returns>True if all strings are valid; otherwise, false.</returns>
        public override bool IsValid()
        {

            if (ParcelId == null || ParcelId.Length == 0)
            {
                ErrorMessage = $"Error: Invalid Input {Environment.NewLine}" +
                               $"Description: Parcel ID '{ParcelId}' is null or less than zero characters {Environment.NewLine}" +
                               $"ParcelID: {ParcelId} {Environment.NewLine}";
                return false;
            }
            if (ReasonCode == null || ReasonCode.Length == 0)
            {
                ErrorMessage = $"Error: Invalid Input {Environment.NewLine}" +
                               $"Description: ReasonCode '{ReasonCode}' is null or less than zero characters {Environment.NewLine}" +
                               $"ReasonCode: {ReasonCode} {Environment.NewLine}";
                return false;
            }
            if (Notes == null || Notes.Length == 0)
            {
                ErrorMessage = $"Error: Invalid Input {Environment.NewLine}" +
                               $"Description: Notes '{Notes}' is null or less than zero characters {Environment.NewLine}" +
                               $"Notes: {Notes} {Environment.NewLine}";
                return false;
            }
            if (ManualCode == null || ManualCode.Length == 0)
            {
                ErrorMessage = $"Error: Invalid Input {Environment.NewLine}" +
                               $"Description: ManualCode '{ManualCode}' is null or less than zero characters {Environment.NewLine}" +
                               $"ManualCode: {ManualCode} {Environment.NewLine}";
                return false;
            }

            return true;
        }
