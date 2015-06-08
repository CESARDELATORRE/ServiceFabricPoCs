
using Owin;
using System;
using System.Diagnostics;

namespace StatelessGatewayWebAPIService
{
    using System.Web.Http;

    public class Startup : IOwinAppBuilder
    {
        public void Configuration(IAppBuilder appBuilder)
        {    
            try
            {         
                //Web API config for self-host. 
                HttpConfiguration config = new HttpConfiguration();

                //Enable attribute based routing
                //http://www.asp.net/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2
                config.MapHttpAttributeRoutes();

                //Configure Formatters for JsonSerializer
                FormatterConfig.ConfigureFormatters(config.Formatters);

                //Register Default Routes
                RouteConfig.RegisterRoutes(config.Routes);                

                appBuilder.UseWebApi(config);

                config.EnsureInitialized();
            }
            catch (Exception exception)
            {
                Trace.Assert(false, "Unexpected exception {0}", exception.Message);
                throw;
            }

}
    }
}
