using OPAOWebService.Server.Business.Validators.Interfaces;
using System;
using System.Collections.Generic;

namespace OPAOWebService.Server.Business.Validators
{
    /// <summary>
    /// Validates appraised values for land and buildings to ensure they meet business and mathematical rules.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 09-MAR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> AppraisedValidator.cs</para>
    /// </remarks>
    public class AppraisedValidator : BaseValidator, IValidationError
    {
        private readonly int RevisedLand;
        private readonly int RevisedBldg;
        private readonly int RevisedTot;
        /// <inheritdoc />
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppraisedValidator"/> class with specific appraisal values.
        /// </summary>
        /// <param name="RevisedLand">The revised land value.</param>
        /// <param name="RevisedBldg">The revised building value.</param>
        /// <param name="RevisedTot">The total revised value.</param>
        public AppraisedValidator(int RevisedLand, int RevisedBldg, int RevisedTot)
        {
            this.RevisedLand = RevisedLand;
            this.RevisedBldg = RevisedBldg;
            this.RevisedTot = RevisedTot;
        }

        /// <summary>
        /// Validates that all values are non-negative and that the sum of land and building equals the total.
        /// </summary>
        /// <returns>True if all appraisal rules are met; otherwise, false.</returns>
        public override bool IsValid()
        {
            // 1. Map labels to the actual int values
            var data = new Dictionary<string, int>
            {
                { "RevisedLand", this.RevisedLand },
                { "RevisedBldg", this.RevisedBldg },
                { "RevisedTot",  this.RevisedTot }
            };

            // 2. Loop through and check: Is it positive?
            foreach (var entry in data)
            {
                // Since entry.Value is already an int, no TryParse is needed
                if (entry.Value < 0)
                {
                    this.ErrorMessage = $"Error: Invalid Input{Environment.NewLine}" +
                                        $"Description: {entry.Key} cannot be less than 0{Environment.NewLine}" +
                                        $"{entry.Key}: {entry.Value}";
                    return false;
                }
            }

            // 3. Math Check (Business Rule)
            if (this.RevisedLand + this.RevisedBldg != this.RevisedTot)
            {
                this.ErrorMessage = $"Error: Computation Mismatch.{Environment.NewLine}" +
                                    $"Description: Land ({this.RevisedLand}) + Building ({this.RevisedBldg}) must equal Total ({this.RevisedTot}).";
                return false;
            }

            return true;
        }
    }
}