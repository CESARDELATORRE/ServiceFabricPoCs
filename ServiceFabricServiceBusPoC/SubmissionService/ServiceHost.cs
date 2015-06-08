using System;
using System.Diagnostics;
using System.Fabric;
using System.Threading;

namespace SubmissionService
{
    public static class ServiceHost
    {
        public static void Main(string[] args)
        {
            try
            {
                using (FabricRuntime fabricRuntime = FabricRuntime.Create())
                {
                    Trace.WriteLine("Starting Service Host for Web API Service.");

                    fabricRuntime.RegisterServiceType(Service.ServiceTypeName, typeof(Service));

                    ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, Service.ServiceTypeName);

                    Thread.Sleep(Timeout.Infinite);
                }
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e);
            }
        }
    }
}
