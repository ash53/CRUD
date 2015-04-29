using System;
using System.Data;
using Rti.InternalInterfaces.DataContracts;
using Rti.RemoteProcedureCalls.Embillz;

namespace Rti.EagleIqGatewayServer.RPCs
{
    public class NavMatching
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RpcOutMessage CreateMatch(RpcInMessage rpcInMessage, string server, DataTable dataTable)
        {
            Log.Debug("Enter CreateMatch() UserId:[" + rpcInMessage.UserName + "]");

            var rpcOutMessage = new RpcOutMessage();
            var myDataSet = new DataSet("temp");
            var myDataTable = myDataSet.Tables.Add("intmp");

            try
            {
                var createMatch = new CreateMatch
                {
                    UserId = rpcInMessage.UserName
                };
                //Mark Lane changing this to explicitly set the columns otherwise will not be in eagleiq inputset table
                //also must set it by name and type or you will get an error that column belongs to another table.
                foreach(DataColumn column in dataTable.Columns)
                {
                    myDataTable.Columns.Add(column.ColumnName, column.DataType);
                }
                //Mark Lane changing this to get the right columns and rows otherwise its empty
                foreach (DataRow dr in dataTable.Rows)
                {
                    myDataTable.ImportRow(dr);
                }
                
                createMatch.InputSet = myDataSet;
                createMatch.CallRpc();

                rpcOutMessage.IsSuccess = createMatch.Results.getBoolean("outResult");
                rpcOutMessage.OutMessage = createMatch.Results.getString("outParams");

                Log.Debug("Exit CreateMatch() Status:[" + rpcOutMessage.IsSuccess + "]");
                return rpcOutMessage;
            }
            catch(Exception exception)
            {
                Log.Error("Exit CreateMatch(), FAIL:" + exception);
                return null;
            }
        }

        public RpcOutMessage ApproveMatch(RpcInMessage rpcInMessage, string server, int matchId)
        {
            Log.Debug("Enter ApproveMatch() UserId:[" + rpcInMessage.UserName + "] matchId:[" + matchId + "]");

            var rpcOutMessage = new RpcOutMessage();

            try
            {
                var approveMatch = new ApproveMatch
                {
                    UserId = rpcInMessage.UserName,
                    MatchId = matchId
                };

                approveMatch.CallRpc();

                rpcOutMessage.IsSuccess = approveMatch.Results.getBoolean("outResult");
                rpcOutMessage.OutMessage = approveMatch.Results.getString("outParams");

                Log.Debug("Exit ApproveMatch() Status:[" + rpcOutMessage.IsSuccess + "]");
                return rpcOutMessage;
            }
            catch(Exception exception)
            {
                Log.Error("Exit ApproveMatch(), FAIL:" + exception);
                return null;
            }
        }

        public string ProcessR2P(string strWorkstation, UpdR2PInputParams rpcInMessage)
        {
            Log.Debug("Enter ProcessR2P() UserId:[" + rpcInMessage.UserId + "] key:[" + rpcInMessage.InKey + "]");

            UpdR2P updR2P = null;

            try
            {
                updR2P = new UpdR2P(rpcInMessage);
                string strRet = updR2P.CallRPC();

                updR2P = null;
                if (strRet.Equals("") || strRet.Split('|').Length < 2)
                {
                    strRet = "False|" + "Error: Unknown";
                }

                Log.Debug("Exit ProcessR2P() Status:[" + strRet + "]");
                return strRet;
            }
            catch (Exception exception)
            {
                Log.Error("Exit ProcessR2P(), FAIL:" + exception);
                if (updR2P != null)
                {
                    updR2P = null;
                }
                return "";
            }
        }
    }
}
