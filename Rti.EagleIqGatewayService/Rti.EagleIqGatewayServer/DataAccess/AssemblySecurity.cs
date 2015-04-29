using System.Collections.Generic;

namespace Rti.AdministrationServer.DataAccess
{
    class AssemblySecurity
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Dictionary<string, bool> GetAssemblyPermissions(string workstation, string unixUserId)
        {
            Log.Debug("Enter AssemblySecurity.GetAssemblyPermissions() UserId:[" + unixUserId + "]");

            // TODO Implement a retrieval of the assembly permissions based on the unixUserId

            var assemblyPermissions = new Dictionary<string, bool>();

            // Remove once implemented correctly
            assemblyPermissions.Add("MatchEnabled", true);
            assemblyPermissions.Add("ReleaseEnabled", true);
            assemblyPermissions.Add("PendingMatchEnabled", true);
            assemblyPermissions.Add("PendingReleaseEnabled", true);
            // End Remove

            Log.Debug("Exit AssemblySecurity.GetAssemblyPermissions()");

            return assemblyPermissions;
        }
    }
}

