using System.Runtime.Serialization;

namespace OPAOWebService.Server.Models.Exceptions
{
    /// <summary>
    /// Thrown when a requested entity (e.g., Database record) cannot be found.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 20-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> EntityNotFoundException.cs</para>
    /// </remarks>
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        // Custom properties for better error tracking
        public string EntityName { get; set; }
        public object EntityKey { get; set; }

        // 1. Basic Constructor
        public EntityNotFoundException() : base() { }

        // 2. Simple Message Constructor
        public EntityNotFoundException(string message) : base(message) { }

        // 3. Detailed Constructor
        public EntityNotFoundException(string message, string entityName, object entityKey)
            : base(message)
        {
            this.EntityName = entityName;
            this.EntityKey = entityKey;
        }

        // 4. Inner Exception Constructor
        public EntityNotFoundException(string message, Exception inner)
            : base(message, inner) { }

        // 5. REQUIRED for WCF / Serialization support
        protected EntityNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info != null)
            {
                this.EntityName = info.GetString("EntityName");
                this.EntityKey = info.GetValue("EntityKey", typeof(object));
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("EntityName", this.EntityName);
            info.AddValue("EntityKey", this.EntityKey);
        }
    }
}