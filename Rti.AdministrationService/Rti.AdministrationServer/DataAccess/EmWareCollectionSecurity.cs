using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Rti.InternalInterfaces.DataContracts;
using System.Linq;
using Rti.RemoteProcedureCalls.Embillz;
using Rti.DataModel;


namespace Rti.AdministrationServer.DataAccess
{
    class EmWareCollectionSecurity
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private RpcOutMessage GetAssemblyPermissions(RpcInMessage rpcInMessage)
        {
            Log.Debug("Enter GetAssemblyPermissions() UserId:[" + rpcInMessage.UserName + "]");

            try
            {
                var rpcOutMessage = new RpcOutMessage();
                
                var permissions = new GetPermissions {UserId = rpcInMessage.UserName};

                permissions.CallRpc();

                rpcOutMessage.IsSuccess = permissions.Results.getBoolean(permissions.GetSuccessResult());

                if (rpcOutMessage.IsSuccess)
                {
                    rpcOutMessage.OutDataTable = new DataTable();
                    rpcOutMessage.OutDataTable = CommonFunctions.RecordSetToDataTable(permissions.Results.getResultSet("permissionSet").convertToRecordset(), "permissionSet", "permissionTable");
                }

                // User exists but has no permissions listed
                if (rpcOutMessage.OutDataTable.Select().Length < 1)
                {
                    rpcOutMessage.IsSuccess = false;
                }

                return rpcOutMessage;
            }
            catch (Exception exception)
            {
                Log.Error("Exit GetAssemblyPermissions(), FAIL:" + exception);
                return null;
            }
        }

        private RpcOutMessage ValidateUnixLogin(RpcInMessage rpcInMessage, string password, bool setContext, string fetchOptions)
        {
            Log.Debug("Enter ValidateUnixLogin() UserId:[" + rpcInMessage.UserName + "] fetchOptions:[" + fetchOptions + "]");

            try
            {
                var rpcOutMessage = new RpcOutMessage();

                var login = new ValidateUnixLogin
                {
                    UnixUserId = rpcInMessage.UserName,
                    Password = password,
                    SetContext = setContext,
                    FetchOptions = fetchOptions
                };
                
                login.CallRpc();

                rpcOutMessage.IsSuccess = login.Results.getBoolean("outSuccess");
                rpcOutMessage.OutMessage = login.Results.getString("outMessage");

                rpcOutMessage.OutDictionary = new Dictionary<string, string>();
                if(rpcOutMessage.IsSuccess)
                {
                    rpcOutMessage.OutDictionary.Add("outUserInfo", login.Results.getString("outUserInfo"));

                    rpcOutMessage.OutDictionary.Add("outEmpKey",
                        login.Results.getBoolean("outSuccess") ? login.Results.getInteger("outEmpKey").ToString(CultureInfo.InvariantCulture) : null);
                }

                Log.Debug("Exit ValidateUnixLogin() Status:[" + rpcOutMessage.IsSuccess + "]");
                return rpcOutMessage;
            }
            catch(Exception exception)
            {
                Log.Error("Exit ValidateUnixLogin(), FAIL:" + exception);
                return null;
            }
        }

        private string GetGroupNameByUserName(string unixUserName)
        {
            using (var db = new TowerModelContainer())
            {
                var userAttributesQuery = (from a in db.USERATTRIBUTES
                                           where a.USERNAME == unixUserName
                                           && a.USERATTRIBUTEKEY == "groupname"
                                           select a).FirstOrDefault();

                if (userAttributesQuery != null)
                {
                    return userAttributesQuery.USERATTRIBUTEVALUE;
                }
                return "";
            }
        }

        public Login.Permissions EmWareCollectionLogin(string workstation, string userName,
            Login.UserCredentials loginCredentials)
        {
            Log.Debug("Enter EmWareCollectionSecurity.EmWareCollectionLogin() UserId:[" + userName + "]");
            Log.Debug("UnixUserId:[" + loginCredentials.UnixUserName + "] Domain:[" + loginCredentials.WindowsDomain + "]");

            // Initialize our permissions data structure
            var permissions = new Login.Permissions
            {
                LoginSuccessful = false,
                LoginMessage = "",
                UserGroup = "",
                WindowsUsername = "",
                Assembly =
                {
                    IsAutoView = false,
                    SupervisorId = "",
                    IsSupervisor = false,
                    CanApproveMatch = false,
                    CanAssembleAll= false,
                    CanDelete = false,
                    CanPendMatch = false,
                    CanUpdate = false,
                    CanView = false,
                    Experience = "",
                    Status = ""
                },
                IDM =
                {
                    CanAdminAnnotations = false,
                    CanAnnotate = false,
                    CanAppend = false,
                    CanExport = false,
                    CanImport = false,
                    CanModify = false,
                    CanModifyAnnotations = false,
                    CanPrint = false,
                    CanViewAnnotations = false,
                    DefaultApplication = "",
                    AnnotateGroup = "",
                    AnnotateAll = "",
                    CanModifyTitle = false
                },
                WorkFlow =
                {
                    CanAutoView = false,
                    IsSupervisor = false,
                    Supervisor = ""
                }
            };

            // RPC EmBillz to verify the unix user credentials
            var rpcOutMessage = ValidateUnixLogin(new RpcInMessage {Context = "", UserName = userName, Workstation = workstation},
                    loginCredentials.UnixPassword, loginCredentials.SetContext, loginCredentials.FetchOptions );

            // Total failure
            if (rpcOutMessage == null)
            {
                permissions.LoginMessage = "Validate login failure!";
                permissions.LoginSuccessful = false;
                Log.Debug("Exit. Status:[" + permissions.LoginSuccessful + "] Message:[" + permissions.LoginMessage + "]");
                return permissions;
            }

            // Call worked, just didn't validate, tell user why
            if (!rpcOutMessage.IsSuccess)
            {
                permissions.LoginMessage = rpcOutMessage.OutMessage;
                permissions.LoginSuccessful = rpcOutMessage.IsSuccess;
                Log.Debug("Exit. Status:[" + permissions.LoginSuccessful + "] Message:[" + permissions.LoginMessage + "]");
                return permissions;
            }
            
            permissions.LoginSuccessful = rpcOutMessage.IsSuccess;
            permissions.UnixUsername = userName;

            // RPC worked so lookup group, then user attributes
            if (rpcOutMessage.IsSuccess)
            {
                permissions.UserGroup = GetGroupNameByUserName(userName);

                if (string.IsNullOrEmpty(permissions.UserGroup))
                {
                    Log.Debug("User:[" + userName + "] has no group defined.");
                }
                else
                {
                    using (var db = new TowerModelContainer())
                    {
                        var groupAttributesQuery = (from b in db.GROUPATTRIBUTES
                            where
                                b.GROUPNAME == permissions.UserGroup
                            select new
                            {
                                b.GROUPATTRIBUTEKEY,
                                b.GROUPATTRIBUTEVALUE
                            }).ToList();

                        if (groupAttributesQuery.Count > 0)
                        {
                            foreach (var row in groupAttributesQuery)
                            {
                                Log.Debug("IDM Key:[" + row.GROUPATTRIBUTEKEY + "] Value:[" + row.GROUPATTRIBUTEVALUE + "]");

                                if (row.GROUPATTRIBUTEKEY == "p_annotate"
                                    && row.GROUPATTRIBUTEVALUE == "Y")
                                    permissions.IDM.CanAnnotate = true;

                                if (row.GROUPATTRIBUTEKEY == "p_import"
                                    && row.GROUPATTRIBUTEVALUE == "Y")
                                    permissions.IDM.CanImport = true;

                                if (row.GROUPATTRIBUTEKEY == "p_export"
                                    && row.GROUPATTRIBUTEVALUE == "Y")
                                    permissions.IDM.CanExport = true;

                                if (row.GROUPATTRIBUTEKEY == "p_admin"
                                    && row.GROUPATTRIBUTEVALUE == "Y")
                                    permissions.IDM.CanAdminAnnotations = true;

                                if (row.GROUPATTRIBUTEKEY == "p_print"
                                    && row.GROUPATTRIBUTEVALUE == "Y")
                                    permissions.IDM.CanPrint = true;

                                if (row.GROUPATTRIBUTEKEY == "p_modify_title"
                                    && row.GROUPATTRIBUTEVALUE == "Y")
                                    permissions.IDM.CanModifyTitle = true;

                                if (row.GROUPATTRIBUTEKEY == "default_application")
                                    permissions.IDM.DefaultApplication = row.GROUPATTRIBUTEVALUE;

                                if (row.GROUPATTRIBUTEKEY == "p_annotate_all")
                                    permissions.IDM.AnnotateAll = row.GROUPATTRIBUTEVALUE;

                                if (row.GROUPATTRIBUTEKEY == "p_annotate_group")
                                    permissions.IDM.AnnotateGroup = row.GROUPATTRIBUTEVALUE;
                            }
                        }
                    }
                }
            }
            // Login failed, pass message as to why
            else
            {
                permissions.LoginSuccessful = false;
                permissions.LoginMessage = rpcOutMessage.OutMessage;
            }

            if (permissions.LoginSuccessful)
            {
                var rpcOutAssembly =
                    GetAssemblyPermissions(new RpcInMessage
                    {
                        Context = "",
                        UserName = userName,
                        Workstation = workstation
                    });

                if (rpcOutAssembly.IsSuccess)
                {
                    foreach (DataRow row in rpcOutAssembly.OutDataTable.Rows)
                    {
                        foreach (DataColumn column in rpcOutAssembly.OutDataTable.Columns)
                        {
                            Log.Debug("Assembly Key:[" + column.ColumnName + "] Value:[" + row[column] + "]");

                            if (column.ColumnName == "isSupervisor")
                                if (row[column].ToString() == "Y")
                                    permissions.Assembly.IsSupervisor = true;

                            if (column.ColumnName == "asmbl_all")
                                if (row[column].ToString() == "Y")
                                    permissions.Assembly.CanAssembleAll = true;

                            if (column.ColumnName == "asmbl_aprvmat")
                                if (row[column].ToString() == "Y")
                                    permissions.Assembly.CanApproveMatch = true;

                            if (column.ColumnName == "asmbl_view")
                                if (row[column].ToString() == "Y")
                                    permissions.Assembly.CanView = true;

                            if (column.ColumnName == "isAutoView")
                                if (row[column].ToString() == "Y")
                                    permissions.Assembly.IsAutoView = true;

                            if (column.ColumnName == "asmbl_del")
                                if (row[column].ToString() == "Y")
                                    permissions.Assembly.CanDelete = true;

                            if (column.ColumnName == "asmbl_pendmat")
                                if (row[column].ToString() == "Y")
                                    permissions.Assembly.CanPendMatch = true;

                            if (column.ColumnName == "asmbl_upd")
                                if (row[column].ToString() == "Y")
                                    permissions.Assembly.CanUpdate = true;

                            if (column.ColumnName == "supervisorID")
                                permissions.Assembly.SupervisorId = row[column].ToString();

                            if (column.ColumnName == "winID")
                                permissions.WindowsUsername = row[column].ToString();

                            if (column.ColumnName == "embillzID")
                                permissions.EmBillzUsername = row[column].ToString();

                            if (column.ColumnName == "cstatus")
                                permissions.Assembly.Status = row[column].ToString();
                        }
                    }
                }
                else
                {
                    permissions.EmBillzUsername = "";
                }
            }

            if (permissions.LoginSuccessful)
            {
                Log.Info(CommonFunctions.ObjectToXml(permissions));
            }

            Log.Debug("Exit EmWareCollectionSecurity.EmWareCollectionLogin() IsSuccess:[" + permissions.LoginSuccessful + "]");
            
            return permissions;
        }
    }
}
