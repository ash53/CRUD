using System.ServiceModel;
using System.Data;
using Rti.AdministrationServer.DataAccess;
using Rti.InternalInterfaces.DataContracts;
using Rti.InternalInterfaces.ServiceContracts;

//Updated By Shariq

namespace Rti.AdministrationServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    [System.Runtime.InteropServices.ClassInterface(System.Runtime.InteropServices.ClassInterfaceType.None)]
    public class AdministrationServer : IAdministrationService
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static EmWareCollectionSecurity _emWareCollectionSecurity;
        //Updated By Shariq
        private static RACServiceController _racServiceController;

        public AdministrationServer()
        {
            _emWareCollectionSecurity = new EmWareCollectionSecurity();
            //Updated By Shariq
            _racServiceController = new RACServiceController();
        }

        /// <summary>
        ///  Provides a simple ping mechanism for server status
        /// </summary>
        /// <returns></returns>
        public bool IsAlive()
        {
            Log.Debug("Administration Server reporting IsAlive()");
            return true;
        }

        /// <summary>
        ///  Main EmWare Collection Login
        /// </summary>
        /// <param name="workstation"></param>
        /// <param name="username"></param>
        /// <param name="loginCredentials"></param>
        /// <returns></returns>
        public Login.Permissions EmWareCollectionLogin(string workstation, string username,
            Login.UserCredentials loginCredentials)
        {
            return _emWareCollectionSecurity.EmWareCollectionLogin(workstation, username, loginCredentials);
        }

        #region Service Management

        //Updated By Shariq
        public bool StopService(string workstation, string username, string serviceName)
        {
            return _racServiceController.StopService(workstation, username, serviceName);
        }

        public bool StartService(string workstation, string username, string serviceName)
        {
            return _racServiceController.StartService(workstation, username, serviceName);
        }

        public DataTable ListServicesStatus(string workstation, string username)
        {
            return _racServiceController.ListServicesStatus(workstation, username);
        }

        public NavServiceInfo GetServiceInfo(string workstation, string username)
        {
            return _racServiceController.GetServiceInfo(workstation, username);
        }

        #endregion ServiceControl

    }
}
