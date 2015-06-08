namespace SubmissionService
{
    using Microsoft.Owin.Hosting;
    using System;
    using System.Diagnostics;
    using System.Fabric;
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
        private string publishAddress;
        private string listeningAddress;
        private string appRoot;

        public OwinCommunicationListener(string appRoot, IOwinAppBuilder startup)
        {
            this.startup = startup;
            this.appRoot = appRoot;
        }

        public void Initialize(ServiceInitializationParameters serviceInitializationParameters)
        {
            Trace.WriteLine("Initialize Stateless OWIN Web API Service");

            int port = 80;

            if (serviceInitializationParameters.CodePackageActivationContext.GetConfigurationPackageNames().Contains("Config"))
            {
                ConfigurationPackage configPackage = serviceInitializationParameters.CodePackageActivationContext.GetConfigurationPackageObject("Config");

                if (configPackage.Settings.Sections.Contains("ServiceConfig"))
                {
                    var serviceConfig = configPackage.Settings.Sections["ServiceConfig"];
                    if (serviceConfig.Parameters.Contains("port"))
                    {
                        port = Int32.Parse(serviceConfig.Parameters["port"].Value);
                    }
                }
            }

            this.listeningAddress = String.Format(
                CultureInfo.InvariantCulture, "http://+:{0}/{1}/",
                port,
                this.appRoot);

            this.publishAddress = this.listeningAddress.Replace("+", FabricRuntime.GetNodeContext().IPAddressOrFQDN);
        }

        public Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            Trace.WriteLine("Opening Stateless Gateway on " + this.listeningAddress);

            try
            {
                // start the web server 
                this.StartWebServer(this.listeningAddress);

                return Task.FromResult(this.publishAddress);
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
        /// This method starts Katana.
        /// </summary>
        private void StartWebServer(string url)
        {
            Trace.WriteLine("Starting web server on " + url);

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
