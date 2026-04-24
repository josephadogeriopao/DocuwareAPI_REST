namespace OPAOWebService.Server.Business.Validators.Interfaces
{
    /// <summary>
    /// Defines a contract for objects that perform data or business rule validation.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 24-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> IValidator.cs</para>
    /// </remarks>
    public interface IValidator
    {
        /// <summary>
        /// Evaluates the current state of the object against defined validation rules.
        /// </summary>
        /// <returns>True if the object meets all validation criteria; otherwise, false.</returns>
        public bool IsValid();
    }
}