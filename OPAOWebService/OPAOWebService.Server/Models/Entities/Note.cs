using OPAOWebService.Server.Models.Entities.Interfaces;
using System.Xml.Linq;

namespace OPAOWebService.Server.Models.Entities
{
    /// <summary>
    /// Represents a property comment entity. 
    /// Handles the logic for splitting long text into 80-character chunks required by iasWorld.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 24-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> Note.cs</para>
    /// </remarks>
    public class Note : IXElement
    {
        /// <summary> Gets or sets the Parcel Identification Number. </summary>
        public string ParcelId { get; set; }

        /// <summary> Gets or sets the raw comment text. </summary>
        public string Comment { get; set; }

        /// <summary> Gets or sets the starting sequence number for the comment entries. </summary>
        public string CommentNo { get; set; }

        /// <summary> Gets or sets the internal iasWorld unique identifier. </summary>
        public string IaswId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Note"/> class with default values.
        /// </summary>
        public Note() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Note"/> class with specific data.
        /// Forces Uppercase on Parid and Comment for database consistency.
        /// </summary>
        /// <param name="parcelId">The Parcel ID.</param>
        /// <param name="comment">The text to be stored as a comment.</param>
        /// <param name="commentNo">The initial sequence number.</param>
        /// <param name="iaswId">The iasWorld reference ID.</param>
        public Note(string parcelId, string comment, string commentNo, string iaswId)
        {
            ParcelId = parcelId.ToUpper();
            Comment = comment.ToUpper();
            CommentNo = commentNo;
            IaswId = iaswId;
        }

        /// <summary>
        /// Logic to divide the main comment into multiple 'COMNT' nodes based on an 80-character limit.
        /// </summary>
        /// <inheritdoc />
        public XElement ToXElement()
        {

            DateTime currentDateTime = DateTime.Now;
            string formattedDateTime = currentDateTime.ToString("dd-MMM-yyyy HH:mm:ss");
            int CommentNoCounter = Int32.Parse(this.CommentNo);
            int chunkSize = 80;

            XElement comntList = new XElement("COMNTS");

            // Loop through the string, chunking by the specified size
            for (int i = 0; i < this.Comment.Length; i += chunkSize)
            {
                // Calculate the length of the current chunk
                int chunkLength = Math.Min(chunkSize, this.Comment.Length - i);

                // Extract the chunk using Substring
                string chunk = this.Comment.Substring(i, chunkLength);

                // Do something with the chunk (e.g., print it)
                Console.WriteLine(chunk);
                //CommentList = CommentList + chunk;
                CommentNoCounter++;

                comntList.Add(new XElement("COMNT",
                             new XAttribute("MODE", "I"),
                             new XElement("CODE", "DW"),
                             new XElement("COMNT", chunk),
                             new XElement("COMNTNO", CommentNoCounter),
                             new XElement("IASW_ID", this.IaswId),
                             new XElement("JUR", 9),
                             new XElement("PARID", this.ParcelId),
                             new XElement("WEN", formattedDateTime.ToString()),
                             new XElement("WHO", "DOCUWARE")
                             )

                 );

            }
            return comntList;
        }

    }
}