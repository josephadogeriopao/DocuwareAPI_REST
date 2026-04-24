using OPAOWebService.Server.Models.Entities.Interfaces;
using System.Xml.Linq;

namespace OPAOWebService.Server.Models.Entities
{
    /// <summary>
    /// Represents the Appraisal data entity.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 24-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> Appraisal.cs</para>
    /// </remarks>
    public class Appraisal : IXElement
    {

        /// <summary> Gets or sets the Parcel Identification Number. </summary>
        public string ParcelId { get; set; }
        /// <summary> Gets or sets the Reason Code for the appraisal change. </summary>
        public string ReasonCode { get; set; }
        /// <summary> Gets or sets the Revision Reason description. </summary>
        public string RevisedReason { get; set; }
        /// <summary> Gets or sets the Revised Land value. </summary>
        public int RevisedLand { get; set; }
        /// <summary> Gets or sets the Revised Building value. </summary>
        public int RevisedBldg { get; set; }
        /// <summary> Gets or sets the Revised Total value. </summary>
        public int RevisedTot { get; set; }
        /// <summary> Gets or sets the target Tax Year. </summary>
        public int TaxYear { get; set; }
        /// <summary> Gets or sets the internal iasWorld unique identifier. </summary>
        public string IaswId { get; set; }

        /// <summary>
        /// Default constructor for the <see cref="Appraisal"/> class.
        /// </summary>
        public Appraisal() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Appraisal"/> class with assessment details.
        /// </summary>
        /// <param name="paricelId">The Parcel Identification Number.</param>
        /// <param name="reasonCode">Reason Code for the appraisal change.</param>
        /// <param name="revisedReason">Reviewed Reason text.</param>
        /// <param name="revisedLand">Revised Land value.</param>
        /// <param name="revisedBldg">Revised Building value.</param>
        /// <param name="revisedTot">Revised Total value.</param>
        /// <param name="taxYear">The target Tax Year.</param>
        /// <param name="iaswId">The internal iasWorld identifier.</param>
        public Appraisal(string parcelId, string reasonCode, string revisedReason, int revisedLand, int revisedBldg, int revisedTot, int taxYear, string iaswId)
        {
            ParcelId = parcelId;
            ReasonCode = reasonCode;
            RevisedReason = revisedReason;
            RevisedLand = revisedLand;
            RevisedBldg = revisedBldg;
            RevisedTot = revisedTot;
            TaxYear = taxYear;
            IaswId = iaswId;

        }

        /// <summary>
        /// Maps the object properties to the specific XML structure required for iasWorld transactions.
        /// </summary>
        /// <inheritdoc />
        public XElement ToXElement()
        {
            XElement aprvalXElement = new XElement("APRVALS",
                        new XElement("APRVAL",
                        new XAttribute("id", 1),
                        new XElement("JUR", 9),
                        new XElement("IASW_ID", this.IaswId),
                        new XElement("PARID", this.ParcelId),
                        new XElement("CUR", "Y"),
                        new XElement("REVBLDG", this.RevisedBldg),
                        new XElement("REVCODE", 3),
                        new XElement("REVDT", DateTime.Now.ToString("dd-MMM-yyyy").ToString().ToUpper()),
                        new XElement("REVLAND", this.RevisedLand),
                        new XElement("REVREAS", this.RevisedReason),
                        new XElement("REVTOT", this.RevisedTot),
                        new XElement("REASCD", this.ReasonCode),
                        new XElement("TAXYR", this.TaxYear)
                        ));
            return aprvalXElement;
        }
    }
}


