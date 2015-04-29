using GalaSoft.MvvmLight;
using System.ComponentModel;

namespace WpfDocViewer.ViewModel
{
    /// <summary>
    /// Author: Mark Lane
    /// The PermissionsViewModel is bound to the permissionscontrol WPF usercontrol.
    /// the Data is retrieved from the static PermissionsModel.  
    /// The PermissionsModel is filled when we login to UNIX.
    /// This class contains properties that a View can data bind to.
    /// Being static we may not need OnPropertyChanged OneWay it will never change
    /// during the life of the application.
    /// Date: 10/17/2014
    /// <para>
    /// </para>
    /// </summary>
    public class PermissionsViewModel :  INotifyPropertyChanged
    {
        string idmGroup;
        bool idmCanModify = false;
        bool idmCanImport = false;
        bool idmCanAppend = false;
        bool idmCanAnnotate = false;
        bool idmCanModifyAnnotations = false;
        bool idmCanAdminAnnotations = false;
        bool idmCanViewAnnotations = false;
        bool idmCanPrint = false;
        bool idmCanExport = false;
        string idmDefaultApplication;
        string idmAnnotateGroup;
        string idmAnnotateAll;
        bool idmCanModifyTitle = false;
        string windowsUsername;
        bool workFlowCanAutoView = false;
        string workFlowSupervisor;
        bool workFlowIsSupervisor = false;
        bool assemblyIsSupervisor;
        bool assemblyCanView;
        bool assemblyCanDelete;
        bool assemblyCanMatch;
        bool assemblyCanCommit;
        bool assemblyCanUpdate;
        string assemblyExperience;
        string assemblySupervisorID;
            
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;


        //used to enable or disable commands from buttons or events.
        bool canExecuteMyCommand = false;
       

        /// <summary>
        /// Initializes a new instance of the PermissionsViewModel class.
        /// </summary>
        public PermissionsViewModel()
        {
            LoadPermissions();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);

                handler(this, e);
            }
        }
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real, 
            // public, instance property on this object. 
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

            }
        }
        #endregion
        #region permissions from the dal
        /// <summary>
        /// Load the ViewModel with the variables from the static Model.
        /// </summary>
        public void LoadPermissions()
        {
                //UNIX login success
                LoginSuccessful = Model.PermissionsModel.Permissions.LoginSuccessful;
                LoginMessage = Model.PermissionsModel.Permissions.LoginMessage;
                //IDM Group permissions
                IDMGroup = Model.PermissionsModel.Permissions.IDMPermissions.IDMGroup;
                IDMCanModify = Model.PermissionsModel.Permissions.IDMPermissions.CanModify;
                IDMCanImport = Model.PermissionsModel.Permissions.IDMPermissions.CanImport;
                IDMCanAppend = Model.PermissionsModel.Permissions.IDMPermissions.CanAppend;
                IDMCanAnnotate = Model.PermissionsModel.Permissions.IDMPermissions.CanAnnotate;
                IDMCanModifyAnnotations = Model.PermissionsModel.Permissions.IDMPermissions.CanModifyAnnotations;
                IDMCanAdminAnnotations = Model.PermissionsModel.Permissions.IDMPermissions.CanAdminAnnotations;
                IDMCanViewAnnotations = Model.PermissionsModel.Permissions.IDMPermissions.CanViewAnnotations;
                IDMCanPrint = Model.PermissionsModel.Permissions.IDMPermissions.CanPrint;
                IDMCanExport = Model.PermissionsModel.Permissions.IDMPermissions.CanExport;
                IDMDefaultApplication  = Model.PermissionsModel.Permissions.IDMPermissions.DefaultApplication;
                IDMAnnotateGroup = Model.PermissionsModel.Permissions.IDMPermissions.AnnotateGroup; // MODIFY, DELETE, NONE, VIEW
                IDMAnnotateAll = Model.PermissionsModel.Permissions.IDMPermissions.AnnotateAll;  // MODIFY, DELETE, NONE, VIEW
                IDMCanModifyTitle = Model.PermissionsModel.Permissions.IDMPermissions.CanModifyTitle;
                //Windows
                WindowsUsername = Model.PermissionsModel.Permissions.WorkFlowPermissions.WindowsUsername;
                //Embillz WF Experience 
                WorkFlowCanAutoView  = Model.PermissionsModel.Permissions.WorkFlowPermissions.CanAutoView;
                WorkFlowSupervisor  = Model.PermissionsModel.Permissions.WorkFlowPermissions.Supervisor;
                WorkFlowIsSupervisor  = Model.PermissionsModel.Permissions.WorkFlowPermissions.IsSupervisor;
          
                //Assembly Screen Permissions
                AssemblyIsSupervisor  = Model.PermissionsModel.Permissions.AssemblyPermissions.IsSupervisor;
                AssemblyCanView  = Model.PermissionsModel.Permissions.AssemblyPermissions.CanView;
                AssemblyCanDelete = Model.PermissionsModel.Permissions.AssemblyPermissions.CanDelete;
                AssemblyCanMatch = Model.PermissionsModel.Permissions.AssemblyPermissions.CanMatch;
                AssemblyCanCommit = Model.PermissionsModel.Permissions.AssemblyPermissions.CanCommit;
                AssemblyCanUpdate = Model.PermissionsModel.Permissions.AssemblyPermissions.CanUpdate;
                AssemblyExperience = Model.PermissionsModel.Permissions.AssemblyPermissions.Experience;
                AssemblySupervisorID = Model.PermissionsModel.Permissions.AssemblyPermissions.SupervisorID;
        }
                public bool LoginSuccessful { get; set; }
         
                public string LoginMessage { get; set; }

                public string IDMGroup 
                {
                    get { return idmGroup;}
                    set 
                    { 
                        idmGroup = value;
                        OnPropertyChanged("IDMGroup");
                    }
                }

                public bool IDMCanModify
                {
                    get { return idmCanModify; }
                    set
                    {
                        idmCanModify = value;
                        OnPropertyChanged("IDMCanModify");
                    }
                }

                public bool IDMCanImport
                {
                    get { return idmCanImport; }
                    set
                    {
                        idmCanImport = value;
                        OnPropertyChanged("IDMCanImport");
                    }
                }

                public bool IDMCanAppend
                {
                    get { return idmCanAppend; }
                    set
                    {
                        idmCanAppend = value;
                        OnPropertyChanged("IDMCanAppend");
                    }
                }

                public bool IDMCanAnnotate
                {
                    get { return idmCanAnnotate; }
                    set
                    {
                        idmCanAnnotate = value;
                        OnPropertyChanged("IDMCanAnnotate");
                    }
                }

                public bool IDMCanModifyAnnotations
                {
                    get { return idmCanModifyAnnotations; }
                    set
                    {
                        idmCanModifyAnnotations = value;
                        OnPropertyChanged("IDMCanModifyAnnotations");
                    }
                }

                public bool IDMCanAdminAnnotations
                {
                    get { return idmCanAdminAnnotations; }
                    set
                    {
                        idmCanAdminAnnotations = value;
                        OnPropertyChanged("IDMCanAdminAnnotations");
                    }
                }

                public bool IDMCanViewAnnotations
                {
                    get { return idmCanViewAnnotations; }
                    set
                    {
                        idmCanViewAnnotations = value;
                        OnPropertyChanged("IDMCanViewAnnotations");
                    }
                }

                public bool IDMCanPrint
                {
                    get { return idmCanPrint; }
                    set
                    {
                        idmCanPrint = value;
                        OnPropertyChanged("IDMCanPrint");
                    }
                }

                public bool IDMCanExport
                {
                    get { return idmCanExport; }
                    set
                    {
                        idmCanExport = value;
                        OnPropertyChanged("IDMCanExport");
                    }
                }

                public string IDMDefaultApplication
                {
                    get { return idmDefaultApplication; }
                    set
                    {
                        idmDefaultApplication = value;
                        OnPropertyChanged("IDMDefaultApplication");
                    }
                }

                public string IDMAnnotateGroup
                {
                    get { return idmAnnotateGroup; }
                    set
                    {
                        idmAnnotateGroup = value;
                        OnPropertyChanged("IDMAnnotateGroup");
                    }
                }  // MODIFY, DELETE, NONE, VIEW

                public string IDMAnnotateAll
                {
                    get { return idmAnnotateAll; }
                    set
                    {
                        idmAnnotateAll = value;
                        OnPropertyChanged("IDMAnnotateAll");
                    }
                }   // MODIFY, DELETE, NONE, VIEW

                // User can use IDM controls to update the tower.billdoc or tower.postdoc records
                public bool IDMCanModifyTitle
                {
                    get { return idmCanModifyTitle; }
                    set
                    {
                        idmCanModifyTitle = value;
                        OnPropertyChanged("IDMCanModifyTitle");
                    }
                }


                public string WindowsUsername
                {
                    get { return windowsUsername; }
                    set
                    {
                        windowsUsername = value;
                        OnPropertyChanged("WindowsUsername");
                    }
                }

                public bool WorkFlowCanAutoView
                {
                    get { return workFlowCanAutoView; }
                    set
                    {
                        workFlowCanAutoView = value;
                        OnPropertyChanged("WorkFlowCanAutoView");
                    }
                }

                public string WorkFlowSupervisor
                {
                    get { return workFlowSupervisor; }
                    set
                    {
                        workFlowSupervisor = value;
                        OnPropertyChanged("WorkFlowSupervisor");
                    }
                }

                public bool WorkFlowIsSupervisor
                {
                    get { return workFlowIsSupervisor; }
                    set
                    {
                        workFlowIsSupervisor = value;
                        OnPropertyChanged("WorkFlowIsSupervisor");
                    }
                }


                public bool AssemblyIsSupervisor
                {
                    get { return assemblyIsSupervisor; }
                    set
                    {
                        assemblyIsSupervisor = value;
                        OnPropertyChanged("AssemblyIsSupervisor");
                    }
                }

                public bool AssemblyCanView
                {
                    get { return assemblyCanView; }
                    set
                    {
                        assemblyCanView = value;
                        OnPropertyChanged("AssemblyCanView");
                    }
                }

                public bool AssemblyCanDelete
                {
                    get { return assemblyCanDelete; }
                    set
                    {
                        assemblyCanDelete = value;
                        OnPropertyChanged("AssemblyCanDelete");
                    }
                }

                public bool AssemblyCanMatch
                {
                    get { return assemblyCanMatch; }
                    set
                    {
                        assemblyCanMatch = value;
                        OnPropertyChanged("AssemblyCanMatch");
                    }
                }

                public bool AssemblyCanCommit
                {
                    get { return assemblyCanCommit; }
                    set
                    {
                        assemblyCanCommit = value;
                        OnPropertyChanged("AssemblyCanCommit");
                    }
                }

                public bool AssemblyCanUpdate
                {
                    get { return assemblyCanUpdate; }
                    set
                    {
                        assemblyCanUpdate = value;
                        OnPropertyChanged("AssemblyCanUpdate");
                    }
                }

                public string AssemblyExperience
                {
                    get { return assemblyExperience; }
                    set
                    {
                        assemblyExperience = value;
                        OnPropertyChanged("AssemblyExperience");
                    }
                }

                public string AssemblySupervisorID
                {
                    get { return assemblySupervisorID; }
                    set
                    {
                        assemblySupervisorID = value;
                        OnPropertyChanged("AssemblySupervisorID");
                    }
                }
            
        
        #endregion
    }
}