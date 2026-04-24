using System.Runtime.Serialization;

namespace OPAOWebService.Server.Models.Exceptions
{
    /// <summary>
    /// Thrown when input data is valid, but the resulting tax calculation 
    /// violates a business rule (e.g., negative tax, exemption exceeds value).
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 24-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> TaxCalculationException.cs</para>
    /// </remarks>
    [Serializable]
    public class TaxCalculationException : Exception
    {
        // Custom properties to help with debugging or UI messaging
        public decimal CalculatedValue { get; set; }
        public string RuleName { get; set; }

        // 1. Basic Constructor
        public TaxCalculationException() : base() { }

        // 2. Simple Message Constructor
        public TaxCalculationException(string message) : base(message) { }

        // 3. Detailed Constructor (The one you'll use most)
        public TaxCalculationException(string message, string ruleName, decimal value)
            : base(message)
        {
            this.RuleName = ruleName;
            this.CalculatedValue = value;
        }

        // 4. Inner Exception Constructor (To wrap database or math errors)
        public TaxCalculationException(string message, Exception inner)
            : base(message, inner) { }

        // 5. REQUIRED for WCF / Serialization support
        protected TaxCalculationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info != null)
            {
                this.CalculatedValue = info.GetDecimal("CalculatedValue");
                this.RuleName = info.GetString("RuleName");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("CalculatedValue", this.CalculatedValue);
            info.AddValue("RuleName", this.RuleName);
        }
    }
}