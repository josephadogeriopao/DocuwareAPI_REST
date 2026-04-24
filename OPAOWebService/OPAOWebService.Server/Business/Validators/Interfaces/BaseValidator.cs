namespace OPAOWebService.Server.Business.Validators.Interfaces
{
    /// <summary>
    /// Provides a base implementation for validation logic across the OPAO Web Service.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 24-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> BaseValidator.cs</para>
    /// </remarks>
    public abstract class BaseValidator : IValidator
    {
        /// <summary>
        /// When overridden in a derived class, validates the specific business rules for that entity.
        /// </summary>
        /// <returns>True if validation passes; otherwise, false.</returns>
        public abstract bool IsValid();
    }
}