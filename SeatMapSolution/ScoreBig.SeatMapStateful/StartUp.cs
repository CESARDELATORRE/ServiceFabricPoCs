﻿
namespace ScoreBig.SeatMapStateful
{
    using System.Web.Http;
    using Microsoft.ServiceFabric.Data;
    using Owin;
    using ScoreBig.Common;

    /// <summary>
    /// OWIN configuration
    /// </summary>
    public class Startup : IOwinAppBuilder
    {
        private readonly IReliableStateManager stateManager;

        public Startup(IReliableStateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        /// <summary>
        /// Configures the app builder using Web API.
        /// </summary>
        /// <param name="appBuilder"></param>
        public void Configuration(IAppBuilder appBuilder)
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 256;

            HttpConfiguration config = new HttpConfiguration();

            FormatterConfig.ConfigureFormatters(config.Formatters);
            RouteConfig.RegisterRoutes(config.Routes);
            UnityConfig.RegisterComponents(config, this.stateManager);

            appBuilder.UseWebApi(config);
        }
    }
}