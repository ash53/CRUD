using System;
using System.Collections.Generic;
using com.solvepoint.iqrpc.client;

namespace Rti.RemoteProcedureCalls.Embillz
{
    public class ProcNotes : RpcBase
    {
        //parameters
        public string UserId { get; set; }
        public int InKey { get; set; }
        public string InTxt { get; set; }
        public string InType { get; set; }
        public string InAction { get; set; }

        public IqResultSet resultSet;

        /// <summary>
        /// CreateMatch()
        /// <remarks>
        /// Used to call match_create RPC
        /// </remarks>
        /// </summary>
        public ProcNotes()
            : base()
        {
            SetDefaultParameters();
        }


        /// <summary>
        /// ProcNotes(...)
        /// <remarks>
        /// Used to call procNotes RPC
        /// </remarks>
        /// </summary>
        public ProcNotes(string strServerName, string strUserId, int intNoteKey, string strNote, string strType, string strAction)
            : base()
        {
            SetDefaultParameters();

            RpcServer = strServerName;
            UserId = strUserId;
            InKey =  intNoteKey;
            InTxt = strNote;
            InType = strType;
            InAction = strAction;

        }


        /// <summary>
        /// SetDefaultParameters()
        /// <remarks>
        /// Set the default parameters for the RPC
        /// </remarks>
        /// </summary>
        private void SetDefaultParameters()
        {
            RpcName = "procNotes";
            RpcVersion = "1";
            RpcServer = "mpi";
            SetSuccessResult("outResult");

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
                SetInputParameter("inuserid", UserId);
                SetInputParameter("inkey", InKey);
                SetInputParameter("intxt", InTxt);
                SetInputParameter("intype", InType);
                SetInputParameter("inAction", InAction);

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
        new public string CallRPC()
        {
            string rpcResult = "";
            try
            {

                SetInputParameters();
                base.CallRpc();

                rpcResult = OutResult.ToString() + "|" + OutParams;
                return rpcResult;
            }
            catch (Exception ex)
            {
                rpcResult = "false|" + ex.Message.ToString();
                return rpcResult;
            }

        }
    }
}
