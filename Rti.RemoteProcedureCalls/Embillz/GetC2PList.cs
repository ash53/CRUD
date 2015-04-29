using System;
using com.solvepoint.iqrpc.client;
using System.Data;
using ADODB;
using Rti.ExternalInterfaces.DataContracts;

namespace Rti.RemoteProcedureCalls.Embillz
{
    /// <summary>
    /// GetR2P
    /// <remarks>
    ///     Class to handle the GetC2P RPC
    /// </remarks>
    /// </summary>

    public class GetC2PList : RpcBase
    {
        //parameters
        public string strStatusFlag { get; set; }
        public string strSuspendFlag { get; set; }
        public string strMatchFlag { get; set; }
        public int intMaxRows { get; set; }
        public int intStartRow { get; set; }
        public string strSortField { get; set; }
        public string strSortDir { get; set; }
        public string strDocNo { get; set; }
        public AssemblyFilter Fltr { get; set; }

        public IqResultSet resultSet;
        public DataTable dtRemitInfo;



        /// <summary>
        /// GetCashItems()
        /// <remarks>
        /// Used to call GetC2P RPC
        /// </remarks>
        /// </summary>
        public GetC2PList()
            : base()
        {
            SetDefaultParameters();
        }


        /// <summary>
        /// GetC2Ps()
        /// <remarks>
        /// Used to call GetC2P RPC
        /// </remarks>
        /// </summary>
        //public GetR2P(string StatusFlag, string SuspendFlag, string MatchFlag, int MaxRows, int StartRow, string SortField, string SortDir, string DocNo)
        //    : base()
        public GetC2PList(GetCashItemsInputParams inputParams)
            : base()
        {
            SetDefaultParameters();

            RpcServer = inputParams.ServerName;
            intMaxRows = Convert.ToInt32(inputParams.MaxRows);
            intStartRow = Convert.ToInt32(inputParams.StartRow);
            strDocNo = inputParams.DocNo;
            Fltr = inputParams.Filters;
            strStatusFlag = Fltr.Status;
            strSuspendFlag = Fltr.PostingSuspends;
            strMatchFlag = Fltr.MatchStatus;
        }

        /// <summary>
        /// SetDefaultParameters()
        /// <remarks>
        /// Set the default parameters for the RPC
        /// </remarks>
        /// </summary>
        private void SetDefaultParameters()
        {
            RpcName = "getC2Ps";
            RpcVersion = "1";
            RpcServer = "longrpc";
            InputSetNames = new string[2];
            InputSetNames[0] = "inDocNoSet";
            InputSetNames[1] = "inttFilter";
            strStatusFlag = "Show only active items";
            strSuspendFlag = "Show all cash items";
            strMatchFlag = "Show all items";
            intMaxRows = 500;
            intStartRow = 1;
            strSortField = "";
            strSortDir = "";
        }

        /// <summary>
        /// SetInputParameters
        /// 
        /// Adds the parameter to the list of parmeters to be used in the RPC Call
        /// Base class will loop through this list
        /// </summary>
        public void SetInputParameters()
        {
            try
            {
                SetInputParameter("cstatus_flag", strStatusFlag);
                SetInputParameter("suspend_flag", strSuspendFlag);
                SetInputParameter("match_flag", strMatchFlag);
                SetInputParameter("inMaxRows", intMaxRows);
                SetInputParameter("inStartRow", intStartRow);
                SetInputParameter("inSortField", strSortField);
                SetInputParameter("inSortDir", strSortDir);
                //SetInputParameter("inDocNo", strDocNo);

                base.InputSet = new DataSet();
                DataTable dt = base.InputSet.Tables.Add(InputSetNames[0]);
                dt.Columns.Add("docno", typeof(String));
                for (int i = 0; i < strDocNo.Split(',').Length; i++)
                {
                    DataRow row = dt.NewRow();
                    dt.Rows.Add(row);
                    row.SetField("docno", strDocNo.Split(',')[i].ToString());
                }
                DataTable dt1 = base.InputSet.Tables.Add(InputSetNames[1]);
                dt1.Columns.Add("cfield", typeof(String));
                dt1.Columns.Add("coperator", typeof(String));
                dt1.Columns.Add("cvalue", typeof(String));
                
                //begin input filter
                if (!Fltr.Practice.Equals(""))
                {
                    DataRow row = dt1.NewRow();
                    dt1.Rows.Add(row);
                    row.SetField("cfield", "practice");
                    row.SetField("coperator", "=");
                    row.SetField("cvalue", Fltr.Practice);
                }

                if (!Fltr.ExtPayor.Equals(""))
                {
                    DataRow row = dt1.NewRow();
                    dt1.Rows.Add(row);
                    row.SetField("cfield", "extpaycd");
                    row.SetField("coperator", "=");
                    row.SetField("cvalue", Fltr.ExtPayor);
                }

                if ((!Fltr.ServiceType.Equals("")) && (!Fltr.ServiceType.Equals("ALL")))
                {
                    DataRow row = dt1.NewRow();
                    dt1.Rows.Add(row);
                    row.SetField("cfield", "serviceid");
                    row.SetField("coperator", "=");
                    row.SetField("cvalue", Fltr.ServiceType.Substring(0, 1));
                }

                if (!Fltr.DocDetail.Equals(""))
                {
                    DataRow row = dt1.NewRow();
                    dt1.Rows.Add(row);
                    row.SetField("cfield", "docdet");
                    row.SetField("coperator", "=");
                    row.SetField("cvalue", Fltr.DocDetail.Substring(1));
                }

                if (Convert.ToDecimal(Fltr.CheckAmountMin) != -1)
                {
                    DataRow row = dt1.NewRow();
                    dt1.Rows.Add(row);
                    row.SetField("cfield", "checkamt");
                    row.SetField("coperator", Fltr.CheckAmountMin.ToString());
                    if (Convert.ToDecimal(Fltr.CheckAmountMax) != -1)
                    {
                        row.SetField("cvalue", Fltr.CheckAmountMax.ToString());
                    }
                    else
                    {
                        row.SetField("cvalue", Fltr.CheckAmountMin.ToString());
                    }
                }

                if (Convert.ToDateTime(Fltr.DateMin) != DateTime.MinValue)
                {
                    DataRow row = dt1.NewRow();
                    dt1.Rows.Add(row);
                    row.SetField("cfield", "depdt");
                    row.SetField("coperator", Fltr.DateMin.ToString());
                    if (Convert.ToDateTime(Fltr.DateMax) != DateTime.MinValue)
                    {
                        row.SetField("cvalue", Fltr.DateMax.ToString());
                    }
                    else
                    {
                        row.SetField("cvalue", Fltr.DateMin.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// CallRPC()
        /// <remarks>
        /// Sets the input parameters
        /// </remarks>
        /// </summary>
        new public GetCashItemsResults1 CallRPC()
        {
            var rpcResult = new GetCashItemsResults1();
            try
            {

                SetInputParameters();
                base.CallRpc();

                //Process results
                resultSet = base.Results.getResultSet("outcash2post");
                Recordset rs = resultSet.convertToRecordset();
                System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter();
                DataSet ds = new DataSet("AssemblyInfo");
                da.Fill(ds, rs, "CashInfo");
                if (ds != null && ds.Tables.Count > 0)
                {
                    //dtRemitInfo = ds.Tables["RemitInfo"];
                    rpcResult.CashItems = ds.Tables["CashInfo"];
                }
                rpcResult.OutMsg = OutParams ?? "";
                rpcResult.Success = OutResult;

                return rpcResult;
            }
            catch (Exception ex)
            {
                rpcResult.Success = false;
                rpcResult.OutMsg = ex.Message.ToString();
                return rpcResult;
            }

        }

    }
}
