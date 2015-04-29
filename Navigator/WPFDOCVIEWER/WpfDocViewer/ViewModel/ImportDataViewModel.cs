using System;
using System.Data;
using System.Windows.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Rti;
using Rti.InternalInterfaces.DataContracts;
using Rti.InternalInterfaces.ServiceProxies;
using WpfDocViewer.Model;
using System.Globalization;
using System.Collections;
using Rti.EncryptionLib;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;




namespace WpfDocViewer.ViewModel
{
    
    class ImportDataViewModel : INotifyPropertyChanged,IDataErrorInfo
    {
       
        private WpfDocViewer.Model.ImportDataModel modelObj;
        private string selectedDocumentType;
        private string selectedDocumentDetail;
        private string selectedDivision;
        private string selectedPractice;
        private string selectedStartActivity;
        private string selectedDocumentSource;
        private string _docNumber;
        private string selectedserviceId;
        private string ActName;
        private string MpName;
        private ObservableCollection<Model.ImportDataModel.DocumentType> _documentTypes;
        private ObservableCollection<Model.ImportDataModel.DocumentDetail> _documentDetails;
        private ObservableCollection<Model.ImportDataModel.StartActivity> _startActivity;
        private ObservableCollection<Model.ImportDataModel.DocumentSource> _documentSource;
        private List<string> _practice;
        private List<string> _serviceId;
        private List<string> _division;
        private List<string> importList;      

        readonly string _userName = CommonFunctions.GetUserName();
        readonly string _stationName = CommonFunctions.GetFqdn();
   

        EncryptionFuncs encryptpass = new EncryptionFuncs();
               
        DataAccessLayer.GetDocumentData DocumentService = new DataAccessLayer.GetDocumentData();
        DocumentManagerServiceClient _documentManagerService = 
            new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME);
       
        ResourceDictionary resources = App.Current.Resources;
       
        //Variables used for Submit function
        private string fileName;
        private string newFileName;
        private string towId;
        //private int ifnds = 0;
        //private string ifnid = "";
        private string postdetail_id;
        private string courier_inst_id;
        private string sAction = "F2P";
        private string workflowitemkey;
        private string workflowthreadkey;
        private decimal? mapInstId;
        private decimal? actInstId;
        private bool isbusy;
        private bool isOverwriteEnabled;
        public Dictionary<string, string> validationErrors = new Dictionary<string, string>();

        public ImportDataViewModel()
        {

             modelObj = new Model.ImportDataModel();
            _documentTypes = new ObservableCollection<Model.ImportDataModel.DocumentType>();
            _documentDetails = new ObservableCollection<Model.ImportDataModel.DocumentDetail>();
            _documentSource = new ObservableCollection<Model.ImportDataModel.DocumentSource>();
            _startActivity = new ObservableCollection<Model.ImportDataModel.StartActivity>();
            _practice = new List<string>();
            _division = new List<string>();
            _serviceId = new List<string>();
            _docNumber = getDocNumber();
            getDocumentTypes();        
            getDocumentDetails(selectedDocumentType);
            if ((PermissionsModel.Permissions.IDMPermissions.IDMGroup.ToUpper().Contains("SUPER") == true) || (PermissionsModel.Permissions.IDMPermissions.IDMGroup.ToUpper() == "ISADMIN"))
            {
                IsOverwriteEnabled = true;
            }
            else
            {
                IsOverwriteEnabled = false;
            }
            getActivitiesbyDocumentType(selectedDocumentType);
            getDocumentSource();
            getActivities();
            getPractices();         
            getDivisions(selectedPractice);
            getServiceIdList(selectedPractice);
            ImportList = Model.AttachmentClass.Filelist;
            getSelectedActivity(selectedStartActivity, selectedDocumentType, selectedDocumentDetail);            

          
            this.SubmitCommand = new RelayCommand(OnSubmit);
            this.CancelCommand = new RelayCommand(OnCancel);
        }
      
        //Populate DocumentType Combobox
        public ObservableCollection<Model.ImportDataModel.DocumentType> getDocumentTypes()
        {
            DataTable docTypes = DocumentService.GetDocTypes(Environment.MachineName, Environment.UserName);
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


        //Populate DocumentDetails Combobox on selection of documentType
        public ObservableCollection<Model.ImportDataModel.DocumentDetail> getDocumentDetails(string doctype)
        {
            DataTable docDetail = DocumentService.GetDocTypes(Environment.MachineName, Environment.UserName);

            var q = from data in docDetail.AsEnumerable()
                    where data.Field<string>("TYPE_KEY") == doctype && data.Field<string>("DETAIL_KEY") != null && data.Field<string>("DETAIL_DESCRIPTION") != null
                    select new Model.ImportDataModel.DocumentDetail()
                    {
                        detailkey = data.Field<string>("DETAIL_KEY"),
                        detaildescription = data.Field<string>("DETAIL_DESCRIPTION"),
                        fullDetailDescription = data.Field<string>("DETAIL_DESCRIPTION") + "  " + "[" + data.Field<string>("DETAIL_KEY") + "]"

                    };
            _documentDetails = new ObservableCollection<ImportDataModel.DocumentDetail>(q);

            return _documentDetails;

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
                if ((doctype == null) || (docsource == null))
                    return;

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
            else if(IsChkNeedsPrepSelected == true)
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
                         where ((data["DOCTYPE"].ToString() == doctype) && data["DOCDET"].ToString() == docdetail && data["SOURCETYPE"].ToString() == docsource &&  data["SECURITYGROUP"].ToString() == PermissionsModel.Permissions.IDMPermissions.IDMGroup)
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
                        getSelectedActivity(selectedStartActivity, selectedDocumentType, selectedDocumentDetail);

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
               

        //Populate Practices
        public List<string> getPractices()
        {
            DataTable practices = DocumentService.GetPractices(Environment.MachineName, Environment.UserName);
            var query = (from data in practices.AsEnumerable()
                         select (string)data["PRACTICE"]).Distinct().ToList();
            _practice = query;

            return _practice;
        }

        //Populate ServiceIds
        public List<string> getServiceIdList(string currentPractice)
        {
            string Message;
            string status;
            bool isBilling;
            bool isCoding;
            bool isFacBilling;
            bool isFacCoding;
            
            DataTable servers = DocumentService.GetPractices(Environment.MachineName, Environment.UserName);
            DataView view = new DataView(servers);
            DataTable distinctServernames = view.ToTable(true, "PRACTICE", "IQSERVER_NAME");

            var query =from data in distinctServernames.AsEnumerable()
                         where (string)data["PRACTICE"] == currentPractice
                         select (string)data["IQSERVER_NAME"];
           string _sname = query.SingleOrDefault();
            //SaIqClientRTI file for QA needs to be there inorder for the below call to work.
           if (currentPractice != null)
           {
               bool result = _documentManagerService.PracServiceMode(_sname, currentPractice, out Message, out status, out isBilling, out isCoding, out isFacBilling, out isFacCoding);
               if (result)
               {
                   _serviceId = new List<string> { "B" };
                   if (isBilling && !isFacBilling)
                       _serviceId = new List<string> { "P" };
                   if (isFacBilling && !isBilling)
                       _serviceId = new List<string> { "F" };
                   
               }
           }      
            return _serviceId;
        }

        //Populate Divisons on selection of practice
        public List<string> getDivisions(string currentPractice)
        {
            DataTable divisions = DocumentService.GetPractices(Environment.MachineName, Environment.UserName);
            DataView view = new DataView(divisions);
            DataTable distinctDivisions = view.ToTable(true, "PRACTICE", "DIVISION");

            var query = from data in distinctDivisions.AsEnumerable()
                        where (string)data["PRACTICE"] == currentPractice
                        select (string)data["DIVISION"];

            _division = query.ToList();
            return _division;

        }

        //Populate Start Activity Combobox       
        public ObservableCollection<Model.ImportDataModel.StartActivity> getActivities()
        {
            
            if (IsChkNeedsPrepSelected == false)
            {
                _startActivity.Clear();
                DataTable activitiesOne = DocumentService.GetActivities(Environment.MachineName, Environment.UserName);

                DataTable distinctActivitiesOne = activitiesOne.DefaultView.ToTable(true);
                foreach (DataRow row in distinctActivitiesOne.Rows)
                 {
                     var obj1 = new Model.ImportDataModel.StartActivity()
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
                    var obj2 = new Model.ImportDataModel.StartActivity()
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

        //Populate Activities by DocumentType
        public void getActivitiesbyDocumentType(string currentdoctype)
        {
            DataTable activities = _documentManagerService.GetStartActivityJoinedPostDetail(Environment.MachineName, Environment.UserName);
                var query = from data in activities.AsEnumerable()
                            where data["DOCTYPE"].ToString() == currentdoctype
                            select (string)data["DESCRIPTION"];
                if (currentdoctype != null)
                {
                    SelectedStartActivity = query.FirstOrDefault();
                    if (SelectedStartActivity != null)
                    {
                        MpName = StartActivities.Where(a => a.startdescription == SelectedStartActivity)
                                     .Select(b => b.mapName).FirstOrDefault();
                        ActName = StartActivities.Where(a => a.startdescription == SelectedStartActivity)
                                 .Select(b => b.activityName).FirstOrDefault();
                    }     
                    
                }
        }

        //Populate Document Source
        public ObservableCollection<Model.ImportDataModel.DocumentSource> getDocumentSource()
        {
             DataTable docSource = DocumentService.GetDocSource(Environment.MachineName, Environment.UserName);
             DataView view = new DataView(docSource);
             DataTable distinctSource = view.ToTable(true);
             foreach (DataRow row in distinctSource.Rows)
            {
                var obj = new Model.ImportDataModel.DocumentSource()
                {
                    sourceTypecode = row.Field<string>("TYPECODE"),
                    sourceDescription = row.Field<string>("DESCRIPTION"),
                    fullsourceDescription = row.Field<string>("DESCRIPTION") + "  " + "[" + row.Field<string>("TYPECODE") + "]",
                };
                _documentSource.Add(obj);
            }

            return _documentSource;
        }


        //Populate Document Number,OriginalDocNumber and ParentDocNumber
        public string getDocNumber()
        {
            string docno = DocumentService.GetDocNumber(Environment.MachineName, Environment.UserName);
            return docno;

        }

        //Submitting Data
        public RelayCommand SubmitCommand
        {
            get;
            private set;

        }

      
        public void OnSubmit()
        {

            IsBusy = true;
               
                string strAction = "AddDoc";
                //Save form data to database           
                string userPwd = Model.PermissionsModel.Permissions.EncodedPassword;

                List<string> files = Model.AttachmentClass.Filelist;

                if ((IsChkOverwriteSelected == true) && (IsRemitSelected == false) && ((PermissionsModel.Permissions.IDMPermissions.IDMGroup.ToUpper().Contains("SUPER") == true) || (PermissionsModel.Permissions.IDMPermissions.IDMGroup.ToUpper() == "ISADMIN")))
                {
                    MpName = StartActivities.Where(a => a.startdescription == SelectedStartActivity)
                          .Select(b => b.mapName).SingleOrDefault();
                    ActName = StartActivities.Where(a => a.startdescription == SelectedStartActivity)
                             .Select(b => b.activityName).SingleOrDefault();
                }


                resources["SearchType"] = "Docno";
                resources["SearchValue"] = DocumentNumber;



                // Import Image
                var navImages = new List<NavImage>();

                List<string> Files = ImportList;
                if (Files != null)
                {
                    foreach (var fn in Files)
                    {
                        //always will be 1 for every import but multi-page tif
                        //TODO calculate the npages when its a multi-page tif.
                        var navImage = new NavImage();
                        {
                            for (int i = 1; i <= Files.Count; i++)
                            {
                                String guid = Guid.NewGuid().ToString();
                                string ext = System.IO.Path.GetExtension(fn);
                                string filename = System.IO.Path.GetFileName(fn);
                                string unixPath = Constants.ImportImageSftpPath;
                                string winPath = Constants.ImportImageNASPath;
                                string uniqueid = _userName + "_" + DocumentNumber + "_" + guid + ext;

                                string winfn = winPath + @"\" + uniqueid;
                                System.IO.File.Copy(fn, winfn, true);
                                string unixfn = unixPath + @"/" + uniqueid;

                                navImage.action = "Import";
                                navImage.filename = winfn + "|" + unixfn;
                                navImage.seq = i;
                                navImage.docNo = DocumentNumber;
                                navImage.parDocNo = DocumentNumber;
                                navImage.origDocNo = DocumentNumber;
                                navImage.DocGroup = "";
                                navImage.docType = SelectedDocumentType;
                                navImage.docDet = SelectedDocumentDetail;
                                navImage.docSrc = SelectedDocumentSource;
                                navImage.acctNo = AccountNumber;
                                navImage.practice = SelectedPractice;
                                navImage.div = SelectedDivision;
                                navImage.serviceId = SelectedServiceId;
                                navImage.deptCode = DepartmentCode;
                                navImage.extPayor = ExtPayor;
                                navImage.checkAmt = Convert.ToString(CheckAmount);
                                navImage.checkNum = Convert.ToString(CheckNumber);
                                navImage.checkDt = (DepositDate == null) ? DateTime.Now.ToShortDateString() : DepositDate;
                                navImage.cStatus = "X";
                                navImage.supervisorId = PermissionsModel.Permissions.AssemblyPermissions.SupervisorID;
                                navImage.nPages = 1;

                            }
                        }

                        navImages.Add(navImage);
                    }
                }

                var idgparam = new IDGInputParams();
                {
                    idgparam.IsChkNeedsPrep = IsChkNeedsPrepSelected;
                    idgparam.IsRemitOnly = IsRemitSelected;
                    idgparam.strMapName = MpName;
                    idgparam.strActName = ActName;
                    if (SelectedStartActivity == "Auto-Post")
                        idgparam.strInformational = "E";
                    else
                        idgparam.strInformational = "Y";
                    idgparam.strReason = "";
                }

                var submitDocResults = _documentManagerService.SubmitDocuments(_stationName, _userName, userPwd, strAction, navImages, idgparam);

                if (submitDocResults.IsSuccess)
                {
                    towId = submitDocResults.strTowId;
                    postdetail_id = submitDocResults.strPostdetailId;
                    courier_inst_id = submitDocResults.strCourierInstId;
                    workflowitemkey = submitDocResults.strWfitemkey;
                    workflowthreadkey = submitDocResults.strWfthreadkey;
                }
                else
                {
                    if ((submitDocResults.strTowId != null) && (submitDocResults.strTowId != ""))
                        towId = submitDocResults.strTowId;
                    if ((submitDocResults.strPostdetailId != null) && (submitDocResults.strPostdetailId != ""))
                        postdetail_id = submitDocResults.strPostdetailId;
                    if ((submitDocResults.strCourierInstId != null) && (submitDocResults.strCourierInstId != ""))
                        courier_inst_id = submitDocResults.strCourierInstId;
                    MessageBox.Show(submitDocResults.OutMessage);
                }

                /*Commented this section as the program now handles this from server side call instead of clientside.*/
                //    //ImportImage call inserts record into postdoc table          
                //    var importImageResults = _documentManagerService.ImportImages(_stationName, _userName, navImages);

                //    if (importImageResults != null)
                //    {
                //        if(importImageResults.IsSuccess)
                //        {
                //            //Retrieving  the first row 
                //            fileName = importImageResults.OutDataTable.Rows[0]["filename"].ToString();
                //            towId = Convert.ToInt64(importImageResults.OutDataTable.Rows[0]["outTowId"]);
                //            ifnds = Convert.ToInt32(importImageResults.OutDataTable.Rows[0]["outIFNDs"]);
                //            ifnid = importImageResults.OutDataTable.Rows[0]["outIFNId"].ToString();

                //            foreach (DataRow row in importImageResults.OutDataTable.Rows)
                //            {
                //                if (importImageResults.OutDataTable.Rows.Count > 1)
                //                {
                //                    string fileNameVar = row["filename"].ToString();
                //                    long towIdVar = Convert.ToInt64(row["outTowId"]);
                //                    int ifndsVar = Convert.ToInt32(row["outIFNDs"]);
                //                    string ifnidVar = row["outIFNId"].ToString();
                //                }
                //            }          
                //        }
                //        else
                //        {
                //            //fancy messagebox
                //            MessageBox.Show("Failed to Import Image ");
                //            return;
                //        }
                //    }
                //    else
                //    {
                //       //fancy message box
                //        MessageBox.Show("Failed run RPC Import Image ");
                //        return;
                //    }


                //    //Insert Into Postdetail.
                //    //Towid,ifnds and ifnid from importimage call output are passed to InsertIntoPostdetail call.
                //    try
                //    {
                //        //Based on the selected map name,the respective towid is passed.
                //        //If its RTI Remit Processing,postdetail.towid is passed
                //        //If its RTI Bulk Post-Billing or RTI Hospital Update ,postdoc.towid is passed.
                //        if ((!(IsChkNeedsPrepSelected)) || (MpName == "RTI Remit Processing"))
                //        {
                //            var postDetail = new NavPostDetail();
                //            postDetail.TOWID = towId;
                //            postDetail.SVCDT = DateTime.Now;                 
                //            postDetail.PAIDAMT = (decimal?)CheckAmount;
                //            postDetail.EMBACCT = AccountNumber;
                //            postDetail.ROUTEINFO = "";
                //            postDetail.STATUS = "X";
                //            postDetail.DOCPAGE = 1;
                //            postDetail.MODIFYUID = Environment.UserName;
                //            postDetail.MODIFYDATE = DateTime.Now;
                //            postDetail.CREATEUID = Environment.UserName;
                //            postDetail.CREATEDATE = DateTime.Now;
                //            postDetail.TOP = 0;
                //            postDetail.LEFT = 0;
                //            postDetail.HEIGHT = 0;
                //            postDetail.WIDTH = 0;
                //            postDetail.XSCALE = 0;
                //            postDetail.YSCALE = 0;
                //            postDetail.ORIENTATION = 0;
                //            postDetail.HSCROLL = 0;
                //            postDetail.VSCROLL = 0;
                //            postDetail.IFN = ifnid;
                //            postDetail.NEXT_ACT_NAME = "";
                //            postDetail.NEXT_MAP_NAME = "";
                //            postDetail.REASON = "";
                //            if (SelectedStartActivity == "Auto-Post")
                //                postDetail.INFORMATIONAL = "E";
                //            else
                //            postDetail.INFORMATIONAL = "Y";
                //            postDetail.PRACTICE = SelectedPractice;
                //            postDetail.IFNDS = ifnds;
                //            postDetail.IFNID = Convert.ToInt32(ifnid);
                //            postDetail.ORIG_TOWID = 0;
                //            postDetail.ALT_TOWID = 0;
                //            postDetail.COMINGLED_STATUS = "N";
                //            postDetail.SERVICEID = SelectedServiceId;

                //            uint postdid = _documentManagerService.InsertIntoPostDetail(_stationName, _userName, postDetail);
                //            postdetail_id = Convert.ToInt32(postdid);
                //        }
                //    }
                //    catch(Exception ex)
                //    { throw ex; }
                //    //Needs Prep Selected
                //    if(IsChkNeedsPrepSelected == true)
                //    {
                //        MpName = "RTI Bulk Post-Billing";
                //        ActName = "RTI Prep Documents";
                //    }



                //    //Insert Into Deplog 
                //     try
                //     {
                //         if ((SelectedDocumentType == "CK" || SelectedDocumentType == "CE") && (SelectedDocumentDetail != null))
                //         {
                //             //Is this cash2post
                //             var depositLogInsertResults = _documentManagerService.InsertDepositLog(SelectedPractice, DepositDate, SelectedDocumentDetail, SelectedDocumentType, ParentDocumentNumber
                //                 , Convert.ToString(DateTime.Today), Convert.ToDouble(CheckAmount), ExtPayor, CheckNumber, Convert.ToInt32(SequenceNumber));
                //         }
                //         else if(SelectedDocumentType == "RM")
                //         {
                //             var remitToPost = _documentManagerService.CreateR2P(SelectedPractice, SelectedDocumentDetail, SelectedDocumentType, ParentDocumentNumber,
                //             CheckAmount, ExtPayor, CheckNumber, sAction);

                //         }
                //     }
                //    catch(Exception ex)
                //     { throw ex; }




                //    //FlowareLibrary
                //    try{
                //    if(towId != 0)
                //    {  
                //        //postdetail and postdoc tables are joined based on ifnds and ifnid with  towerid as input. 
                //        ServiceReferenceFlowareWeb.FlowareWebServiceManagerClient flowareClient = new ServiceReferenceFlowareWeb.FlowareWebServiceManagerClient();
                //       courier_inst_id = flowareClient.createWorkItem(ActName, (int)towId);              

                //       //To retrieve Act_Node_Id and Map_Inst_Id

                //           var results = _documentManagerService.GetPostBdMapInstIdNodeId(_stationName, _userName, courier_inst_id);
                //           mapInstId = results.Item1;
                //           actInstId = results.Item2;              

                //       if (courier_inst_id > 0)
                //       { 

                //        //Create WorkflowItem
                //        var workFlowItem = new NavWorkFlowItem();
                //        workFlowItem.practice = SelectedPractice;
                //        workFlowItem.division = SelectedDivision;
                //        DateTime? depdt = (DepositDate !=null) ? DateTime.Parse(DepositDate) : DateTime.Now;
                //        workFlowItem.depDate = depdt;
                //        workFlowItem.recDate = DateTime.Now;
                //        workFlowItem.docType = SelectedDocumentType; 
                //        workFlowItem.docNo = DocumentNumber;
                //        workFlowItem.parDocNo = ParentDocumentNumber;
                //        workFlowItem.embAcctNo =AccountNumber;
                //        workFlowItem.checkNum = Convert.ToString(CheckNumber);
                //        workFlowItem.checkAmt = Convert.ToString(CheckAmount);
                //        workFlowItem.paidAmt = Convert.ToString(CheckAmount); ;
                //        workFlowItem.extPayorCd = ExtPayor;
                //        workFlowItem.depCode = DepartmentCode;
                //        workFlowItem.docGroup = "";
                //        workFlowItem.docDetail = SelectedDocumentDetail;             
                //        workFlowItem.courier_Inst_Id = courier_inst_id;
                //        workFlowItem.acctNum = AccountNumber;
                //        workFlowItem.act_Name = ActName;
                //        workFlowItem.map_Name = MpName;
                //        workFlowItem.act_Node_Id = (int?)actInstId;
                //        workFlowItem.map_Inst_Id = (int?)mapInstId;
                //        workFlowItem.svcDate = DateTime.Now;
                //        workFlowItem.docPage = 1;             
                //        workFlowItem.postdetail_Id = postdetail_id;
                //        workFlowItem.regionLeft = null;
                //        workFlowItem.regionTop =null;
                //        workFlowItem.finClass =null ;
                //        workFlowItem.escReason = null;
                //        workFlowItem.corrFolder = null;
                //        workFlowItem.serviceId = SelectedServiceId;
                //        workFlowItem.reason = null;
                //        workFlowItem.informational = "Y";
                //        var createWorkflowResult = _documentManagerService.CreateWorkflowItem(_stationName, _userName, workFlowItem);
                //        workflowitemkey = Convert.ToInt32(createWorkflowResult.OutDataTable.Rows[0]["wfitemkey"]);
                //        workflowthreadkey = Convert.ToInt32(createWorkflowResult.OutDataTable.Rows[0]["wfthreadkey"]);
                //        }
                //    }
                //}
                // catch(Exception ex)
                //     { throw ex; }
                IsBusy = false;

           
      }       


        //Cancelling Data
        public RelayCommand CancelCommand
        {
            get;
            private set;

        }

        public void OnCancel()
        {
            //Clear Validations

            validationErrors.Clear();
            NotifyPropertyChanged("SelectedPractice");
            NotifyPropertyChanged("SelectedDocumentType");
            NotifyPropertyChanged("DepositDate");

              //Cancel all the selections 
                DepartmentCode = null;
                ExtPayor = null;
                AccountNumber = null;              
                IsChkAutoSelected = false;
                IsChkManualSelected = false;
                IsChkOverwriteSelected = false;
                IsChkNeedsPrepSelected = false;
                IsRemitSelected = false;
                CheckNumber = null;
                CheckAmount = 0;
                DepositDate = null;
                SequenceNumber = 0;
        }

        //DocumentNumber
        public string DocumentNumber
        {
            get { return _docNumber; }
            set
            {
                if (_docNumber != value)
                {
                    _docNumber = value;
                    OnPropertyChanged("DocumentNumber");
                }
            }
        }
        public string ParentDocumentNumber
        {
            get { return _docNumber; }
            set
            {
                _docNumber = value;
                OnPropertyChanged("ParentDocumentNumber");

            }
        }
        public string OriginalDocumentNumber
        {
            get { return _docNumber; }
            set
            {
                _docNumber = value;
                OnPropertyChanged("OriginalDocumentNumber");

            }
        }

        //Document Types
        public ObservableCollection<Model.ImportDataModel.DocumentType> DocumentTypes
        {
            get
            {
                return _documentTypes;
            }     
            set
            {
                _documentTypes = value;
            }
        }
        public string SelectedDocumentType
        {
            get { return this.selectedDocumentType; }
            set
            {
           if(selectedDocumentType != value)
           {
                    this.selectedDocumentType = value;
                    validationErrors.Remove("SelectedDocumentType");
               if((selectedDocumentType == "CK" )||(selectedDocumentType == "CE"))
               { ExtPayor = "COM"; }
               else { ExtPayor = ""; }
                    NotifyPropertyChanged("SelectedDocumentType");
                    this.DocumentDetails = getDocumentDetails(selectedDocumentType);
                    OnPropertyChanged("DocumentDetails");
                    getActivitiesbyDocumentType(selectedDocumentType);
           }
            }
                   
        }

        //DocumentDetails 
        public ObservableCollection<Model.ImportDataModel.DocumentDetail> DocumentDetails
        {
            get
            {
                    if (!(_documentDetails.Count == 0))
                    {
                        SelectedDocumentDetail = _documentDetails[0].detailkey;

                    }
                return _documentDetails;
             }
            set
            { 
                _documentDetails = value;
                OnPropertyChanged("DocumentDetails");
            }
        }
        public string SelectedDocumentDetail
        {
            get {
                return this.selectedDocumentDetail;
            
            }
            set
            { 
                this.selectedDocumentDetail = value;
                UpdateRoutingInfo(selectedDocumentType, selectedDocumentDetail, selectedDocumentSource);
                OnPropertyChanged("SelectedDocumentDetail");
            }
        }

        //DocumentSource
        public ObservableCollection<Model.ImportDataModel.DocumentSource> DocumentSources
        {
            get
            {
                if (!(_documentSource.Count == 0))
                    {
                        SelectedDocumentSource = _documentSource[0].sourceTypecode;
                    }
                return _documentSource;
            }
            set
            {
                _documentSource = value;
                OnPropertyChanged("DocumentSources");
            }
        }
        public string SelectedDocumentSource
        {
            get { return this.selectedDocumentSource; }
            set
            {              
                this.selectedDocumentSource = value;
                UpdateRoutingInfo(selectedDocumentType, selectedDocumentDetail, selectedDocumentSource);
                NotifyPropertyChanged("SelectedDocumentSource");
            }
        }


        //ServiceId
        public List<string> ServiceIds
        {
            get
            {
                    if (!(_serviceId.Count == 0))
                    {
                        SelectedServiceId = _serviceId[0];
                    }
                return _serviceId;
            }
            set
            {
                _serviceId = value;
                OnPropertyChanged("ServiceIds");
            }
        }
        public string SelectedServiceId
        {

            get { return this.selectedserviceId; }
            set
            {
                this.selectedserviceId = value;
                OnPropertyChanged("SelectedServiceId");
            }
        }

       
        //Division
        public List<string> Divisions
        {
            get
            {
                    if (!(_division.Count == 0))
                    {
                        SelectedDivision = _division[0];
                    }
               
                return _division;
            }
            set
            {
                _division = value;
                OnPropertyChanged("Divisions");
            }
        }
        public string SelectedDivision
        {

            get { return this.selectedDivision; }
            set
            {
                this.selectedDivision = value;
                OnPropertyChanged("SelectedDivision");
            }
        }


        //PracticeDetails 
        public List<string> Practice
        {
            get
            {
                return _practice;
            }
            set
            {
                _practice = value;

            }
        }
        public string SelectedPractice
        {
            get { return selectedPractice; }
            set
            {
                if (this.selectedPractice != value)
                {
                    this.selectedPractice = value;
                    validationErrors.Remove("SelectedPractice");
                    this.Divisions = getDivisions(selectedPractice);
                    this.ServiceIds = getServiceIdList(selectedPractice);
                    OnPropertyChanged("SelectedPractice");
                }
            }
        }

        //AccountNumber
        public string AccountNumber
        {
            get { return modelObj.AccountNumber; }
            set
            {
                if (modelObj.AccountNumber != value)
                {
                    modelObj.AccountNumber = value;
                    OnPropertyChanged("AccountNumber");
                }
            }
        }

        //DepartmentCode
        public string DepartmentCode
        {
            get { return modelObj.DepartmentCode; }
            set
            {
                if (modelObj.DepartmentCode != value)
                {
                    modelObj.DepartmentCode = value;
                    OnPropertyChanged("DepartmentCode");
                }
            }
        }

        //EXtPayor
        public string ExtPayor
        {
            get { return modelObj.ExtPayor; }
            set
            {
                if (modelObj.ExtPayor != value)
                {
                    modelObj.ExtPayor = value;
                    OnPropertyChanged("ExtPayor");
                }
            }
        }

        //CheckNumber
        public string CheckNumber
        {
            get { return modelObj.CheckNumber; }
            set
            {
                if (modelObj.CheckNumber != value)
                {
                    modelObj.CheckNumber = value;
                    OnPropertyChanged("CheckNumber");
                }
            }
        }

        //CheckAmount
        public double CheckAmount
        {
            get { return modelObj.CheckAmount ; }
            set
            {
                if (modelObj.CheckAmount != value)
                {
                    modelObj.CheckAmount = value;
                    OnPropertyChanged("CheckAmount");
                }
            }
        }

        //SequenceNumber
        public int SequenceNumber
        {
            get { return modelObj.SequenceNumber ; }
            set
            {
                if (modelObj.SequenceNumber != value)
                {
                    modelObj.SequenceNumber = value;
                    OnPropertyChanged("SequenceNumber");
                }
            }
        }

        //DepositDate
        public string DepositDate
        {
            get { return modelObj.DepositDate; }
            set
            {
                if (modelObj.DepositDate != value)
                {

                    modelObj.DepositDate = value;
                    validationErrors.Remove("DepositDate");
                    OnPropertyChanged("DepositDate");
                }
            }
        }

        //StartActivity
        public ObservableCollection<Model.ImportDataModel.StartActivity> StartActivities
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
                OnPropertyChanged("SelectedStartActivity");
            }
        }

        //Retrieving AutogenerateCheckBox Selection 
        public bool IsChkAutoSelected
        {
            get { return modelObj.AutogenerateDSeq; }
            set
            {
                if (modelObj.AutogenerateDSeq = value) return;

                modelObj.AutogenerateDSeq = value;
                NotifyPropertyChanged("IsChkAutoSelected");
            }
        }

        //Retrieving ManuallyEnterCheckBox Selection        
        public bool IsChkManualSelected
        {
            get { return modelObj.ManuallyEnter; }
            set
            {
                if (modelObj.ManuallyEnter = value) return;

                modelObj.ManuallyEnter = value;
                NotifyPropertyChanged("IsChkManualSelected");
            }
        }

        //Retrieving RemitOnly Selection
        public bool IsRemitSelected
        {
            get { return modelObj.RemitOnly; }
            set
            {
                modelObj.RemitOnly = value;
                if (modelObj.RemitOnly == true)
                {
                    SelectedStartActivity = "";
                    MpName = "";
                    ActName = "";
                }
           if(modelObj.RemitOnly == false)
                {
                    UpdateRoutingInfo(selectedDocumentType, selectedDocumentDetail, selectedDocumentSource);

                }
               NotifyPropertyChanged("IsRemitSelected");
                
            }
        }

        //Retrieving OverwritingDocCheckBox Selection        
        public bool IsChkOverwriteSelected
        {
            get { return modelObj.OverWriteDRoutingInfo; }
            set
            {
                if (modelObj.OverWriteDRoutingInfo = value) return;
               
                modelObj.OverWriteDRoutingInfo = value;
                NotifyPropertyChanged("IsChkOverwriteSelected");
            }
        }

        //Needs Prepping
        public bool IsChkNeedsPrepSelected
        {
            get { return modelObj.NeedsPrepping; }
            set
            {
                modelObj.NeedsPrepping = value;              
                this.StartActivities = getActivities();
                UpdateRoutingInfo(selectedDocumentType, selectedDocumentDetail, selectedDocumentSource);             
                NotifyPropertyChanged("IsChkNeedsPrepSelected");
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


        public bool IsOverwriteEnabled
        {
            get
            {
                return isOverwriteEnabled;
            }
            set
            {
                isOverwriteEnabled = value;
                NotifyPropertyChanged("IsOverwriteEnabled");
            }
        }

        //ImportList Listbox
        public List<string> ImportList
        {
            get
            {

                return importList;
            }
            set
            {

                importList = value;
                OnPropertyChanged("ImportList");

            }
        }


        //Properties to bind to SubmitStatus screen
      
        public string TowId
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
        //Commented ifnid and ifnds as the service call doesnt give these.
        //public int Ifnds
        //{
        //    get { return ifnds; }
        //    set
        //    {
        //        if (ifnds != value)
        //        {
        //            ifnds = value;
        //            OnPropertyChanged("Ifnds");
        //        }
        //    }
        //}

        //public string IfnId
        //{
        //    get { return ifnid; }
        //    set
        //    {
        //        if (ifnid != value)
        //        {
        //            ifnid = value;
        //            OnPropertyChanged("IfnId");
        //        }
        //    }
        //}

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
            if (SelectedDocumentType == "" || SelectedDocumentType == null)
            {
                validationErrors.Add("SelectedDocumentType", "SelectedDocumentType is mandatory.");
            }

            if (((SelectedDocumentType == "CK") || (SelectedDocumentType == "CE")) && (string.IsNullOrEmpty(DepositDate)))
            {
                validationErrors.Add("DepositDate", "DepositDate is mandatory.");
            }
            if((SelectedPractice == "") || (SelectedPractice == null))
            {
                validationErrors.Add("SelectedPractice", "SelectedPractice is mandatory.");
            }


            // Call OnPropertyChanged(null) to refresh all bindings and have WPF check the this[string columnName] indexer.
            if(validationErrors.Count != 0)
            OnPropertyChanged(null);
        }


        public string Error
        {
            get { return null; }
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

