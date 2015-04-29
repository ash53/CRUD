using System.ServiceModel;

namespace Rti.InternalInterfaces.ServiceContracts
{
    [ServiceContract]
    public interface IAccountManagerService
    {
        [OperationContract]
        bool IsAlive();
    }
}

