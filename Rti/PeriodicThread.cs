using System;
using System.Diagnostics;
using System.Threading;

namespace Rti
{
    //
    // Represents a task that should be executed periodically in a separate thread.
    //
    public abstract class PeriodicThread
    {
        public string Name { get; set; }

        public int Interval { get; set; }

        public ulong Iterations { get; private set; }

        public DateTime StartTime { get; private set; }

        public DateTime StopTime { get; private set; }

        public StatusType Status { get; private set; }

        public string FaultMessage { get; private set; }

        protected PeriodicThread(string name)
        {
            Name = name;
            Iterations = 0;
            Interval = Settings.Get(Name + "Interval", -1);

            Status = StatusType.Offline;
            FaultMessage = "Waiting for Start";

            if (Interval == -1)
            {
                Log.Debug("Setting [" + name + "] not specified: using [" + _defaultInterval  + "] ms default.");

                Interval = _defaultInterval;
            }
            else
            {
                Log.Debug("Settings: Thread interval set to [" + Interval + "] ms");
            }
        }

        //
        // Starts the periodic thread.
        //
        public void Start()
        {
            var timer = Stopwatch.StartNew();

            _shutdown = false;
            _shutdownEvt = new ManualResetEvent(false);

            try
            {
                OnStart();
                Status = StatusType.Online;
                FaultMessage = string.Empty;
            }
            catch (Exception ex)
            {
                SetFault(ex.Message);
                Log.Error("Error on thread start: " + ex.Message);
                throw;
            }

            _thread = new Thread(Run) {Name = Name};
            _thread.Start();

            Log.Debug("Thread started (" + timer.ElapsedMilliseconds + " ms).");

            StartTime = DateTime.UtcNow;
        }

        //
        // Stops the periodic thread.
        //
        public void Stop()
        {
            _shutdown = true;
            if (_shutdownEvt != null && _thread != null && _thread.IsAlive)
            {
                Stopwatch.StartNew();

                _shutdownEvt.Set();

                _thread.Join();

                Status = StatusType.Offline;
                FaultMessage = "Waiting for Start";

                OnStop();

                Log.Debug("Thread stopped (" + _timeActive.ElapsedMilliseconds + " ms).");
            }

            StopTime = DateTime.UtcNow;
        }

        //
        // Called before the periodic thread's first cycle.
        //
        protected virtual void OnStart()
        {
        }

        //
        // Called each time the period thread cycles.
        //
        protected abstract void Process();

        //
        // Called after the periodic thread's last cycle.
        //
        protected virtual void OnStop()
        {
        }

        //
        // Cause the thread to execute its process method immediately.
        //
        public void Kick()
        {
            _shutdownEvt.Set();
        }

        private void Run()
        {
            while (!_shutdown)
            {
                try
                {
                    _timeActive.Start();

                    Process();
                    Iterations++;
                    _lastError = string.Empty;
                    _lastErrorInstances = 0;
                }
                catch (Exception ex)
                {
                    if (string.CompareOrdinal(_lastError, ex.Message) == 0)
                    {
                        _lastErrorInstances++;
                    }

                    if ((_lastErrorInstances % 100) == 0)
                    {
                        Log.Warn(String.Format(
                            "{0} from thread process (#{1}): {2}",
                            ex.GetType().Name,
                            (_lastErrorInstances + 1),
                            ex.Message
                        ));
                        _lastError = ex.Message;
                    }
                }
                _timeActive.Stop();

                _shutdownEvt.WaitOne(Interval);
                _shutdownEvt.Reset();
            }
        }

        protected void SetFault(string message)
        {
            Status = StatusType.Failed;
            FaultMessage = message;
        }

        protected void ClearFault()
        {
            Status = StatusType.Online;
            FaultMessage = string.Empty;
        }

        private readonly int _defaultInterval = Settings.Get("PeriodicThreadDefaultInterval", 500);
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Stopwatch _timeActive = new Stopwatch();
        private string _lastError = string.Empty;
        private uint _lastErrorInstances;
        private bool _shutdown;
        private ManualResetEvent _shutdownEvt;
        private Thread _thread;
    }
}