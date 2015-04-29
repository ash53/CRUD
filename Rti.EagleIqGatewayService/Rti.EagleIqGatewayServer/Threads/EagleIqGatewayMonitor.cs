
namespace Rti.EagleIqGatewayServer.Threads
{
    public class EagleIqGatewayMonitor : PeriodicThread
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public EagleIqGatewayMonitor()
            : base("EagleIqGatewayMonitor")
        {
        }

        protected override void OnStart()
        {
            Log.Debug("########## Open for business! ##########");
        }

        protected override void Process()
        {
            Log.Debug("Looping...");
        }

        protected override void OnStop()
        {
            Log.Debug("########## Server shutdown requested! ##########");
            Stop();
        }
    }
}
