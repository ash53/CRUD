using System.Collections.Generic;
using System.ServiceProcess;
using Rti.AccountManagerServer.Threads;

namespace Rti.AccountManagerService
{
    partial class AccountManagerService : ServiceBase
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<PeriodicThread> _periodicThreads;

        public AccountManagerService()
        {
            InitializeComponent();
        }

        public void Start()
        {
            OnStart(new string[0]);
        }

        protected override void OnStart(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            Log.Debug("########## Server Starting up... ##########");

            _periodicThreads = new List<PeriodicThread> 
            {
                new AccountManagerMonitor(){Name="AccountManagerMonitor", Interval=15000},
                new ServiceMonitor<AccountManagerServer.Servers.AccountManagerServer>(){Name="AccountManagerServer"}
            };

            // Starting server/worker/monitor threads
            _periodicThreads.ForEach(t => t.Start());
        }

        protected override void OnStop()
        {
            Log.Debug("########## Server is Shutting down... ##########");

            // Shutdown server/worker/monitor threads
            _periodicThreads.ForEach(t => t.Stop());
        }
    }
}
