using System;
using System.Data;
using Rti;
using Rti.InternalInterfaces.ServiceProxies;


namespace WpfDocViewer.DataAccessLayer
{
    class GetDocumentData
    {
        //Creating a client object
        readonly DocumentManagerServiceClient _documentManagerService = 
            new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME);
        
        //Testing the Service
        public void something()
        {
            _documentManagerService.IsAlive();
        }
        
        //Load Document Types
        public DataTable GetDocTypes(string workstation,string username )
        {
            DataTable docTypes = _documentManagerService.GetDocTypes(Environment.MachineName, Environment.UserName);
            return docTypes;           
        }

       
        //Load Practices        
        public DataTable GetPractices(string workstation, string username)        
        {
            DataTable practices = _documentManagerService.ListPractices(Environment.MachineName, Environment.UserName);
            return practices;
        }


        //Load Start Activity 
        public DataTable GetActivities(string workstation, string username)
        {
            DataTable activities = _documentManagerService.ListPostDetailStatusLookups(Environment.MachineName, Environment.UserName);
            return activities;
        }


        //Load DocumentSource
        public DataTable GetDocSource(string workstation, string username)
        {
            DataTable docsource = _documentManagerService.GetSourceTypeLookupByTypeCode(Environment.MachineName, Environment.UserName);
            return docsource;
        }

        //Load DocNo,ParentDoc and OriginalParentdocNo
        public string GetDocNumber(string workstation, string username)
        {
            string docNo = _documentManagerService.CreateDocNo(Environment.MachineName, Environment.UserName);
            return docNo;
        }
    }
}
