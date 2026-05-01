using System.Runtime.Serialization;

namespace OPAOWebService.Server.Models.Exceptions
{
    /// <summary>
    /// Thrown when an infrastructure component or external service client (e.g., IasWorld WCF) 
    /// fails to initialize or connect.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 30-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> InfrastructureException.cs</para>
    /// </remarks>
    [Serializable]
    public class InfrastructureException : Exception
    {
        /// <summary>Gets or sets the name of the external service that failed.</summary>
        public string ServiceName { get; set; }

        public InfrastructureException() : base() { }

        public InfrastructureException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InfrastructureException"/> class with a service name and inner exception.
        /// </summary>
        public InfrastructureException(string message, string serviceName, Exception inner)
            : base(message, inner)
        {
            this.ServiceName = serviceName;
        }

        public InfrastructureException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Serialization constructor for cross-process communication.
        /// </summary>
        protected InfrastructureException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info != null)
            {
                this.ServiceName = info.GetString("ServiceName");
            }
        }

        /// <summary>
        /// Sets the SerializationInfo with information about the exception.
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ServiceName", this.ServiceName);
        }
    }
}