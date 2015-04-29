using System.Data;
using System.ServiceModel;
using Rti.EagleIqGatewayServer.RPCs;
using Rti.EagleIqGatewayServer.StoredProcedures;
using Rti.InternalInterfaces.DataContracts;
using Rti.InternalInterfaces.ServiceContracts;


namespace Rti.EagleIqGatewayServer.Servers
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Single)]
    [System.Runtime.InteropServices.ClassInterface(System.Runtime.InteropServices.ClassInterfaceType.None)]
    public class EagleIqGatewayServer : IEagleIqGatewayService
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static NavProcedures _navProcedures;
        private static NavMatching _navMatching;

        public EagleIqGatewayServer()
        {
            _navProcedures = new NavProcedures();
            _navMatching = new NavMatching();
        }

        public bool IsAlive()
        {
            Log.Debug("Alive!!!");
            return true;
        }

        public NavServiceInfo GetServiceInfo(string workstation, string username)
        {
            return Rti.ServiceInfo.GetServiceInfo();
        }

        #region Stored Procedures

        public DataTable GetPostBd(string workstation, string username,
            string mapName, string actName, string practice, string division,
            string depDtStart, string depDtEnd, string embAcct, string reason, string checkNum,
            string checkAmountMin, string checkAmountMax, string paidAmountMin, string paidAmountMax,
            string docType, string docDet, string who, string docNo, string parDocNo)
        {
            return _navProcedures.GetPostBd(workstation, username,
                mapName, actName, practice, division,
                depDtStart, depDtEnd, embAcct, reason, checkNum,
                checkAmountMin, checkAmountMax, paidAmountMin, paidAmountMax,
                docType, docDet, who, docNo, parDocNo);
        }

        #endregion


        #region RPCs

        public RpcOutMessage ApproveMatch(RpcInMessage rpcInMessage, string server, int matchId)
        {
            return _navMatching.ApproveMatch(rpcInMessage, server, matchId);
        }

        public RpcOutMessage CreateMatch(RpcInMessage rpcInMessage, string server, DataTable dataTable)
        {
            return _navMatching.CreateMatch(rpcInMessage, server, dataTable);
        }

        public string ProcessR2P(string strWorkstation, UpdR2PInputParams rpcInMessage)
        {
            return _navMatching.ProcessR2P(strWorkstation, rpcInMessage);
        }

        #endregion


    }
}
