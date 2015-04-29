
namespace Rti.AccountManagerServer.Threads
{
    public class AccountManagerMonitor : PeriodicThread
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AccountManagerMonitor()
            : base("AccountManagerMonitor")
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
