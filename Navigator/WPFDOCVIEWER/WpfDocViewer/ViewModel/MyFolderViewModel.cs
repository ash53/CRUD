using GalaSoft.MvvmLight;
using Microsoft.Windows.Input;
using System.ComponentModel;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.IO.IsolatedStorage;

namespace WpfDocViewer.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MyFolderViewModel : INotifyPropertyChanged
    {
        private bool isWorkExpanded;
        private bool isImportExpanded;
        private bool isFindExpanded;
        private bool isMatchExpanded;
        private Controls.SearchControl updateFindContent;
        private Controls.MatchingControl updateMatchContent;
        private Controls.ImportControl updateImportContent;
        private Controls.MyWork updateWorkContent;

        #region INotifyPropertyChanged Member Details
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Initializes a new instance of the MyFolderViewModel class.
        /// </summary>
        public MyFolderViewModel()
        {
            ClickedWorkExpander = new RelayCommand(OnWork);
            ClickedFindExpander = new RelayCommand(OnFind);
            ClickedImportExpander = new RelayCommand(OnImport);
            ClickedMatchExpander = new RelayCommand(OnMatch);
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
        #region ViewModel Binding Properties
        public string GetContent
        {
            get;
            set;
        }
        public bool IsWorkExpanded
        {
            get
            {
                return isWorkExpanded;
            }
            set
            {
                isWorkExpanded = value;
                OnPropertyChanged("IsWorkExpanded");
            }
        }
        public bool IsMatchExpanded
        {
            get
            {
                return isMatchExpanded;
            }
            set
            {
                if (value == true)
                {
                    IsImportExpanded = false;
                    IsFindExpanded = false;
                }
                isMatchExpanded = value;
                OnPropertyChanged("IsMatchExpanded");
            }
        }
        public bool IsImportExpanded
        {
            get
            {
                return isImportExpanded;
            }
            set
            {
                if (value == true)
                {
                    IsFindExpanded = false;
                    IsMatchExpanded = false;
                }
                isImportExpanded = value;
                OnPropertyChanged("IsImportExpanded");
            }
        }
        public bool IsFindExpanded
        {
            get
            {
               
                return isFindExpanded;
            }
            set
            {
                if(value == true)
                {
                    IsImportExpanded = false;
                    IsMatchExpanded = false;
                }
                isFindExpanded = value;
                OnPropertyChanged("IsFindExpanded");
            }
        }
        /// <summary>
        /// The content inside the ContentControl in the Expander
        /// </summary>
        public Controls.SearchControl UpdateFindContent
        {
            get
            {
                
                    if (updateFindContent == null)
                    {
                        updateFindContent = new Controls.SearchControl();
                    }
                    return updateFindContent;
              
            }
            set
            {
               
                    if (updateFindContent == null)
                    {
                        updateFindContent = new Controls.SearchControl();
                    }
               
                OnPropertyChanged("UpdateFindContent");
            }
        }
        public Controls.ImportControl UpdateImportContent
        {
            get
            {

                if (updateImportContent == null)
                {
                    updateImportContent = new Controls.ImportControl();
                }
                return updateImportContent;

            }
            set
            {

                if (updateImportContent == null)
                {
                    updateImportContent = new Controls.ImportControl();
                }

                OnPropertyChanged("UpdateImportContent");
            }
        }
        public Controls.MatchingControl UpdateMatchContent
        {
            get
            {

                if (updateMatchContent == null)
                {
                    updateMatchContent = new Controls.MatchingControl();
                }
                return updateMatchContent;

            }
            set
            {

                if (updateMatchContent == null)
                {
                    updateMatchContent = new Controls.MatchingControl();
                }

                OnPropertyChanged("UpdateMatchContent");
            }
        }
        public UserControl UpdateWorkContent
        {
            get;
            set;
        }
#endregion
        #region Relay Commands and Actions
        public RelayCommand ClickedWorkExpander
        {
            get;
            set;
        }
        public RelayCommand ClickedImportExpander
        {
            get;
            set;
        }
        public RelayCommand ClickedFindExpander
        {
            get;
            set;
        }
        public RelayCommand ClickedMatchExpander
        {
            get;
            set;
        }
       public void OnWork()
       {
            
       }
       public void OnMatch()
       {

       }
       public void OnImport()
       {

       }
       public void OnFind()
       {

       }
        #endregion

    }
}