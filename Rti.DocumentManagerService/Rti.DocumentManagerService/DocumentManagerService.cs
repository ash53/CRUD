using System.Collections.Generic;
using System.ServiceProcess;
using Rti.DocumentManagerServer.Threads;

namespace Rti.DocumentManagerService
{
    partial class DocumentManagerService : ServiceBase
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<PeriodicThread> _periodicThreads;

        public DocumentManagerService()
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
            Log.DebugFormat("########## Server Starting up, Build:[{0}]... ##########", Constants.RtiDllBuild);

            _periodicThreads = new List<PeriodicThread> 
            {
                new DocumentManagerMonitor(){Name="DocumentManagerMonitor", Interval=Constants.MONITOR_INTERVAL},
                new ServiceMonitor<DocumentManagerServer.Servers.DocumentManagerServer>(){Name="DocumentManagerServer"}
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
