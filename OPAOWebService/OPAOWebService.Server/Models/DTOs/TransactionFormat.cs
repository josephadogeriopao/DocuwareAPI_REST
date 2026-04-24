using OPAOWebService.Server.Models.Entities;
using System.Xml.Linq;

namespace OPAOWebService.Server.Models.DTOs
{
    /// <summary>
    /// Handles the formatting and generation of XML transaction structures for property data.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 24-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0</para>
    /// <para><strong>File:</strong> TransactionFormat.cs</para>
    /// </remarks>
    public class TransactionFormat
    {
        /// <summary>Gets or sets the Parcel ID for the transaction.</summary>
        public string ParcelId { get; set; }

        /// <summary>Gets or sets the Tax Year for the transaction.</summary>
        public int TaxYear { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionFormat"/> class.
        /// </summary>
        /// <param name="parcelId">The target Parcel ID.</param>
        /// <param name="taxYear">The applicable tax year.</param>
        public TransactionFormat(string parcelId, int taxYear)
        {
            ParcelId = parcelId;
            TaxYear = taxYear;
        }

        /// <summary>
        /// Converts the transaction data into a formatted XML string.
        /// </summary>
        /// <param name="appraisal">The appraisal entity data.</param>
        /// <param name="assessment">The assessment entity data.</param>
        /// <param name="note">The Note entity data.</param>
        /// <returns>A string representation of the XML transaction.</returns>
        public string ToXmlString(Appraisal appriasal, Assessment assessment, Note note)
        {
            return ToXElement(appriasal, assessment, note).ToString();
        }

        /// <summary>
        /// Generates an <see cref="XElement"/> containing the full PRC transaction structure.
        /// </summary>
        /// <param name="appraisal">The appraisal node data.</param>
        /// <param name="assessment">The assessment node data.</param>
        /// <param name="note">The note node data.</param>
        /// <returns>An XElement representing the "PRC" root node and its children.</returns>
        public XElement ToXElement(Appraisal appraisal, Assessment assessment, Note note)
        {
            return
            new XElement("PRC",
                new XAttribute("version", 1.2),
                // INFO NODE SECTION
                new XElement("INFO",
                    new XElement("TRANS_ID", 0),
                    new XElement("TRANS_NAME", "CAMA_RES"),
                    new XElement("PARAMETERS",
                        new XElement("PARID", ParcelId),
                        new XElement("JUR", 9),
                        new XElement("TAXYR", TaxYear)
                    )
                ),

                // PROPERTY NODE SECTION
                new XElement("PROPERTY",
                    appraisal.ToXElement(),
                    assessment.ToXElement(),
                    note.ToXElement()
                )
            );
        }
    }
}