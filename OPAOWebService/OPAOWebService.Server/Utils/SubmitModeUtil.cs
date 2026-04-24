namespace OPAOWebService.Server.Utils
{
    /// <summary>
    /// Utility class for mapping and describing iasWorld transaction submission modes.
    /// Provides translation between string inputs and the service's internal enum types.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 24-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> SubmitModeUtil.cs</para>
    /// </remarks>
    public static class SubmitModeUtil
    {

        /// <summary>
        /// Converts a string representation of a submission mode into the corresponding 
        /// <see cref="IasworldTransactionService.TransactionSubmitMode"/> enum value.
        /// Defaults to 'Save' if the input is null, empty, or unrecognized.
        /// </summary>
        /// <param name="mode">The string value indicating the desired submission behavior (e.g., "COMMIT", "VALIDATE", "SAVE", "FORCECOMMIT").</param>
        /// <returns>The matched <see cref="IasworldTransactionService.TransactionSubmitMode"/>.</returns>
        public static IasworldTransactionService.TransactionSubmitMode GetMode(string mode)
        {
            if (string.IsNullOrWhiteSpace(mode))
                return IasworldTransactionService.TransactionSubmitMode.Save;

            switch (mode.Trim().ToUpper())
            {
                case "COMMIT":
                    return IasworldTransactionService.TransactionSubmitMode.Commit;
                case "VALIDATE":
                    return IasworldTransactionService.TransactionSubmitMode.Validate;
                case "FORCECOMMIT":
                    return IasworldTransactionService.TransactionSubmitMode.ForceCommit;
                case "SAVE":
                default:
                    return IasworldTransactionService.TransactionSubmitMode.Save;
            }
        }

        /// <summary>
        /// Returns a user-friendly string representation of the TransactionSubmitMode for logging and reporting purposes.
        /// </summary>
        /// <param name="mode">The enum value to be converted to text.</param>
        /// <returns>A formatted string describing the submission mode.</returns>
        public static string GetSubmitText(IasworldTransactionService.TransactionSubmitMode mode)
        {
            // Since it's an enum, .ToString() usually works, 
            // but this ensures specific formatting or custom labels if needed.
            return mode switch
            {
                IasworldTransactionService.TransactionSubmitMode.Commit => "Commit",
                IasworldTransactionService.TransactionSubmitMode.Validate => "Validate Only",
                IasworldTransactionService.TransactionSubmitMode.ForceCommit => "Force Commit",
                IasworldTransactionService.TransactionSubmitMode.Save => "Save",
                _ => "Save"
            };
        }
    }
}