using OPAOWebService.Server.Models.Entities.Interfaces;
using System.Xml.Linq;

namespace OPAOWebService.Server.Models.Entities
{
    /// <summary>
    /// Represents the Assessment (ASMT) entity used to update status flags and date stamps.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 24-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> Assessment.cs</para>
    /// </remarks>
    public class Assessment : IXElement
    {

        /// <summary> Gets or sets the custom manual code status (e.g., Appraisal status). </summary>
        public string ManualCode { get; set; }

        /// <summary> Gets or sets the internal iasWorld unique identifier for the assessment. </summary>
        public string IaswId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Assessment"/> class with default values.
        /// </summary>
        public Assessment() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Assessment"/> class with a specific flag and ID.
        /// </summary>
        /// <param name="manualCode">The status of manual code.</param>
        /// <param name="iaswId">The unique system identifier.</param>
        public Assessment(string manualCode, string iaswId)
        {
            ManualCode = manualCode;
            IaswId = iaswId;
        }

        /// <inheritdoc />
        public XElement ToXElement()
        {
            DateTime currentDateTime = DateTime.Now;
            string formattedDate = currentDateTime.ToString("dd-MMM-yyyy");
            XElement asmtXElement = new XElement("ASMTS",
                        new XElement("ASMT",
                        new XElement("CUR", "Y"),
                        new XElement("FLAG3", this.ManualCode),
                        new XElement("IASW_ID", this.IaswId),
                        new XElement("UDATE1", formattedDate)
                        ));
            return asmtXElement;
        }
    }

}