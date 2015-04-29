using System.ServiceModel;
using System.Data;
using Rti.InternalInterfaces.DataContracts;

namespace Rti.InternalInterfaces.ServiceContracts
{
    [ServiceContract]
    public interface IAdministrationService
    {
        [OperationContract]
        bool IsAlive();


        #region EmWare Collection Permissions

        [OperationContract]
        Login.Permissions EmWareCollectionLogin(string workstation, string username,
            Login.UserCredentials loginCredentials);

        #endregion


        //Updated By Shariq
        #region Service Controller

        [OperationContract]
        bool StopService(string workstation, string username, string serviceName);

        [OperationContract]
        bool StartService(string workstation, string username, string serviceName);

        [OperationContract]
        DataTable ListServicesStatus(string workstation, string username);

        [OperationContract]
        NavServiceInfo GetServiceInfo(string workstation, string username);

        #endregion

    }
}

