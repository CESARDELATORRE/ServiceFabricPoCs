﻿using System;
using System.Diagnostics;
using System.Fabric;
using System.IO;
using System.Threading;

namespace StatefulWebAPIService
{
    public class ServiceHost
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
                    Trace.WriteLine("Starting Service Host for Stateful Web Service.");

                    fabricRuntime.RegisterServiceType(Service.ServiceTypeName, typeof(Service));

                    Thread.Sleep(Timeout.Infinite);
                    GC.KeepAlive(fabricRuntime);
                }
                catch (Exception e)
                {
                    //Trace.WriteLine(e.Message);
                    ServiceEventSource.Current.ServiceHostInitializationFailed(e);
                }
            }
        }

    }
}
