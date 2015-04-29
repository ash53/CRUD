using System.Data;
using Rti.InternalInterfaces.DataContracts;
using Rti.InternalInterfaces.ServiceContracts;

namespace Rti.InternalInterfaces.ServiceProxies
{
    public class AdministrationServiceClient : ServiceProxyBase<IAdministrationService>
    {
        public AdministrationServiceClient(string serviceEndpointUri, string serviceEndpointName)
            : base(serviceEndpointUri, serviceEndpointName)
        {
        }

        public bool IsAlive()
        {
            return Channel.IsAlive();
        }

        #region EmWare Collection Permissions

        public Login.Permissions EmWareCollectionLogin(string workstation, string username,
            Login.UserCredentials loginCredentials)
        {
            return Channel.EmWareCollectionLogin(workstation, username, loginCredentials);
        }

        #endregion

        //Updated By Shariq
        #region Service Controller

        public bool StopService(string workstation, string username, string serviceName)
        {
            return Channel.StopService(workstation, username, serviceName);
        }

        public bool StartService(string workstation, string username, string serviceName)
        {
            return Channel.StartService(workstation, username, serviceName);
        }

        public DataTable ListServicesStatus(string workstation, string username)
        {
            return Channel.ListServicesStatus(workstation, username);
        }

        public NavServiceInfo GetServiceInfo(string workstation, string username)
        {
            return Channel.GetServiceInfo(workstation, username);
        }

        #endregion
    }
}
