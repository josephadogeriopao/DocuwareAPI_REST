using System.Xml.Linq;

namespace OPAOWebService.Server.Models.DTOs
{
    /// <summary>
    /// Represents transaction and ownership information for a specific parcel.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 24-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> Information.cs</para>
    /// </remarks>
    public class Information
    {
        /// <summary>Gets or sets the Parcel ID.</summary>
        public string ParcelId { get; set; }

        /// <summary>Gets or sets the date of the transaction or record.</summary>
        public string Date { get; set; }

        /// <summary>Gets or sets the Transaction ID. A value of "0" typically indicates an unlocked record.</summary>
        public string TransactionId { get; set; }

        /// <summary>Gets or sets the name of the property owner.</summary>
        public string Owner { get; set; }

        /// <summary>Gets or sets the status or informational message.</summary>
        public string Message { get; set; }

        /// <summary>Gets or sets the tax year associated with the record.</summary>
        public string TaxYear { get; set; }

        /// <summary>
        /// Initializes a new empty instance of the <see cref="Information"/> class.
        /// </summary>
        public Information() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Information"/> class with specific property values.
        /// Maps parcel, transaction, and ownership data to the object instance.
        /// </summary>
        /// <param name="parcelId">The Parcel Identification Number.</param>
        /// <param name="date">The timestamp or processing date of the record.</param>
        /// <param name="transactionId">The unique iasWorld Transaction ID.</param>
        /// <param name="owner">The name of the property owner or the user responsible for the transaction.</param>
        /// <param name="message">A status message or description related to the record.</param>
        /// <param name="taxYear">The specific tax year associated with the assessment data.</param>
        /// <remarks>
        /// <para><strong>Author:</strong> Joseph Adogeri</para>
        /// <para><strong>Since:</strong> 23-MAR-2026</para>
        /// <para><strong>Version:</strong> 1.0.0</para>
        /// </remarks>
        public Information(string parcelId, string date, string transactionId, string owner, string message, string taxYear)
        {
            ParcelId = parcelId;
            Date = date;
            TransactionId = transactionId;
            Owner = owner;
            Message = message;
            TaxYear = taxYear;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Info"/> class by parsing an XML node.
        /// </summary>
        /// <param name="ChildNode">The XElement containing parcel information.</param>
        public Information(XElement ChildNode)
        {
            if (ChildNode == null) throw new ArgumentNullException(nameof(ChildNode), "The child node does not exist in the Info XML structure.");
            this.ParcelId = ChildNode.Element("PARID")?.Value;
            this.Date = ChildNode.Element("DATE")?.Value;
            this.TransactionId = ChildNode.Element("TRANS_ID")?.Value;
            this.Owner = ChildNode.Element("OWNER")?.Value;
            this.Message = ChildNode.Element("MSG")?.Value;
            this.TaxYear = ChildNode.Element("TAXYR")?.Value;
        }

        /// <summary>
        /// Determines if the record is currently locked based on the Transaction ID.
        /// </summary>
        /// <returns>True if the <see cref="TransactionID"/> is not "0"; otherwise, false.</returns>
        public bool IsLocked()
        {
            return TransactionId != "0";
        }
    }
}