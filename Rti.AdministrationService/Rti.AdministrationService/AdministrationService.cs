using System;
using System.Collections.Generic;
using System.ServiceProcess;
using Rti.AdministrationServer.Threads;

namespace Rti.AdministrationService
{
    partial class AdministrationService : ServiceBase
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<PeriodicThread> _periodicThreads;

        public AdministrationService()
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
            Log.Debug(String.Format("Monitor interval:[{0}]", Constants.MONITOR_INTERVAL));

            _periodicThreads = new List<PeriodicThread> 
            {
                new AdministrationMonitor(){Name="AdministrationMonitor", Interval=Constants.MONITOR_INTERVAL},
                new ServiceMonitor<AdministrationServer.AdministrationServer>(){Name="AdministrationServer"}
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
