using System;
using Rti;
using Rti.InternalInterfaces.DataContracts;
using Rti.InternalInterfaces.ServiceProxies;

namespace WpfDocViewer.DataAccessLayer
{
    class GetPermissions
    {
        public bool UnixLogin(string computername, string windowsusername, string username, string password, string domainname)
        {
            try
            {

                //Service call
                using (
                    var administrationService = new AdministrationServiceClient(Constants.AdministrationServiceURL,
                        Constants.AdministrationServiceENDPOINT_NAME))
                {
                    var permissions = administrationService.EmWareCollectionLogin(computername, windowsusername,
                        new Login.UserCredentials(username, password, domainname));

                    Model.PermissionsModel.Permissions.LoginSuccessful = permissions.LoginSuccessful;
                    Model.PermissionsModel.Permissions.LoginMessage = permissions.LoginMessage;
                    
                    if (Model.PermissionsModel.Permissions.LoginSuccessful)
                    {
                        //fill the static model permissions
                        Model.PermissionsModel.Permissions.IDMPermissions.AnnotateAll = permissions.IDM.AnnotateAll;
                        Model.PermissionsModel.Permissions.IDMPermissions.AnnotateGroup = permissions.IDM.AnnotateGroup;
                        Model.PermissionsModel.Permissions.IDMPermissions.CanAdminAnnotations =
                            permissions.IDM.CanAdminAnnotations;
                        Model.PermissionsModel.Permissions.IDMPermissions.CanAnnotate = permissions.IDM.CanAnnotate;
                        Model.PermissionsModel.Permissions.IDMPermissions.CanAppend = permissions.IDM.CanAppend;
                        Model.PermissionsModel.Permissions.IDMPermissions.CanExport = permissions.IDM.CanExport;
                        Model.PermissionsModel.Permissions.IDMPermissions.CanImport = permissions.IDM.CanImport;
                        Model.PermissionsModel.Permissions.IDMPermissions.CanModify = permissions.IDM.CanModify;
                        Model.PermissionsModel.Permissions.IDMPermissions.CanModifyAnnotations =
                            permissions.IDM.CanModifyAnnotations;
                        Model.PermissionsModel.Permissions.IDMPermissions.CanModifyTitle =
                            permissions.IDM.CanModifyTitle;
                        Model.PermissionsModel.Permissions.IDMPermissions.CanPrint = permissions.IDM.CanPrint;
                        Model.PermissionsModel.Permissions.IDMPermissions.CanViewAnnotations =
                            permissions.IDM.CanViewAnnotations;
                        Model.PermissionsModel.Permissions.IDMPermissions.DefaultApplication =
                            permissions.IDM.DefaultApplication;
                        Model.PermissionsModel.Permissions.IDMPermissions.IDMGroup = permissions.UserGroup;

                        Model.PermissionsModel.Permissions.AssemblyPermissions.CanCommit =
                            permissions.Assembly.CanApproveMatch;
                        Model.PermissionsModel.Permissions.AssemblyPermissions.CanDelete =
                            permissions.Assembly.CanDelete;
                        Model.PermissionsModel.Permissions.AssemblyPermissions.CanMatch =
                            permissions.Assembly.CanPendMatch;
                        Model.PermissionsModel.Permissions.AssemblyPermissions.CanUpdate =
                            permissions.Assembly.CanUpdate;
                        Model.PermissionsModel.Permissions.AssemblyPermissions.CanView = permissions.Assembly.CanView;
                        Model.PermissionsModel.Permissions.AssemblyPermissions.Experience =
                            permissions.Assembly.Experience;
                        Model.PermissionsModel.Permissions.AssemblyPermissions.IsSupervisor =
                            permissions.Assembly.IsSupervisor;
                        Model.PermissionsModel.Permissions.AssemblyPermissions.SupervisorID =
                            permissions.Assembly.SupervisorId;

                        Model.PermissionsModel.Permissions.WorkFlowPermissions.CanAutoView =
                            permissions.WorkFlow.CanAutoView;
                        Model.PermissionsModel.Permissions.WorkFlowPermissions.IsSupervisor =
                            permissions.WorkFlow.IsSupervisor;
                        Model.PermissionsModel.Permissions.WorkFlowPermissions.Supervisor =
                            permissions.WorkFlow.Supervisor;

                        Model.PermissionsModel.Permissions.WorkFlowPermissions.WindowsUsername =
                            permissions.WindowsUsername;
                    }
                    else
                    {
                        return false;
                    } //true false out put
                }
                return true;
            }
            catch(Exception ex)
            {
                Model.PermissionsModel.Permissions.LoginMessage = "Unix Login Error: " + ex.Message;
                return false;
            }
            
        }
    }
}
