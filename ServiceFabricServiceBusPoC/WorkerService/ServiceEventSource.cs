using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandHandlerService
{
    [EventSource(Name = "MyCompany-ServiceFabricServiceBusPoC-CommandHandlerService")]
    internal sealed class ServiceEventSource : EventSource
    {
        public static ServiceEventSource Current = new ServiceEventSource();

        [Event(1, Level = EventLevel.Verbose)]
        public void MessageEvent(string message)
        {
            if (this.IsEnabled())
            {
                WriteEvent(1, message);
            }
        }


        [Event(2, Level = EventLevel.Informational, Message = "Service host {0} registered service type {1}")]
        public void ServiceTypeRegistered(int hostProcessId, string serviceType)
        {
            WriteEvent(2, hostProcessId, serviceType);
        }

        [NonEvent]
        public void ServiceHostInitializationFailed(Exception e)
        {
            ServiceHostInitializationFailed(e.ToString());
        }

        [Event(3, Level = EventLevel.Error, Message = "Service host initialization failed")]
        private void ServiceHostInitializationFailed(string exception)
        {
            WriteEvent(3, exception);
        }
    }
}
