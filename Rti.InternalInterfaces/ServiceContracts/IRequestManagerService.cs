using System.ServiceModel;

namespace Rti.InternalInterfaces.ServiceContracts
{
    [ServiceContract]
    public interface IRequestManagerService
    {
        [OperationContract]
        bool IsAlive();
    }
}

