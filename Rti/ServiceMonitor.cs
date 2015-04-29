using System;
using System.ServiceModel;

namespace Rti
{
    //
    // Holds a service host and acts as a monitor to help ensure it stays available.
    //
    public class ServiceMonitor<T> : PeriodicThread where T : class
    {
        private readonly T _svcInstance;
        private ServiceHost _svcHost;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ServiceMonitor()
            : base(typeof(T).Name + "Monitor")
        { }

        public ServiceMonitor(T instance)
            : base(typeof(T).Name + "Monitor")
        {
            _svcInstance = instance;
        }

        public event Action OnServiceCreated;

        protected override void OnStop()
        {
            try
            {
                if (_svcHost != null &&
                    _svcHost.State != CommunicationState.Faulted &&
                    _svcHost.State != CommunicationState.Closing &&
                    _svcHost.State != CommunicationState.Closed)
                {
                    _svcHost.Close();
                    _svcHost = null;
                }
            }
            catch (Exception ex)
            {
                Log.Warn("Error stopping service:" + ex.Message);
            }

            base.OnStop();
        }

        protected override void Process()
        {
            if (_svcHost == null ||
                _svcHost.State == CommunicationState.Faulted ||
                _svcHost.State == CommunicationState.Closing ||
                _svcHost.State == CommunicationState.Closed)
            {
                if (_svcInstance == null)
                {
                    _svcHost = new ServiceHost(typeof(T));
                }
                else
                {
                    _svcHost = new ServiceHost(_svcInstance);
                }

                Log.Info("Created service");

                var handler = OnServiceCreated;
                if (handler != null)
                {
                    handler();
                }
            }

            if (_svcHost.State != CommunicationState.Opened &&
                _svcHost.State != CommunicationState.Opening)
            {
                try
                {
                    _svcHost.Open();
                    Log.Info("Opened service");
                }
                catch (Exception ex)
                {
                    Log.Warn("Error starting service:" + ex.Message);
                }
            }
        }
    }
}
