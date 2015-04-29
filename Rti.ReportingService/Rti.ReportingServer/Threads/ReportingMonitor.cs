
namespace Rti.ReportingServer.Threads
{
    public class ReportingMonitor : PeriodicThread
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ReportingMonitor()
            : base("ReportingMonitor")
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
