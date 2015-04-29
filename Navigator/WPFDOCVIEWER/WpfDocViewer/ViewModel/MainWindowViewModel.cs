using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Data;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Controls;
using Rti;
using Rti.InternalInterfaces.ServiceProxies;


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
    public class MainWindowViewModel : INotifyPropertyChanged 
    {
        private ContentControl contentControlOne;
        private ContentControl contentControlTwo;
        private ContentControl contentControlThree;
        private ContentControl contentControlFour;
        private ContentControl contentControlFive;
        private ContentControl contentControlSix;
        private string savedControl1;
        private string savedControl2;
        private string savedControl3;
        private string savedControl4;
        //private IsolatedStorage appSettings = new IsolatedStorage();
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        MainWindow openMain;
        private string shoppingCartText = "Shopping Cart";
        
   
        /// <summary>
        /// Mark Lane
        /// load practice list, doctype and docdetail list
        /// </summary>
        public MainWindowViewModel()
        {
           CheckOutCommand = new RelayCommand(OnCheckOut);
           SaveToHistory1 = new RelayCommand(OnSaveToHistory);
           SaveToHistory2 = new RelayCommand(OnSaveToHistory2);
           SaveToHistory3 = new RelayCommand(OnSaveToHistory3);
           SaveToHistory4 = new RelayCommand(OnSaveToHistory4);
           SaveToHistory5 = new RelayCommand(OnSaveToHistory5);
           SaveToHistory6 = new RelayCommand(OnSaveToHistory6);
           ClearHistory = new RelayCommand(OnClearHistory);
           ResourceDictionary resources = App.Current.Resources; // If in a Window/UserControl/etc
           resources["History1"] = Properties.Settings.Default["History1"];
           resources["History2"] = Properties.Settings.Default["History2"];
           resources["History3"] = Properties.Settings.Default["History3"];
           resources["History4"] = Properties.Settings.Default["History4"];
           resources["History5"] = Properties.Settings.Default["History5"];
           resources["History6"] = Properties.Settings.Default["History6"];
           LoadDocumentTypes();
        }
        
        private void LoadDocumentTypes()
        {
            Model.Doctype.DocTypeList = getDocumentTypes();
            Model.DocDetail.DocDetailList = getDocumentDetails();
        }
       
        /// <summary>
        /// Mark Lane
        /// load the document types and doclist to static list for 
        /// reuse by other controls.
        /// </summary>
        /// <returns></returns>
        public List<string> getDocumentTypes()
        {
            DataTable docTypes = new DataTable();
            List<string> _documentTypes = new List<string>();
            using (var DocumentService = new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME))
            {
                docTypes = DocumentService.GetDocTypes(Environment.MachineName, Environment.UserName);
            }
            _documentTypes = docTypes.AsEnumerable().Select(abc => abc.Field<string>("TYPE_KEY")).Distinct().ToList();
            return _documentTypes;
        }


        //Populate DocumentDetails Combobox 
        public List<string> getDocumentDetails()
        {
            DataTable docDetail;
            List<string> _getDocumentDetails = new List<string>();
            using (var DocumentService = new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME))
            {
                docDetail = DocumentService.GetDocTypes(Environment.MachineName, Environment.UserName);
            }
            _getDocumentDetails = docDetail.AsEnumerable().Select(abc => abc.Field<string>("detail_key")).Distinct().ToList();
            return _getDocumentDetails;
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
        /// Mark Lane
        /// The number of itmems in the Shopping Cart
        /// make it strongly typed
        /// </summary>
        public string ShoppingCartText
        {
            get
            {
                return shoppingCartText;
            }
            set
            {
                shoppingCartText = value;
                OnPropertyChanged("ShoppingCartText");
            }
        }
        public DataTable CartContents
        {
            get;
            set;
        }
        public Model.MatchingChecksCollection CashCartContents
        {
            
            get;
            set;
        }
        #endregion
        #region viewmodel commands
        //binding call 
        public void OnCheckOut()
        {
            Model.MatchingChecksCollection.MatchingChecksObservableCollection.Count();
          
            //Take the items in the DataGrid.

            //Fill the Match Control.

            //Open the Match Control.
            
      
        }
    

        public RelayCommand CheckOutCommand
        {
            get;
            private set;

        }
        /// <summary>
        /// Mark Lane used to save the control they used
        /// be able to launch from history.
        /// </summary>
        public RelayCommand SaveToHistory1
        {
            get;
            set;
        }
        public RelayCommand SaveToHistory2
        {
            get;
            set;
        }
        public RelayCommand SaveToHistory3
        {
            get;
            set;
        }
        public RelayCommand SaveToHistory4
        {
            get;
            set;
        }
        public RelayCommand SaveToHistory5
        {
            get;
            set;
        }
        public RelayCommand SaveToHistory6
        {
            get;
            set;
        }
        public RelayCommand ClearHistory
        {
            get;
            set;
        }
        ///TODO Mark Lane to aid in moving stuff around we will use binding rather than codebehind for setting the 
        ///content control
        ///
        public ContentControl ContentControlOne
        {
            get
            {
                return contentControlOne;
            }
            set
            {
                contentControlOne = value;
                OnPropertyChanged("ContentControlOne");
            }
        }
        public ContentControl ContentControlTwo
        {
            get
            {
                return contentControlTwo;
            }
            set
            {
                contentControlTwo = value;
                OnPropertyChanged("ContentControlTwo");
            }
        }
        public ContentControl ContentControlThree
        {
            get
            {
                return contentControlThree;
            }
            set
            {
                contentControlThree = value;
                OnPropertyChanged("ContentControlThree");
            }
        }
        public ContentControl ContentControlFour
        {
            get
            {
                return contentControlFour;
            }
            set
            {
                contentControlFour = value;
                OnPropertyChanged("ContentControlFour");
            }
        }
        public ContentControl ContentControlFive
        {
            get
            {
                return contentControlFive;
            }
            set
            {
                contentControlFive = value;
                OnPropertyChanged("ContentControlFive");
            }
        }
        public ContentControl ContentControlSix
        {
            get
            {
                return contentControlSix;
            }
            set
            {
                contentControlSix = value;
                OnPropertyChanged("ContentControlSix");
            }
        }
        public string SavedControl1
        {
            get
            {
                return savedControl1;
            }
            set
            {
                savedControl1 = value;
                OnPropertyChanged("SavedControl1");
            }
        }
        public string SavedControl2
        {
            get
            {
                return savedControl2;
            }
            set
            {
                savedControl2 = value;
                OnPropertyChanged("SavedControl2");
            }
        }
        public string SavedControl3
        {
            get
            {
                return savedControl3;
            }
            set
            {
                savedControl3 = value;
                OnPropertyChanged("SavedControl3");
            }
        }
        public string SavedControl4
        {
            get
            {
                return savedControl4;
            }
            set
            {
                savedControl4 = value;
                OnPropertyChanged("SavedControl4");
            }
        }
        public string HeaderText1
        {
            get;
            set;
        }
        public string HeaderText2
        {
            get;
            set;
        }
        public string HeaderText3
        {
            get;
            set;
        }
        public string HeaderText4
        {
            get;
            set;
        }
        public string HeaderText5
        {
            get;
            set;
        }
        public string HeaderText6
        {
            get;
            set;
        }
        #endregion
        #region public functions
        public void OnSaveToHistory()
        {
           //var getdata = Properties.Settings.Default["History1"];
           Properties.Settings.Default["History1"] = ReturnControlHeaderName(contentControlOne.GetType().ToString());
           Properties.Settings.Default.Save();
            ResourceDictionary resources = App.Current.Resources; // If in a Window/UserControl/etc
           resources["History1"] = Properties.Settings.Default["History1"];
           
        }
        public void OnSaveToHistory2()
        {
            //var getdata = Properties.Settings.Default["History1"];
            Properties.Settings.Default["History2"] = ReturnControlHeaderName(contentControlTwo.GetType().ToString());
            Properties.Settings.Default.Save();
            ResourceDictionary resources = App.Current.Resources; // If in a Window/UserControl/etc
            resources["History2"] = Properties.Settings.Default["History2"];

        }
        public void OnSaveToHistory3()
        {
            //var getdata = Properties.Settings.Default["History1"];
            Properties.Settings.Default["History3"] = ReturnControlHeaderName(contentControlThree.GetType().ToString());
            Properties.Settings.Default.Save();
            ResourceDictionary resources = App.Current.Resources; // If in a Window/UserControl/etc
            resources["History3"] = Properties.Settings.Default["History3"];

        }
        public void OnSaveToHistory4()
        {
            //var getdata = Properties.Settings.Default["History1"];
            Properties.Settings.Default["History4"] = ReturnControlHeaderName(contentControlFour.GetType().ToString());
            Properties.Settings.Default.Save();
            ResourceDictionary resources = App.Current.Resources; // If in a Window/UserControl/etc
            resources["History4"] = Properties.Settings.Default["History4"];

        }
        public void OnSaveToHistory5()
        {
            //var getdata = Properties.Settings.Default["History1"];
            Properties.Settings.Default["History5"] = ReturnControlHeaderName(contentControlFive.GetType().ToString());
            Properties.Settings.Default.Save();
            ResourceDictionary resources = App.Current.Resources; // If in a Window/UserControl/etc
            resources["History5"] = Properties.Settings.Default["History5"];

        }
        public void OnSaveToHistory6()
        {
            //var getdata = Properties.Settings.Default["History1"];
            Properties.Settings.Default["History6"] = ReturnControlHeaderName(contentControlSix.GetType().ToString());
            Properties.Settings.Default.Save();
            ResourceDictionary resources = App.Current.Resources; // If in a Window/UserControl/etc
            resources["History6"] = Properties.Settings.Default["History6"];

        }
        public void OnClearHistory()
        {
            ResourceDictionary resources = App.Current.Resources;
            Properties.Settings.Default["History1"] = "";
            Properties.Settings.Default["History2"] = "";
            Properties.Settings.Default["History3"] = "";
            Properties.Settings.Default["History4"] = "";
            Properties.Settings.Default["History5"] = "";
            Properties.Settings.Default["History6"] = "";
            Properties.Settings.Default.Save();
            resources["History1"] = Properties.Settings.Default["History1"];
            resources["History2"] = Properties.Settings.Default["History2"];
            resources["History3"] = Properties.Settings.Default["History3"];
            resources["History4"] = Properties.Settings.Default["History4"];
            resources["History5"] = Properties.Settings.Default["History5"];
            resources["History6"] = Properties.Settings.Default["History6"];
            
        }
        private string ReturnControlHeaderName(string controlName)
        {
            string retval;
            switch (controlName)
            {
                case "WpfDocViewer.Controls.FindWork":
                    retval = "Find Work";
                    break;
                case "WpfDocViewer.Controls.SearchControl":
                    retval = "Find Docs";
                    break;
                case "WpfDocViewer.Controls.MainImportControl":
                    retval = "Add Docs";
                    break;
                case "WpfDocViewer.Controls.MatchingControl":
                    retval = "Match Cash";
                    break;
                case "WpfDocViewer.Controls.SettingsControl":
                    retval = "Settings";
                    break;
                case "WpfDocViewer.Controls.MainWorkControl":
                    retval = "Add Work";
                    break;
                default:
                    retval = "";
                    break;
            }
            return retval;
        }
        /// <summary>
        /// Some data needs to be loaded on startup dynamically to fill the comboboxes which 
        /// are shared among controls.
        /// </summary>
        public void LoadPracticeList()
        {

            try
            {
                using (DocumentManagerServiceClient docManagerServiceClient = new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME))
                {
                    //Mark Lane convert DataTable into a List
                    //List<DataRow> practiceList = docManagerServiceClient.ListPractices(Environment.MachineName, Environment.UserName).AsEnumerable().ToList();

                    //Mark Lane how to use linq and lambda to get a specific columns data in a datatable output to List.
                    var practiceList = docManagerServiceClient.ListPractices(Environment.MachineName, Environment.UserName).Rows.OfType<DataRow>()
                        .Select(dr => dr.Field<string>("Practice")).ToList().Distinct();


                    ResourceDictionary resources = App.Current.Resources; // If in a Window/UserControl/etc
                    List<string> Practices = new List<string>();
                    //MLane make a new list for a default empty choice
                    Practices.Add("");
                    foreach (string str in practiceList.ToList())
                    {
                        Practices.Add(str);
                    }
                    resources["PracticeList"] = Practices;
                    Model.Practices.PracticeList = Practices;
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Failed to get PracticeList" + ex.Message.ToString());
            }
        }
        public ObservableCollection<Model.MatchingChecks> CashCart
        {
            get { return Model.MatchingChecksCollection.MatchingChecksObservableCollection; }
            set
            {
                ShoppingCartText = "Cash Contents" + Model.MatchingChecksCollection.MatchingChecksObservableCollection.Count();
                this.CashCart = Model.MatchingChecksCollection.MatchingChecksObservableCollection;
                OnPropertyChanged("CashCart");

            }
        }
        #endregion
    }
}
