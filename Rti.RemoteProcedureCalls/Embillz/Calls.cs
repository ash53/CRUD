using System;

namespace Rti.RemoteProcedureCalls.Embillz
{
    public class ValidateAccountNumber : RpcBase
    {
        public string ContextString { get; set; }
        public string Practice { get; set; }
        public string AccountNumber { get; set; }
        public string EncounterNumber { get; set; }
        public string PatientName { get; set; }
        public string ServiceDate { get; set; }
        public string OriginAddress1 { get; set; }
        public string OriginZipCode { get; set; }
        public string AccountNoOut { get; set; }

        public ValidateAccountNumber()
        {
            RpcName = "validateacctno";
            RpcVersion = "1";
        }

        public new void CallRpc()
        {
            SetInputParameter("contextString", ContextString);
            SetInputParameter("practice", Practice);
            SetInputParameter("acctNo", AccountNumber);
            SetInputParameter("enctrNo", EncounterNumber);
        }
    }

    public class SearchAccounts : RpcBase
    {
        public string ContextString { get; set; }
        public int NumRecsReturned { get; set; }
        public int BasisPtr { get; set; }
        public int DataSetPtr { get; set; }
        public string Ssn { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PolicyNo { get; set; }
        public string AcctNo { get; set; }
        public string PhoneNo { get; set; }
        public string GroupNo { get; set; }
        public bool LastNameBegins { get; set; }
        public bool FirstNameBegins { get; set; }
        public string Practice { get; set; }
        public string EnctrNo { get; set; }
        public DateTime SvcDt { get; set; }
        public string OriginAddr1 { get; set; }
        public string OriginZip { get; set; }

        public SearchAccounts()
        {
            RpcName = "searchaccts";
            RpcVersion = "1";
        }

        public new void CallRpc()
        {
            SetInputParameter("contextString", ContextString);
            SetInputParameter("numRecsReturned", NumRecsReturned);
            SetInputParameter("basisPtr", BasisPtr);
            SetInputParameter("ssn", Ssn);
            SetInputParameter("lastName", LastName);
            SetInputParameter("firstName", FirstName);
            SetInputParameter("policyNo", PolicyNo);
            SetInputParameter("acctNo", AcctNo);
            SetInputParameter("phoneNo", PhoneNo);
            SetInputParameter("groupNo", GroupNo);
            SetInputParameter("lastNameBegins", LastNameBegins);
            SetInputParameter("firstNameBegins", FirstNameBegins);
            SetInputParameter("practice", Practice);
            SetInputParameter("enctrNo", EnctrNo);
            SetInputParameter("svcDt", SvcDt);
            SetInputParameter("originAddr1", OriginAddr1);
            SetInputParameter("originZip", OriginZip);
            SetInputParameter("dataSetPtr", DataSetPtr);
        }
    }

    public class ValidateUnixLogin : RpcBase
    {
        public string UnixUserId { get; set; }
        public string Password { get; set; }
        public bool SetContext { get; set; }
        public string FetchOptions { get; set; }

        public ValidateUnixLogin()
        {
            RpcName = "authenticateUser";
            RpcVersion = "1";
            SetSuccessResult("outSuccess");
            SetHasOutParams(false);
        }

        public new void CallRpc()
        {
            SetInputParameter("inUserId", UnixUserId);
            SetInputParameter("inPassword", Password);
            SetInputParameter("inSetContext", SetContext);
            SetInputParameter("inFetchUserInfo", FetchOptions);

            base.CallRpc();
        }
    }

    public class ApproveMatch : RpcBase
    {
        public string UserId { get; set; }
        public int MatchId { get; set; }

        public ApproveMatch()
        {
            RpcName = "match_approval";
            RpcVersion = "1";
        }

        public new void CallRpc()
        {
            SetInputParameter("inuserid", UserId);
            SetInputParameter("inmatchid", MatchId);

            base.CallRpc();
        }
    }

    public class CreateMatch : RpcBase
    {
        public string UserId { get; set; }

        public CreateMatch()
        {
            RpcName = "match_create";
            RpcVersion = "1";
        }

        public new void CallRpc()
        {
            SetInputParameter("inuserid", UserId);

            base.CallRpc();
        }
    }

    public class GetPermissions : RpcBase
    {
        public string UserId { get; set; }

        public GetPermissions()
        {
            RpcName = "get_permissions";
            RpcVersion = "1";
            SetSuccessResult("outResult");
            SetHasOutParams(true);
        }

        public new void CallRpc()
        {
            SetInputParameter("user_id", UserId);

            base.CallRpc();
        }
    }
}
