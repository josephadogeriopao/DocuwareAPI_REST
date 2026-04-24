using System.Xml.Linq;

namespace OPAOWebService.Server.Models.Entities.Interfaces
{
    /// <summary>
    /// Defines a contract for objects that can be serialized into an XML element.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 24-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> IXElement.cs</para>
    /// </remarks>
    public interface IXElement
    {
        /// <summary>
        /// Converts the current object instance into its <see cref="XElement"/> representation.
        /// </summary>
        /// <returns>An <see cref="XElement"/> containing the serialized data of the object.</returns>
        public XElement ToXElement();
    }
}