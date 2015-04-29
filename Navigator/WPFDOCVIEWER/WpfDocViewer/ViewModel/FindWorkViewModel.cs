using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Data;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using Rti;
using Rti.InternalInterfaces.ServiceProxies;
using WpfDocViewer.Model;
using System.Windows.Threading;
using Rti.InternalInterfaces.DataContracts;





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
    
    public class FindWorkViewModel : INotifyPropertyChanged 
    {
       
        public event PropertyChangedEventHandler PropertyChanged;
        ObservableCollection<string> retrievedItems;
        private RelayCommand searchCommand;
        RelayCommand openViewer;
        BackgroundWorker BackgroundSearch = new BackgroundWorker();
        BackgroundWorker BackgroundMoreSearchData = new BackgroundWorker();
        BackgroundWorker BackgroundGetCount = new BackgroundWorker();
        string maxResults = "";
        private string searchValueOne;
        private string searchValueTwo;
        private string searchValueThree;
        private string searchValueFour;
        private string searchValueFive;
        private string searchValueSix;
        private string searchTypeOne;
        private string searchTypeTwo;
        private string searchTypeThree;
        private string searchTypeFour;
        private string searchTypeFive;
        private string searchTypeSix;
        private List<string> searchList1 = new List<string>();
        private List<string> searchList2 = new List<string>();
        private List<string> searchList3 = new List<string>();
        private List<string> searchList4 = new List<string>();
        private List<string> searchList5 = new List<string>();
        private List<string> searchList6 = new List<string>();
        private Visibility visibleTwo;
        private Visibility visibleThree;
        private Visibility visibleFour;
        private Visibility visibleFive;
        private Visibility visibleSix;
        bool canExecuteMyCommand = false;
        private DataTable getIDMSearchData;
        private DataTable getFlowareSearchData;
        private DataTable getEmbillzSearchData;
        private DataTable getAssemblySearchData;
        private View.DocumentViewer doc;
        bool showIDMData;
        bool showFlowareData;
        bool showEmbillzData;
        bool showResults;
        int selectedRow;
        int selectedIDMRow;
        string searchResults;
        string workflowResults;
        string idmResults;
        string flowareResults;
        string embillzResults;
        string currentMapName = "";
        string currentActName = "";
        string currentActivityName = "";
        string docno = "";
        string matchStatus = "";
        string pardocno = "";
        string docDet = "";
        string docType = "";
        string depdt = "";
        string checknum = "";
        string checkamt = "";
        string minCheckamt = "";
        string maxCheckamt = "";
        string paidamt = "";
        string minPaidamt = "";
        string maxPaidamt = "";
        string embacct = "";
        string practice = "";
        string division = "";
        string assigned_To = "";
        string reason = "";
        private string documentNumber;
        private string notes;
        private string createUser;
        private string createDate;
        private string modUser;
        private string modDate;
      
        private string selectedIDMItem;
        private string selectedFlowareItem;
        private List<string> getMaps;
        private List<string> getActivities;
        ObservableCollection<Model.ImportDataModel.DocumentType> _documentTypes = new ObservableCollection<Model.ImportDataModel.DocumentType>();
        ObservableCollection<Model.ImportDataModel.DocumentDetail> _documentDetails = new ObservableCollection<Model.ImportDataModel.DocumentDetail>();
        List<string> documentTypes = new List<string>();
        List<string> documentDetails = new List<string>();
        List<string> practiceList = new List<string>();
        private DateTime depositdateFrom = DateTime.Now.AddDays(-365);
        private DateTime depositdateTo = DateTime.Now.AddDays(-1);
        private int searchIndex1 = 1;//practice
        private int searchIndex2 = 11;//docno
        private int searchIndex3 = 9;//doctype
        private int searchIndex4 = 10;//matchstatus
        private int searchIndex5 = 11;
        private int searchIndex6 = 8;
        private bool isSearchChecksEnabled = true;
        private bool isMatched = false;
        private bool shouldSearchContinue = false;
        private List<int> rowsToReturn = new List<int>();
        private int searchesRun = 0;
        readonly string _userName = CommonFunctions.GetUserName();
        readonly string _stationName = CommonFunctions.GetFqdn();

        DataAccessLayer.GetDocumentData DocumentService = new DataAccessLayer.GetDocumentData();
        DocumentManagerServiceClient _documentManagerService =
            new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME);
        /// <summary>
        /// Author: Mark Lane
        /// this is intended to search in Floware postbd tables and link to EagleIQ Embillz Cash data
        /// 
        /// </summary>
        
        public FindWorkViewModel()
        {
            this.SearchCommand = new RelayCommand(OnSearch);
            this.OpenViewer = new RelayCommand(OnView);
            this.AddToCart = new RelayCommand(OnAddToCart);
            this.ClearSearch = new RelayCommand(OnClearSearch);
            this.SaveSearch = new RelayCommand(OnSaveSearch);
            this.SaveColumns = new RelayCommand(OnSaveColumns);
            this.ClearColumns = new RelayCommand(OnClearColumns);
            this.DataBinding = new RelayCommand(OnDataBinding);
            this.ClearData = new RelayCommand(OnClearData);
            this.AddNotes = new RelayCommand(OnAddNotes);
            this.SaveNotes = new RelayCommand(OnSaveNotes);

            GetObservableCollectionMapActNames = GetObservableMapActivity();
            //set the Maps
            GetUsersMapList();
            VisibleTwo = Visibility.Hidden;
            VisibleThree = Visibility.Hidden;
            VisibleFour = Visibility.Hidden;
            VisibleFive = Visibility.Hidden;
            VisibleSix = Visibility.Hidden;
            //fill previous search if Map has not been saved then forget it.
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default["saveMap"].ToString()))
            {
                //load our search data.
                GetSavedSearchData();
            }
            getFlowareSearchData = new DataTable("Floware");

            documentTypes = Doctype.DocTypeList;
            documentDetails = DocDetail.DocDetailList;
            practiceList = Practices.PracticeList;
            
            BackgroundSearch.DoWork += BackgroundSearch_DoWork;
            BackgroundSearch.RunWorkerCompleted += BackgroundSearch_RunWorkerCompleted;
            BackgroundMoreSearchData.DoWork += BackgroundMoreSearchData_DoWork;
            BackgroundMoreSearchData.RunWorkerCompleted += BackgroundMoreSearchData_RunWorkerCompleted;
            BackgroundGetCount.DoWork += BackgroundGetCount_DoWork;
            MaxRowsToReturn = maxRowsToReturn();
            MaximumRowCount = 10000;
            
        }

        void BackgroundGetCount_DoWork(object sender, DoWorkEventArgs e)
        {

            int SearchTotal = 0;
            string dt1 = "";
            string dt2 = "";
            if (IsDepDateEnabled == true)
            {
                dt1 = DepositDateFrom.ToString("yyyyMMdd");
                dt2 = DepositDateTo.ToString("yyyyMMdd");
            }
            
            using (var docManagerServiceClient = new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME))
            {
                SearchTotal = docManagerServiceClient.SearchPostBdCount(Environment.MachineName, Environment.UserName,
                                         CurrentMapName, CurrentActivityName,
                                         Practice, Division, dt1, dt2, Embacct,
                                         Reason, Checknum, MinCheckamt, MaxCheckamt, MinPaidamt, MaxPaidamt,
                                         DocType, DocDet, Assigned_To, Docno, Pardocno);
            }
             Dispatcher dispatcher = Application.Current.Dispatcher;
                string ifn = "";
                Int16 pageno = 1;

                if (!dispatcher.CheckAccess())
                {
                    dispatcher.BeginInvoke((Action)(() =>
                    {
                        MaxResults = "Total Rows " + SearchTotal.ToString();
                    
                     }));
                }
                else
                {
                    MaxResults = "Total Rows " + SearchTotal.ToString();
                }
             
        }
        //Dejan would like to allow users to set the max rows they want to return.
        //and to limit the max to 10,000
        private List<int> maxRowsToReturn()
        {
            List<int> list = new List<int>();
            int i = 0;
            while (i < 10000)
            {
                i = i + 500;
                list.Add(i);

            }
            return list;

        }
        public int? MaximumRowCount
        {
            get;
            set;
        }
        void BackgroundMoreSearchData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }
        private int StartRow = 1;
        private int EndRow = 500;
        private void GetRowsToRetrieve()
        {
            EnumerateSearch getRange = new EnumerateSearch();

            StartRow = getRange.GetStart500(SearchesRun);
            EndRow = getRange.GetEnd500(SearchesRun);

            if(MaximumRowCount != null && StartRow > MaximumRowCount)
            {
                ShouldSearchContinue = false;
            }
           
        }

        public int SearchesRun
        {
            get { return searchesRun; }
            set { searchesRun = value; }
        }
  
        void BackgroundMoreSearchData_DoWork(object sender, DoWorkEventArgs e)
        {
           
            try
            {
                SearchesRun++;
                GetRowsToRetrieve();
                if (ShouldSearchContinue)
                {
                    if (IsSearchChecksEnabled)
                    {

                        ReturnMoreFlowareWithC2PR2P(StartRow, EndRow);

                    }
                    else
                    {
                        ReturnMoreFloware(StartRow, EndRow);
                    }
                }

                
            }
            catch (Exception ex)
            {
                // Mouse.OverrideCursor = Cursors.Arrow;
                MessageBox.Show("Error OnSearch : " + ex.Message);
            }
        }
        
        public void OnSearchMore()
        {
            if (ShouldSearchContinue == true)
            {
                if (BackgroundMoreSearchData.IsBusy == false)
                {

                        Mouse.OverrideCursor = Cursors.Wait;
                        
                        BackgroundMoreSearchData.RunWorkerAsync();
                    
                }
            }
        }
        [STAThread]
        void BackgroundSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }
        /// <summary>
        /// Mark Lane
        /// changed searches to be run as background workers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [STAThread]
        void BackgroundSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            int totalRows = 0;
            try
            {
                //use SelectedItem if you wish to use selected item locally.
                // GetIDMSearchData = ReturnSearchIDM(App.Current.FindResource("SearchType").ToString(), App.Current.FindResource("SearchValue").ToString());
                // Mouse.OverrideCursor = Cursors.Wait;
                ShouldSearchContinue = false;
                if (IsSearchChecksEnabled)
                {
                    ReturnSearchFlowareWithC2PR2P(1, 500);
                }
                else
                {
                    ReturnSearchFloware(1, 500);
                }

                //GetEmbillzSearchData = ReturnSearchEmbillz(App.Current.FindResource("SearchType").ToString(), App.Current.FindResource("SearchValue").ToString());
                //set the headers
               // string MaxRows = GetFlowareSearchData.Rows[0]["TotalRows"].ToString();
                FlowareResults = "Workflow Results Found " + GetFlowareSearchData.Rows.Count + " Rows";
                //EmbillzResults = "Embillz Results Found " + GetEmbillzSearchData.Rows.Count + " Rows";
                totalRows = GetFlowareSearchData.Rows.Count;
                SearchResults = "Search Results (Total Hits = " + totalRows + ")";
                if (totalRows <= 0) { ShowFlowareData = false; } else { ShowFlowareData = true; }
                SearchesRun = 1;
                // Mouse.OverrideCursor = Cursors.Arrow;
                //expand the expanders
                //if data greater than 0 open the expander
                //ShowEmbillzData = false;
                //if (getEmbillzSearchData.Rows.Count > 0) ShowEmbillzData = true;
                // ShowFlowareData = false;
                // if (getFlowareSearchData.Rows.Count > 0) ShowFlowareData = true;
                // ShowIDMData = false;
                // if (getIDMSearchData.Rows.Count > 0) ShowIDMData = true;
                ShouldSearchContinue = true;
            }
            catch (Exception ex)
            {
               // Mouse.OverrideCursor = Cursors.Arrow;
                MessageBox.Show("Error OnSearch : " + ex.Message);
            }
        }


        /*
         * public RelayCommand SearchCommand
        {
            get
            {
                return searchCommand
                    ?? (searchCommand = new RelayCommand(async () =>
                    {
                        if (canExecuteMyCommand) { return; } canExecuteMyCommand = true;
                        SearchCommand.RaiseCanExecuteChanged();
                        OnSearch();
                        canExecuteMyCommand = false;
                        SearchCommand.RaiseCanExecuteChanged();
                    }, () => !canExecuteMyCommand));
            }
            set
            {
                searchCommand = value;
            }

        }*/
        #region view binding commands
        public RelayCommand SaveColumns
        {
            get;
            set;
        }
        public RelayCommand ClearColumns
        {
            get;
            set;
        }
        public RelayCommand SearchCommand
        {
            get;
            private set;

        }
        public RelayCommand SaveSearch
        {
            get;
            set;
        }
        public RelayCommand DataBinding
        {
            get;
            set;
        }
        public RelayCommand ClearData
        {
            get;
            set;
        }
        public RelayCommand SaveNotes
        {
            get;
            set;
        }
        public RelayCommand AddNotes
        {
            get;
            set;
        }
        /// <summary>
        /// Raised from the Mouse Double Click in DataGrid
        /// Index is set from SelectedIndex binding.
        /// </summary>
        /*public RelayCommand OpenViewer
        {
            get
            {
                return openViewer
                    ?? (openViewer = new RelayCommand(async () =>
                    {
                        if (canExecuteMyCommand) { return; } canExecuteMyCommand = true;
                        openViewer.RaiseCanExecuteChanged();
                        OnView();
                        canExecuteMyCommand = false;
                        openViewer.RaiseCanExecuteChanged();
                    }, () => !canExecuteMyCommand));
            }
            set
            {
                openViewer = value;
            }
        }
         * */
        public RelayCommand OpenViewer
        {
            get;
            set;
        }
        
        public RelayCommand AddToCart
        {
            get;
            set;
        }
        public RelayCommand ClearSearch
        {
            get;
            set;
        }
        #endregion
        #region binding properties INotifyPropertyChanged
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
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

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
        public List<int> MaxRowsToReturn
        {
            get { return rowsToReturn; }
            set 
            { 
                rowsToReturn = value;
                OnPropertyChanged("MaxRowsToReturn");
            }
        }
        //for combo list search by lists
        public List<string> SearchList1
        {
            get { return searchList1; }
            set 
            { 
                searchList1 = value;
                OnPropertyChanged("SearchList1");
            }
        }
        public List<string> SearchList2
        {
            get { return searchList2; }
            set
            {
                searchList2 = value;
                OnPropertyChanged("SearchList2");
            }
        }
        public List<string> SearchList3
        {
            get { return searchList3; }
            set
            {
                searchList3 = value;
                OnPropertyChanged("SearchList3");
            }
        }
        public List<string> SearchList4
        {
            get { return searchList4; }
            set
            {
                searchList4 = value;
                OnPropertyChanged("SearchList4");
            }
        }
        public List<string> SearchList5
        {
            get { return searchList5; }
            set
            {
                searchList5 = value;
                OnPropertyChanged("SearchList5");
            }
        }
        public List<string> SearchList6
        {
            get { return searchList6; }
            set
            {
                searchList6 = value;
                OnPropertyChanged("SearchList6");
            }
        }
        //for the selected type of search
        public string SearchTypeOne
        {
            get
            {
                return searchTypeOne;
            }
            set
            {
                //need to clear the data set from the previous selection!!!
                if(searchTypeOne != value)
                {
                    ClearSearchValues(searchTypeOne);
                }
                searchTypeOne = value;
                if(searchTypeOne != "[NONE]")
                {
                    VisibleTwo = Visibility.Visible;
                }
                else
                {
                    VisibleTwo = Visibility.Hidden;
                }
                //set the search lists
                if (searchTypeOne == "Doctype")
                {
                    SearchList1 = documentTypes;
                }
                else if (searchTypeOne == "Docdet")
                {
                    SearchList1 = documentDetails;
                }
                else if (searchTypeOne == "Practice")
                {
                    SearchList1 = practiceList;
                }
                else
                {
                    SearchList1 = null;
                }
                OnPropertyChanged("SearchTypeOne");
            }
        }
        public string SearchTypeTwo
        {
            get
            {
                return searchTypeTwo;
            }
            set
            {
                if (searchTypeTwo != value)
                {
                    ClearSearchValues(searchTypeTwo);
                }
                searchTypeTwo = value;
                if (searchTypeTwo != "[NONE]")
                {
                    VisibleThree = Visibility.Visible;
                }
                else
                {
                    VisibleThree = Visibility.Hidden;
                }
                //set the search lists
                if (searchTypeTwo == "Doctype")
                {
                    SearchList2 = documentTypes;
                }
                else if (searchTypeTwo == "Docdet")
                {
                    SearchList2 = documentDetails;
                }
                else if (searchTypeTwo == "Practice")
                {
                    SearchList2 = practiceList;
                }
                else
                {
                    SearchList2 = null;
                }
                OnPropertyChanged("SearchTypeTwo");
            }
        }
        public string SearchTypeThree
        {
            get
            {
                return searchTypeThree;
            }
            set
            {
                if (searchTypeThree != value)
                {
                    ClearSearchValues(searchTypeThree);
                }
                searchTypeThree = value;
                if (searchTypeThree != "[NONE]")
                {
                    VisibleFour = Visibility.Visible;
                }
                else
                {
                    VisibleFour = Visibility.Hidden;
                }
                //set the search lists
                if (searchTypeThree == "Doctype")
                {
                    SearchList3 = documentTypes;
                }
                else if (searchTypeThree == "Docdet")
                {
                    SearchList3 = documentDetails;
                }
                else if (searchTypeThree == "Practice")
                {
                    SearchList3 = practiceList;
                }
                else
                {
                    SearchList3 = null;
                }
                OnPropertyChanged("SearchTypeThree");
            }
        }
        public string SearchTypeFour
        {
            get
            {
                return searchTypeFour;
            }
            set
            {
                if (searchTypeFour != value)
                {
                    ClearSearchValues(searchTypeFour);
                }
                searchTypeFour = value;
                if (searchTypeFour != "[NONE]")
                {
                    VisibleFive = Visibility.Visible;
                }
                else
                {
                    VisibleFive = Visibility.Hidden;
                }
                //set the search lists
                if (searchTypeFour == "Doctype")
                {
                    SearchList4 = documentTypes;
                }
                else if (searchTypeFour == "Docdet")
                {
                    SearchList4 = documentDetails;
                }
                else if (searchTypeFour == "Practice")
                {
                    SearchList4 = practiceList;
                }
                else
                {
                    SearchList4 = null;
                }
                OnPropertyChanged("SearchTypeFour");
            }
        }
        public string SearchTypeFive
        {
            get
            {
                return searchTypeFive;
            }
            set
            {
                if (searchTypeFive != value)
                {
                    ClearSearchValues(searchTypeFive);
                }
                searchTypeFive = value;
                if (searchTypeFive != "[NONE]")
                {
                    VisibleSix = Visibility.Visible;
                }
                else
                {
                    VisibleSix = Visibility.Hidden;
                }
                //set the search lists
                if (searchTypeFive == "Doctype")
                {
                    SearchList5 = documentTypes;
                }
                else if (searchTypeFive == "Docdet")
                {
                    SearchList5 = documentDetails;
                }
                else if (searchTypeFive == "Practice")
                {
                    SearchList5 = practiceList;
                }
                else
                {
                    SearchList5 = null;
                }
                OnPropertyChanged("SearchTypeFive");
            }
        }
        public string SearchTypeSix
        {
            get
            {
                return searchTypeSix;
            }
            set
            {
                if (searchTypeSix != value)
                {
                    ClearSearchValues(searchTypeSix);
                }
                searchTypeSix = value;
                //set the search lists
                if (searchTypeSix == "Doctype")
                {
                    SearchList6 = documentTypes;
                }
                else if (searchTypeSix == "Docdet")
                {
                    SearchList6 = documentDetails;
                }
                else if (searchTypeSix == "Practice")
                {
                    SearchList6 = practiceList;
                }
                else
                {
                    SearchList6 = null;
                }
                OnPropertyChanged("SearchTypeSix");
            }
        }
        public bool ShouldSearchContinue
        {
            get { return shouldSearchContinue; }
            set 
            { 
                    shouldSearchContinue = value;
                    OnPropertyChanged("ShouldSearchContinue");
            }
        }
        public Visibility VisibleTwo
        {
            get
            {
                return visibleTwo;
            }
            set
            {
                visibleTwo = value;
                OnPropertyChanged("VisibleTwo");
            }
        }
        public Visibility VisibleThree
        {
            get
            {
                return visibleThree;
            }
            set
            {
                visibleThree = value;
                OnPropertyChanged("VisibleThree");
            }
        }
        public Visibility VisibleFour
        {
            get
            {
                return visibleFour;
            }
            set
            {
                visibleFour = value;
                OnPropertyChanged("VisibleFour");
            }
        }
        public Visibility VisibleFive
        {
            get
            {
                return visibleFive;
            }
            set
            {
                visibleFive = value;
                OnPropertyChanged("VisibleFive");
            }
        }
        public Visibility VisibleSix
        {
            get
            {
                return visibleSix;
            }
            set
            {
                visibleSix = value;
                OnPropertyChanged("VisibleSix");
            }
        }
        //For the value to search on.
        public string SearchValueOne
        {
            get
            {
                return searchValueOne;
            }
            set
            {
                searchValueOne = value;
                OnPropertyChanged("SearchValueOne");
            }
        }
        public string SearchValueTwo
        {
            get
            {
                return searchValueTwo;
            }
            set
            {
                searchValueTwo = value;
                OnPropertyChanged("SearchValueTwo");
            }
        }
        public string SearchValueThree
        {
            get
            {
                return searchValueThree;
            }
            set
            {
                searchValueThree = value;
                OnPropertyChanged("SearchValueThree");
            }
        }
        public string SearchValueFour
        {
            get
            {
                return searchValueFour;
            }
            set
            {
                searchValueFour = value;
                OnPropertyChanged("SearchValueFour");
            }
        }
        public string SearchValueFive
        {
            get
            {
                return searchValueFive;
            }
            set
            {
                searchValueFive = value;
                OnPropertyChanged("SearchValueFive");
            }
        }
        public string SearchValueSix
        {
            get
            {
                return searchValueSix;
            }
            set
            {
                searchValueSix = value;
                OnPropertyChanged("SearchValueSix");
            }
        }
        /// <summary>
        /// used to allow date search or not.
        /// </summary>
        public bool IsDepDateEnabled
        {
            get;
            set;
        }
        public bool IsSearchChecksEnabled
        {
            get
            {
               return isSearchChecksEnabled;
            }
            set
            {
                isSearchChecksEnabled = value;
                OnPropertyChanged("IsSearchChecksEnabled");
            }
        }
        public bool IsMatched
        {
            get
            {
                return isMatched;
            }
            set
            {
                isMatched = value;
                OnPropertyChanged("IsMatched");
            }
        }
        public string SelectedItem
        {
            get;
            set;
        }
        //used to set defaults in the search 
        public int SearchIndex1
        {
            get { return searchIndex1; }
            set
            {
                searchIndex1 = value;
                OnPropertyChanged("SearchIndex1");
            }
        }
        public int SearchIndex2
        {
            get { return searchIndex2; }
            set
            {
                searchIndex2 = value;
                OnPropertyChanged("SearchIndex2");
            }
        }
        public int SearchIndex3
        {
            get { return searchIndex3; }
            set
            {
                searchIndex3 = value;
                OnPropertyChanged("SearchIndex3");
            }
        }
        public int SearchIndex4
        {
            get { return searchIndex4; }
            set
            {
                searchIndex4 = value;
                OnPropertyChanged("SearchIndex4");
            }
        }
        public int SearchIndex5
        {
            get { return searchIndex5; }
            set
            {
                searchIndex5 = value;
                OnPropertyChanged("SearchIndex5");
            }
        }
        public int SearchIndex6
        {
            get { return searchIndex6; }
            set
            {
                searchIndex6 = value;
                OnPropertyChanged("SearchIndex6");
            }
        }
        //Populate DocumentType Combobox
        
        /// <summary>
        /// used to set the IFN to be exported which is the row from the IDM Search.
        /// </summary>
        public string SelectedIDMItem
        {
            get
            {
                return selectedIDMItem;
            }
            set
            {
                selectedIDMItem = value;
                if (selectedIDMItem != null)
                {

                    ResourceDictionary resources = App.Current.Resources; // If in a Window/UserControl/etc
                    string ifn = GetIDMSearchData.Rows[SelectedIDMRow]["ifn"].ToString();
                    string docno = GetIDMSearchData.Rows[SelectedIDMRow]["docno"].ToString();
                    resources["ExportIFN"] = ifn;
                    resources["ExportDocno"] = docno;
                }
                
                OnPropertyChanged("SelectedIDMItem");
            }
        }
        /// <summary>
        /// used to set the IFN to be exported which is the row from the Floware Search.
        /// </summary>
        public string SelectedFlowareItem
        {
            get
            {
                return selectedFlowareItem;
            }
            set
            {
                selectedFlowareItem = value;
                ResourceDictionary resources = App.Current.Resources; // If in a Window/UserControl/etc
                if((getFlowareSearchData != null) && (SelectedRow != -1))
                {
                    try
                    {
                        string ifn = GetFlowareSearchData.Rows[SelectedRow]["ifn"].ToString();
                        string docno = GetFlowareSearchData.Rows[SelectedRow]["docno"].ToString();
                        
                        resources["ExportIFN"] = ifn;
                        resources["ExportDocno"] = docno;
                    }
                    catch
                    {
                        //columns are wrong because query is wrong
                    }
                    OnPropertyChanged("SelectedFlowareItem");
                }
            }
        }
        private bool _isOpen;
        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                if (_isOpen == value) return;
                _isOpen = value;
                OnPropertyChanged("IsOpen");
            }
        }

        public string DocumentNumber
        {
            get { return documentNumber; }
            set
            {
                if(documentNumber!=value)
                {
                    documentNumber = value;
                    OnPropertyChanged("DocumentNumber");
                }
            }
        }

        public string Notes
        {
            get { return notes; }
            set
            {
                if (notes != value)
                {
                    notes = value;
                    OnPropertyChanged("Notes");
                }
            }
        }

        public string CreatedUser
        {

            get { return createUser ; }
            set
            {
                createUser = value;
                OnPropertyChanged("CreatedUser");
            }
        }
        public string ModifiedUser
        {

            get { return modUser; }
            set
            {
                modUser = value;
                OnPropertyChanged("ModifiedUser");
            }
        }
        public string CreatedDate
        {

            get { return createDate ; }
            set
            {
                createDate = value;
                OnPropertyChanged("CreatedDate");
            }
        }
        public string ModifiedDate
        {

            get { return modDate; }
            set
            {
                modDate = value;
                OnPropertyChanged("ModifiedDate");
            }
        }
     
        /// <summary>
        /// Used to enable the Export button only when the IDM Search Control has focus
        /// </summary>
        public bool ExportEnabled
        {
            get;
            set;
        }
        //Used for more search criteria
        public ObservableCollection<string> RetrievedItems
        {
            get
            {
                return retrievedItems;
            }
            set
            {
                retrievedItems = value;

                OnPropertyChanged("RetrievedItems");
            }
        }

        public DataTable GetIDMSearchData
        {
            get
            {
                return getIDMSearchData;
            }
            set
            {
                getIDMSearchData = value;
                OnPropertyChanged("GetIDMSearchData");
            }
        }
        public DataTable GetFlowareSearchData
        {
            get
            {
                return getFlowareSearchData;
            }
            set
            {
                getFlowareSearchData = value;
                OnPropertyChanged("GetFlowareSearchData");
            }
        }
        public DataTable GetEmbillzSearchData
        {
            get
            {
                return getEmbillzSearchData;
            }
            set
            {
                getEmbillzSearchData = value;
                OnPropertyChanged("GetEmbillzSearchData");
            }
        }
        public DataTable GetAssemblySearchData
        {
            get
            {
                return getAssemblySearchData;
            }
            set
            {
                getAssemblySearchData = value;
                OnPropertyChanged("GetAssemblySearchData");
            }
        }
        public ObservableCollection<Model.MapActivityModel> GetObservableCollectionMapActNames
        {
            get;
            private set;
        }
        ///Comboboxes are bound to these two lists.  On Map selection CurrentMapName will set GetActivities
        public List<string> GetMaps
        {
            get { return getMaps; }
            set
            {
                getMaps = value;
                OnPropertyChanged("GetMaps");
            }
        }
        public List<string> GetActivities
        {
            get { return getActivities; }
            set
            {
                getActivities = value;
                OnPropertyChanged("GetActivities");
            }
        }
        /// <summary>
        /// This is the index of the selected row from our workflow table.
        /// let the view model do the work!!!!
        /// mode is twoway for it to be set or used.
        /// </summary>
        /// <returns></returns>
        public int SelectedRow
        {
            get
            {
                return selectedRow;
            }
            set
            {

                selectedRow = value;
                OnPropertyChanged("SelectedRow");

            }
        }
        /// <summary>
        /// The selected row from IDM Search
        /// </summary>
        public int SelectedIDMRow
        {
            get
            {
                return selectedIDMRow;
            }
            set
            {

                selectedIDMRow = value;
                OnPropertyChanged("SelectedIDMRow");

            }
        }
        /// <summary>
        /// Used in Expander to show results
        /// </summary>
        ///
        public bool ShowIDMData
        {
            get { return showIDMData; }
            set
            {
                showIDMData = value;
                OnPropertyChanged("ShowIDMData");
            }
        }
        public bool ShowEmbillzData
        {
            get { return showEmbillzData; }
            set
            {
                showEmbillzData = value;
                OnPropertyChanged("ShowEmbillzData");
            }
        }
        public bool ShowFlowareData
        {
            get { return showFlowareData; }
            set
            {
                showFlowareData = value;
                OnPropertyChanged("ShowFlowareData");
            }
        }
        public bool ShowResults
        {
            get { return showResults; }
            set
            {
                showResults = value;
                OnPropertyChanged("ShowResults");
            }
        }
        /// <summary>
        /// Used to show Expander headers with counts
        /// </summary>
        public string SearchResults
        {
            get { return searchResults; }
            set
            {
                searchResults = value;
                OnPropertyChanged("SearchResults");
            }
        }
        public string WorkflowResults
        {
            get { return workflowResults; }
            set
            {
                workflowResults = value;
                OnPropertyChanged("WorkflowResults");
            }
        }
        public string IDMResults
        {
            get { return idmResults; }
            set
            {
                idmResults = value;
                OnPropertyChanged("IDMResults");
            }
        }
        public string FlowareResults
        {
            get { return flowareResults; }
            set
            {
                flowareResults = value;
                OnPropertyChanged("FlowareResults");
            }
        }
        public string MaxResults
        {
            get { return maxResults; }
            set
            {
                maxResults = value;
                OnPropertyChanged("MaxResults");
            }
        }
        public string EmbillzResults
        {
            get { return embillzResults; }
            set
            {
                embillzResults = value;
                OnPropertyChanged("EmbillzResults");
            }
        }
      

        #endregion
        #region Functions 

        ///
        /// 
        /// 
        public void OnAddNotes()
        {   
            if((getFlowareSearchData != null) && (SelectedRow != -1))
            {
               OnClearData();              
               OnDataBinding();   
            }
        }
       

       public void   OnClearData()
        {
           //Clear all the fields in the popup
            DocumentNumber = "";
            Notes = "";
           CreatedDate="";
           ModifiedDate="";
           CreatedUser="";
           ModifiedUser = "";

        }


        public void OnDataBinding()
        {
           
                try
                {
                    DocumentNumber = GetFlowareSearchData.Rows[SelectedRow]["docno"].ToString();
                    string snotes = GetFlowareSearchData.Rows[SelectedRow]["note"].ToString();

                    if (snotes == "Yes")
                    {

                        int keyvalue = Convert.ToInt32(GetFlowareSearchData.Rows[SelectedRow]["key"]);
                        string NoteMsg = "";
                        string Typevar = GetFlowareSearchData.Rows[SelectedRow]["datasource"].ToString();
                        string Type = Typevar.Substring(0, 1);
                        string Action = "READ";

                        var result = _documentManagerService.ProcessNote(_stationName, _userName, keyvalue, NoteMsg, Type, Action);

                        if (result == null)
                        {
                            MessageBox.Show("Note is not available");
                            return;
                        }
                        else
                        {
                            CreatedDate = result.CreateDt;
                            CreatedUser = result.CreateUId;
                            ModifiedDate = result.ModDt;
                            ModifiedUser = result.ModUId;
                            Notes = result.NoteTxt;
                            IsOpen = true;
                        }
                    }
                    else if (snotes == "No")
                    {

                        IsOpen = true;
                    }
                }
                catch(Exception ex)
                {
                    throw ex;
                }
           }


      
        public void OnSaveNotes()
        {
            //Save Notes             
            int keyvalue = Convert.ToInt32(GetFlowareSearchData.Rows[SelectedRow]["key"]);
            string NoteMsg = Notes;
            string Typevar = GetFlowareSearchData.Rows[SelectedRow]["datasource"].ToString();
            string Type = Typevar.Substring(0, 1);
            string Action = "SAVE";
            var result = _documentManagerService.ProcessNote(_stationName, _userName, keyvalue, NoteMsg, Type, Action);

            if (result == null)
            {
                MessageBox.Show("Error in Saving Notes.Please close and retry.");
            }
            else
                IsOpen = false;
        }

        ///
        ///Search Command raised event to fill datagrids with bound tables.
        ///Fill IDM, Assembly and Floware search.
        ///Search uses the updated Xaml.Resources and selected Dropdown search.
        ///
        ///
        [STAThread]
        public void OnSearch()
        {
            
            Mouse.OverrideCursor = Cursors.Wait;
            BackgroundSearch.RunWorkerAsync();
            BackgroundGetCount.RunWorkerAsync();
        }
       
      
        public void OnSaveSearch()
        {
            PrepareSearch();
            Properties.Settings.Default["Docno"] = Docno;
            Properties.Settings.Default["Pardocno"] = Pardocno;
            Properties.Settings.Default["Practice"] = Practice;
            Properties.Settings.Default["Division"] = Division;
            Properties.Settings.Default["DepdtStart"] = DepositDateFrom;
            Properties.Settings.Default["DepdtEnd"] = DepositDateTo;
            Properties.Settings.Default["Embacct"] = Embacct;
            Properties.Settings.Default["Reason"] = Reason;
            Properties.Settings.Default["Doctype"] = DocType;
            Properties.Settings.Default["Docdet"] = DocDet;
            Properties.Settings.Default["saveActivity"] = CurrentActivityName;
            Properties.Settings.Default["saveMap"] = CurrentMapName;
            Properties.Settings.Default["saveSelectedItem1"] = SearchTypeOne;
            Properties.Settings.Default["saveSelectedItem2"] = SearchTypeTwo;
            Properties.Settings.Default["saveSelectedItem3"] = SearchTypeThree;
            Properties.Settings.Default["saveSelectedItem4"] = SearchTypeFour;
            Properties.Settings.Default["saveSelectedItem5"] = SearchTypeFive;
            Properties.Settings.Default["saveSelectedItem6"] = SearchTypeSix;
            Properties.Settings.Default["SearchChecksEnabled"] = IsSearchChecksEnabled;
            Properties.Settings.Default.Save();

        }
        /// <summary>
        /// Mark lane
        /// get the data saved and load up the search control
        /// </summary>
        private void GetSavedSearchData()
        {
            try
            {
                CurrentMapName = Properties.Settings.Default["saveMap"].ToString();
                CurrentActivityName = Properties.Settings.Default["saveActivity"].ToString();
                if (!string.IsNullOrWhiteSpace(Properties.Settings.Default["saveSelectedItem1"].ToString()))
                {
                    SearchTypeOne = Properties.Settings.Default["saveSelectedItem1"].ToString();
                    SearchValueOne = PrepareSavedSearchData(SearchTypeOne);
                }
                if (!string.IsNullOrWhiteSpace(Properties.Settings.Default["saveSelectedItem2"].ToString()))
                {
                    SearchTypeTwo = Properties.Settings.Default["saveSelectedItem2"].ToString();
                    SearchValueTwo = PrepareSavedSearchData(SearchTypeTwo);
                }
                if (!string.IsNullOrWhiteSpace(Properties.Settings.Default["saveSelectedItem3"].ToString()))
                {
                    SearchTypeThree = Properties.Settings.Default["saveSelectedItem3"].ToString();
                    SearchValueThree = PrepareSavedSearchData(SearchTypeThree);
                }
                if (!string.IsNullOrWhiteSpace(Properties.Settings.Default["saveSelectedItem4"].ToString()))
                {
                    SearchTypeFour = Properties.Settings.Default["saveSelectedItem4"].ToString();
                    SearchValueFour = PrepareSavedSearchData(SearchTypeFour);
                }
                if (!string.IsNullOrWhiteSpace(Properties.Settings.Default["saveSelectedItem5"].ToString()))
                {
                    SearchTypeFive = Properties.Settings.Default["saveSelectedItem5"].ToString();
                    SearchValueFive = PrepareSavedSearchData(SearchTypeFive);
                }
                if (!string.IsNullOrWhiteSpace(Properties.Settings.Default["saveSelectedItem6"].ToString()))
                {
                    SearchTypeSix = Properties.Settings.Default["saveSelectedItem6"].ToString();
                    SearchValueSix = PrepareSavedSearchData(SearchTypeSix);
                }
                if (!string.IsNullOrWhiteSpace(Properties.Settings.Default["DepdtStart"].ToString()))
                {
                    DepositDateFrom = DateTime.Parse(Properties.Settings.Default["DepdtStart"].ToString());

                }
                if (!string.IsNullOrWhiteSpace(Properties.Settings.Default["DepdtEnd"].ToString()))
                {
                    DepositDateTo = DateTime.Parse( Properties.Settings.Default["DepdtEnd"].ToString());
                }
                if(Properties.Settings.Default["SearchChecksEnabled"].ToString() != null)
                {
                    IsSearchChecksEnabled = Convert.ToBoolean(Properties.Settings.Default["SearchChecksEnabled"].ToString());
                }
            }
            catch(Exception ex)
            {
                //assigned_To cares if object itself is null for now.
            }
        }
        public string PrepareSavedSearchData(string searchValueIn)
        {
            string retval = "";
            switch (searchValueIn)
            {
                case "Practice":
                        retval = Properties.Settings.Default["Practice"].ToString();
                        break;
                    case "Division":
                        retval = Properties.Settings.Default["Division"].ToString();
                        break;
                    case "Min Checkamt":
                        retval = "";
                        break;
                    case "Max Checkamt":
                        retval = "";
                        break;
                    case "Checknum":
                        retval = "";
                        break;
                    case "Embacct":
                        retval = Properties.Settings.Default["Embacct"].ToString();;
                        break;
                    case "Reason":
                        retval = Properties.Settings.Default["Reason"].ToString();
                        break;
                    case "Docdet":
                        retval = Properties.Settings.Default["Docdet"].ToString();
                        break;
                    case "Doctype":
                        retval = Properties.Settings.Default["Doctype"].ToString();
                        break;
                    
                    case "Docno":
                        retval = Properties.Settings.Default["Docno"].ToString();
                        break;
                    case "Pardocno":
                        retval = Properties.Settings.Default["Pardocno"].ToString();
                        break;
                    case "Min Paidamt":
                        retval = "";
                        break;
                    case "Max Paidamt":
                        retval = "";
                        break;
                    case "Assigned_To":
                        retval = "";
                        break;
                    default:
                         retval = "";
                        break;
                
            }
            return retval;
        }
        /// <summary>
        /// Mark Lane 
        /// clear the settings ...
        /// </summary>
        private void OnClearSearch()
        {
            //var getdata = Properties.Settings.Default["History1"];
            Properties.Settings.Default["Docno"] = "";
            Properties.Settings.Default["Pardocno"] = "";
            Properties.Settings.Default["Practice"] = "";
            Properties.Settings.Default["Division"] = "";
            Properties.Settings.Default["DepdtStart"] = null;
            Properties.Settings.Default["DepdtEnd"] = null;
            Properties.Settings.Default["Embacct"] = "";
            Properties.Settings.Default["Reason"] = "";
            Properties.Settings.Default["Doctype"] = "";
            Properties.Settings.Default["Docdet"] = "";
            Properties.Settings.Default["saveMap"] = "";
            Properties.Settings.Default["saveActivity"] = "";
            Properties.Settings.Default["saveSelectedItem1"] = "";
            Properties.Settings.Default["saveSelectedItem2"] = "";
            Properties.Settings.Default["saveSelectedItem3"] = "";
            Properties.Settings.Default["saveSelectedItem4"] = "";
            Properties.Settings.Default["saveSelectedItem5"] = "";
            Properties.Settings.Default["saveSelectedItem6"] = "";
            Properties.Settings.Default["SearchChecksEnabled"] = false;
            Properties.Settings.Default.Save();
            SearchTypeSix = "[NONE]";
            SearchTypeFive = "[NONE]";
            SearchTypeFour = "[NONE]";
            SearchTypeThree = "[NONE]";
            SearchTypeTwo = "[NONE]";
            SearchTypeOne = "[NONE]";
            SearchValueSix = "";
            SearchValueFive = "";
            SearchValueFour = "";
            SearchValueThree = "";
            SearchValueTwo = "";
            SearchValueOne = "";
            CurrentActivityName = "";
            CurrentMapName = "";
            
        }

        private void ClearBusinessData()
        {
                    
                    Practice = "";
                    Division = "";
                    MinCheckamt = "";
                    MaxCheckamt = "";
                    MinCheckamt = "";
                    Embacct = ""; 
                    Reason = "";
                    DocDet = "";
                    DocType = "";
                    MatchStatus = "";
                    Docno = "";
                    Pardocno = "";
                    MinPaidamt = "";
                    MaxPaidamt = "";
                    Assigned_To = "";
                  
        }
        /// <summary>
        /// Author Mark Lane
        /// Search Control should contain its own viewer.
        /// This viewer needs to export from the searches viewer.
        /// Or should it not have a viewer?
        /// could have search raise data to MainWindow to handle all viewer
        /// functions?
        /// all the big work here.
        /// </summary>
        public void OnView()
        {
            try
            {
                Dispatcher dispatcher = Application.Current.Dispatcher;
                string ifn = "";
                Int16 pageno = 1;

                if (!dispatcher.CheckAccess())
                {
                    dispatcher.BeginInvoke((Action)(() =>
                    {
                       //what you want to do...
                        try
                        {
                            ifn = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["ifn"].ToString();
                            pageno = Convert.ToInt16(GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["npages"].ToString());
                        }
                        catch
                        {
                            //its Embillz source
                        }
                        string docno = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["docno"].ToString();
                        
                        if (doc == null)
                        {
                            
                            doc = new View.DocumentViewer(Constants.IDMServerName, PermissionsModel.Permissions.WorkFlowPermissions.WindowsUsername, "");
                          
                            doc.Visibility = System.Windows.Visibility.Visible;
                            doc.Show();
                            doc.GetParentDocnoData(docno);
                            if (ifn != "")
                            {
                                doc.DrawDocument(ifn, pageno);
                            }
                            else
                            {
                                doc.ShowDocno();
                            }
                            
                        }
                        else
                        {
                            doc.Visibility = System.Windows.Visibility.Visible;
                            doc.Show();
                            doc.GetParentDocnoData(docno);
                            if (ifn != "")
                            {
                                doc.DrawDocument(ifn, pageno);
                            }
                            else
                            {
                                doc.ShowDocno();
                            }
                            
                        }
                    }));
                }
                else
                {
                  //carry on...
                    try
                    {
                        ifn = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["ifn"].ToString();
                        pageno = Convert.ToInt16(GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["npages"].ToString());
                    }
                    catch
                    {
                        //its Embillz source.
                    }
                    string docno = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["docno"].ToString();
                    if (doc == null)
                    {
                       
                        doc = new View.DocumentViewer(Constants.IDMServerName, PermissionsModel.Permissions.WorkFlowPermissions.WindowsUsername, "");
                        doc.Visibility = System.Windows.Visibility.Visible;
                        doc.Show();
                        doc.GetParentDocnoData(docno);
                        if (ifn != "")
                        {
                            doc.DrawDocument(ifn, pageno);
                        }
                        else
                        {
                            doc.ShowDocno();
                        }
                        
                    }
                    else
                    {
                        doc.Visibility = System.Windows.Visibility.Visible;
                        doc.Show();
                        doc.GetParentDocnoData(docno);
                        if (ifn != "")
                        {
                            doc.DrawDocument(ifn, pageno);
                        }
                        else
                        {
                            doc.ShowDocno();
                        }
                        
                    }
                }
               
            }
            catch(Exception ex)
            {
               //handle
            }
        }
        
        #endregion
        

        #region viewmodel commands
        public ICommand RunQuickSearch
        {
            get;
            set;
        }
        public void Search(string searchParamThatWasInConstructor)
        {
            // do something to get results (deserialization)
            // var results = new JavascriptSerializer( ).Deserialize<List<string>>( searchParamThatWasInConstructor );
         
            List<string> results = new List<string>();
            results.Add("Search Account");
            results.Add("Search Docno");
            results.Add("Search Check Amount");
            RetrievedItems = new ObservableCollection<string>(results);
            SelectedItem = RetrievedItems.Count > 0 ? RetrievedItems[0] : string.Empty;
        }
        /// <summary>
        /// Binding method to fill the Map and Activity on FindWorkLoad
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Model.MapActivityModel> GetObservableMapActivity()
        {
            Model.MapActivityModel dm;
            ObservableCollection<Model.MapActivityModel> obc = new ObservableCollection<Model.MapActivityModel>();

            DataTable getMapAct = new DataTable("docnos");
            using (var dataAccess = new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME))
            {
                // set the first blank record
                dm = new MapActivityModel("", "");
                obc.Add(dm);
                //get the docno and pardocno records for imaging
                getMapAct = dataAccess.GetMapAndActivityByLoginName(Environment.MachineName, Environment.UserName, Environment.UserName);
                foreach (DataRow row in getMapAct.Rows)
                {
                    dm = new Model.MapActivityModel(row["MAP_NAME"].ToString(), row["ACT_NAME"].ToString());
                    obc.Add(dm);
                }
            }

            return obc;
        }
       /// <summary>
       /// set the Maps for the user
       /// </summary>
        public void GetUsersMapList()
        {
            try
            {
                var newlist = from table in GetObservableCollectionMapActNames.AsEnumerable() select table.MapName;
                GetMaps = newlist.AsEnumerable().Distinct().ToList();
            }
            catch(Exception ex)
            {
                //on initialization this wil not be set yet
            }
        
            
        }
        /// <summary>
        /// set the Activites for the selected Map.
        /// </summary>
        /// <param name="selectedMapName"></param>
        public void GetUsersActivityList(string selectedMapName)
        {
            
                var newlist = from table in GetObservableCollectionMapActNames.AsEnumerable() where table.MapName == selectedMapName select table.ActivityName;
                GetActivities = newlist.ToList();
           
            
        }
        ///
        ///Author: Mark Lane
        ///Using this function to add to the static class for Assembly Check/Cash Matching control
        ///throw this into a datatable or shared observable collection.
        ///Used in the cart.
        ///.DefaultView.ToTable() allows the two way sorting from datatable to datagrid MVVM style
        ///
        ///
        public void OnAddToCart()
        {
              //string checknum = GetIDMSearchData.Rows[SelectedIDMRow]["checknum"].ToString();

            int totalRows = 0;
            string cstatus = "";
            string origBank = "";
            string rstatus = "";
            string origBankAccount = "";
            string depdate = "";
            string checkDate = "";
            string suspendNumber = "";
            string expectEPR = "";
            string filedate = "";
            string status = "";
            string comingledstatus = "";

            try
            {

                if (SelectedRow >= -1)
                {
                    if(GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["key"].ToString() == "")
                    { return; }
                    string checknum = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["checknumber"].ToString();
                    double checkamt = Convert.ToDouble(GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["amount"].ToString());
                    string practice = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["client"].ToString();
                    string docno = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["docno"].ToString();
                    string pardocno = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["docno"].ToString();
                    string doctype = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["doctype"].ToString();
                    string docdet = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["docdetail"].ToString();
                    int matchID = Convert.ToInt32(GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["matchid"].ToString());
                    string externalPayor = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["externalpayor"].ToString();
                    string note = "";
                    int key = Convert.ToInt32(GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["key"].ToString());
                    comingledstatus = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["ComingleStat"].ToString();
                    /*  DataTable mergedDataTable = new DataTable("merged");
                        mergedDataTable.Columns.Add("MatchId");
                        mergedDataTable.Columns.Add("Client");
                        mergedDataTable.Columns.Add("Div");
                        mergedDataTable.Columns.Add("ExternalPayor");
                        mergedDataTable.Columns.Add("Amount");
                        mergedDataTable.Columns.Add("Dep/FileDate");
                        mergedDataTable.Columns.Add("CheckDate");
                        mergedDataTable.Columns.Add("CheckNumber");
                        mergedDataTable.Columns.Add("SuspNo");
                        mergedDataTable.Columns.Add("ProvID");
                        mergedDataTable.Columns.Add("DocNo");
                        mergedDataTable.Columns.Add("DocType");
                        mergedDataTable.Columns.Add("DocDetail");
                        mergedDataTable.Columns.Add("Status");
                        mergedDataTable.Columns.Add("MatchStatus");
                        mergedDataTable.Columns.Add("WorkFlowStatus");
                        mergedDataTable.Columns.Add("OrigBank");
                        mergedDataTable.Columns.Add("OrigBankAcct");
                        mergedDataTable.Columns.Add("FileDt");
                        mergedDataTable.Columns.Add("PmtMethod");
                        mergedDataTable.Columns.Add("ExpectEPR");
                        mergedDataTable.Columns.Add("RemitFileID");
                        mergedDataTable.Columns.Add("Note");
                        mergedDataTable.Columns.Add("MatchUid");
                        mergedDataTable.Columns.Add("SvcType");
                        mergedDataTable.Columns.Add("CourierInstId");
                        mergedDataTable.Columns.Add("MapName");
                        mergedDataTable.Columns.Add("ActName");
                        mergedDataTable.Columns.Add("Assigned_To");
                        mergedDataTable.Columns.Add("Reason");
                        mergedDataTable.Columns.Add("EmbAcct");
                        mergedDataTable.Columns.Add("DataSource");
                        */
                    //its a check?
                    try
                    {
                       // cstatus = GetFlowareSearchData.Rows[SelectedRow]["cstatus"].ToString();
                        //cstatus is now status
                        depdate = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["dep/filedate"].ToString();
                        checkDate = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["checkdate"].ToString();
                        suspendNumber = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["suspno"].ToString();
                        expectEPR = ""; //not found in progress sql
                        origBank = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["origbank"].ToString();
                        origBankAccount = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["origbankacct"].ToString();
                        filedate = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["dep/filedate"].ToString();
                        note = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["note"].ToString();
                    }
                    catch { }
                    //is it a remit?
                    try
                    {
                        status = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["status"].ToString();
                       // origBank = GetFlowareSearchData.Rows[SelectedRow]["origbank"].ToString();
                       // origBankAccount = GetFlowareSearchData.Rows[SelectedRow]["origbankacct"].ToString();
                    }
                    catch
                    { //not a remit
                    }
                    string matchStatus = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["matchstatus"].ToString();
                    string workflowStatus = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["workflowstatus"].ToString(); //wfstatus
                    string transType = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["pmtmethod"].ToString();
                    string matchUser = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["matchuid"].ToString();
                    string serviceType = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["svctype"].ToString();//serviceid
                    //set the srcfileflag used by the CreateMatch 
                    string srcfileflag = "";
                    if(doctype == "RM")
                    {
                        srcfileflag = "R";
                    }
                    else if(doctype == "CK" || doctype == "CE")
                    {
                        srcfileflag = "C";
                    }
                    string courierinstid = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["courierinstid"].ToString();//serviceid
                    Model.MatchingChecks mc = new MatchingChecks(checknum, docno, checkamt, practice, depdate, docdet, doctype, "UNMATCHED",
                             matchID, externalPayor, note, checkDate, suspendNumber,
                             status, matchStatus, workflowStatus, expectEPR, origBank,
                             origBankAccount, filedate, matchUser, serviceType, status, key, srcfileflag, transType, pardocno, comingledstatus);
                    //fill the cart and set the count.
                    var results = (from table in Model.MatchingChecksCollection.MatchingChecksObservableCollection.AsEnumerable() where table.Key == mc.Key select table.Key).FirstOrDefault();
                    if (results == 0)
                    {
                        //determines the correct selected row in dataview even when rows are sorted
                        int? viewsIndex = GetFlowareSearchData.AsDataView().ToTable(false, new[] { "courierinstid" })
                                    .AsEnumerable()
                                    .Select(row => row.Field<string>("courierinstid")) // ie. project the col(s) needed
                                    .ToList()
                                    .FindIndex(col => col == courierinstid.ToString()); // returns 2
                        if (viewsIndex != null)
                        {
                            GetFlowareSearchData.Rows[viewsIndex.Value]["datasource"] = "CART";
                        }
                    
                        //Model.MatchingChecks mc = new MatchingChecks(checknum, docno, checkamt, practice, depdate, docdet, doctype, "UNMATCHED");
                        Model.MatchingChecksCollection.MatchingChecksObservableCollection.Add(mc);
                        Model.MatchingChecksCollection.CartCount = Model.MatchingChecksCollection.MatchingChecksObservableCollection.Count;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error AddToCart : " + ex.Message);
            }
        }
        /// <summary>
        /// Execute Search and return from four sources.
        /// Floware
        /// Embillz
        /// Assembly
        /// IDM
        /// updates the ViewModels DataTables which are bound to the View.
        /// </summary>
        /// <param name="SearchBy"></param>
        /// <param name="SearchValue"></param>
        /// <returns></returns>
        public DataTable ReturnSearchIDM(string SearchBy, string SearchValue)
        {

            getIDMSearchData = new DataTable("IDM");
            Model.GetSearchData searchData = new Model.GetSearchData();
            getIDMSearchData = searchData.SearchIDM(SearchBy, SearchValue);
            
            return getIDMSearchData;
        }
        public DataTable ReturnSearchEmbillz(string SearchBy, string SearchValue)
        {
            getEmbillzSearchData = new DataTable("Embillz");
            Model.GetSearchData searchData = new Model.GetSearchData();
            getEmbillzSearchData = searchData.SearchEmbillz(SearchBy, SearchValue);
            
            return getEmbillzSearchData;
        }
        public DataTable ReturnSearchAssembly(string SearchBy, string SearchValue)
        {
            getAssemblySearchData = new DataTable("Assembly");
            Model.GetSearchData searchData = new Model.GetSearchData();
            getAssemblySearchData = searchData.SearchAssembly(SearchBy, SearchValue);
            return getAssemblySearchData;
        }
        /// <summary>
        /// Mark Lane
        /// Save the users custom orderization to the Settings.
        /// 2/17/2015
        /// </summary>
        private void OnSaveColumns()
        {
           // Model.OrderDataGridColumns orderDataGridColumns = new OrderDataGridColumns();
           // orderDataGridColumns.SaveOrder(GetFlowareSearchData.DefaultView.ToTable());
           
        }
        private void OnClearColumns()
        {
            Properties.Settings.Default["SaveColumnOrder"] = "";
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Remove duplicate records from data table
        /// </summary>
        /// <param name="table">DataTable for removing duplicate records</param>
        /// <param name="DistinctColumn">Column to check for duplicate values or records</param>
        /// <returns></returns>
        public DataTable RemoveDuplicateRows(DataTable table, string DistinctColumn)
        {
            try
            {
                List<DataRow> UniqueRecords = new List<DataRow>();
                List<DataRow> DuplicateRecords = new List<DataRow>();
                // Check if records is already added to UniqueRecords otherwise,
                // Add the records to DuplicateRecords
                foreach (DataRow dRow in table.Rows)
                {
                    if (UniqueRecords.Contains(dRow[DistinctColumn]))
                        DuplicateRecords.Add(dRow);
                    else
                        UniqueRecords.Add(dRow);
                }

                // Remove dupliate rows from DataTable added to DuplicateRecords
                foreach (DataRow dRow in DuplicateRecords)
                {
                    table.Rows.Remove(dRow);
                }

                // Return the clean DataTable which contains unique records.
                return table;
            }
            catch (Exception ex)
            {
                return table;
            }
        }
        /// <summary>
        /// Mark Lane
        /// due to background worker check to see if you can jump back
        /// on the main UI thread via beginInvoke.
        /// </summary>
        /// <param name="StartRow"></param>
        /// <param name="EndRow"></param>
        private void ReturnMoreFlowareWithC2PR2P(int StartRow, int EndRow)
        {

                        ShouldSearchContinue = false;
                        //what you want to do...
                        DataTable dtCashRemit = new DataTable("outc2p");

                        PrepareSearch();
                        using (var docManagerServiceClient = new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME))
                        {
                            string dt1 = null;
                            string dt2 = null;
                            if (IsDepDateEnabled == true)
                            {
                                dt1 = DepositDateFrom.ToString("yyyyMMdd");
                                dt2 = DepositDateTo.ToString("yyyyMMdd");
                            }
                            else
                            {
                                dt1 = "";
                                dt2 = "";
                            }

                            dtCashRemit = docManagerServiceClient.GetPostBdWithC2PR2PDataMerged(Environment.MachineName, Environment.UserName,
                                    CurrentMapName, CurrentActivityName,
                                    Practice, Division, dt1, dt2, Embacct,
                                    Reason, Checknum, MinCheckamt, MaxCheckamt, MinPaidamt, MaxPaidamt,
                                    DocType, DocDet, Assigned_To, Docno, Pardocno, IsMatched, StartRow, EndRow);
                        }

                        Dispatcher dispatcher = Application.Current.Dispatcher;

                        if (!dispatcher.CheckAccess())
                        {
                            dispatcher.BeginInvoke((Action)(() =>
                            {
                                //order by docno, keep it civil
                                if (dtCashRemit.Rows.Count > 0)
                                {
                                    Model.OrderDataGridColumns orderDataGridColumns = new OrderDataGridColumns();
                                    if (Properties.Settings.Default["SaveColumnOrder"].ToString() != "" && Properties.Settings.Default["SaveColumnOrder"] != null)
                                    {
                                        GetFlowareSearchData.Merge(orderDataGridColumns.Reorder(dtCashRemit));
                                    }
                                    else
                                    {
                                        GetFlowareSearchData.Merge(dtCashRemit.AsEnumerable().OrderBy(c => c.Field<string>("DocNo")).CopyToDataTable());
                                        GetFlowareSearchData.Columns["Client"].SetOrdinal(0);
                                        GetFlowareSearchData.Columns["Div"].SetOrdinal(1);
                                        GetFlowareSearchData.Columns["DocNo"].SetOrdinal(2);
                                        GetFlowareSearchData.Columns["ExternalPayor"].SetOrdinal(3);
                                        GetFlowareSearchData.Columns["DocType"].SetOrdinal(4);
                                        GetFlowareSearchData.Columns["DocDetail"].SetOrdinal(5);
                                        GetFlowareSearchData.Columns["Dep/FileDate"].SetOrdinal(6);
                                        GetFlowareSearchData.Columns["CheckNumber"].SetOrdinal(7);
                                        GetFlowareSearchData.Columns["Amount"].SetOrdinal(8);
                                        GetFlowareSearchData.Columns["MatchStatus"].SetOrdinal(9);
                                        GetFlowareSearchData.Columns["Status"].SetOrdinal(10);
                                        GetFlowareSearchData.Columns["PmtMethod"].SetOrdinal(11);
                                        GetFlowareSearchData.Columns["FileDt"].SetOrdinal(12);
                                        GetFlowareSearchData.Columns["MatchId"].SetOrdinal(13);
                                        GetFlowareSearchData.Columns["EmbAcct"].SetOrdinal(14);
                                        GetFlowareSearchData.Columns["Reason"].SetOrdinal(15);
                                        GetFlowareSearchData.Columns["MapName"].SetOrdinal(16);
                                        GetFlowareSearchData.Columns["ActName"].SetOrdinal(17);
                                        // GetFlowareSearchData.Columns["Notes"].Vi TODO hide.
                                    }
                                    ShouldSearchContinue = true;
                                    //string MaxRows = GetFlowareSearchData.Rows[0]["TotalRows"].ToString();
                                    FlowareResults = "Workflow Results Truncated " + GetFlowareSearchData.Rows.Count + " Rows";
                                
                                }
                                else
                                {
                                    ShouldSearchContinue = false;
                                    
                                    FlowareResults = "Workflow Results Total Found " + GetFlowareSearchData.Rows.Count + " Rows";
                                
                                }
                                GetFlowareSearchData = GetFlowareSearchData.DefaultView.ToTable(true);
                                int totalRows = GetFlowareSearchData.Rows.Count;
                                SearchResults = "Search Results (Total Hits = " + totalRows + ")";
                                if (totalRows <= 0) { ShowFlowareData = false; } else { ShowFlowareData = true; }

                       
                    }));
                }
                else
                {
                    //order by docno, keep it civil
                    if (dtCashRemit.Rows.Count > 0)
                    {
                        Model.OrderDataGridColumns orderDataGridColumns = new OrderDataGridColumns();
                        if (Properties.Settings.Default["SaveColumnOrder"].ToString() != "" && Properties.Settings.Default["SaveColumnOrder"] != null)
                        {
                            GetFlowareSearchData.Merge(orderDataGridColumns.Reorder(dtCashRemit));
                        }
                        else
                        {
                            GetFlowareSearchData.Merge(dtCashRemit.AsEnumerable().OrderBy(c => c.Field<string>("DocNo")).CopyToDataTable());
                            GetFlowareSearchData.Columns["Client"].SetOrdinal(0);
                            GetFlowareSearchData.Columns["Div"].SetOrdinal(1);
                            GetFlowareSearchData.Columns["DocNo"].SetOrdinal(2);
                            GetFlowareSearchData.Columns["ExternalPayor"].SetOrdinal(3);
                            GetFlowareSearchData.Columns["DocType"].SetOrdinal(4);
                            GetFlowareSearchData.Columns["DocDetail"].SetOrdinal(5);
                            GetFlowareSearchData.Columns["Dep/FileDate"].SetOrdinal(6);
                            GetFlowareSearchData.Columns["CheckNumber"].SetOrdinal(7);
                            GetFlowareSearchData.Columns["Amount"].SetOrdinal(8);
                            GetFlowareSearchData.Columns["MatchStatus"].SetOrdinal(9);
                            GetFlowareSearchData.Columns["Status"].SetOrdinal(10);
                            GetFlowareSearchData.Columns["PmtMethod"].SetOrdinal(11);
                            GetFlowareSearchData.Columns["FileDt"].SetOrdinal(12);
                            GetFlowareSearchData.Columns["MatchId"].SetOrdinal(13);
                            GetFlowareSearchData.Columns["EmbAcct"].SetOrdinal(14);
                            GetFlowareSearchData.Columns["Reason"].SetOrdinal(15);
                            GetFlowareSearchData.Columns["MapName"].SetOrdinal(16);
                            GetFlowareSearchData.Columns["ActName"].SetOrdinal(17);
                            // GetFlowareSearchData.Columns["Notes"].Vi TODO hide.
                        }
                        ShouldSearchContinue = true;
                        
                        FlowareResults = "Workflow Results Truncated " + GetFlowareSearchData.Rows.Count + " Rows";
                    
                    }
                    else
                    {
                        ShouldSearchContinue = false;
                        
                        FlowareResults = "Workflow Results Total Found " + GetFlowareSearchData.Rows.Count + " Rows";
                    
                    }
                    GetFlowareSearchData = GetFlowareSearchData.DefaultView.ToTable(true);
                    int totalRows = GetFlowareSearchData.Rows.Count;
                    SearchResults = "Search Results (Total Hits = " + totalRows + ")";
                    if (totalRows <= 0) { ShowFlowareData = false; } else { ShowFlowareData = true; }

                }
           
        }

        private void ReturnSearchFlowareWithC2PR2P(int StartRow, int EndRow)
        {

          
            DataTable dtCashRemit = new DataTable("outc2p");
            
            PrepareSearch();
            using (var docManagerServiceClient = new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME))
            {
                string dt1 = null;
                string dt2 = null;
                if (IsDepDateEnabled == true)
                {
                    dt1 = DepositDateFrom.ToString("yyyyMMdd");
                    dt2 = DepositDateTo.ToString("yyyyMMdd");
                }
                else
                {
                    dt1 = "";
                    dt2 = "";
                }

                dtCashRemit = docManagerServiceClient.GetPostBdWithC2PR2PDataMerged(Environment.MachineName, Environment.UserName,
                        CurrentMapName, CurrentActivityName,
                        Practice, Division, dt1, dt2, Embacct,
                        Reason, Checknum, MinCheckamt, MaxCheckamt, MinPaidamt, MaxPaidamt,
                        DocType, DocDet, Assigned_To, Docno, Pardocno, IsMatched, StartRow, EndRow);
            }

            //order by docno, keep it civil
            if (dtCashRemit.Rows.Count > 0)
            {
                Model.OrderDataGridColumns orderDataGridColumns = new OrderDataGridColumns();
                if (Properties.Settings.Default["SaveColumnOrder"].ToString() != "" && Properties.Settings.Default["SaveColumnOrder"] != null)
                {
                    GetFlowareSearchData = orderDataGridColumns.Reorder(dtCashRemit);
                }
                else
                {
                    GetFlowareSearchData = dtCashRemit.AsEnumerable().OrderBy(c => c.Field<string>("DocNo")).CopyToDataTable();
                    GetFlowareSearchData.Columns["Client"].SetOrdinal(0);
                    GetFlowareSearchData.Columns["Div"].SetOrdinal(1);
                    GetFlowareSearchData.Columns["DocNo"].SetOrdinal(2);
                    GetFlowareSearchData.Columns["ExternalPayor"].SetOrdinal(3);
                    GetFlowareSearchData.Columns["DocType"].SetOrdinal(4);
                    GetFlowareSearchData.Columns["DocDetail"].SetOrdinal(5);
                    GetFlowareSearchData.Columns["Dep/FileDate"].SetOrdinal(6);
                    GetFlowareSearchData.Columns["CheckNumber"].SetOrdinal(7);
                    GetFlowareSearchData.Columns["Amount"].SetOrdinal(8);
                    GetFlowareSearchData.Columns["MatchStatus"].SetOrdinal(9);
                    GetFlowareSearchData.Columns["Status"].SetOrdinal(10);
                    GetFlowareSearchData.Columns["PmtMethod"].SetOrdinal(11);
                    GetFlowareSearchData.Columns["FileDt"].SetOrdinal(12);
                    GetFlowareSearchData.Columns["MatchId"].SetOrdinal(13);
                    GetFlowareSearchData.Columns["EmbAcct"].SetOrdinal(14);
                    GetFlowareSearchData.Columns["Reason"].SetOrdinal(15);
                    GetFlowareSearchData.Columns["MapName"].SetOrdinal(16);
                    GetFlowareSearchData.Columns["ActName"].SetOrdinal(17);
                   // GetFlowareSearchData.Columns["Notes"].Vi TODO hide.
                }
            }
            else
            {
                GetFlowareSearchData = dtCashRemit;
            }
        }
        private void ReturnMoreFloware(int StartRow, int EndRow)
        {
                        ShouldSearchContinue = false;

                        getFlowareSearchData = new DataTable("Floware");
                        PrepareSearch();
                        using (var docManagerServiceClient = new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME))
                        {

                            string dt1 = null;
                            string dt2 = null;
                            if (IsDepDateEnabled == true)
                            {
                                dt1 = DepositDateFrom.ToString("yyyyMMdd");
                                dt2 = DepositDateTo.ToString("yyyyMMdd");
                            }
                            else
                            {
                                dt1 = "";
                                dt2 = "";
                            }

                            getFlowareSearchData = docManagerServiceClient.SearchPostBd(Environment.MachineName, Environment.UserName,
                                CurrentMapName, CurrentActivityName,
                                Practice, Division, dt1, dt2, Embacct,
                                Reason, Checknum, MinCheckamt, MaxCheckamt, MinPaidamt, MaxPaidamt,
                                DocType, DocDet, Assigned_To, Docno, Pardocno, StartRow, EndRow).DefaultView.ToTable();
                        } 

                //add back to main thread
                Dispatcher dispatcher = Application.Current.Dispatcher;
                        
                if (!dispatcher.CheckAccess())
                {
                    dispatcher.BeginInvoke((Action)(() =>
                    {
                            GetFlowareSearchData.Merge(getFlowareSearchData);
                            GetFlowareSearchData = GetFlowareSearchData.DefaultView.ToTable(true);

                    }));
                }
                else
                {
                            GetFlowareSearchData.Merge(getFlowareSearchData);
                            GetFlowareSearchData = GetFlowareSearchData.DefaultView.ToTable(true);
                }
                ShouldSearchContinue = true;

        }
        private void ReturnSearchFloware(int StartRow, int EndRow)
        {
            //getFlowareSearchData = new DataTable("Floware");
            PrepareSearch();
            using (var docManagerServiceClient = new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME))
            {

                string dt1 = null;
                string dt2 = null;
                if (IsDepDateEnabled == true)
                {
                    dt1 = DepositDateFrom.ToString("yyyyMMdd");
                    dt2 = DepositDateTo.ToString("yyyyMMdd");
                }
                else
                {
                    dt1 = "";
                    dt2 = "";
                }

                GetFlowareSearchData = docManagerServiceClient.SearchPostBd(Environment.MachineName, Environment.UserName,
                    CurrentMapName, CurrentActivityName,
                    Practice, Division, dt1, dt2, Embacct,
                    Reason, Checknum, MinCheckamt, MaxCheckamt, MinPaidamt, MaxPaidamt,
                    DocType, DocDet, Assigned_To, Docno, Pardocno, StartRow, EndRow).DefaultView.ToTable();

                //var results = from a in getFlowareSearchData.AsEnumerable()
                //              where a.Field<string>("ACT_NAME") == "RTI Manual Posting" &&
                //                              a.Field<string>("PRACTICE") == "CCM" &&
                //                              a.Field<string>("DEPDT").CompareTo("2004-01-14 00:00:00") >= 0 && a.Field<string>("DEPDT").CompareTo("2010-01-14 00:00:00") <= 0
                //                            select a;

            }
           
        }

        /// <summary>
        /// Mark Lane
        /// need to clear previous set search type values because the search type is dynamic!!!
        /// </summary>
        /// <param name="searchType"></param>
        private void ClearSearchValues(string searchType)
        {
            try
            {
                switch (searchType)
                {
                    case "Practice":
                        Practice = "";
                        break;
                    case "Division":
                        Division = "";
                        break;
                    case "Min Checkamt":
                        MinCheckamt = "";
                        break;
                    case "Max Checkamt":
                        MaxCheckamt = "";
                        break;
                    case "Checknum":
                        MinCheckamt = "";
                        break;
                    case "Embacct":
                        Embacct = "";
                        break;
                    case "Reason":
                        Reason = "";
                        break;
                    case "Docdet":
                        DocDet = "";
                        break;
                    case "Doctype":
                        DocType = "";
                        break;
                    case "MatchStatus":
                        MatchStatus = "";
                        break;
                    case "Docno":
                        Docno = "";
                        break;
                    case "Pardocno":
                        Pardocno = "";
                        break;
                    case "Min Paidamt":
                        MinPaidamt = "";
                        break;
                    case "Max Paidamt":
                        MaxPaidamt = "";
                        break;
                    case "Assigned_To":
                        Assigned_To = "";
                        break;
                    default:
                        break;
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            
        }
        /// <summary>
        /// Author Mark Lane
        /// right now we are limiting them to six search types but this could change.
        /// </summary>
        private void PrepareSearch()
        {
            //clear previous
            ClearBusinessData();
 
            //set the optional params
            SetSearchValues(SearchTypeOne, SearchValueOne);
            SetSearchValues(SearchTypeTwo, SearchValueTwo);
            SetSearchValues(SearchTypeThree, SearchValueThree);
            SetSearchValues(SearchTypeFour, SearchValueFour);
            SetSearchValues(SearchTypeFive, SearchValueFive);
            SetSearchValues(SearchTypeSix, SearchValueSix);
        }
        /// <summary>
        /// Mark Lane
        /// Set the values for our search
        /// </summary>
        ///  
        /// <param name="searchType"></param>
        /// <param name="searchValue"></param>
        private void SetSearchValues(string searchType, string searchValue)
        {
            switch (searchType)
            {
                case "Practice":
                    Practice = searchValue;
                    break;
                case "Division":
                    Division = searchValue;
                    break;
                case "Min Checkamt":
                    MinCheckamt = searchValue;
                    break;
                case "Max Checkamt":
                    MaxCheckamt = searchValue;
                    break;
                case "Checknum":
                    MinCheckamt = searchValue;
                    break;
                case "Embacct":
                    Embacct = searchValue;
                    break;
                case "Reason":
                    Reason = searchValue;
                    break;
                case "Docdet":
                    DocDet = searchValue;
                    break;
                case "Doctype":
                    DocType = searchValue;
                    break;
                case "MatchStatus":
                    MatchStatus = searchValue;
                    break;
                case "Docno":
                    Docno = searchValue;
                    break;
                case "Pardocno":
                    Pardocno = searchValue;
                    break;
                case "Min Paidamt":
                    MinPaidamt = searchValue;
                    break;
                case "Max Paidamt":
                    MaxPaidamt = searchValue;
                    break;
                case "Assigned_To":
                    Assigned_To = searchValue;
                    break;
                default:
                    break;
            }
        }
                  
      
        #endregion
        #region SearchCriteria parameters
        public DateTime DepositDateFrom
        {
            get { return depositdateFrom; }
            set
            {
                depositdateFrom = value;
                OnPropertyChanged("DepositDateFrom");
            }
        }
        public DateTime DepositDateTo
        {
            get { return depositdateTo; }
            set
            {
                depositdateTo = value;
                OnPropertyChanged("DepositDateTo");
            }
        }
        public string CurrentMapName
        {
            get { return currentMapName;}
            set 
            { 
                currentMapName = value;
                
                GetUsersActivityList(currentMapName);
                OnPropertyChanged("CurrentMapName");
            }
        }
        public string CurrentActivityName
        {
            get { return currentActivityName;}
            set 
            { 
                currentActivityName = value;
                OnPropertyChanged("CurrentActivityName");
            }
        }
        public string Docno
        {
            get { return docno; }
            set { docno = value; }
        }
        private string MatchStatus
        {
            get { return matchStatus; }
            set { matchStatus = value; }
        }
        private string Pardocno
        {
            get { return pardocno; }
            set { pardocno = value; }
        }
        private string DocDet
        {
            get { return docDet; }
            set { docDet = value; }
        }
        private string DocType
        {
            get { return docType; }
            set { docType = value; }
        }
        private string Depdt
        {
            get { return depdt; }
            set { depdt = value; }
        }
        private string Checknum
        {
            get { return checknum; }
            set { checknum = value; }
        }
        private string Checkamt
        {
            get { return checkamt; }
            set { checkamt = value; }
        }
        private string MinCheckamt
        {
            get { return minCheckamt; }
            set { minCheckamt = value; }
        }
        private string MaxCheckamt
        {
            get { return maxCheckamt; }
            set { maxCheckamt = value; }
        }
        private string Paidamt
        {
            get { return paidamt; }
            set { paidamt = value; }
        }
        private string MinPaidamt
        {
            get { return minPaidamt; }
            set { minPaidamt = value; }
        }
        private string MaxPaidamt
        {
            get { return maxPaidamt; }
            set { maxPaidamt = value; }
        }
        private string Embacct
        {
            get { return embacct; }
            set { embacct = value; }
        }
        private string Practice
        {
            get { return practice; }
            set { practice = value; }
        }
        private string Division
        {
            get { return division; }
            set { division = value; }
        }
        private string Assigned_To
        {
            get { return assigned_To; }
            set { assigned_To = value; }
        }
        private string Reason
        {
            get { return reason; }
            set { reason = value; }
        }
        #endregion


        
    }
}
