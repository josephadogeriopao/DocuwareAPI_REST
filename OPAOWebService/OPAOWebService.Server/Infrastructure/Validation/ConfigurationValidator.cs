namespace OPAOWebService.Server.Infrastructure.Validation
{
    public static class ConfigurationValidator
    {

        /// <summary>
        /// Validates that the provided configuration values are not null, empty, or set to "placeholder".
        /// </summary>
        /// <param name="fieldss">A collection of string values to validate.</param>
        /// <exception cref="Exception">Thrown when a value is missing or contains default placeholder text.</exception>
        public static void ValidateFields(Dictionary<string, string?> fields)
        {
            foreach (var field in fields)
            {
                if (string.IsNullOrWhiteSpace(field.Value) ||
                    field.Value.Equals("placeholder", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException(
                        $"Configuration Setting '{field.Key}' is missing or still set to 'placeholder'. " +
                        "Check your .env file, appsettings.json, or environment variables.");
                }
            }
        }
    }
}