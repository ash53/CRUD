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
    public class MyWorkViewModel : INotifyPropertyChanged 
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        ObservableCollection<string> retrievedItems;
        
        private string getUsername;
        bool canExecuteMyCommand = false;
        bool showPreview;
        bool collapse;
   

        public MyWorkViewModel()
        {
           //LoginCommand = new RelayCommand(OnLogin);
           ShowPreview = true;
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
        public bool CollapseExpander
        {
            get
            {
                return collapse;
            }
            set
            {
                collapse = value;
                OnPropertyChanged("CollapseExpander");
            }
        }
        

        #endregion
        #region viewmodel commands

        private void OnLogin()
        {
            //throw new NotImplementedException();
        }


        public RelayCommand LoginCommand
        {
            get;
            private set;

        }
        #endregion
    }
}
