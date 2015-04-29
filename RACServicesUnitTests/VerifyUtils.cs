using System;
using System.Data;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RACServicesUnitTests
{
    public static class VerifyUtils
    {
        // Default remote server to target
        private static string _remoteServer = "net.tcp://apps.rti.com";

        public static string DocMgrRemoteUrl = "net.tcp://apps.rti.com:8064/Rti.DocumentManagerService/";
        public static string DocMgrRemoteEndpointName = "netTcpRemoteDocMgr";
        public static string AdminRemoteUrl = "net.tcp://apps.rti.com:8025/Rti.AdministrationService/";
        public static string AdminRemoteEndpointName = "netTcpRemoteAdmin";
        public static string EiqRemoteUrl = "net.tcp://localhost:8044/Rti.EagleIqGatewayService/";
        public static string EiqRemoteEndpointName = "netTcpRemote";

        /// <summary>
        /// Set's the server by name. I.e. "apps.rti.com"
        /// </summary>
        public static string RemoteServer
        {
            set
            {
                _remoteServer = value;
                DocMgrRemoteUrl = "net.tcp://" + value + ":8064/Rti.DocumentManagerService/";
                AdminRemoteUrl = "net.tcp://" + value + ":8025/Rti.AdministrationService/";
                EiqRemoteUrl = "net.tcp://" + value + ":8044/Rti.EagleIqGatewayService/";
            }
        }

        /// <summary>
        /// Simple catch for if a data table has rows
        /// </summary>
        /// <param name="dt"></param>
        public static void VerifyDataTable(DataTable dt)
        {
            Assert.IsNotNull(dt);
            Debug.Print("Rows in result:[{0}]", dt.Rows.Count);

            if (dt.Rows.Count > 0)
            {
                // Dump one row
                DataRow row = dt.Rows[0];
                foreach (DataColumn column in dt.Columns)
                {
                    Debug.Print(column.ColumnName + ":[" + row[column] + "]");
                }
            }

            Assert.IsTrue(dt.Rows.Count > 0);
        }

        /// <summary>
        /// Assert for testing exceptions
        /// </summary>
        public static class MyAssert
        {
            public static void Throws<T>( Action func ) where T : Exception
            {
                var exceptionThrown = false;
                try
                {
                    func.Invoke();
                }
                catch ( T )
                {
                    exceptionThrown = true;
                }

                if ( !exceptionThrown )
                {
                    throw new AssertFailedException(
                        String.Format("An exception of type {0} was expected, but not thrown", typeof(T))
                        );
                }
            }
        }

    }
}
