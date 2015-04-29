using Rti;
using Rti.InternalInterfaces.ServiceProxies;


namespace WpfDocViewer.DataAccessLayer
{
   public class GetDocumentServiceData
    {
       readonly DocumentManagerServiceClient docManagerServiceClient = 
           new DocumentManagerServiceClient(Constants.DocumentManagerServiceURL, Constants.DocumentManagerServiceENDPOINT_NAME);
       
    }
}
