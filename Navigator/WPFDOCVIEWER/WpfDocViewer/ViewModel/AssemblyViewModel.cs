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
using WpfDocViewer.Model;

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
    public class AssemblyViewModel : INotifyPropertyChanged 
    {
        #region INotifyPropertyChanged Members
        private List<string> remitList;
        private int remitCount;
        public event PropertyChangedEventHandler PropertyChanged;
        ObservableCollection<string> retrievedItems;
        string getCheckDetails;
        private string getUsername;
        bool canExecuteMyCommand = false;
        bool showPreview;
        private string getPassword;
        readonly ReadOnlyCollection<TreeRootViewModel> _regions;

        public AssemblyViewModel(TreeRoot[] regions)
        {
            _regions = new ReadOnlyCollection<TreeRootViewModel>(
                (from region in regions
                 select new TreeRootViewModel(region))
                .ToList());
            //LoginCommand = new RelayCommand(OnLogin);
            ShowPreview = true;
            CheckDetails = "Checks(0)";
        }

        public ReadOnlyCollection<TreeRootViewModel> TreeRoots
        {
            get { return _regions; }
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
        public string CheckDetails
        {
            get
            {
                return getCheckDetails;
            }
            set
            {
                getCheckDetails = value;
                OnPropertyChanged("CheckDetails");
            }
        }
        public void AddRemit(string remitdata)
        {
            if (RemitList != null)
            {
                RemitList.Add(remitdata);
                RemitCount = RemitList.Count;
            }
            else
            {
                RemitList = new List<string>();
                RemitList.Add(remitdata);
                RemitCount = RemitList.Count;
            }
        }
        public int RemitCount
        {
            get
            {
                return remitCount;
            }
            set
            {
                remitCount = value;
                OnPropertyChanged("RemitCount");
            }
        }
        public List<string> RemitList
        {
            get
            {
                return remitList;
            }
            set
            {
                remitList = value;
                OnPropertyChanged("RemitList");
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
        /// <summary>
        /// Can be set by model searching for check or by user adjusting the slider
        /// which is why its two way and not a private set.
        /// </summary>
        public int CheckOrRemit
        {
            get;
            set;
        }
        /// <summary>
        /// used to allow Expander to be open to start.
        /// </summary>
        public bool ShowPreview
        {
            get
            {
                return showPreview;
            }
            set
            {
                showPreview = value;
                OnPropertyChanged("ShowPreview");
            }
        }
        #endregion
        #region viewmodel commands

        private void OnLogin()
        {
            //throw new NotImplementedException();
        }


        public RelayCommand FindChecksCommand
        {
            get;
            private set;

        }
        #endregion
    }
}
