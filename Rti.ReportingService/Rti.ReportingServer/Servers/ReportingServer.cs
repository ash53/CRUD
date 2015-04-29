using System.ServiceModel;
using Rti.InternalInterfaces.ServiceContracts;

namespace Rti.ReportingServer.Servers
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    [System.Runtime.InteropServices.ClassInterface(System.Runtime.InteropServices.ClassInterfaceType.None)]
    public class ReportingServer : IReportingService
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool IsAlive()
        {
            Log.Debug("Alive!!!");
            return true;
        }
    }
}
