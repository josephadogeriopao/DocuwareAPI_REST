using System.Xml.Linq;

namespace OPAOWebService.Server.Utils
{
    /// <summary>
    /// Utility class providing helper methods for processing iasWorld comment XML data.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 24-APR-2026</para>
    /// </remarks>
    public static class CommentUtil
    {
        /// <summary>
        /// Extracts all "COMNTNO" sequence numbers from a collection of XML comment elements.
        /// </summary>
        /// <param name="commentList">An enumerable collection of <see cref="XElement"/> representing comments.</param>
        /// <returns>A list of integers representing the comment sequence numbers found.</returns>
        /// <exception cref="FormatException">Thrown if a COMNTNO value is not a valid integer.</exception>
        /// <exception cref="NullReferenceException">Thrown if the COMNTNO element or its value is missing.</exception>
        public static List<int> GetCommentNumbers(IEnumerable<XElement> commentList)
        {
            List<int> list = new List<int>();
            foreach (var comment in commentList)
            {
                if (comment.Name == "COMNT")
                {
                    // Extracts the text value of the COMNTNO child element and parses it to an int
                    string value = comment.Element("COMNTNO")?.Value;
                    if (!string.IsNullOrEmpty(value))
                    {
                        list.Add(int.Parse(value));
                    }
                }
            }

            return list;

        }

    }

}