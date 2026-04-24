using OPAOWebService.Server.Utils;
using System.Xml.Linq;

namespace OPAOWebService.Server.Models.DTOs
{
    /// <summary>
    /// Represents a property record parsed from an XML data source.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 24-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> Property.cs</para>
    /// </remarks>
    public class Property
    {
        /// <summary>Gets or sets the Land Use Code (LUC).</summary>
        public string LandUseCode { get; set; }
        /// <summary>Gets or sets the Assessment IASW ID.</summary>
        public string AssessmentId { get; set; }
        /// <summary>Gets or sets the Appraisal IASW ID.</summary>
        public string AppraisalId { get; set; }
        /// <summary>Gets or sets the Note IASW ID.</summary>
        public string NoteId { get; set; }
        /// <summary>Gets or sets the Comment Number.</summary>
        public string CommentNumber { get; set; }

        /// <summary>Internal list of comment XML elements for processing.</summary>
        IEnumerable<XElement> CommentList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Property"/> class by parsing an XML node.
        /// </summary>
        /// <param name="ChildNode">The XElement containing property and comment data.</param>
        /// 
        /*
        public Property(XElement ChildNode)
        {
            // retrieve land use code value 
            this.LandUseCode = ChildNode.Element("PARDATS").Element("PARDAT").Element("LUC").Value;
            // retrieve asmt iasw id
            this.AssessmentId = ChildNode.Element("ASMTS")
                                    .Elements("ASMT")
                                    .FirstOrDefault(x => (string)x.Element("CUR") == "Y")
                                    ?.Element("IASW_ID")?.Value;
            // retrieve appraisal iasw id
            this.AppraisalId = ChildNode.Element("APRVALS").Element("APRVAL").Element("IASW_ID").Value;
            // assign all comnt tags to a list
            this.CommentList = ChildNode.Element("COMNTS").Elements();

            this.NoteId = GetNoteId();

            this.CommentNumber = GetCommentNumber();
        }
        */

        public Property(XElement ChildNode)
        {
            // Basic Field Initialization
            this.LandUseCode = ChildNode.Element("PARDATS")?.Element("PARDAT")?.Element("LUC")?.Value;
            this.AppraisalId = ChildNode.Element("APRVALS")?.Element("APRVAL")?.Element("IASW_ID")?.Value;
            this.AssessmentId = ChildNode.Element("ASMTS")?.Elements("ASMT")
                                    .FirstOrDefault(x => (string)x.Element("CUR") == "Y")
                                    ?.Element("IASW_ID")?.Value;

            // 1. Process all comments into a simple list of numbers and IDs once
            var comments = ChildNode.Element("COMNTS")?.Elements("COMNT")
                .Select(c => new {
                    Num = int.TryParse(c.Element("COMNTNO")?.Value, out var n) ? n : 0,
                    Id = c.Element("IASW_ID")?.Value
                })
                .ToList();

            // 2. Find the highest one once
            var latest = comments?.OrderByDescending(x => x.Num).FirstOrDefault();

            // 3. Set properties immediately
            this.NoteId = latest?.Id;
            this.CommentNumber = latest?.Num.ToString() ?? "0";
        }

        /// <summary>
        /// Retrieves the Comment ID based on the highest comment number in the XML list.
        /// </summary>
        /// <returns>The IASW ID of the most recent Note.</returns>
        private string GetNoteId()
        {
            // Fix: Only calculate from XML if ComntID is currently null or empty
            if (string.IsNullOrEmpty(this.NoteId))
            {
                foreach (var Comment in CommentList)
                {
                    if (Comment.Name == "COMNT")
                    {
                        if (int.Parse(Comment.Element("COMNTNO").Value) == MaxCommentNumber())
                        {
                            this.NoteId = Comment.Element("IASW_ID").Value;
                            break;
                        }
                    }
                }
            }
            return this.NoteId;
        }

        /// <summary>
        /// Calculates the highest comment number from the internal comment list.
        /// </summary>
        /// <returns>The maximum integer value found in the COMNTNO field.</returns>
        private int MaxCommentNumber()
        {
            List<int> commentNumbers = CommentUtil.GetCommentNumbers(CommentList);
            return commentNumbers.Any() ? commentNumbers.Max() : 0;
        }

        /// <summary>Returns the comment number as a string.</summary>
        private string GetCommentNumber()
        {
            return Convert.ToString(MaxCommentNumber());
        }
    }
}