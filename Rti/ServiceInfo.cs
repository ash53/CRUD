using System;
using System.Diagnostics;


namespace Rti
{
    public class NavServiceInfo
    {
        public string ServiceName { get; set; }
        public int ThreadCount { get; set; }
        public DateTime ServiceStartTime { get; set; }
        public TimeSpan TotalProcessorTime { get; set; }
        public bool Responding { get; set; }
        public string MachineName { get; set; }
        public DateTime ServiceBuildTime { get; set; }
    }

    public static class ServiceInfo
    {
        public static NavServiceInfo GetServiceInfo()
        {
            var currentProcess = Process.GetCurrentProcess();
            var serviceInfo = new NavServiceInfo()
            {
                MachineName = Environment.MachineName,
                Responding = currentProcess.Responding,
                ServiceName = currentProcess.ProcessName,
                ServiceStartTime = currentProcess.StartTime,
                ThreadCount = currentProcess.Threads.Count,
                TotalProcessorTime = currentProcess.TotalProcessorTime,
                ServiceBuildTime = CommonFunctions.RetrieveBuildTimestamp()
            };
            return serviceInfo;
        }
    }
}
