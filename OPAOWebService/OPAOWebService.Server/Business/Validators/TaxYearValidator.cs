using OPAOWebService.Server.Business.Validators.Interfaces;

namespace OPAOWebService.Server.Business.Validators
{
    /// <summary>
    /// Validates that the provided tax year falls within the legally allowed processing range.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 24-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> TaxYearValidator.cs</para>
    /// </remarks>
    public class TaxYearValidator : BaseValidator, IValidationError
    {

        private readonly int TaxYear;

        /// <inheritdoc />
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaxYearValidator"/> class.
        /// </summary>
        /// <param name="TaxYear">The tax year to be validated.</param>

        public TaxYearValidator(int TaxYear) : base()
        {
            this.TaxYear = TaxYear;
        }

        /// <summary>
        /// Validates that the tax year is not earlier than the system minimum of 1995.
        /// </summary>
        /// <returns>True if the year is 1995 or greater; otherwise, false.</returns>
        public override bool IsValid()
        {

            //if Checking for negative values
            if (TaxYear < 1995)
            {
                ErrorMessage = $"Error: Invalid Tax Year {Environment.NewLine}" +
                               $"Description: Tax Year must be greater than 1995 {Environment.NewLine}" +
                               $"Tax Year : {TaxYear} {Environment.NewLine}";
                return false;
            }

            return true;
        }

    }

}