using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rti;

namespace WpfDocViewer.Model
{
    static class PermissionsModel
    {

        public static class Permissions
        {
         
            public static bool LoginSuccessful;
         
            public static string LoginMessage;

            public static string EncodedPassword;

            public static class IDMPermissions
            {
                public static string IDMGroup;

                public static bool CanModify;

                public static bool CanImport;

                public static bool CanAppend;

                public static bool CanAnnotate;

                public static bool CanModifyAnnotations;

                public static bool CanAdminAnnotations;

                public static bool CanViewAnnotations;

                public static bool CanPrint;

                public static bool CanExport;

                public static string DefaultApplication;

                public static string AnnotateGroup;  // MODIFY, DELETE, NONE, VIEW

                public static string AnnotateAll;  // MODIFY, DELETE, NONE, VIEW

                // User can use IDM controls to update the tower.billdoc or tower.postdoc records
                public static bool CanModifyTitle;

                public static string IDMServer = Constants.IDMServerName;

            }

            public static class WorkFlowPermissions
            {
             
                public static string WindowsUsername;
                
                public static bool CanAutoView;
                
                public static string Supervisor;
              
                public static bool IsSupervisor;
            }

         
            public static class AssemblyPermissions
            {
              
                public static bool IsSupervisor;
               
                public static bool CanView;
               
                public static bool CanDelete;
              
                public static bool CanMatch;
                
                public static bool CanCommit;
               
                public static bool CanUpdate;
                
                public static string Experience;

                public static string SupervisorID;
            }
        }


    }
}
