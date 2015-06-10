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
            try
            {
                using (FabricRuntime fabricRuntime = FabricRuntime.Create())
                {
                    // This is the name of the ServiceType that is registered with FabricRuntime. 
                    // This name must match the name defined in the ServiceManifest. If you change
                    // this name, please change the name of the ServiceType in the ServiceManifest.
                    
                    fabricRuntime.RegisterServiceType(Service.ServiceTypeName, typeof(Service));

                    ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(Service).Name);

                    Thread.Sleep(Timeout.Infinite);
                }
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e);
                throw;
            }
        }

    }
}
