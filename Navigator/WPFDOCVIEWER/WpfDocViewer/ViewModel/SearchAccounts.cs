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
    public class SearchAccounts : INotifyPropertyChanged 
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        ObservableCollection<string> retrievedItems;
        private RelayCommand searchCommand;
        RelayCommand openViewer;
        RelayCommand openIDMViewer;
        private string searchValue;
        private string searchValue2;
        private string searchType;
        private string searchType2;
        ObservableCollection<Model.ImportDataModel.DocumentType> _documentTypes = new ObservableCollection<Model.ImportDataModel.DocumentType>();
        ObservableCollection<Model.ImportDataModel.DocumentDetail> _documentDetails = new ObservableCollection<Model.ImportDataModel.DocumentDetail>();
        List<string> documentTypes = new List<string>();
        List<string> documentDetails = new List<string>();
        private List<string> searchList1 = new List<string>();
        private List<string> searchList2 = new List<string>();
        private bool isFilterChecked = false;
        private bool isWorkFlowFilterChecked = false;
        private bool isIDMFilterChecked = false;
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
        string selectedPractice;
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
        private int selectedEIQRow;
        private string selectedIDMItem;
        private string selectedFlowareItem;
        private bool isSearchChecksEnabled = false;
        bool shouldSearchContinue = false;
        private Visibility isDoctypeSelected = Visibility.Hidden;
        private List<string> comboBoxSearchBy = new List<string>();
        private readonly BackgroundWorker backgroundWorker = new BackgroundWorker();
        private readonly BackgroundWorker getMoreSearchData = new BackgroundWorker();

        public SearchAccounts()
        {
            this.SearchCommand = new RelayCommand(OnSearch);
            this.SearchMore = new RelayCommand(OnSearchMore);
            this.OpenViewer = new RelayCommand(OnView);
            this.OpenIDMViewer = new RelayCommand(OnIDMView);
            this.OpenEIQViewer = new RelayCommand(OnEIQView);
            this.AddToCart = new RelayCommand(OnAddToCart);
            this.AddToIDMCart = new RelayCommand(OnAddIDMToCart);
            this.AddToEIQCart = new RelayCommand(OnAddToEIQCart);
            comboBoxSearchBy.Add("");
            comboBoxSearchBy.Add("Docdet");
            getIDMSearchData = new DataTable("IDM");
            getEmbillzSearchData = new DataTable("Embillz");
            getFlowareSearchData = new DataTable("Floware");
            this.ClearColumns = new RelayCommand(OnClearColumns);
            SearchValue = App.Current.FindResource("SearchValue").ToString();//dynamic resource filled by import
            getDocumentTypes();
            getDocumentDetails();
            documentTypes = _documentTypes.AsEnumerable().Select(abc => abc.key).ToList();
            documentDetails = _documentDetails.AsEnumerable().Select(abc => abc.detailkey).ToList();
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            getMoreSearchData.DoWork += getMoreSearchData_DoWork;
            getMoreSearchData.RunWorkerCompleted += getMoreSearchData_RunWorkerCompleted;
        }

        public void OnSearchMore()
        {
            if (ShouldSearchContinue == true)
            {
                if (getMoreSearchData.IsBusy == false)
                {
                    if (GetEmbillzSearchData.Rows.Count == 500 || GetEmbillzSearchData.Rows.Count == 1000
                        || GetEmbillzSearchData.Rows.Count == 756)
                    {
                        shouldSearchContinue = false;
                        getMoreSearchData.RunWorkerAsync();
                    }
                }
            }
        }
        /// <summary>
        /// Mark Lane run after worker is done
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void getMoreSearchData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            //Mouse.OverrideCursor = Cursors.Arrow;
        }
        /// <summary>
        /// Mark Lane
        /// use this for get more search data. as the user moves down.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void getMoreSearchData_DoWork(object sender, DoWorkEventArgs e)
        {
            ReturnMoreEmbillzData();
            UpdateViewTableHeaders();
            ShouldSearchContinue = true;
        }

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            PrepareSearch();

            try
            {
                if ((SearchType == null) || (SearchValue == null))
                {
                    SearchType = App.Current.FindResource("SearchType").ToString();
                    SearchValue = App.Current.FindResource("SearchValue").ToString();
                }

                //use SelectedItem if you wish to use selected item locally.
                ReturnSearchIDM(SearchType, SearchValue);
                ReturnSearchFloware(SearchType, SearchValue);
                ReturnSearchEmbillz(SearchType, SearchValue);
                //set the headers

                UpdateViewTableHeaders();
            }
            catch (Exception ex)
            {
                //throw 
                MessageBox.Show("Error OnSearch : " + ex.Message);
            }
            //Mouse.OverrideCursor = null;
           
        }
        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ShouldSearchContinue = true;
            Mouse.OverrideCursor = Cursors.Arrow;
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
        public RelayCommand SearchCommand
        {
            get;
            private set;

        }
        public RelayCommand SearchMore
        {
            get;
            private set;
        }
        public RelayCommand ClearColumns
        {
            get;
            set;
        }
        public bool IsFilterChecked
        {
            get 
            {
                return isFilterChecked; 
            }
            set {
                isFilterChecked = value;
                OnPropertyChanged("IsFilterChecked");
                }
        }
        public bool IsWorkFlowFilterChecked
        {
            get
            {
                return isWorkFlowFilterChecked;
            }
            set
            {
                isWorkFlowFilterChecked = value;
                OnPropertyChanged("IsWorkFlowFilterChecked");
            }
        }
        public bool ShouldSearchContinue
        {
            get { return shouldSearchContinue; }
            set { shouldSearchContinue = value;
            OnPropertyChanged("ShouldSearchContinue");
            }
        }
        public bool IsIDMFilterChecked
        {
            get
            {
                return isIDMFilterChecked;
            }
            set
            {
                isIDMFilterChecked = value;
                OnPropertyChanged("IsIDMFilterChecked");
            }
        }
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
        public RelayCommand OpenEIQViewer
        {
            get;
            set;
        }
        public RelayCommand OpenIDMViewer
        {
            get;
            set;
        }
        public RelayCommand AddToCart
        {
            get;
            set;
        }
        public RelayCommand AddToIDMCart
        {
            get;
            set;
        }
        public RelayCommand AddToEIQCart
        {
            get;
            set;
        }
        #endregion
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
                OnPropertyChanged("SelectedIDMItem");
                try
                {
                    if (selectedIDMItem != null)
                    {
                        ResourceDictionary resources = App.Current.Resources; // If in a Window/UserControl/etc
                        string ifn = GetIDMSearchData.DefaultView.ToTable().Rows[SelectedIDMRow]["ifn"].ToString();
                        string docno = GetIDMSearchData.DefaultView.ToTable().Rows[SelectedIDMRow]["docno"].ToString();
                        resources["ExportIFN"] = ifn;
                        resources["ExportDocno"] = docno;

                    }
                }
                catch(Exception ex)
                {
                    //row index out of bounds.
                }
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
                OnPropertyChanged("SelectedFlowareItem");
                try
                {
                    ResourceDictionary resources = App.Current.Resources; // If in a Window/UserControl/etc
                    if ((getFlowareSearchData != null) && (SelectedRow != -1))
                    {
                        string ifn = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["ifn"].ToString();
                        string docno = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["docno"].ToString();
                        resources["ExportIFN"] = ifn;
                        resources["ExportDocno"] = docno;

                    }
                }
                catch(Exception ex)
                {
                    //selected row is not set yet.
                }
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

        private void OnClearColumns()
        {
            Properties.Settings.Default["SaveEmbillzColumnOrder"] = "";
            Properties.Settings.Default["SaveNavC2PColumnOrder"] = "";
            Properties.Settings.Default.Save();
        }
        
        ///
        ///
        ///Search Command raised event to fill datagrids with bound tables.
        ///Fill IDM, Assembly and Floware search.
        ///Search uses the updated Xaml.Resources and selected Dropdown search.
        ///
        ///
        public void OnSearch()
        {

            Mouse.OverrideCursor = Cursors.Wait;
            backgroundWorker.RunWorkerAsync();
           
        }
        private void UpdateViewTableHeaders()
        {
                int totalRows = 0;
                IDMResults = "IDM Results Found " + GetIDMSearchData.Rows.Count + " Rows";
                FlowareResults = "Workflow Results Found " + GetFlowareSearchData.Rows.Count + " Rows";
                EmbillzResults = "Embillz Results Found " + GetEmbillzSearchData.Rows.Count + " Rows";
                totalRows = GetIDMSearchData.Rows.Count + GetFlowareSearchData.Rows.Count + GetEmbillzSearchData.Rows.Count;
                SearchResults = "Search Results (Total Hits = " + totalRows + ")";
                ShowResults = true;
                if (totalRows <= 0) ShowResults = false;
                //expand the expanders
                //if data greater than 0 open the expander
                ShowEmbillzData = false;
                if (getEmbillzSearchData.Rows.Count > 0) ShowEmbillzData = true;
                ShowFlowareData = false;
                if (getFlowareSearchData.Rows.Count > 0) ShowFlowareData = true;
                ShowIDMData = false;
                if (getIDMSearchData.Rows.Count > 0) ShowIDMData = true;
        }
        ///
        ///Author: Mark Lane
        ///Using this function to add to the static class for Assembly Check/Cash Matching control
        ///
        ///
        public void OnAddToCart()
        {
            //no embillz data from IDM or Floware in this search
            /*
            int totalRows = 0;
            try
            {

                if (SelectedRow >= 0)
                {
                    string checknum = GetFlowareSearchData.Rows[SelectedRow]["checknum"].ToString();
                    
                    double checkamt = Convert.ToDouble(GetFlowareSearchData.Rows[SelectedRow]["checkamt"].ToString());
                    string practice = GetFlowareSearchData.Rows[SelectedRow]["practice"].ToString();
                    
                    string docno = GetFlowareSearchData.Rows[SelectedRow]["docno"].ToString();
                    string depdate = GetFlowareSearchData.Rows[SelectedRow]["depdt"].ToString();
                    string doctype = GetFlowareSearchData.Rows[SelectedRow]["doctype"].ToString();
                    string docdet = GetFlowareSearchData.Rows[SelectedRow]["docdet"].ToString();

                    int matchID = Convert.ToInt32(GetFlowareSearchData.Rows[SelectedRow]["matchid"].ToString());
                    string externalPayor = GetFlowareSearchData.Rows[SelectedRow]["extpaycd"].ToString();
                    string note = GetFlowareSearchData.Rows[SelectedRow]["note"].ToString();
                    string checkDate = GetFlowareSearchData.Rows[SelectedRow]["checkdate"].ToString();
                    string suspendNumber = GetFlowareSearchData.Rows[SelectedRow]["suspdno"].ToString();
                    string status = GetFlowareSearchData.Rows[SelectedRow]["cstatus"].ToString();
                    string matchStatus = GetFlowareSearchData.Rows[SelectedRow]["matchstatus"].ToString();
                    string workflowStatus = GetFlowareSearchData.Rows[SelectedRow]["workflowstatus"].ToString();
                    string expectEPR = GetFlowareSearchData.Rows[SelectedRow]["expepr"].ToString();
                    string origBank = GetFlowareSearchData.Rows[SelectedRow]["origbankcd"].ToString();
                    string origBankAccount = GetFlowareSearchData.Rows[SelectedRow]["origbankacct"].ToString();
                    string filedate = GetFlowareSearchData.Rows[SelectedRow]["filedate"].ToString();
                    string matchUser = GetFlowareSearchData.Rows[SelectedRow]["matchuid"].ToString();
                    string serviceType = GetFlowareSearchData.Rows[SelectedRow]["serviceid"].ToString();

                    Model.MatchingChecks mc = new MatchingChecks(checknum, docno, checkamt, practice, depdate, docdet, doctype, "UNMATCHED",
                            matchID, externalPayor, note, checkDate, suspendNumber,
                             status, matchStatus, workflowStatus, expectEPR, origBank,
                             origBankAccount, filedate, matchUser, serviceType, "");
                    //fill the cart and set the count.
                    //Model.MatchingChecks mc = new MatchingChecks(checknum, docno, checkamt, practice, depdate, docdet, doctype, "UNMATCHED");
                    Model.MatchingChecksCollection.MatchingChecksObservableCollection.Add(mc);
                    Model.MatchingChecksCollection.CartCount = Model.MatchingChecksCollection.MatchingChecksObservableCollection.Count;
                }
            }
            catch (Exception ex)
            {
                //throw 
            }
             * */
        }
        public void OnAddToEIQCart()
        {
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
            string checknum;
            double checkamt;
            string practice;
            string docno;
            string pardocno;
            string doctype;
            string docdet;
            int matchID;
            string externalPayor;
            string note;
            int key;
            string transType;
            string matchStatus;
            string workflowStatus;
            string matchUser;
            string serviceType;
            string comingledstatus = "";

            try
            {

                if (SelectedEIQRow >= 0)
                {
                    //special case to check for PostBd courier first before adding to cart.
                    docno = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["docno"].ToString();
                    string finddoc = (from table in GetFlowareSearchData.AsEnumerable() where table.Field<string>("docno") == docno select table.Field<string>("Act_Name")).FirstOrDefault();
                    if (finddoc  != null)
                    {
                        if (MessageBox.Show("Docno " + docno + " already has a courier in workflow. " + "\n" + " in Activity : " + finddoc + "\n" + " Do you wish to continue and Match this item?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.No)
                        {
                            return;
                        }
                    }
                    if (GetEmbillzSearchData.Columns.Contains("client"))
                    {
                        checknum = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["checknum"].ToString();

                        checkamt = Convert.ToDouble(GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["checkamt"].ToString());
                        practice = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["client"].ToString();

                        
                        pardocno = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["docno"].ToString();
                        doctype = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["doctype"].ToString();
                        docdet = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["docdet"].ToString();

                        matchID = Convert.ToInt32(GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["matchid"].ToString());
                        externalPayor = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["extpaycd"].ToString();
                        note = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["noteflag"].ToString();
                        key = Convert.ToInt32(GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["key"].ToString());
                        transType = ""; //not found in c2p or r2p
                         //its a check?
                        try
                        {
                            cstatus = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["cstatus"].ToString();
                            depdate = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["depdt"].ToString();
                            checkDate = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["checkdt"].ToString();
                            suspendNumber = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["suspendno"].ToString();
                            expectEPR = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["expectepr"].ToString();
                            origBank = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["origcd"].ToString();
                            origBankAccount = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["origno"].ToString();
                            filedate = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["filedate"].ToString();
                            comingledstatus = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["comingledstat"].ToString();
                       
                        }
                        catch { }
                        //is it a remit?
                        try
                        {
                            rstatus = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["rstatus"].ToString();
                            origBank = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["origcd"].ToString();
                            origBankAccount = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["origno"].ToString();
                            comingledstatus = "";
                        }
                        catch
                        { //not a remit
                        }
                        matchStatus = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["matchstat"].ToString();
                        workflowStatus = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["wfstatus"].ToString();



                        matchUser = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["matchuid"].ToString();
                        serviceType = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["svctype"].ToString();
                    }
                    else
                    {
                        //its from NavC2P
                        checknum = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["checknum"].ToString();

                        checkamt = Convert.ToDouble(GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["checkamt"].ToString());
                        practice = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["practice"].ToString();

                        docno = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["docno"].ToString();
                        pardocno = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["pardocno"].ToString();
                        doctype = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["doctype"].ToString();
                        docdet = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["docdet"].ToString();

                        matchID = Convert.ToInt32(GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["matchid"].ToString());
                        externalPayor = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["extpaycd"].ToString();
                         note = "";
                         key = Convert.ToInt32(GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["key"].ToString());
                         

                        //its a check?
                        try
                        {
                            cstatus = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["cstatus"].ToString();
                            depdate = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["depdt"].ToString();
                            checkDate = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["checkdt"].ToString();
                            suspendNumber = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["suspendno"].ToString();
                            expectEPR = ""; //not found in progress sql
                            origBank = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["origcd"].ToString();
                            origBankAccount = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["origno"].ToString();
                            filedate = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["filedt"].ToString();
                            note = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["noteskey"].ToString();
                            comingledstatus = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["comingledstat"].ToString();
                       
                        }
                        catch { }
                        //is it a remit?
                        try
                        {
                            rstatus = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["rstatus"].ToString();
                            origBank = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["origcd"].ToString();
                            origBankAccount = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["origno"].ToString();
                            comingledstatus = "";
                        }
                        catch
                        { //not a remit
                        }
                        matchStatus = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["matchstat"].ToString();
                        workflowStatus = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["stat"].ToString(); //wfstatus


                         transType = ""; // GetEmbillzSearchData.Rows[SelectedEIQRow]["transtype"].ToString();
                         matchUser = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["matchuid"].ToString();
                         serviceType = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["serviceid"].ToString();//serviceid
                    
                    }
                    //set the srcfileflag used by the CreateMatch 
                    string srcfileflag = "";
                    if (rstatus != "")
                    {
                        srcfileflag = "R";
                    }
                    else if (cstatus != "")
                    {
                        srcfileflag = "C";
                    }
                    Model.MatchingChecks mc = new MatchingChecks(checknum, docno, checkamt, practice, depdate, docdet, doctype, "UNMATCHED",
                            matchID, externalPayor, note, checkDate, suspendNumber,
                             cstatus, matchStatus, workflowStatus, expectEPR, origBank,
                             origBankAccount, filedate, matchUser, serviceType, rstatus, key, srcfileflag, transType, pardocno, comingledstatus);
                    //fill the cart and set the count. make sure there are not duplicates
                    //Model.MatchingChecks mc = new MatchingChecks(checknum, docno, checkamt, practice, depdate, docdet, doctype, "UNMATCHED");
                    var results = (from table in Model.MatchingChecksCollection.MatchingChecksObservableCollection.AsEnumerable() where table.Key == mc.Key select table.Key).FirstOrDefault();
                    if (results == 0)
                    {
                        int? viewsIndex = GetEmbillzSearchData.AsDataView().ToTable(false, new[] { "key" })
                                    .AsEnumerable()
                                    .Select(row => row.Field<string>("key")) // ie. project the col(s) needed
                                    .ToList()
                                    .FindIndex(col => col == key.ToString()); // returns 2
                        if (viewsIndex != null)
                        {
                            GetEmbillzSearchData.Rows[viewsIndex.Value]["datasource"] = "CART";
                        }
                        
                        Model.MatchingChecksCollection.MatchingChecksObservableCollection.Add(mc);

                        Model.MatchingChecksCollection.CartCount = Model.MatchingChecksCollection.MatchingChecksObservableCollection.Count;
                    }
                }
            }
            catch (Exception ex)
            {
                //throw 
            }
        }
        ///
        ///Author: Mark Lane
        ///Using this function to add to the static class for Assembly Check/Cash Matching control
        ///throw this into a datatable or shared observable collection.
        ///Used in the cart.
        ///
        ///
        public void OnAddIDMToCart()
        {
            //No IDM or FLoware data can go in cart
            /*
            int totalRows = 0;
            try
            {

                if (SelectedIDMRow >= 0)
                {
                    string checknum = GetIDMSearchData.Rows[SelectedIDMRow]["checknum"].ToString();
                    double checkamt = Convert.ToDouble(GetIDMSearchData.Rows[SelectedIDMRow]["checkamt"].ToString().DefaultIfEmpty('0'));
                    string practice = GetIDMSearchData.Rows[SelectedIDMRow]["practice"].ToString();
                    string division = GetIDMSearchData.Rows[SelectedIDMRow]["division"].ToString();
                    string docno = GetIDMSearchData.Rows[SelectedIDMRow]["docno"].ToString();
                    string depdate = GetIDMSearchData.Rows[SelectedIDMRow]["depdt"].ToString();
                    string doctype = GetIDMSearchData.Rows[SelectedIDMRow]["doctype"].ToString();
                    string docdet = GetIDMSearchData.Rows[SelectedIDMRow]["docdet"].ToString();

                    int matchID = Convert.ToInt32(GetIDMSearchData.Rows[SelectedIDMRow]["matchid"].ToString());
                    string externalPayor = GetIDMSearchData.Rows[SelectedIDMRow]["extpaycd"].ToString();
                    string note = GetIDMSearchData.Rows[SelectedIDMRow]["note"].ToString();
                    string checkDate = GetIDMSearchData.Rows[SelectedIDMRow]["checkdate"].ToString();
                    string suspendNumber = GetIDMSearchData.Rows[SelectedIDMRow]["suspdno"].ToString();
                    string status = GetIDMSearchData.Rows[SelectedIDMRow]["cstatus"].ToString();
                    string matchStatus = GetIDMSearchData.Rows[SelectedIDMRow]["matchstatus"].ToString();
                    string workflowStatus = GetIDMSearchData.Rows[SelectedIDMRow]["workflowstatus"].ToString();
                    string expectEPR = GetIDMSearchData.Rows[SelectedIDMRow]["expepr"].ToString();
                    string origBank = GetIDMSearchData.Rows[SelectedIDMRow]["origbankcd"].ToString();
                    string origBankAccount = GetIDMSearchData.Rows[SelectedIDMRow]["origbankacct"].ToString();
                    string filedate = GetIDMSearchData.Rows[SelectedIDMRow]["filedate"].ToString();
                    string matchUser = GetIDMSearchData.Rows[SelectedIDMRow]["matchuid"].ToString();
                    string serviceType = GetIDMSearchData.Rows[SelectedIDMRow]["serviceid"].ToString();
                   
                    //Model.MatchingChecks mc = new MatchingChecks(checknum, docno, checkamt, practice, depdate, docdet, doctype, "UNMATCHED"
                    //       );

                    Model.MatchingChecks mc = new MatchingChecks(checknum, docno, checkamt, practice, depdate, docdet, doctype, "UNMATCHED",
                            matchID, externalPayor, note, checkDate, suspendNumber,
                             status, matchStatus, workflowStatus, expectEPR, origBank,
                             origBankAccount, filedate, matchUser, serviceType, "");
                    
                    //fill the cart and set the count.
                    Model.MatchingChecksCollection.MatchingChecksObservableCollection.Add(mc);
                    Model.MatchingChecksCollection.CartCount = Model.MatchingChecksCollection.MatchingChecksObservableCollection.Count;
                }
            }
            catch (Exception ex)
            {
                //throw 
            }
             * */
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


                if (!dispatcher.CheckAccess())
                {
                    dispatcher.BeginInvoke((Action)(() =>
                    {
                        //what you want to do...
                        string ifn = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["ifn"].ToString();
                        Int16 pageno = Convert.ToInt16(GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["npages"].ToString());
                        string docno = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["docno"].ToString();
                        if (doc == null)
                        {

                           // doc = null;
                            doc = new View.DocumentViewer(Constants.IDMServerName, PermissionsModel.Permissions.WorkFlowPermissions.WindowsUsername, "");
                          
                            doc.Visibility = System.Windows.Visibility.Visible;
                            doc.Show();
                            doc.DrawDocument(ifn, pageno);
                            doc.GetParentDocnoData(docno);
                        }
                        else
                        {
                            //doc = new View.DocumentViewer(PermissionsModel.Permissions.IDMPermissions.IDMServer, PermissionsModel.Permissions.WorkFlowPermissions.WindowsUsername, "");
                            doc.Visibility = System.Windows.Visibility.Visible;
                            doc.Show();
                            doc.DrawDocument(ifn, pageno);
                            doc.GetParentDocnoData(docno);
                        }
                    }));
                }
                else
                {
                    //carry on...
                    string ifn = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["ifn"].ToString();
                    Int16 pageno = Convert.ToInt16(GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["npages"].ToString());
                    string docno = GetFlowareSearchData.DefaultView.ToTable().Rows[SelectedRow]["docno"].ToString();
                    if (doc == null)
                    {
                        //doc = null;
                        doc = new View.DocumentViewer(Constants.IDMServerName, PermissionsModel.Permissions.WorkFlowPermissions.WindowsUsername, "");
                          
                        doc.Visibility = System.Windows.Visibility.Visible;
                        doc.Show();
                        doc.DrawDocument(ifn, pageno);
                        doc.GetParentDocnoData(docno);
                    }
                    else
                    {
                        //doc = new View.DocumentViewer(PermissionsModel.Permissions.IDMPermissions.IDMServer, PermissionsModel.Permissions.WorkFlowPermissions.WindowsUsername, "");
                        doc.Visibility = System.Windows.Visibility.Visible;
                        doc.Show();
                        doc.DrawDocument(ifn, pageno);
                        doc.GetParentDocnoData(docno);
                    }
                }

               
            }
            catch(Exception ex)
            {
               //handle
            }
        }
        public void OnIDMView()
        {
            string ifn = "";
            try
            {
                if (SelectedIDMRow >= 0)
                {
                    ifn = GetIDMSearchData.DefaultView.ToTable().Rows[SelectedIDMRow]["ifn"].ToString();
                    Int16 pageno = Convert.ToInt16(GetIDMSearchData.DefaultView.ToTable().Rows[SelectedIDMRow]["npages"].ToString());
                    string docno = GetIDMSearchData.DefaultView.ToTable().Rows[SelectedIDMRow]["docno"].ToString();
                    if (doc == null)
                    {
                       // doc = null;
                        doc = new View.DocumentViewer(Constants.IDMServerName, PermissionsModel.Permissions.WorkFlowPermissions.WindowsUsername, "");
                          
                        doc.Visibility = System.Windows.Visibility.Visible;
                        doc.Show();
                        doc.DrawDocument(ifn, pageno);
                        doc.GetParentDocnoData(docno);
                    }
                    else
                    {
                        //doc = new View.DocumentViewer(PermissionsModel.Permissions.IDMPermissions.IDMServer, Environment.UserName, "");
                        doc.Visibility = System.Windows.Visibility.Visible;
                        doc.Show();
                        doc.DrawDocument(ifn, pageno);
                        doc.GetParentDocnoData(docno);
                    }
                }
            }
            catch (Exception ex)
            {
                //handle
            }
        }
        public void OnEIQView()
        {
           
            try
            {
                if (SelectedEIQRow >= 0)
                {
                   // ifn = GetIDMSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["ifn"].ToString();
                   // Int16 pageno = Convert.ToInt16(GetIDMSearchData.DefaultView.ToTable().Rows[SelectedIDMRow]["npages"].ToString());
                    string docno = GetEmbillzSearchData.DefaultView.ToTable().Rows[SelectedEIQRow]["docno"].ToString();
                    if (doc == null)
                    {
                        //doc = null;
                        doc = new View.DocumentViewer(Constants.IDMServerName, PermissionsModel.Permissions.WorkFlowPermissions.WindowsUsername, "");

                        doc.Visibility = System.Windows.Visibility.Visible;
                        doc.Show();
                       
                        doc.GetParentDocnoData(docno);
                        doc.ShowDocno();
                    }
                    else
                    {
                        //doc = new View.DocumentViewer(PermissionsModel.Permissions.IDMPermissions.IDMServer, Environment.UserName, "");
                        doc.Visibility = System.Windows.Visibility.Visible;
                        doc.Show();
                        
                        doc.GetParentDocnoData(docno);
                        doc.ShowDocno();
                    }
                }
            }
            catch (Exception ex)
            {
                //handle
            }
            
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
        public string SearchType
        {
            get
            {
                return searchType;
            }
            set
            {
                searchType = value;
                
                if (searchType == "Doctype")
                {
                    IsDoctypeSelected = Visibility.Visible;
                    //set the search lists
                    SearchList1 = documentTypes;
                }
                else if (searchType == "Docdet")
                {
                    SearchList1 = documentDetails;
                }
                else
                {
                    SearchList1 = null;
                }
                OnPropertyChanged("SearchType");
            }
        }
        public string SearchType2
        {
            get
            {
                return searchType2;
            }
            set
            {
                searchType2 = value;
                
                //set the search lists
                if (searchType2 == "Doctype")
                {
                    SearchList2 = documentTypes;
                }
                else if (searchType2 == "Docdet")
                {
                    SearchList2 = documentDetails;
                }
                else
                {
                    SearchList2 = null;
                }
                if (searchType == "EmbAcct" || searchType == "Depdt" || searchType == "Checknum")
                {
                    IsDoctypeSelected = Visibility.Hidden;
                 
                }
                else
                {
                    IsDoctypeSelected = Visibility.Hidden;
                }
                OnPropertyChanged("SearchType2");
            }
        }
        public string SearchValue
        {
            get
            {
                return searchValue;
            }
            set
            {
                searchValue = value;
                VerifyPropertyName("SearchValue");
            }
        }
        public List<string> ComboBoxSearchBy
        {
            get { return comboBoxSearchBy; }
            set { comboBoxSearchBy = value;
            OnPropertyChanged("ComboBoxSearchBy");
            }
        }

        public string SearchValue2
        {
            get
            {
                return searchValue2;
            }
            set
            {
                searchValue2 = value;
                VerifyPropertyName("SearchValue2");
            }
        }
        public string SelectedItem
        {
            get;
            set;
        }
        public int SelectedEIQRow
        {
            get { return selectedEIQRow; }
            set { selectedEIQRow = value;
            OnPropertyChanged("SelectedEIQRow");
                if(IsFilterChecked == true)
                {
                    if (selectedEIQRow > -1)
                    {
                        try
                        {
                            using (var docdata = new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME))
                            {
                                GetIDMSearchData = docdata.GetPostDocByDocNo(Environment.MachineName, Environment.UserName, GetEmbillzSearchData.DefaultView.ToTable().Rows[selectedEIQRow]["docno"].ToString());
                                GetFlowareSearchData = docdata.GetPostBDByDocNo(Environment.MachineName, Environment.UserName, GetEmbillzSearchData.DefaultView.ToTable().Rows[selectedEIQRow]["docno"].ToString());
                            
                            }
                            UpdateViewTableHeaders();
                        }
                        catch (Exception ex)
                        {
                            //was returning the search results
                        }
                    }
                }
            }
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

                VerifyPropertyName("RetrievedItems");
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
                    if (IsWorkFlowFilterChecked == true)
                    {
                        if (selectedRow > -1)
                        {
                            try
                            {
                                using (var docdata = new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME))
                                {
                                    GetEmbillzSearchData = docdata.GetCashItemsByDocNo(Environment.MachineName, Environment.UserName, GetFlowareSearchData.DefaultView.ToTable().Rows[selectedRow]["DOCNO"].ToString());
                                    if (GetEmbillzSearchData.Rows.Count == 0)
                                    {
                                        GetEmbillzSearchData = docdata.GetRemitItemsByDocNo(Environment.MachineName, Environment.UserName, GetFlowareSearchData.DefaultView.ToTable().Rows[selectedRow]["DOCNO"].ToString());
                                    }
                                    GetIDMSearchData = docdata.GetPostDocByDocNo(Environment.MachineName, Environment.UserName, GetFlowareSearchData.DefaultView.ToTable().Rows[selectedRow]["DOCNO"].ToString());
                                }
                                UpdateViewTableHeaders();
                            }
                            catch (Exception ex)
                            {
                                //was returning search results
                            }
                        }
                    }
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
               
                    if (IsIDMFilterChecked == true)
                    {
                        if (selectedIDMRow > -1)
                        {
                            try
                            {
                                using (var docdata = new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME))
                                {
                                    GetEmbillzSearchData = docdata.GetCashItemsByDocNo(Environment.MachineName, Environment.UserName, GetIDMSearchData.DefaultView.ToTable().Rows[selectedIDMRow]["Docno"].ToString());
                                    if (GetEmbillzSearchData.Rows.Count == 0)
                                    {
                                        GetEmbillzSearchData = docdata.GetRemitItemsByDocNo(Environment.MachineName, Environment.UserName, GetIDMSearchData.DefaultView.ToTable().Rows[selectedIDMRow]["Docno"].ToString());
                                    }
                                    GetFlowareSearchData = docdata.GetPostBDByDocNo(Environment.MachineName, Environment.UserName, GetIDMSearchData.DefaultView.ToTable().Rows[selectedIDMRow]["Docno"].ToString());
                                }
                                UpdateViewTableHeaders();
                            }
                            catch (Exception ex)
                            {
                                //was returning search results
                            }
                        }
                    }
                
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
        public string EmbillzResults
        {
            get { return embillzResults; }
            set
            {
                embillzResults = value;
                OnPropertyChanged("EmbillzResults");
            }
        }
        public string SelectedPractice
        {
            get { return selectedPractice; }
            set
            {
                selectedPractice = value;
                OnPropertyChanged("SelectedPractice");
            }
        }
        public Visibility IsDoctypeSelected
        {
            get { return isDoctypeSelected; }
            set 
            {
                isDoctypeSelected = value;
                OnPropertyChanged("IsDoctypeSelected");
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
            // That's just a fake example
            List<string> results = new List<string>();
            results.Add("Search Account");
            results.Add("Search Docno");
            results.Add("Search Check Amount");
            RetrievedItems = new ObservableCollection<string>(results);
            SelectedItem = RetrievedItems.Count > 0 ? RetrievedItems[0] : string.Empty;
        }
        
        /// <summary>
        /// Execute Search against Document Manager Service and return from four sources.
        /// Floware
        /// Embillz
        /// Assembly
        /// IDM
        /// updates the ViewModels DataTables which are bound to the View.
        /// </summary>
        /// <param name="SearchBy"></param>
        /// <param name="SearchValue"></param>
        /// <returns></returns>
        private void ReturnSearchIDM(string SearchBy, string SearchValue)
        {
            DataTable idmSearchData = new DataTable("IDM");
            using (var docManagerServiceClient = new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME))
            {

                
                switch (SearchBy)
                {
                    case "Docno":

                        idmSearchData = docManagerServiceClient.GetPostDocSearchByDocNo(Environment.MachineName, Environment.UserName, SearchValue);
                        break;
                    case "EmbAcct":
                        if (SelectedPractice == null)
                        {
                            MessageBox.Show("Please Select a Practice For this Search.");
                            break;
                        }
                        idmSearchData = docManagerServiceClient.GetPostDetailByPracticeAndAccount(Environment.MachineName, Environment.UserName, SelectedPractice, SearchValue);

                        break;
                    case "Depdt":
                        if (SelectedPractice == null)
                        {
                            MessageBox.Show("Please Select a Practice For this Search.");
                            break;
                        }
                        idmSearchData = docManagerServiceClient.GetPostDocSearchByPracticeAndDeptDt(Environment.MachineName, Environment.UserName, SelectedPractice, Convert.ToDateTime(SearchValue));
                        break;
                    case "Pardocno":
                        idmSearchData = docManagerServiceClient.GetPostDocSearchByParDocNo(Environment.MachineName, Environment.UserName, SearchValue);
                        break;
                    case "CheckNum":

                        idmSearchData = docManagerServiceClient.GetPostDocSearchByCheckNum(Environment.MachineName, Environment.UserName, SearchValue);
                        break;
                    case "Doctype":
                        // getIDMSearchData = docManagerServiceClient.
                        break;
                    case "Docdet":
                        // getIDMSearchData = docManagerServiceClient.
                        break;
                    default:
                        break;
                }
            }
            //Model.GetSearchData searchData = new Model.GetSearchData();
            //getIDMSearchData = searchData.SearchIDM(SearchBy, SearchValue);
            
            GetIDMSearchData = idmSearchData;
        }
        private void ReturnMoreEmbillzData()
        {
            DataTable embillzSearchData = new DataTable("Embillz");
            DataTable getEmbillzSearchData2 = new DataTable("Embillz2");
            using (var docManagerServiceClient = new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME))
            {

                if (SearchType == "Docno" || SearchType == "Pardocno")
                {


                    embillzSearchData = docManagerServiceClient.GetRemitItemsByDocNo(Environment.MachineName, Environment.UserName, SearchValue);
                    getEmbillzSearchData2 = docManagerServiceClient.GetCashItemsByDocNo(Environment.MachineName, Environment.UserName, SearchValue);

                    embillzSearchData.Merge(getEmbillzSearchData2);

                }
                else if (SearchType == "Doctype" || SearchType == "Docdet")
                {
                    if (Docdet != "" && Doctype != "")
                    {
                        embillzSearchData = docManagerServiceClient.GetEiqNavC2PByDocTypeDocDetPractice(Environment.MachineName, Environment.UserName, Doctype, Docdet, SelectedPractice);
                    }
                }
                else if (SearchType == "EmbAcct")
                {
                    // getEmbillzSearchData = docManagerServiceClient.
                }
            }
             
            //apply custom ordering to columns based on the multiple sources.
            LoadMergedEmbillzDataTable(embillzSearchData);
        }
        /// <summary>
        /// load the search more merged datatable.
        /// </summary>
        /// <param name="dt"></param>
        private void LoadMergedEmbillzDataTable(DataTable dt)
        {
            Dispatcher dispatcher = Application.Current.Dispatcher;

            if (!dispatcher.CheckAccess())
            {
                dispatcher.BeginInvoke((Action)(() =>
                {

                    try
                    {
                        Model.OrderDataGridColumns orderDataGridColumns = new OrderDataGridColumns();
                        if (dt.Columns.Contains("Client"))
                        {
                            if (Properties.Settings.Default["SaveNavC2PColumnOrder"].ToString() != "" && Properties.Settings.Default["SaveNavC2PColumnOrder"] != null)
                            {
                                GetEmbillzSearchData.Merge(orderDataGridColumns.ReorderNavC2PColumns(dt));
                            }
                            else
                            {
                                GetEmbillzSearchData.Merge(dt);
                            }
                        }
                        else if (Properties.Settings.Default["SaveEmbillzColumnOrder"].ToString() != "" && Properties.Settings.Default["SaveEmbillzColumnOrder"] != null)
                        {
                            GetEmbillzSearchData.Merge(orderDataGridColumns.ReorderEmbillzColumns(dt));
                            // GetEmbillzSearchData.Columns["KEY"].ColumnMapping = MappingType.Hidden;
                        }
                        else
                        {

                            dt.Columns["PRACTICE"].SetOrdinal(0);
                            dt.Columns["DIV"].SetOrdinal(1);
                            dt.Columns["DOCTYPE"].SetOrdinal(2);
                            dt.Columns["DOCDET"].SetOrdinal(3);
                            dt.Columns["CHECKNUM"].SetOrdinal(4);
                            dt.Columns["CHECKAMT"].SetOrdinal(5);
                            dt.Columns["EXTPAYCD"].SetOrdinal(6);
                            dt.Columns["DEPDT"].SetOrdinal(7);
                            dt.Columns["DOCNO"].SetOrdinal(8);
                            //getEmbillzSearchData.Columns["ACCOUNT"].SetOrdinal(9);
                            dt.Columns["MATCHSTAT"].SetOrdinal(10);
                            dt.Columns["STAT"].SetOrdinal(11);
                            dt.Columns["CHECKDT"].SetOrdinal(12);
                            dt.Columns["FILEDT"].SetOrdinal(13);
                            //embillzSearchData.Columns["KEY"].ColumnMapping = MappingType.Hidden;
                            GetEmbillzSearchData.Merge(dt);
                        }
                    }
                    catch (Exception ex)
                    {
                        //a column name has changed
                        GetEmbillzSearchData = dt;
                    }
                }));
            }
            else
            {
                try
                {
                    Model.OrderDataGridColumns orderDataGridColumns = new OrderDataGridColumns();
                    if (dt.Columns.Contains("Client"))
                    {
                        if (Properties.Settings.Default["SaveNavC2PColumnOrder"].ToString() != "" && Properties.Settings.Default["SaveNavC2PColumnOrder"] != null)
                        {
                            GetEmbillzSearchData.Merge(orderDataGridColumns.ReorderNavC2PColumns(dt));
                        }
                        else
                        {
                            GetEmbillzSearchData.Merge(dt);
                        }
                    }
                    else if (Properties.Settings.Default["SaveEmbillzColumnOrder"].ToString() != "" && Properties.Settings.Default["SaveEmbillzColumnOrder"] != null)
                    {
                        GetEmbillzSearchData.Merge(orderDataGridColumns.ReorderEmbillzColumns(dt));
                        // GetEmbillzSearchData.Columns["KEY"].ColumnMapping = MappingType.Hidden;
                    }
                    else
                    {

                        dt.Columns["PRACTICE"].SetOrdinal(0);
                        dt.Columns["DIV"].SetOrdinal(1);
                        dt.Columns["DOCTYPE"].SetOrdinal(2);
                        dt.Columns["DOCDET"].SetOrdinal(3);
                        dt.Columns["CHECKNUM"].SetOrdinal(4);
                        dt.Columns["CHECKAMT"].SetOrdinal(5);
                        dt.Columns["EXTPAYCD"].SetOrdinal(6);
                        dt.Columns["DEPDT"].SetOrdinal(7);
                        dt.Columns["DOCNO"].SetOrdinal(8);
                        //getEmbillzSearchData.Columns["ACCOUNT"].SetOrdinal(9);
                        dt.Columns["MATCHSTAT"].SetOrdinal(10);
                        dt.Columns["STAT"].SetOrdinal(11);
                        dt.Columns["CHECKDT"].SetOrdinal(12);
                        dt.Columns["FILEDT"].SetOrdinal(13);
                        //embillzSearchData.Columns["KEY"].ColumnMapping = MappingType.Hidden;
                        GetEmbillzSearchData.Merge(dt);
                    }
                }
                catch (Exception ex)
                {
                    //a column name has changed
                    GetEmbillzSearchData = dt;
                }
            }
        }

        private void ReturnSearchEmbillz(string SearchBy, string SearchValue)
        {
            
           DataTable embillzSearchData = new DataTable("Embillz");
            DataTable getEmbillzSearchData2 = new DataTable("Embillz2");
            using (var docManagerServiceClient = new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME))
            {

                if (SearchBy == "Docno" || SearchBy == "Pardocno")
                {


                    embillzSearchData = docManagerServiceClient.GetRemitItemsByDocNo(Environment.MachineName, Environment.UserName, SearchValue);
                    getEmbillzSearchData2 = docManagerServiceClient.GetCashItemsByDocNo(Environment.MachineName, Environment.UserName, SearchValue);

                    embillzSearchData.Merge(getEmbillzSearchData2);

                }
                else if (SearchBy == "Doctype" || SearchBy == "Docdet")
                {
                    if (Docdet != "" && Doctype != "")
                    {
                        embillzSearchData = docManagerServiceClient.GetEiqNavC2PByDocTypeDocDetPractice(Environment.MachineName, Environment.UserName, Doctype, Docdet, SelectedPractice);
                    }
                }
                else if (SearchBy == "EmbAcct")
                {
                    // getEmbillzSearchData = docManagerServiceClient.
                }
            }
            
               //apply custom ordering to columns based on the multiple sources.
                try
                {
                    embillzSearchData.Columns.Add("DataSource");
                    Model.OrderDataGridColumns orderDataGridColumns = new OrderDataGridColumns();
                   if (embillzSearchData.Columns.Contains("Client"))
                   {
                        if (Properties.Settings.Default["SaveNavC2PColumnOrder"].ToString() != "" && Properties.Settings.Default["SaveNavC2PColumnOrder"] != null)
                        {
                            GetEmbillzSearchData = orderDataGridColumns.ReorderNavC2PColumns(embillzSearchData);
                        }
                        else
                        {
                            GetEmbillzSearchData = embillzSearchData;
                        }
                    }
                    else if (Properties.Settings.Default["SaveEmbillzColumnOrder"].ToString() != "" && Properties.Settings.Default["SaveEmbillzColumnOrder"] != null)
                    {
                        GetEmbillzSearchData = orderDataGridColumns.ReorderEmbillzColumns(embillzSearchData);
                       // GetEmbillzSearchData.Columns["KEY"].ColumnMapping = MappingType.Hidden;
                    }
                    else
                    {

                        embillzSearchData.Columns["PRACTICE"].SetOrdinal(0);
                        embillzSearchData.Columns["DIV"].SetOrdinal(1);
                        embillzSearchData.Columns["DOCTYPE"].SetOrdinal(2);
                        embillzSearchData.Columns["DOCDET"].SetOrdinal(3);
                        embillzSearchData.Columns["CHECKNUM"].SetOrdinal(4);
                        embillzSearchData.Columns["CHECKAMT"].SetOrdinal(5);
                        embillzSearchData.Columns["EXTPAYCD"].SetOrdinal(6);
                        embillzSearchData.Columns["DEPDT"].SetOrdinal(7);
                        embillzSearchData.Columns["DOCNO"].SetOrdinal(8);
                        //getEmbillzSearchData.Columns["ACCOUNT"].SetOrdinal(9);
                        embillzSearchData.Columns["MATCHSTAT"].SetOrdinal(10);
                        embillzSearchData.Columns["STAT"].SetOrdinal(11);
                        embillzSearchData.Columns["CHECKDT"].SetOrdinal(12);
                        embillzSearchData.Columns["FILEDT"].SetOrdinal(13);
                        //embillzSearchData.Columns["KEY"].ColumnMapping = MappingType.Hidden;
                        GetEmbillzSearchData = embillzSearchData;
                    }
                }
                catch (Exception ex)
                {
                    //a column name has changed
                    GetEmbillzSearchData = embillzSearchData;
                }
            //alternate practice filter
            //if (SelectedPractice != "" && SelectedPractice != null && embillzSearchData.Rows.Count > 0)
            //{
            //        GetEmbillzSearchData = (from table in embillzSearchData.AsEnumerable() where table.Field<string>("PRACTICE") == SelectedPractice select table).CopyToDataTable();
           // }
           
        }
        public DataTable ReturnSearchAssembly(string SearchBy, string SearchValue)
        {
            getAssemblySearchData = new DataTable("Assembly");
            Model.GetSearchData searchData = new Model.GetSearchData();
            getAssemblySearchData = searchData.SearchAssembly(SearchBy, SearchValue);
            return getAssemblySearchData;
        }
        private void  ReturnSearchFloware(string SearchBy, string SearchValue)
        {
           DataTable flowareSearchData = new DataTable("Floware");
           using (var docManagerServiceClient = 
               new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME))
           {

               switch (SearchBy)
               {
                   case "Docno":

                       flowareSearchData = docManagerServiceClient.GetPostBDByDocNo(Environment.MachineName, Environment.UserName, SearchValue);
                       break;
                   case "EmbAcct":
                       if (SelectedPractice == null)
                       {
                           MessageBox.Show("Please Select a Practice For this Search.");
                           break;
                       }
                       flowareSearchData = docManagerServiceClient.GetPostBDByPracticeAndAccount(Environment.MachineName, Environment.UserName, SelectedPractice, SearchValue);

                       break;
                   case "Depdt":
                       if (SelectedPractice == null)
                       {
                           MessageBox.Show("Please Select a Practice For this Search.");
                           break;
                       }
                       flowareSearchData = docManagerServiceClient.GetPostBDByPracticeAndDepDate(Environment.MachineName, Environment.UserName, SelectedPractice, SearchValue);
                       break;
                   case "Pardocno":
                       flowareSearchData = docManagerServiceClient.GetPostBDByParDocNo(Environment.MachineName, Environment.UserName, SearchValue);
                       break;
                   case "CheckNum":
                       flowareSearchData = docManagerServiceClient.SearchPostBd(Environment.MachineName, Environment.UserName, "", "", "", "",
                           "", "", "", "", Checknum, "", "", "", "", "", "", "", "", "");
                       //getFlowareSearchData = docManagerServiceClient.GetPostBDByCheckNum(Environment.MachineName, Environment.UserName, SearchValue);
                       break;
                   case "Doctype":
                       if (Docdet == null)
                       {
                           Docdet = "";
                       }
                       flowareSearchData = docManagerServiceClient.SearchPostBd(Environment.MachineName, Environment.UserName, "", "", "", "",
                           "", "", "", "", "", "", "", "", "", Doctype, Docdet, "", "", "", 1, 500);
                       break;
                   case "Docdet":
                       if (Doctype == null)
                       {
                           Doctype = "";
                       }
                       flowareSearchData = docManagerServiceClient.SearchPostBd(Environment.MachineName, Environment.UserName, "", "", "", "",
                           "", "", "", "", "", "", "", "", "", Doctype, Docdet, "", "", "", 1, 500);
                       break;

                   default:
                       break;
               }
           }

            if (SelectedPractice != "" && SelectedPractice != null && flowareSearchData.Rows.Count > 0)
            {
               
                    GetFlowareSearchData = (from table in flowareSearchData.AsEnumerable() where table.Field<string>("practice") == SelectedPractice select table).CopyToDataTable();
              
            }
            else
            {
                GetFlowareSearchData = flowareSearchData;
            }
           
        }


        //Populate DocumentType Combobox
        public ObservableCollection<Model.ImportDataModel.DocumentType> getDocumentTypes()
        {
            DataTable docTypes;
            using (var DocumentService = new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME))
            {
                docTypes = DocumentService.GetDocTypes(Environment.MachineName, Environment.UserName);
            }
                 DataView view = new DataView(docTypes);
            DataTable distinctValues = view.ToTable(true, "TYPE_KEY", "TYPE_DESCRIPTION");
            foreach (DataRow row in distinctValues.Rows)
            {
                var obj = new Model.ImportDataModel.DocumentType()
                {
                    key = row.Field<string>("TYPE_KEY"),
                    description = row.Field<string>("TYPE_DESCRIPTION"),
                    fullDocTypeDescription = row.Field<string>("TYPE_DESCRIPTION") + "  " + "[" + row.Field<string>("TYPE_KEY") + "]"
                };

                _documentTypes.Add(obj);
            }
            return _documentTypes;
        }


        //Populate DocumentDetails Combobox 
        public ObservableCollection<Model.ImportDataModel.DocumentDetail> getDocumentDetails()
        {
            DataTable docDetail;
            using (var DocumentService = new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME))
            {
                docDetail = DocumentService.GetDocTypes(Environment.MachineName, Environment.UserName);
            }
            DataView view = new DataView(docDetail);
            DataTable distinctValues = view.ToTable(true, "DETAIL_KEY", "DETAIL_DESCRIPTION");

            foreach (DataRow row in docDetail.Rows)
            {
                var obj = new Model.ImportDataModel.DocumentDetail()
                {
                    detailkey = row.Field<string>("DETAIL_KEY"),
                    detaildescription = row.Field<string>("DETAIL_DESCRIPTION"),
                    fullDetailDescription = row.Field<string>("DETAIL_DESCRIPTION") + "  " + "[" + row.Field<string>("DETAIL_KEY") + "]"

                };
                if ((obj.detailkey != null) && (obj.detaildescription != null))
                {
                    _documentDetails.Add(obj);
                }
            }
            return _documentDetails;
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
                    Docdet = searchValue;
                    break;
                case "Doctype":
                    Doctype = searchValue;
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
        private void PrepareSearch()
        {
            ClearBusinessData();
            SetSearchValues(SearchType, SearchValue);
            SetSearchValues(SearchType2, SearchValue2);
        }
        private void ClearBusinessData()
        {
            CurrentMapName = "";
            CurrentActivityName = "";
            Practice = "";
            Division = "";
            MinCheckamt = "";
            MaxCheckamt = "";
            MinCheckamt = "";
            Embacct = "";
            Reason = "";
            Docdet = "";
            Doctype = "";
            MatchStatus = "";
            Docno = "";
            Pardocno = "";
            MinPaidamt = "";
            MaxPaidamt = "";
            Assigned_To = "";

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
                    retval = Properties.Settings.Default["Embacct"].ToString(); ;
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
        public string CurrentMapName
        {
            get { return currentMapName; }
            set
            {
                currentMapName = value;
               
            }
        }
        public string CurrentActivityName
        {
            get { return currentActivityName; }
            set { currentActivityName = value; }
        }
        private string Docno
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
        private string Docdet
        {
            get { return docDet; }
            set { docDet = value; }
        }
        private string Doctype
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
