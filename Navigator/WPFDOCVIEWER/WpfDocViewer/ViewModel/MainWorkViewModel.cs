using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Rti;
using Rti.InternalInterfaces.DataContracts;
using Rti.InternalInterfaces.ServiceProxies;
using WpfDocViewer.Model;
using System.Windows.Threading;


namespace WpfDocViewer.ViewModel
{
    class MainWorkViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private WpfDocViewer.Model.WorkModel modelObj;
        private string parentDocno;
        private DataTable getdatabyDoc;
        private string selecteddataItem;
        int selectedRow;
        private string selectedStartActivity;
        private ObservableCollection<Model.WorkModel.StartActivity> _startActivity;
        private string showDatabydoc;
        bool showData;
        //Document Viewer
        private View.DocumentViewer doc;      
        //Variables used for StartWork function       
        private int towId;
        private string postdetail_id;
        private string courier_inst_id;
        private string workflowitemkey;
        private string workflowthreadkey;
        //private decimal? mapInstId;
        //private decimal? actInstId;
        private string docNo;
        private string practice;
        private string division;
        private string depositDate;
        private string docType;
        private string checkNum;
        private string checkAmt;    
        private string extPayor;      
        private string docDetail;
        private string depCode;
        private string docSource;
        private string ActName;
        private string MpName;
        private string serviceId;
        private string docGroup;
        private string acctNo;
        private string financialClass;
        string strAction = "AddWork";
        private bool isbusy;
        private bool isEnable;
        private bool isEnableOverwrite;
        public Dictionary<string, string> validationErrors = new Dictionary<string, string>();     

        readonly string _userName = CommonFunctions.GetUserName();
        readonly string _stationName = CommonFunctions.GetFqdn();

        DataAccessLayer.GetDocumentData DocumentService = new DataAccessLayer.GetDocumentData();
        DocumentManagerServiceClient _documentManagerService = 
            new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME);
        ResourceDictionary resources = App.Current.Resources;

        public MainWorkViewModel()
        {
            modelObj = new Model.WorkModel();            
            _startActivity = new ObservableCollection<Model.WorkModel.StartActivity>();
            getActivities();
            ParentDocno = App.Current.FindResource("ParentDocno").ToString();
            this.FindCommand = new RelayCommand(OnFind);
            this.OpenViewer = new RelayCommand(OnView);
        }
        public RelayCommand FindCommand
        {
            get;
            private set;

        }
        public RelayCommand OpenViewer
        {
            get;
            set;
        }
        //Populate DataTable
        public DataTable FillDataTable()
        {
            DataTable dt = new DataTable();
            dt = _documentManagerService.GetPostDocSearchByParDocNo(_stationName, _userName, parentDocno);
            return dt;
           
        }     

        public void OnFind()
        {
            //Get data by docno   
            if ((ParentDocno == null) || (ParentDocno == "0000000000"))
            {
                MessageBox.Show("Please enter pardocno");
                return;
            }

                GetDataByDocNumber = FillDataTable();               
                int totalRows = getdatabyDoc.Rows.Count;
                if (totalRows <= 0)
                {
                    ShowData = false;
                    ShowDataByDocNo = "No Rows found.";
                    IsEnable = false;
                    IsEnableOverwrite = false;                  
                    if (!(IsChkNeedsPrepSelected))
                    {
                        SelectedStartActivity = "";
                        MpName = "";
                        ActName = "";
                    }

                }

                else
                {
                    ShowData = true;
                    ShowDataByDocNo = totalRows + " rows found.";
                    IsEnable = true;
                    if (((PermissionsModel.Permissions.IDMPermissions.IDMGroup.ToUpper().Contains("SUPER") == true) || (PermissionsModel.Permissions.IDMPermissions.IDMGroup.ToUpper() == "ISADMIN")))
                    { IsEnableOverwrite = true; }

                }           
        }

        public void OnStartWork()
        {
            IsBusy = true;
            string userPwd="";
            if ((IsChkOverwriteSelected == true))
            {
                MpName = StartActivities.Where(a => a.startdescription == SelectedStartActivity)
                            .Select(b => b.mapName).SingleOrDefault();
                ActName = StartActivities.Where(a => a.startdescription == SelectedStartActivity)
                         .Select(b => b.activityName).SingleOrDefault();
            }

             var navImages = new List<NavImage>();            

            var navImage = new  NavImage();
            navImage.action = TowId.ToString();
            navImage.filename = (financialClass == null)? "" : financialClass; 
            navImage.seq = 1;
            navImage.docNo = docNo;
            navImage.parDocNo = ParentDocno;
            navImage.origDocNo = ParentDocno;
            navImage.DocGroup = (docGroup == null)? "" : docGroup;
            navImage.docType = (docType == null)? "" : docType;
            navImage.docDet = (docDetail == null)? "" : docDetail;
            navImage.docSrc = (docSource == null)? "" : docSource;
            navImage.acctNo = (acctNo == null)? "" : acctNo;
            navImage.practice = (practice == null)? "": practice;
            navImage.div = (division == null)? "" : division;
            navImage.serviceId = (serviceId == null) ? "" : serviceId;           
            navImage.deptCode = (depCode == null)? "": depCode;
            navImage.extPayor = (extPayor == null)? "" :extPayor;
            navImage.checkAmt = Convert.ToString(checkAmt);
            navImage.checkNum = Convert.ToString(checkNum);
            navImage.checkDt = (depositDate == null) ? DateTime.Now.ToShortDateString() : depositDate;
            navImage.cStatus = "X";
            navImage.supervisorId = PermissionsModel.Permissions.AssemblyPermissions.SupervisorID;
            navImage.nPages = 1;

            navImages.Add(navImage);

            if (towId != 0)
            {
            var idgparam = new IDGInputParams();
            {
                idgparam.IsChkNeedsPrep = false;
                idgparam.IsRemitOnly = false;
                idgparam.strMapName = MpName;
                idgparam.strActName = ActName;
                idgparam.strInformational = "Y";
                idgparam.strReason = "";
            }
            var submitDocResults = _documentManagerService.SubmitDocuments(_stationName, _userName, userPwd, strAction,navImages, idgparam);
            
            if (submitDocResults.IsSuccess)
            {
                towId = TowId;
                postdetail_id = submitDocResults.strPostdetailId;
                courier_inst_id = submitDocResults.strCourierInstId;
                workflowitemkey = submitDocResults.strWfitemkey;
                workflowthreadkey = submitDocResults.strWfthreadkey;
            }
            else
            {
               
                towId = TowId;
                if ((submitDocResults.strPostdetailId != null) && (submitDocResults.strPostdetailId != ""))
                    postdetail_id = submitDocResults.strPostdetailId;
                if ((submitDocResults.strCourierInstId != null) && (submitDocResults.strCourierInstId != ""))
                    courier_inst_id = submitDocResults.strCourierInstId;
                MessageBox.Show(submitDocResults.OutMessage);
            }




                //Commented the below code to include this logic in the service instead of client.
                //        //postdetail and postdoc tables are joined based on ifnds and ifnid with  towerid as input. 
                //        ServiceReferenceFlowareWeb.FlowareWebServiceManagerClient flowareClient = new ServiceReferenceFlowareWeb.FlowareWebServiceManagerClient();
                //        courier_inst_id = flowareClient.createWorkItem(ActName, (int)towId);

                //        //To retrieve Act_Node_Id and Map_Inst_Id

                //        var results = _documentManagerService.GetPostBdMapInstIdNodeId(_stationName, _userName, courier_inst_id);
                //        mapInstId = results.Item1;
                //        actInstId = results.Item2;


                //        if (courier_inst_id > 0)
                //        {

                //            //Create WorkflowItem
                //            var workFlowItem = new NavWorkFlowItem();
                //            workFlowItem.practice = practice;
                //            workFlowItem.division = division;
                //            workFlowItem.depDate = (depositDate != null) ? DateTime.Parse(depositDate) : DateTime.Now;
                //            workFlowItem.recDate = DateTime.Now;
                //            workFlowItem.docType = docType;
                //            workFlowItem.docNo = docNo;
                //            workFlowItem.parDocNo = parentDocno;
                //            workFlowItem.embAcctNo = "";
                //            workFlowItem.checkNum = Convert.ToString(checkNum);
                //            workFlowItem.checkAmt = Convert.ToString(checkAmt);
                //            workFlowItem.paidAmt = "";
                //            workFlowItem.extPayorCd = extPayor;
                //            workFlowItem.depCode = "";
                //            workFlowItem.docGroup = docGroup;
                //            workFlowItem.docDetail = docDetail;
                //            workFlowItem.courier_Inst_Id = courier_inst_id;
                //            workFlowItem.acctNum = "";
                //            workFlowItem.act_Name = ActName;
                //            workFlowItem.map_Name = MpName;
                //            workFlowItem.act_Node_Id = (int?)actInstId;
                //            workFlowItem.map_Inst_Id = (int?)mapInstId;
                //            workFlowItem.svcDate = DateTime.Now;
                //            workFlowItem.docPage = 1;
                //            workFlowItem.postdetail_Id = postdetail_id;
                //            workFlowItem.regionLeft = null;
                //            workFlowItem.regionTop = null;
                //            workFlowItem.finClass = "default";
                //            workFlowItem.escReason = "default";
                //            workFlowItem.corrFolder = "default";
                //            workFlowItem.serviceId = serviceId;
                //            workFlowItem.reason = "default";
                //            workFlowItem.informational = "Y";
                //            var createWorkflowResult = _documentManagerService.CreateWorkflowItem(_stationName, _userName, workFlowItem);
                //            workflowitemkey = Convert.ToInt32(createWorkflowResult.OutDataTable.Rows[0]["wfitemkey"]);
                //            workflowthreadkey = Convert.ToInt32(createWorkflowResult.OutDataTable.Rows[0]["wfthreadkey"]);
                //        }
                //    }
                //}

                //catch (Exception ex)
                //{ throw ex; }
            }

            IsBusy = false;
        }

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
                        string ifn = GetDataByDocNumber.Rows[SelectedRow]["ifn"].ToString();
                        Int16 pageno = Convert.ToInt16(GetDataByDocNumber.Rows[SelectedRow]["npages"].ToString());
                        string docno = GetDataByDocNumber.Rows[SelectedRow]["docno"].ToString();
                        if (doc != null)
                        {

                            doc = null;
                            doc = new View.DocumentViewer(Constants.IDMServerName, PermissionsModel.Permissions.WorkFlowPermissions.WindowsUsername, "");

                            doc.Visibility = System.Windows.Visibility.Visible;
                            doc.Show();
                            doc.DrawDocument(ifn, pageno);
                            doc.GetParentDocnoData(docno);
                        }
                        else
                        {
                            doc = new View.DocumentViewer(Constants.IDMServerName, PermissionsModel.Permissions.WorkFlowPermissions.WindowsUsername, "");
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
                    string ifn = GetDataByDocNumber.Rows[SelectedRow]["ifn"].ToString();
                    Int16 pageno = Convert.ToInt16(GetDataByDocNumber.Rows[SelectedRow]["npages"].ToString());
                    string docno = GetDataByDocNumber.Rows[SelectedRow]["docno"].ToString();
                    if (doc != null)
                    {
                        doc = null;
                        doc = new View.DocumentViewer(Constants.IDMServerName, PermissionsModel.Permissions.WorkFlowPermissions.WindowsUsername, "");

                        doc.Visibility = System.Windows.Visibility.Visible;
                        doc.Show();
                        doc.DrawDocument(ifn, pageno);
                        doc.GetParentDocnoData(docno);
                    }
                    else
                    {
                        doc = new View.DocumentViewer(Constants.IDMServerName, PermissionsModel.Permissions.WorkFlowPermissions.WindowsUsername, "");
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

        //DataBinding
        //Populate Start Activity Combobox       
        public ObservableCollection<Model.WorkModel.StartActivity> getActivities()
        {

            if (IsChkNeedsPrepSelected == false)
            {
                _startActivity.Clear();
                DataTable activitiesOne = DocumentService.GetActivities(Environment.MachineName, Environment.UserName);

                DataTable distinctActivitiesOne = activitiesOne.DefaultView.ToTable(true);
                foreach (DataRow row in distinctActivitiesOne.Rows)
                {
                    var obj1 = new Model.WorkModel.StartActivity()
                    {

                        typecode = row.Field<string>("TYPECODE"),
                        startdescription = row.Field<string>("DESCRIPTION"),
                        fullstartActivityDesc = row.Field<string>("DESCRIPTION") + "  " + "[" + row.Field<string>("TYPECODE") + "]",
                        mapName = row.Field<string>("MAP_NAME"),
                        activityName = row.Field<string>("ACT_NAME")

                    };


                    _startActivity.Add(obj1);
                }
            }
            else
            {
                _startActivity.Clear();
                DataTable activitiesTwo = _documentManagerService.GetPostDocStatusLookup(Environment.MachineName, Environment.UserName);

                DataTable distinctActivitiesTwo = activitiesTwo.DefaultView.ToTable(true);
                foreach (DataRow row in distinctActivitiesTwo.Rows)
                {
                    var obj2 = new Model.WorkModel.StartActivity()
                    {

                        typecode = row.Field<string>("TYPECODE"),
                        startdescription = row.Field<string>("DESCRIPTION"),
                        fullstartActivityDesc = row.Field<string>("DESCRIPTION") + "  " + "[" + row.Field<string>("TYPECODE") + "]",
                        mapName = row.Field<string>("MAP_NAME"),
                        activityName = row.Field<string>("ACT_NAME")

                    };


                    _startActivity.Add(obj2);
                }
            }
            return _startActivity;
        }

        //Properties
        public DataTable GetDataByDocNumber
        {
            get
            {
                return getdatabyDoc;
            }
            set
            {
                getdatabyDoc = value;
                OnPropertyChanged("GetDataByDocNumber");
            }
        }
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
        public string SelectedDataItem
        {
            get
            {
                return selecteddataItem;
            }
            set
            {
                selecteddataItem = value;
                if ((getdatabyDoc.Rows.Count > 0) && (SelectedRow != -1))
                {
                    DataTable postdetailTb = new DataTable();


                    towId = Convert.ToInt32(GetDataByDocNumber.Rows[SelectedRow]["TOWID"]);
                    checkAmt = GetDataByDocNumber.Rows[SelectedRow]["CHECKAMT"].ToString();
                    checkNum = GetDataByDocNumber.Rows[SelectedRow]["CHECKNUM"].ToString();
                    division = GetDataByDocNumber.Rows[SelectedRow]["DIVISION"].ToString();
                    practice = GetDataByDocNumber.Rows[SelectedRow]["PRACTICE"].ToString();
                    serviceId = GetDataByDocNumber.Rows[SelectedRow]["SERVICEID"].ToString();
                    docNo = GetDataByDocNumber.Rows[SelectedRow]["DOCNO"].ToString();
                    acctNo = GetDataByDocNumber.Rows[SelectedRow]["ACCTNUM"].ToString();
                    docType = GetDataByDocNumber.Rows[SelectedRow]["DOCTYPE"].ToString();
                    depositDate = GetDataByDocNumber.Rows[SelectedRow]["DEPDT"].ToString();
                    docDetail = GetDataByDocNumber.Rows[SelectedRow]["DOCDET"].ToString();
                    extPayor = GetDataByDocNumber.Rows[SelectedRow]["EXTPAYOR"].ToString();
                    depCode = GetDataByDocNumber.Rows[SelectedRow]["DEPCODE"].ToString();
                    docGroup = GetDataByDocNumber.Rows[SelectedRow]["DOCGROUP"].ToString();
                    docSource = GetDataByDocNumber.Rows[SelectedRow]["SOURCEID"].ToString();
                    financialClass = GetDataByDocNumber.Rows[SelectedRow]["Financial_class"].ToString();
                    string ifn = GetDataByDocNumber.Rows[SelectedRow]["IFN"].ToString();
                   
                    UpdateRoutingInfo(docType, docDetail, docSource);
                    //Currently retrieving this from service side 
                    //postdetailTb = _documentManagerService.GetPostDetailByTowerId(_stationName, _userName, (int)towId);
                    //if ((postdetailTb.Rows.Count != 0))
                    //    postdetail_id = (postdetailTb.Rows[0]["POSTDETAIL_ID"]).ToString();

                }              

                OnPropertyChanged("SelectedDataItem");

            }
        }


        //Update Routing Info
        public void UpdateRoutingInfo(string doctype, string docdetail, string docsource)
        {
            if (IsChkNeedsPrepSelected == false)
            {
                //No Routing Info
                SelectedStartActivity = "";
                MpName = "";
                ActName = "";
                //if ((doctype == null) || (docsource == null))
                //    return;

                if (PermissionsModel.Permissions.IDMPermissions.IDMGroup.ToUpper().Contains("HospitalistsSuper") == true)
                {
                    SelectedStartActivity = "Correspondence Supervisor";  //"c"
                    MpName = StartActivities.Where(a => a.startdescription == SelectedStartActivity)
                         .Select(b => b.mapName).FirstOrDefault(); ;
                    ActName = StartActivities.Where(a => a.startdescription == SelectedStartActivity)
                         .Select(b => b.activityName).FirstOrDefault();
                }
                else
                {   //call default
                    GetDefaultRouting(doctype, docdetail, docsource);
                }

            }
            else if (IsChkNeedsPrepSelected == true)
            {
                SelectedStartActivity = "Bulk Item Courier";  //"C"
                MpName = StartActivities.Where(a => a.startdescription == SelectedStartActivity)
                     .Select(b => b.mapName).FirstOrDefault(); ;
                ActName = StartActivities.Where(a => a.startdescription == SelectedStartActivity)
                     .Select(b => b.activityName).FirstOrDefault();
            }

        }

        private void GetDefaultRouting(string doctype, string docdetail, string docsource)
        {
            DataTable activities = _documentManagerService.ListRoutingInfo(Environment.MachineName, Environment.UserName);

            var query = from data in activities.AsEnumerable()
                        where ((data["DOCTYPE"].ToString() == doctype) && data["DOCDET"].ToString() == docdetail && data["SOURCETYPE"].ToString() == docsource && data["SECURITYGROUP"].ToString() == PermissionsModel.Permissions.IDMPermissions.IDMGroup)
                        select (string)data["DESCRIPTION"];
            if (query.Count() > 0)
            {
                SelectedStartActivity = query.FirstOrDefault();
                MpName = StartActivities.Where(a => a.startdescription == SelectedStartActivity)
                         .Select(b => b.mapName).FirstOrDefault();
                ActName = StartActivities.Where(a => a.startdescription == SelectedStartActivity)
                         .Select(b => b.activityName).FirstOrDefault();
            }
            else
            {
                var queryOne = from data in activities.AsEnumerable()
                               where ((data["DOCTYPE"].ToString() == doctype) && data["DOCDET"].ToString() == docdetail && data["SOURCETYPE"].ToString() == docsource)
                               select (string)data["DESCRIPTION"];
                if (queryOne.Count() > 0)
                {
                    SelectedStartActivity = queryOne.FirstOrDefault();
                    MpName = StartActivities.Where(a => a.startdescription == SelectedStartActivity)
                             .Select(b => b.mapName).FirstOrDefault();
                    ActName = StartActivities.Where(a => a.startdescription == SelectedStartActivity)
                             .Select(b => b.activityName).FirstOrDefault();
                }
                else
                {
                    var queryTwo = from data in activities.AsEnumerable()
                                   where ((data["DOCTYPE"].ToString() == doctype) && data["DOCDET"].ToString() == docdetail)
                                   select (string)data["DESCRIPTION"];
                    if (queryTwo.Count() > 0)
                    {
                        SelectedStartActivity = queryTwo.FirstOrDefault();
                        MpName = StartActivities.Where(a => a.startdescription == SelectedStartActivity)
                                 .Select(b => b.mapName).FirstOrDefault();
                        ActName = StartActivities.Where(a => a.startdescription == SelectedStartActivity)
                                 .Select(b => b.activityName).FirstOrDefault();
                    }
                    else

                    //try getting a near match if exact not found with  * and "":
                    {
                        var matchq = from data in activities.AsEnumerable()
                                     where ((data["DOCTYPE"].ToString() == "") || (data["DOCTYPE"].ToString() != "")) && ((data["DOCDET"].ToString() == "" || (data["DOCDET"].ToString() != "")) && ((data["SOURCETYPE"].ToString() == "") || (data["SOURCETYPE"].ToString() != "")) && ((data["SECURITYGROUP"].ToString() == "") || (data["SECURITYGROUP"].ToString() != "") || (data["SECURITYGROUP"].ToString() == PermissionsModel.Permissions.IDMPermissions.IDMGroup)))
                                     select (string)data["DESCRIPTION"];
                        if (matchq.Count() > 0)
                        {
                            SelectedStartActivity = matchq.FirstOrDefault();
                            MpName = StartActivities.Where(a => a.startdescription == SelectedStartActivity)
                                     .Select(b => b.mapName).FirstOrDefault();
                            ActName = StartActivities.Where(a => a.startdescription == SelectedStartActivity)
                                     .Select(b => b.activityName).FirstOrDefault();

                        }
                        else
                        {// Still not found ,try for * and "" and dont use usergroup
                            var Finalmatchq = from data in activities.AsEnumerable()
                                              where ((data["DOCTYPE"].ToString() == "") || (data["DOCTYPE"].ToString() != "")) && ((data["DOCDET"].ToString() == "" || (data["DOCDET"].ToString() != "")) && ((data["SOURCETYPE"].ToString() == "") || (data["SOURCETYPE"].ToString() != "")))
                                              select (string)data["DESCRIPTION"];
                            if (Finalmatchq.Count() > 0)
                            {
                                SelectedStartActivity = Finalmatchq.FirstOrDefault();
                                MpName = StartActivities.Where(a => a.startdescription == SelectedStartActivity)
                                         .Select(b => b.mapName).FirstOrDefault();
                                ActName = StartActivities.Where(a => a.startdescription == SelectedStartActivity)
                                         .Select(b => b.activityName).FirstOrDefault();

                            }

                        }

                    }

                }
            }
            getSelectedActivity(selectedStartActivity, doctype, docdetail);

        }

        public void getSelectedActivity(string selectedactivity, string doctype, string docdetail)
        {
            if ((selectedStartActivity == "Patient Services Supervisor") && (IsChkOverwriteSelected == false) && (doctype == "M1"))
            {
                SelectedStartActivity = "Patient Services Department";
                MpName = StartActivities.Where(a => a.startdescription == SelectedStartActivity)
                                 .Select(b => b.mapName).FirstOrDefault();
                ActName = StartActivities.Where(a => a.startdescription == SelectedStartActivity)
                        .Select(b => b.activityName).FirstOrDefault();
            }
            if ((selectedactivity == "Patient Services Supervisor") && (IsChkOverwriteSelected == false) && (doctype == "CO") && (docdetail == "PL"))
            {
                SelectedStartActivity = "Patient Liaison Department";
                MpName = StartActivities.Where(a => a.startdescription == SelectedStartActivity)
                                 .Select(b => b.mapName).FirstOrDefault();
                ActName = StartActivities.Where(a => a.startdescription == SelectedStartActivity)
                        .Select(b => b.activityName).FirstOrDefault();

            }

        }


        public string ShowDataByDocNo
        {
            get { return showDatabydoc; }
            set
            {
                showDatabydoc = value;
                OnPropertyChanged("ShowDataByDocNo");
            }
        }

       
        public bool ShowData
        {
            get { return showData; }
            set
            {
                showData = value;
                OnPropertyChanged("ShowData");
            }
        }

        public string DocNo
        {
            get
            {
                return docNo;
            }
            set
            {

                docNo = value;
                OnPropertyChanged("DocNo");
            }


        }

        public string ParentDocno
        {
            get
            {
                return parentDocno;
            }
            set
            {
             
                parentDocno = value;
                OnPropertyChanged("ParentDocno");
            }


        }
        //StartActivity
        public ObservableCollection<Model.WorkModel.StartActivity> StartActivities
        {
            get
            {
                return _startActivity;
            }
            set
            {
                _startActivity = value;
            }
        }
        public string SelectedStartActivity
        {
            get { return this.selectedStartActivity; }
            set
            {
                this.selectedStartActivity = value;
                validationErrors.Remove("SelectedStartActivity");
                OnPropertyChanged("SelectedStartActivity");
            }
        }

        //Needs Prepping
        public bool IsChkNeedsPrepSelected
        {
            get { return modelObj.NPrepping; }
            set
            {
                modelObj.NPrepping = value;
                if (modelObj.NPrepping == true)
                {
                    OnPropertyChanged("StartActivities");
                    this.StartActivities = getActivities();
                    UpdateRoutingInfo(SelectedDocumentType, SelectedDocumentDetail, SelectedDocumentSource);
                }
                if (modelObj.NPrepping == false)
                {                    
                        if ((GetDataByDocNumber.Rows.Count != 0))
                        {
                            OnPropertyChanged("StartActivities");
                            this.StartActivities = getActivities();
                            UpdateRoutingInfo(SelectedDocumentType, SelectedDocumentDetail, SelectedDocumentSource);
                        }
                    }
               

               
                NotifyPropertyChanged("IsChkNeedsPrepSelected");
            }
        }

        public string SelectedDocumentType
        {
            get { return docType; }
            set
            {
                if (docType != value)
                {
                    docType = value;
                    OnPropertyChanged("SelectedDocumentType");
                }
            }
        }

        public string SelectedDocumentDetail
        {
            get { return docDetail; }
            set
            {
                if (docDetail != value)
                {
                    docDetail = value;
                    OnPropertyChanged("SelectedDocumentDetail");
                }
            }
        }

        public string SelectedDocumentSource
        {
            get { return docSource; }
            set
            {
                if (docSource != value)
                {
                    docSource = value;
                    OnPropertyChanged("SelectedDocumentSource");
                }
            }
        }
        public bool IsBusy
        {
            get
            {
                return isbusy;
            }
            set
            {
                isbusy = value;
                NotifyPropertyChanged("IsBusy");
            }
        }
        public bool IsEnable
        {
            get
            {
                return isEnable;
            }
            set
            {
                isEnable = value;
                NotifyPropertyChanged("IsEnable");
            }
        }

        public bool IsEnableOverwrite
        {
            get
            {
                return isEnableOverwrite;
            }
            set
            {
                isEnableOverwrite = value;
                NotifyPropertyChanged("IsEnableOverwrite");
            }
        }

        //Retrieving OverwritingDocCheckBox Selection        
        public bool IsChkOverwriteSelected
        {
            get { return modelObj.OWriteDRoutingInfo; }
            set
            {
                if (modelObj.OWriteDRoutingInfo = value) return;

                modelObj.OWriteDRoutingInfo = value;
                NotifyPropertyChanged("IsChkOverwriteSelected");
            }
        }

        public int TowId
        {
            get { return towId; }
            set
            {
                if (towId != value)
                {
                    towId = value;
                    OnPropertyChanged("TowId");
                }
            }
        }

        public string PostDetailId
        {
            get { return postdetail_id; }
            set
            {
                if (postdetail_id != value)
                {
                    postdetail_id = value;
                    OnPropertyChanged("PostDetailId");
                }
            }
        }

        public string CourierInstId
        {
            get { return courier_inst_id; }
            set
            {
                if (courier_inst_id != value)
                {
                    courier_inst_id = value;
                    OnPropertyChanged("CourierInstId");
                }
            }
        }

        public string WorkFlowItemKey
        {
            get { return workflowitemkey; }
            set
            {
                if (workflowitemkey != value)
                {
                    workflowitemkey = value;
                    OnPropertyChanged("WorkFlowItemKey");
                }
            }
        }

        public string WorkFlowThreadKey
        {
            get { return workflowthreadkey; }
            set
            {
                if (workflowthreadkey != value)
                {
                    workflowthreadkey = value;
                    OnPropertyChanged("WorkFlowThreadKey");
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            }
        }

        //Validations
        public void Validate()
        {
            validationErrors.Clear();          

            if ((SelectedStartActivity == "") || (SelectedStartActivity == null))
            {
                validationErrors.Add("SelectedStartActivity", "SelectedStartActivity is mandatory.");
            }           

            // Call OnPropertyChanged(null) to refresh all bindings and have WPF check the this[string columnName] indexer.
            OnPropertyChanged(null);
        }      

        public string Error
        {
            get { return this[string.Empty]; }
        }

        public string this[string columnName]
        {
            get
            {
                if (validationErrors.ContainsKey(columnName))
                {
                    return validationErrors[columnName];
                }
                return null;
            }
        }






    }

}
