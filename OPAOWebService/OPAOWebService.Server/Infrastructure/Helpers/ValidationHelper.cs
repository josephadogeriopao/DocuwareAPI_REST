using OPAOWebService.Server.Business.Validators;
using OPAOWebService.Server.Business.Validators.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
//using Serilog;
using System.Collections.Generic;
using OPAOWebService.Server.Models.Exceptions;


namespace OPAOWebService.Server.Infrastructure.Helpers
{

    /// <summary>
    /// Utility class providing centralized validation logic and string parsing.
    /// Simplifies the enforcement of business rules across the application.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 24-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> ValidationHelper.cs</para>
    /// </remarks>
    public static class ValidationHelper
    {
        /// <summary>
        /// Validates that a string is not null or whitespace, then returns a cleaned version.
        /// </summary>
        /// <param name="value">The raw input string.</param>
        /// <param name="fieldName">The name of the field for error reporting.</param>
        /// <returns>A trimmed, uppercase version of the input string.</returns>
        /// <exception cref="ValidationException">Thrown if the value is empty or null.</exception>
        public static string ParseRequiredString(string value, string fieldName)
        {
            // 1. Check if the value exists and isn't just spaces
            // Note: IsNullOrWhiteSpace ALREADY checks for spaces, so you don't need .Trim() here
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ValidationException($"{fieldName} is required.", fieldName);
            }

            // 2. Return the "Cleaned" version (trimmed and upper-cased)
            return value.Trim().ToUpper();
        }

        /// <summary>
        /// Iterates through a collection of validators and executes their logic.
        /// Throws specific exceptions based on the type of validator that fails.
        /// </summary>
        /// <param name="validators">An enumerable collection of <see cref="IValidator"/> objects.</param>
        /// <exception cref="TaxCalculationException">Thrown if an AppraisedValidator fails.</exception>
        /// <exception cref="ValidationException">Thrown for general validation failures.</exception>
        public static void EnsureValid(IEnumerable<IValidator> validators)
        {
            foreach (var validator in validators)
            {
                if (!validator.IsValid())
                {
                    // 1. Get the error message
                    string error = (validator is IValidationError errorProvider)
                        ? errorProvider.ErrorMessage
                        : "Validation failed.";

                    //Log.Error("Validation Error: {Message}", error);

                    // 2. Map specific validator types to specific exceptions
                    if (validator is AppraisedValidator)
                    {
                        throw new TaxCalculationException(error);
                    }

                    // 3. Default fallback
                    throw new ValidationException(error);
                }
            }
        }

    }
}