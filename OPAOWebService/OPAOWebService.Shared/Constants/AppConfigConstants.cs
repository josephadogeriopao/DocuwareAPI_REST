using System;
using System.Collections.Generic;
using System.Text;

namespace OPAOWebService.Shared.Constants
{
    public static class AppConfigConstants
    {
        // 1. The "Label" used for the actual encryption logic
        public const string EncryptionPurpose = "OPAODataProtectionEncryption";

        // 2. The "Key" used to look up the folder path in appsettings.json
        public const string DataProtectionConfigPath = "DataProtection:KeyPath";

        // 3. Application Identity
        public const string ApplicationName = "OPAOWebService";

        // This is for the actual Windows Folder Name
        public const string DefaultFolderName = "OPAOWebService-Keys";
    }
}
