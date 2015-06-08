using System;
using System.Diagnostics;
using System.Fabric;
using System.Threading;
using System.IO;

namespace StatelessGatewayWebAPIService
{
    public static class ServiceHost
    {
        public static void Main(string[] args)
        {
            // Create a Windows Fabric Runtime
            using (FabricRuntime fabricRuntime = FabricRuntime.Create())
            using (TextWriterTraceListener trace = new TextWriterTraceListener(Path.Combine(FabricRuntime.GetActivationContext().LogDirectory, "out.log")))
            {
                Trace.AutoFlush = true;
                Trace.Listeners.Add(trace);

                try
                {
                    Trace.WriteLine("Starting Service Host for Stateless Gateway WebAPI Service.");

                    fabricRuntime.RegisterServiceType(Service.ServiceTypeName, typeof(Service));

                    ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, Service.ServiceTypeName);

                    Thread.Sleep(Timeout.Infinite);
                    GC.KeepAlive(fabricRuntime);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                    ServiceEventSource.Current.ServiceHostInitializationFailed(e);
                }
            }
        }

    }
}
