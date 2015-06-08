using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using System.Diagnostics;

using Microsoft.Owin.Hosting;

namespace SubmissionService
{

    /// <summary>
    /// A stateless OWIN Web API application.
    /// </summary>
    /// 
    public class Service : StatelessService
    {
        public const string ServiceTypeName = "SubmissionServiceType";

        protected override Task RunAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Creates a listener for Web API.
        /// </summary>
        /// <returns></returns>
        protected override ICommunicationListener CreateCommunicationListener()
        {
            return new OwinCommunicationListener("StatelessGatewayWebAPIService", new Startup());
        }
    }

}
