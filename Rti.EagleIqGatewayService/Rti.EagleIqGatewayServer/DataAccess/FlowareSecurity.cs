using System.Collections.Generic;

namespace Rti.AdministrationServer.DataAccess
{
    class FlowareSecurity
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Dictionary<string, string> GetFlowareMapActivities(string workstation, string windowsUserId)
        {
            Log.Debug("Enter FlowareSecurity.GetFlowareMapActivities() UserId:[" + windowsUserId + "]");
            
            var flowareMapActivities = new Dictionary<string, string>();

            //TODO Implement retrieval
            flowareMapActivities.Add("a", "a");
            
            Log.Debug("Exit FlowareSecurity.GetFlowareMapActivities()");

            return flowareMapActivities;
        }

        public List<string> GetFlowareGroups(string workstation, string windowsUserId)
        {
            Log.Debug("Enter FlowareSecurity.GetFlowareGroups() UserId:[" + windowsUserId + "]");
            Log.Debug("Exit FlowareSecurity.GetFlowareGroups()");

            return new List<string> { "a" };
        }
    }
}
