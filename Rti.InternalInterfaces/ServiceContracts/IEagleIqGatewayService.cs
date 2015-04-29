using System.Data;
using System.ServiceModel;
using Rti.InternalInterfaces.DataContracts;

namespace Rti.InternalInterfaces.ServiceContracts
{
    [ServiceContract]
    public interface IEagleIqGatewayService
    {
        [OperationContract]
        bool IsAlive();

        [OperationContract]
        NavServiceInfo GetServiceInfo(string workstation, string username);

        #region Stored Procedures

        [OperationContract]
        DataTable GetPostBd(string workstation, string username,
            string mapName, string actName, string practice, string division,
            string depDtStart, string depDtEnd, string embAcct, string reason, string checkNum,
            string checkAmountMin, string checkAmountMax, string paidAmountMin, string paidAmountMax,
            string docType, string docDet, string who, string docNo, string parDocNo);

        #endregion


        #region RPCs

        [OperationContract]
        RpcOutMessage ApproveMatch(RpcInMessage rpcInMessage, string server, int matchId);
        [OperationContract]
        RpcOutMessage CreateMatch(RpcInMessage rpcInMessage, string server, DataTable dataTable);
        [OperationContract]
        string ProcessR2P(string strWorkstation, UpdR2PInputParams rpcInMessage);

        #endregion
    }
}

