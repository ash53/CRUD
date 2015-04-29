using System.Collections.Generic;
using System.ServiceProcess;
using Rti.EagleIqGatewayServer.Threads;

namespace Rti.EagleIqGatewayService
{
    partial class EagleIqGatewayService : ServiceBase
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<PeriodicThread> _periodicThreads;

        public EagleIqGatewayService()
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
                new EagleIqGatewayMonitor(){Name="EagleIqGatewayMonitor", Interval=Constants.MONITOR_INTERVAL},
                new ServiceMonitor<EagleIqGatewayServer.Servers.EagleIqGatewayServer>(){Name="EagleIqGatewayServer"}
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
