using System.Runtime.Serialization;

namespace Rti.InternalInterfaces.DataContracts
{
    [DataContract]
    public class Login
    {
        [DataContract]
        public class UserCredentials
        {
            public UserCredentials(string unixUserName, string unixPassword, string windowsDomain, 
                bool setContext = false, string fetchOptions = "UserName,IsSupervisor,EmpGroups")
            {
                UnixUserName = unixUserName;
                UnixPassword = unixPassword;
                WindowsDomain = windowsDomain;
                SetContext = setContext;
                FetchOptions = fetchOptions;
            }

            [DataMember]
            public string UnixUserName { get; private set; }
            [DataMember]
            public string UnixPassword { get; private set; }
            [DataMember]
            public string WindowsDomain { get; private set; }
            [DataMember]
            public bool SetContext { get; private set; }
            [DataMember]
            public string FetchOptions { get; private set; }
        }

        [DataContract]
        public class Permissions
        {
            [DataMember]
            public bool LoginSuccessful { get; set; }
            [DataMember]
            public string LoginMessage { get; set; }
            [DataMember]
            public string UserGroup { get; set; }
            [DataMember]
            public string WindowsUsername { get; set; }
            [DataMember]
            public string UnixUsername { get; set; }
            [DataMember]
            public string EmBillzUsername { get; set; }

            [DataMember]
            public IDMPermissions IDM { get; set; }
            [DataMember]
            public WorkFlowPermissions WorkFlow { get; set; }
            [DataMember]
            public AssemblyPermissions Assembly { get; set; }

            public Permissions()
            {
                IDM = new IDMPermissions();
                WorkFlow = new WorkFlowPermissions();
                Assembly = new AssemblyPermissions();
            }

            [DataContract]
            public class IDMPermissions
            {
                [DataMember]
                public bool CanModify { get; set; }
                [DataMember]
                public bool CanImport { get; set; }
                [DataMember]
                public bool CanAppend { get; set; }
                [DataMember]
                public bool CanAnnotate { get; set; }
                [DataMember]
                public bool CanModifyAnnotations { get; set; }
                [DataMember]
                public bool CanAdminAnnotations { get; set; }
                [DataMember]
                public bool CanViewAnnotations { get; set; }
                [DataMember]
                public bool CanPrint { get; set; }
                [DataMember]
                public bool CanExport { get; set; }
                [DataMember]
                public string DefaultApplication { get; set; }

                [DataMember]
                public string AnnotateGroup { get; set; }  // MODIFY, DELETE, NONE, VIEW
                [DataMember]
                public string AnnotateAll { get; set; }  // MODIFY, DELETE, NONE, VIEW

                // User can use IDM controls to update the tower.billdoc or tower.postdoc records
                [DataMember]
                public bool CanModifyTitle { get; set; }
            }

            [DataContract]
            public class WorkFlowPermissions
            {
                [DataMember]
                public bool CanAutoView { get; set; }
                [DataMember]
                public string Supervisor { get; set; }
                [DataMember]
                public bool IsSupervisor { get; set; }
            }

            [DataContract]
            public class AssemblyPermissions
            {
                [DataMember]
                public bool IsAutoView { get; set; }
                [DataMember]
                public string SupervisorId { get; set; }
                [DataMember]
                public bool IsSupervisor { get; set; }
                [DataMember]
                public bool CanAssembleAll { get; set; }
                [DataMember]
                public bool CanView { get; set; }
                [DataMember]
                public bool CanUpdate { get; set; }
                [DataMember]
                public bool CanPendMatch { get; set; }
                [DataMember]
                public bool CanApproveMatch { get; set; }
                [DataMember]
                public bool CanDelete { get; set; }
                [DataMember]
                public string Experience { get; set; }
                [DataMember]
                public string Status { get; set; }
            }
        }
    }
}
