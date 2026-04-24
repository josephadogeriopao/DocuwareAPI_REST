using System.Runtime.Serialization;

namespace OPAOWebService.Server.Models.Exceptions
{
    /// <summary>
    /// Custom exception for Business Rule or Input Validation failures.
    /// These are logged to 'general-exceptions-.txt'.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 24-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> ValidationException.cs</para>
    /// </remarks>
    [Serializable]
    public class ValidationException : Exception
    {
        // Property to store which specific field failed (optional)
        public string FieldName { get; set; }

        // 1. Basic Constructor
        public ValidationException() : base() { }

        // 2. Message Constructor (The one you will use most)
        public ValidationException(string message) : base(message) { }

        // 3. Message + Field Constructor
        public ValidationException(string message, string fieldName) : base(message)
        {
            this.FieldName = fieldName;
        }

        // 4. Inner Exception Constructor (For wrapping other errors)
        public ValidationException(string message, Exception inner) : base(message, inner) { }

        // 5. IMPORTANT: Required for Serialization in .NET 4.8
        protected ValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info != null)
            {
                this.FieldName = info.GetString("FieldName");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("FieldName", this.FieldName);
        }
    }
}