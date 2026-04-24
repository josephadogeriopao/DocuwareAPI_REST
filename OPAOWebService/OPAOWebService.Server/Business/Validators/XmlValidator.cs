using OPAOWebService.Server.Business.Validators.Interfaces;
using System.Xml.Linq;

namespace OPAOWebService.Server.Business.Validators
{
    /// <summary>
    /// Validates whether a provided string is a correctly formatted XML document.
    /// Implements IValidationError to track specific failure messages.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 24-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> XmlValidator.cs</para>
    /// </remarks>
    public class XmlValidator : BaseValidator, IValidationError
    {
        private string xmlString;
        public XmlValidator() { }


        /// <inheritdoc />
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Injects the XML string to be validated.
        /// </summary>
        public void SetXmlString(string xml)
        {
            this.xmlString = xml;
        }

        /// <summary>
        /// Attempts to parse the string using LINQ to XML.
        /// Returns true if valid, false if the string is null, empty, or malformed.
        /// </summary>
        public override bool IsValid()
        {
            try
            {
                XElement.Parse(xmlString);
                return true;
            }
            catch (System.Xml.XmlException)
            {
                ErrorMessage = $"Error: Server null response {Environment.NewLine}" +
                               $"Description: Transaction xml response from iasworld returned null as Response {Environment.NewLine}";
                return false;
            }
            catch (ArgumentNullException)
            {
                // Handles cases where xmlString was never set or is null
                ErrorMessage = "Error: XML string is null.";
                return false;
            }
        }
    }

}