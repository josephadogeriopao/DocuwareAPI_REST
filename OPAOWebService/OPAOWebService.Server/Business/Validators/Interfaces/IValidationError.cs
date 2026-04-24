namespace OPAOWebService.Server.Business.Validators.Interfaces
{
    /// <summary>
    /// Defines a internal contract for capturing and reporting validation error messages.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 24-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> IValidationError.cs</para>
    /// </remarks>
    public interface IValidationError
    {
        /// <summary>
        /// Gets or sets the descriptive message explaining why the validation failed.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}