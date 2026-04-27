using OPAOWebService.Server.Models.DTOs;
using System.Runtime.Serialization;

namespace OPAOWebService.Server.Models.Exceptions
{
    /// <summary>
    /// Thrown when a requested parcel (e.g., Database record) is locked by another user.
    /// </summary>
    /// <remarks>
    /// <para><strong>Author:</strong> Joseph Adogeri</para>
    /// <para><strong>Since:</strong> 24-APR-2026</para>
    /// <para><strong>Version:</strong> 1.0.0</para>
    /// <para><strong>File:</strong> ParcelLockedException.cs</para>
    /// </remarks>

    [Serializable]
    public class ParcelLockedException : Exception
    {
        // Store the object itself (make sure the Info class is also [Serializable])
        public Information LockedInformation { get; set; }
        public string FileName { get; set; }

        public ParcelLockedException() : base() { }

        public ParcelLockedException(string message) : base(message) { }

        // THE UPDATED CONSTRUCTOR: Just pass the Information object and the filename
        public ParcelLockedException(Information information, string fileName)
            : base(information?.Message ?? "Record is locked.")
        {
            this.LockedInformation = information;
            this.FileName = fileName;
        }

        public ParcelLockedException(string message, Exception inner) : base(message, inner) { }

        // Serialization for ASMX / Cross-process
        protected ParcelLockedException(SerializationInfo information, StreamingContext context)
            : base(information, context)
        {
            if (information != null)
            {
                this.LockedInformation = (Information)information.GetValue("LockedInformation", typeof(Information));
                this.FileName = information.GetString("FileName");
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("LockedInformation", this.LockedInformation);
            info.AddValue("FileName", this.FileName);
        }
    }

}