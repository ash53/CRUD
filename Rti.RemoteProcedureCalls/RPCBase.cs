using System;
using System.Collections.Generic;
using System.Data;
using com.solvepoint.iqrpc.client;

namespace Rti.RemoteProcedureCalls
{
    /// <summary>
    /// RPCBase Class
    /// 
    ///<remarks>
    ///This is the Base Class used for all RPC Calls
    ///To implement, design an extended class with  the input, output parameters and input sets
    /// 
    /// </remarks>
    /// </summary>
    public class RpcBase
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IqRpc _rpc;
        private Connection _iqConn;
        private Commander _commander;
        private IqInputData _input;
        private IqInputSet _inputSet;
        private readonly string _strDirPath = AppDomain.CurrentDomain.BaseDirectory;
        private readonly Dictionary<string, dynamic> _inputParameters;
        private Dictionary<string, string> _outputParameters;
        private DataSet _inputDataSet = new DataSet("inputDataSet");
        private string _successResult = "outSuccess";
        private bool _hasOutParams = true;

        public string RpcServer { get; set; }
        public string RpcName { get; set; }
        public string RpcVersion { get; set; }
        public string OutParams { get; set; }
        public bool OutResult { get; set; }
        public string OutMessage { get; set; }
        public KeyValuePair<string, string> ParamaterKeyValuePair;
        public IqResultData Results;
        public string[] InputSetNames;

        public void SetHasOutParams(bool hasOutParams)
        {
            _hasOutParams = hasOutParams;
        }

        public void SetSuccessResult(string successResult)
        {
            _successResult = successResult;
        }

        public string GetSuccessResult()
        {
            return _successResult;
        }

        public DataSet InputSet
        {
            get
            {
                return _inputDataSet;
            }
            set
            {
                _inputDataSet = value;
            }
        }

        /// <summary>
        /// RPCBase()
        /// <remarks>
        /// Implementation with no parameters
        /// _inputParameters and _output parameters are created but must be set in the extended class.
        /// </remarks>
        /// </summary>
        protected RpcBase()
        {
            _inputParameters = new Dictionary<string, dynamic>();
            _outputParameters = new Dictionary<string, string>();
        }

        /// <summary>
        /// RPCBase()
        /// 
        /// <remarks>
        /// Implementation with Parameters.  All parameters are passed on instantiation
        /// </remarks>
        /// </summary>
        /// <param name="rpcName"></param>
        /// <param name="rpcVersion"></param>
        /// <param name="rpcServer"></param>
        /// <param name="inputParameters"></param>
        /// <param name="outputParameters"></param>
        protected RpcBase(string rpcName, string rpcVersion, string rpcServer,
                            Dictionary<string, dynamic> inputParameters, Dictionary<string, string> outputParameters)
        {
            RpcName = rpcName;
            RpcVersion = rpcVersion;
            RpcServer = rpcServer;
            _inputParameters = inputParameters;
            _outputParameters = outputParameters;
        }

        /// <summary>
        /// RPCBase()
        /// <remarks>
        /// IMplementation with Parameters including inputsets
        /// </remarks>
        /// </summary>
        /// <param name="rpcName"></param>
        /// <param name="rpcVersion"></param>
        /// <param name="rpcServer"></param>
        /// <param name="inputParameters"></param>
        /// <param name="inputset"></param>
        /// <param name="outputParameters"></param>
        protected RpcBase(string rpcName, string rpcVersion, string rpcServer,
                            Dictionary<string, dynamic> inputParameters,
                            DataSet inputset,
                            Dictionary<string, string> outputParameters)
        {
            RpcName = rpcName;
            RpcVersion = rpcVersion;
            RpcServer = rpcServer;
            _inputDataSet = inputset;
            _inputParameters = inputParameters;
            _outputParameters = outputParameters;
        }

        /// <summary>
        /// Setup()
        ///<remarks>
        ///sets up the connection information for an RPC call
        /// </remarks> 
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        private void SetUp()
        {
            if (!ConnectionFactory.isConfigured("default"))
            {
                String configFileName = _strDirPath + "\\SaIqClientRTI.xml";
                ConnectionFactory.configure("default", configFileName, null);
                Log.Debug("RPCBase configFileName:[" + configFileName + "]");
            }
            else
            {
                Log.Debug("RPCBase ConnectionFactory isConfigured");
            }
            
            //ML reset the serverName to its default. 8/14/2008
            if (RpcServer.Equals(""))
            {
                RpcServer = "mpi";
            }

            Log.Debug("RPCBase RpcServer:[" + RpcServer + "]");

            if (_iqConn == null)
            {
                _iqConn = ConnectionFactory.getInstance("default").getConnection(RpcServer);
            }
        }

        /// <summary>
        /// Teardown()
        /// <remarks>
        /// Cleans up the connection
        /// </remarks>
        /// </summary>
        protected void TearDown()
        {
            if (_iqConn != null)
            {
                if (_iqConn.isOpen())
                    _iqConn.close();
                _iqConn = null;
                _commander = null;
            }
        }

        /// <summary>
        /// CallRPC()
        /// <remarks>
        /// This is the call to run the actual RPC and is executed from the extended class
        /// 
        /// InputSetName contains the names of the input sets wich are in the same sequence in the array as in the dataset.
        /// 
        /// Results contains all of the results from the RPC call and can be accessed in the extended class
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        protected Boolean CallRpc()
        {
            try
            {
                RpcServer = "";
                SetUp();
                _commander = _iqConn.open();
                _commander.setAutoXA(false);
                _rpc = IqRpcFactory.getInstance(RpcServer).getRpc(RpcName, RpcVersion);
                _input = _rpc.getInputData();

                SetInputParamaters();

                //If _inputSet is not null then loop through the datatables in the dataset and create the input sets.
                if (_inputDataSet != null)
                {
                    if (_inputDataSet.Tables.Count > 0)
                    {
                        for (int i = 0; i < _inputDataSet.Tables.Count; i++)
                        {
                            _inputSet = _input.getInputSet(_inputDataSet.Tables[i].TableName);
                            DataTable dt = _inputDataSet.Tables[i];
                            SetInputSet(dt);
                        }
                    }
                }
                Results = _commander.executeRpc(_rpc);

                // Defaults to "outResult", set to alternate in extended class if needed
                OutResult = Results.getBoolean(_successResult);

                //Save the results in the class variables
                switch (RpcName)
                {
                    case "deplog":
                        OutParams = Results.getInteger("DepSeq").ToString() + "|" + Results.getInteger("Count").ToString();
                        break;
                    case "procNotes":
                        if (Results.getString("outtxt").Split('|').Length > 1)
                        {
                            OutParams = Results.getString("outtxt").Replace('|', '[') + "|" + Results.getString("outcreateperson") + "|" +
                                       Results.getString("outcreatedt") + "|" + Results.getString("outmodperson") + "|" +
                                       Results.getString("outmoddt") + "|" + Results.getString("outParams");
                        }
                        else
                        {
                            OutParams = Results.getString("outtxt") + "|" + Results.getString("outcreateperson") + "|" +
                                       Results.getString("outcreatedt") + "|" + Results.getString("outmodperson") + "|" +
                                       Results.getString("outmoddt") + "|" + Results.getString("outParams");
                        }
                        break;
                    default:
                        if (_hasOutParams)
                        {
                            if (Results.getString("outParams") != null)
                            {
                                OutParams = Results.getString("outParams");
                            }
                            else
                            {
                                OutParams = null;
                            }
                        }
                        else
                        {
                            OutParams = null;
                        }
                        break;
                }

                return true;
            }
            catch (InvalidParameterException ex)
            {
                OutResult = Results.getBoolean("outResult");
                return OutResult;
            }
            catch (Exception ex)
            {
                OutResult = false;
                OutParams = ex.Message;
                Log.Error(ex);
                return false;
            }
            finally
            {
                TearDown();
            }

        }

        /// <summary>
        /// SetInputParamaters()
        /// Loop through the _inputParameters Dictionary and setup the inputParameters for the RPC call
        /// </summary>
        private void SetInputParamaters()
        {
            try
            {
                foreach (KeyValuePair<string, dynamic> kvp in _inputParameters)
                {
                    _input.setParam(kvp.Key, kvp.Value);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// SetInputParameter
        /// <remarks>
        /// Called from extended class
        /// Each passed parameter is added to the Dictionary
        /// </remarks>
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        protected void SetInputParameter(string parameterName, dynamic parameterValue)
        {
            try
            {
                {
                    _inputParameters.Add(parameterName, parameterValue);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// SetInputSet()
        /// Takes a datatable and creates an input set from it
        /// </summary>
        /// <param name="dt"></param>
        private void SetInputSet(DataTable dt)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    _inputSet.addRow();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        string colType = dc.DataType.ToString();
                        switch (colType)
                        {
                            case "System.string":
                            case "System.String":
                                _inputSet.setField(dc.ColumnName, dr[dc].ToString());
                                break;
                            case "System.bool":
                            case "System.Boolean":
                                _inputSet.setField(dc.ColumnName, Convert.ToBoolean(dr[dc]));
                                break;
                            case "System.double":
                            case "System.Double":
                                _inputSet.setField(dc.ColumnName, Convert.ToDouble(dr[dc]));
                                break;
                            case "System.DateTime":
                                _inputSet.setField(dc.ColumnName, Convert.ToDateTime(dr[dc]));
                                break;
                            case "System.decimal":
                            case "System.Decimal":
                                _inputSet.setField(dc.ColumnName, Convert.ToDecimal(dr[dc]));
                                break;
                            case "System.int":
                            case "System.Int32":
                                _inputSet.setField(dc.ColumnName, Convert.ToInt32(dr[dc]));
                                break;
                            case "System.long":
                            case "System.Long":
                                _inputSet.setField(dc.ColumnName, Convert.ToInt64(dr[dc]));
                                break;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw ex;
            }
        }
    }
}
