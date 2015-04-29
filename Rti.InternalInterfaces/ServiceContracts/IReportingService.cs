using System.ServiceModel;
using System.Collections.Generic;
using Rti.InternalInterfaces.DataContracts;

namespace Rti.InternalInterfaces.ServiceContracts
{
    [ServiceContract]
    public interface IReportingService
    {
        [OperationContract]
        bool IsAlive();
    }
}

