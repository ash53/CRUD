using System.Collections.Generic;

namespace Rti.AdministrationServer.DataAccess
{
    class WorkFlowSecurity
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<string> GetWorkFlowCodingTypes(string workstation, string unixUserId)
        {
            Log.Debug("Enter WorkFlowSecurity.GetWorkFlowCodingTypes() UserId:[" + unixUserId + "]");
            Log.Debug("Exit WorkFlowSecurity.GetWorkFlowCodingTypes()");

            return new List<string> {"a"};
        }

        public Dictionary<string, string> GetWorkFlowActivities(string workstation, string unixUserId)
        {
            Log.Debug("Enter WorkFlowSecurity.GetWorkFlowActivities() UserId:[" + unixUserId + "]");

            var workFlowActivities = new Dictionary<string, string>();

            //TODO Implement retrieval
            workFlowActivities.Add("a","a");

            Log.Debug("Exit WorkFlowSecurity.GetWorkFlowActivities()");

            return workFlowActivities;
        }

        public List<string> GetWorkFlowPractices(string workstation, string unixUserId)
        {
            Log.Debug("Enter WorkFlowSecurity.GetWorkFlowPractices() UserId:[" + unixUserId + "]");
            Log.Debug("Exit WorkFlowSecurity.GetWorkFlowPractices()");

            //TODO Implement retrieval

            return new List<string> { "a" };
        }

        public List<string> GetWorkFlowFacilityTypes(string workstation, string unixUserId)
        {
            Log.Debug("Enter WorkFlowSecurity.GetWorkFlowFacilityTypes() UserId:[" + unixUserId + "]");
            Log.Debug("Exit WorkFlowSecurity.GetWorkFlowFacilityTypes()");

            //TODO Implement retrieval

            return new List<string> { "a" };
        }
    }
}
