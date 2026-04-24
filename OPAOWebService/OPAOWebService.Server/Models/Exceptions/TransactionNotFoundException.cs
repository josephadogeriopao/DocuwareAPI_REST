using System.Runtime.Serialization;

namespace OPAOWebService.Server.Models.Exceptions
{
    /// <summary>
    /// Thrown when a request is made for a tax transaction (via Parcel ID and Tax Year)
    /// that does not exist in the system records.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 24-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> TransactionNotFoundException.cs</para>
    /// </remarks>
    [Serializable]
    public class TransactionNotFoundException : Exception
    {
        // Custom properties for identifying the missing record
        public string Parid { get; set; }
        public string Taxyr { get; set; }

        // 1. Basic Constructor
        public TransactionNotFoundException() : base() { }

        // 2. Simple Message Constructor
        public TransactionNotFoundException(string message) : base(message) { }

        // 3. Detailed Constructor (The one you'll use most)
        public TransactionNotFoundException(string message, string parcelId, string taxYear)
            : base(message)
        {
            this.Parid = parcelId;
            this.Taxyr = taxYear;
        }

        // 4. Inner Exception Constructor
        public TransactionNotFoundException(string message, Exception inner)
            : base(message, inner) { }

        // 5. REQUIRED for Serialization support (needed for cross-process boundaries)
        protected TransactionNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info != null)
            {
                this.Parid = info.GetString("Parid");
                this.Taxyr = info.GetString("Taxyr");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            if (info != null)
            {
                info.AddValue("Parid", this.Parid);
                info.AddValue("Taxyr", this.Taxyr);
            }
        }
    }
}