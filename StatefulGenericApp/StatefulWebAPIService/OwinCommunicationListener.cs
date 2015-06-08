

namespace StatefulWebAPIService
{
    using Microsoft.Owin.Hosting;
    using System;
    using System.Diagnostics;
    using System.Fabric;
    using System.Fabric.Data;
    using System.Fabric.Data.Collections;
    using System.Fabric.Description;
    using System.Fabric.Services;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;

    public class OwinCommunicationListener : ICommunicationListener
    {
        /// <summary>
        /// OWIN server handle.
        /// </summary>
        private IDisposable serverHandle;

        private IOwinAppBuilder startup;

        private string webApiServiceUrlFullyQualified;
        private string webApiServiceUrl;

        public OwinCommunicationListener(IOwinAppBuilder startup)
        {
            this.startup = startup;
        }

        public void Initialize(ServiceInitializationParameters serviceInitializationParameters)
        {
            Trace.WriteLine("Initialize OWIN Server with dynamic port and infor from Fabric cluster");

            //(CDLTLL) 
            StatefulServiceInitializationParameters statefulInitParams = serviceInitializationParameters as StatefulServiceInitializationParameters;

            // These endpoints are defined in the ServiceManifest.xml.
            // An available port will automatically be assigned for each endpoint.
            // It's important to use the assigned port rather than a dynamic OS port so that there are no conflicts between user code and the system.
            // The service endpoint is defined as an HTTP endpoint and is used in user code, in this case the Web API service.
            EndpointResourceDescription serviceEndpoint = statefulInitParams.CodePackageActivationContext.GetEndpoint("ServiceEndpoint");

            // Create the URL that the server will listen on.
            // We need to make sure this URL is unique to this replica, because we may have multiple replicas on one machine.

            //this.webApiServiceUrl = String.Format("http://+:{0}/",
            //    serviceEndpoint.Port);

            this.webApiServiceUrl = String.Format(
                CultureInfo.InvariantCulture,
                "http://+:{0}/data/{1}/{2}/{3}/",
                //serviceEndpoint.Port,
                80,
                statefulInitParams.PartitionId.ToString(),
                statefulInitParams.ReplicaId.ToString(),
                Guid.NewGuid().ToString());

            this.webApiServiceUrlFullyQualified = this.webApiServiceUrl.Replace("+", FabricRuntime.GetNodeContext().IPAddressOrFQDN);
        }

        public Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            // start the web server             
            Trace.WriteLine("Opening on " + this.webApiServiceUrl);

            try
            {
                this.StartWebServer(this.webApiServiceUrl);

                return Task.FromResult(this.webApiServiceUrlFullyQualified);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);

                this.StopWebServer();

                return null;
            }
        }

        public Task CloseAsync(CancellationToken cancellationToken)
        {
            Trace.WriteLine("Close");

            this.StopWebServer();

            return Task.FromResult(true);
        }

        public void Abort()
        {
            Trace.WriteLine("Abort");

            this.StopWebServer();
        }


        /// <summary>
        /// This method starts Katana/OWIN.
        /// </summary>
        private void StartWebServer(string url)
        {
            Trace.WriteLine("Starting Stateful Web Service on " + url);

            this.serverHandle = WebApp.Start(url, appBuilder => this.startup.Configuration(appBuilder));
        }

        private void StopWebServer()
        {
            if (this.serverHandle != null)
            {
                try
                {
                    this.serverHandle.Dispose();
                }
                catch (ObjectDisposedException)
                {
                    // no-op
                }
            }
        }

        public bool ListenOnSecondary
        {
            get;
            set;
        }
    }
}
