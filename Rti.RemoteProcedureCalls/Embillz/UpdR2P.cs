using System;
using System.Collections.Generic;
using com.solvepoint.iqrpc.client;
using Rti.InternalInterfaces.DataContracts;

namespace Rti.RemoteProcedureCalls.Embillz
{
    public class UpdR2P : RpcBase
    {
        //parameters
        public string UserId { get; set; }
        public int InKey { get; set; }
        public string InDocDet { get; set; }
        public DateTime InPrChkDt { get; set; }
        public string InCheckNum { get; set; }
        public string InExtPayCd { get; set; }
        public string InPractice { get; set; }
        public string InDiv { get; set; }
        public string InProvId { get; set; }
        
        public IqResultSet resultSet;

        /// <summary>
        /// CreateMatch()
        /// <remarks>
        /// Used to call match_create RPC
        /// </remarks>
        /// </summary>
        public UpdR2P()
            : base()
        {
            SetDefaultParameters();
        }


        /// <summary>
        /// UpdR2P(...)
        /// <remarks>
        /// Used to call procNotes RPC
        /// </remarks>
        /// </summary>
        public UpdR2P(UpdR2PInputParams inputParams)
            : base()
        {
            SetDefaultParameters();

            RpcServer = inputParams.ServerName;
            UserId = inputParams.UserId;
            InKey =  Convert.ToInt32(inputParams.InKey);
            InDocDet = inputParams.InDocDet;
            InPrChkDt = inputParams.InPrCheckDt;
            InCheckNum = inputParams.InCheckNum;
            InExtPayCd = inputParams.InExtPayCd;
            InPractice = inputParams.InPractice;
            InDiv = inputParams.InDivision;
            InProvId = inputParams.InProvId;
        }


        /// <summary>
        /// SetDefaultParameters()
        /// <remarks>
        /// Set the default parameters for the RPC
        /// </remarks>
        /// </summary>
        private void SetDefaultParameters()
        {
            RpcName = "updremit2post";
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
                SetInputParameter("indocdet", InDocDet);
                SetInputParameter("inprchkdt", InPrChkDt);
                SetInputParameter("inchecknum", InCheckNum);
                SetInputParameter("inextpaycd", InExtPayCd);
                SetInputParameter("inpractice", InPractice);
                SetInputParameter("indiv", InDiv);
                SetInputParameter("inprovid", InProvId);
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
                rpcResult = "False|" + ex.Message.ToString();
                return rpcResult;
            }

        }
    }
}
