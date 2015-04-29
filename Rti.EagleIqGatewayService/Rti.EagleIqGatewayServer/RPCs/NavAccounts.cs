using System;
using Rti.InternalInterfaces.DataContracts;
using Rti.RemoteProcedureCalls.Embillz;

namespace Rti.EagleIqGatewayServer.RPCs
{
    public class NavAccounts
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RpcOutMessage SearchAccounts(RpcInMessage rpcInMessage, SearchAccountsAttributes searchAccountsAttributes)
        {
            Log.Debug("Enter SearchAccounts() UserId:[" + rpcInMessage.UserName + "]");

            try
            {
                var rpcOutMessage = new RpcOutMessage();

                var searchAccounts = new SearchAccounts
                {
                    AcctNo = searchAccountsAttributes.AcctNo,
                    BasisPtr = searchAccountsAttributes.BasisPtr,
                    ContextString = rpcInMessage.Context,
                    DataSetPtr = searchAccountsAttributes.DataSetPtr,
                    EnctrNo = searchAccountsAttributes.EnctrNo,
                    FirstName = searchAccountsAttributes.FirstName,
                    FirstNameBegins = searchAccountsAttributes.FirstNameBegins,
                    GroupNo = searchAccountsAttributes.GroupNo,
                    LastNameBegins = searchAccountsAttributes.LastNameBegins,
                    LastName = searchAccountsAttributes.LastName,
                    NumRecsReturned = searchAccountsAttributes.NumRecsReturned,
                    OriginAddr1 = searchAccountsAttributes.OriginAddr1,
                    OriginZip = searchAccountsAttributes.OriginZip,
                    PhoneNo = searchAccountsAttributes.PhoneNo,
                    PolicyNo = searchAccountsAttributes.PolicyNo,
                    Practice = searchAccountsAttributes.Practice,
                    Ssn = searchAccountsAttributes.Ssn,
                    SvcDt = searchAccountsAttributes.SvcDt
                };

                searchAccounts.CallRpc();

                rpcOutMessage.IsSuccess = searchAccounts.Results.getBoolean("success");
                rpcOutMessage.OutMessage = searchAccounts.Results.getString("outMessage");

                if (rpcOutMessage.IsSuccess)
                {
                    rpcOutMessage.OutDataTable =
                        CommonFunctions.RecordSetToDataTable(searchAccounts.Results.getResultSet("acctSet").convertToRecordset(), "A", "B");
                }
                else
                {
                    rpcOutMessage.OutDataTable = null;
                }

                Log.Debug("Exit SearchAccounts() Status:[" + rpcOutMessage.IsSuccess + "]");
                return rpcOutMessage;
            }
            catch (Exception exception)
            {
                Log.Error("Exit SearchAccounts(), FAIL:" + exception);
                return null;
            }
        }
    }
}
