using System;


namespace Rti
{
    public class Constants
    {
        /// <summary>
        /// Private constructor to prevent instantiation of
        /// or inheritence from this class
        /// </summary>
        private Constants()
        {
        }

        public const string SOURCEID = "20";

        /// <summary>
        /// DocType two letter abbreviations
        /// </summary>
        public const string DocType_BundleHeader = "BH";
        public const string DocType_Check = "CK";
        public const string DocType_Remittance = "RM";
        public const string DocType_Correspondence = "CO";
        public const string DocType_PERMail = "M1";
        public const string DocType_PayorMail = "M2";
        public const string DocType_PERPostingDocument = "P1";
        public const string DocType_PayorPostingDocument = "P2";
        public const string DocType_InternalRequest = "IR";
        public const string DocType_InternallyGeneratedClaim = "CL";
        public const string DocType_ElectronicCheck = "CE";

        /// <summary>
        /// Match Status
        /// </summary>
        public const string MatchStatus_UnMatched = "N";
        public const string MatchStatus_Pending = "P";
        public const string MatchStatus_Rejected = "R";
        public const string MatchStatus_Approved = "A";
        public const string MatchStatus_Matched = "M";

        /// <summary>
        /// Progress Database connection strings
        /// </summary>
        #if DEBUG
            public const string DB_PROGRESS_ODBC_CONNECT_STRING = "DSN=initdb;UID=bosql;PWD=sBApgDXz";
        #elif LIVE        
            public const string DB_PROGRESS_ODBC_CONNECT_STRING = "DSN=initdb;UID=bosql;PWD=sBApgDXz";
        #else  // Point to QA
            public const string DB_PROGRESS_ODBC_CONNECT_STRING = "DSN=initdb;UID=bosql;PWD=sBApgDXz";
        #endif

        /// <summary>
        /// Service monitor thread interval
        /// </summary>
        #if DEBUG
            public const int MONITOR_INTERVAL = 30000;  // 30 seconds
        #else
            public const int MONITOR_INTERVAL = 3600000;  // Once an hour
        #endif

        /// <summary>
        /// When a common start time is needed
        /// </summary>
        public static readonly DateTime RtiMinTime = new DateTime(1900, 1, 1, 0, 0, 0);

        /// <summary>
        /// Safety limit of user result set (when needed)
        /// </summary>
        public const int MAX_RESULTS_LIMIT = 10000;

        /// <summary>
        /// Valid monetary range
        /// </summary>
        public static string MIN_MONEY = "-100000000";
        public static string MAX_MONEY = "100000000";

        /// <summary>
        /// Valid date range (strings)
        /// </summary>
        public static string MIN_DATE = "19000101";
        public static string MAX_DATE = "22000101";

        /// <summary>
        /// Constants for WCF proxy calls
        /// </summary>
        #if DEBUG  // Local
            public const string EagleIqGatewayServiceURL = "net.tcp://localhost:8044/Rti.EagleIqGatewayService/";
            public const string AdministrationServiceURL = "net.tcp://localhost:8025/Rti.AdministrationService/";
            public const string DocumentManagerServiceURL = "net.tcp://localhost:8064/Rti.DocumentManagerService/";
            public const string EagleIqGatewayServiceENDPOINT_NAME = "netTcpLocal";
            public const string AdministrationServiceENDPOINT_NAME = "netTcpLocalAdmin";
            public const string DocumentManagerServiceENDPOINT_NAME = "netTcpLocalDocMgr";
            public const string IDMServerName = "rtitsta.rti.com";
            public const string ImportImageNASPath = @"\\rtiedinas.rti.com\FS_RTI_navigator\inbound";
            public const string ImportImageUNIXPath = @"/var/imgstage/navigator/inbound";
            public const string ImportImageSftpPath = @"/var/dlctmp";
            public const string ImportImageSftpHost = @"rtitsta";
            public const string BinDirectory = @"./";
            public const string RtiDllBuild = "DEBUG";
        #elif LIVE
            public const string EagleIqGatewayServiceURL = "net.tcp://navigator.rti.com:8044/Rti.EagleIqGatewayService/";
            public const string AdministrationServiceURL = "net.tcp://navigator.rti.com:8025/Rti.AdministrationService/";
            public const string DocumentManagerServiceURL = "net.tcp://navigator.rti.com:8064/Rti.DocumentManagerService/";
            public const string EagleIqGatewayServiceENDPOINT_NAME = "netTcpRemote";
            public const string AdministrationServiceENDPOINT_NAME = "netTcpRemoteAdmin";
            public const string DocumentManagerServiceENDPOINT_NAME = "netTcpRemoteDocMgr";
            public const string IDMServerName = "imageprod.rti.com";
            public const string ImportImageNASPath = @"\\rtiedinas.rti.com\FS_RTI_navigator\inbound\live";
            public const string ImportImageUNIXPath = @"/var/imgstage/navigator/inbound/live";
            public const string ImportImageSftpPath = @"/var/dlctmpc";
            public const string ImportImageSftpHost = @"prog-dsvc";
            public const string BinDirectory = @"c:\RACServices\bin\";
            public const string RtiDllBuild = "LIVE";
        #else  // QA
            public const string EagleIqGatewayServiceURL = "net.tcp://apps.rti.com:8044/Rti.EagleIqGatewayService/";
            public const string AdministrationServiceURL = "net.tcp://apps.rti.com:8025/Rti.AdministrationService/";
            public const string DocumentManagerServiceURL = "net.tcp://apps.rti.com:8064/Rti.DocumentManagerService/";
            public const string EagleIqGatewayServiceENDPOINT_NAME = "netTcpRemote";
            public const string AdministrationServiceENDPOINT_NAME = "netTcpRemoteAdmin";
            public const string DocumentManagerServiceENDPOINT_NAME = "netTcpRemoteDocMgr";
            public const string IDMServerName = "rtitsta.rti.com";
            public const string ImportImageNASPath = @"\\rtiedinas.rti.com\FS_RTI_navigator\inbound\qa";
            public const string ImportImageUNIXPath = @"/var/imgstage/navigator/inbound/qa";
            public const string ImportImageSftpPath = @"/var/dlctmp";
            public const string ImportImageSftpHost = @"rtitsta";
            public const string BinDirectory = @"c:\RACServices\bin\";
            public const string RtiDllBuild = "QA";
        #endif

        /// <summary>
        /// Pdf to Tif conversion options
        /// </summary>
        public const string ConvertPdfToTiffWith = "ConvertPdfToTiffWith";
        public const string GhostScript = "GhostScript";
        public const string Atalasoft = "Atalasoft";

        // Embillz pads this to their dates when reporting days
        public const int EmbillzPadStartDays = 241;

        public static readonly string[] DbConns = new string[] { 
            "TowerModelContainer",
            "CadAdminModelContainer",
            "IfamModelContainer",
            "RtTransBrokerAdminModelContainer" 
        };
    }
}
