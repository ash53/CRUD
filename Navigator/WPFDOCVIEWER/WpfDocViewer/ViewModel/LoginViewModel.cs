using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Data;
using System.Diagnostics;
using GalaSoft.MvvmLight.Command;


namespace WpfDocViewer.ViewModel
{
    /// <summary>
    /// Author: Mark Lane
    /// View Model for the Search user control to be used in the View.
    /// Contains RelayCommands for Search Buttons.
    /// INotifyPropertyChanged to handle text changes.
    /// XAML Resources updated globally in app to be shared amongst the view.
    /// 
    /// </summary>
    public class LoginViewModel : INotifyPropertyChanged 
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        MainWindow openMain;
        private string getUsername;
        private string getPassword;
        private string domainName;
   

        public LoginViewModel()
        {
           LoginCommand = new RelayCommand(OnLogin);
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

        public string Username
        {
            get
            {
                return getUsername;
            }
            set
            {
                getUsername = value;
                OnPropertyChanged("Username");
            }
        }
        //for security reasons binding is disabled for passwords so this will be directly set.
        public string Password
        {
            get
            {
                return getPassword;
            }
            set
            {
                getPassword = value;
                OnPropertyChanged("Password");
            }
        }
        public string DomainName
        {
            get
            {
                return domainName;
            }
            set
            {
                domainName = value;
                OnPropertyChanged("DomainName");
            }
        }
        #endregion
        #region viewmodel commands
        //binding call 
        public void OnLogin()
        {
           
           Mouse.OverrideCursor = Cursors.Wait;
            //throw new NotImplementedException();
            string domain = Environment.UserDomainName;
            //run RPC 

            //get success or failure

            //load permissions to static model

            //open MainViewer
            openMain = new MainWindow();
            openMain.Show();
            Mouse.OverrideCursor = null;
            
      
        }
        //nonbinding call
        public bool LoginApproved()
        {
            bool approved = false;
            Mouse.OverrideCursor = Cursors.Wait;
            string domain = Environment.UserDomainName;
            DataAccessLayer.GetPermissions loginService = new DataAccessLayer.GetPermissions();
            //run RPC 
            approved = loginService.UnixLogin(Environment.MachineName, Environment.UserName, Username, Password, DomainName);
            //get success or failure
           

            //load permissions to static model
            if (approved)
            {
                //open MainViewer
                openMain = new MainWindow();
                openMain.Show();
            }
            Mouse.OverrideCursor = null;
            return approved;

        }

        public RelayCommand LoginCommand
        {
            get;
            private set;

        }
        #endregion
    }
}
