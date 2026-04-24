namespace OPAOWebService.Server.Models.Exceptions
{

    /// <summary>
    /// Exception thrown when a transaction request is successfully sent to the 
    /// external service but fails to process or commit due to business rule violations.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 24-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> TransactionFailedException.cs</para>
    /// </remarks>
    public class TransactionFailedException : Exception
    {
        public string[] Errors { get; }

        // Standard constructor with message and errors
        public TransactionFailedException(string message, string[] errors)
            : base(message)
        {
            Errors = errors ?? Array.Empty<string>();
        }

        // Recommended: Constructor for an inner exception (good for debugging)
        public TransactionFailedException(string message, string[] errors, Exception innerException)
            : base(message, innerException)
        {
            Errors = errors ?? Array.Empty<string>();
        }

        // Helper to get all errors as one string
        public string FullDetails => $"{Message} | Errors: {string.Join(", ", Errors)}";
    }
}