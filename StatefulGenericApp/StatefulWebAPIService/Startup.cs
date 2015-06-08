

using Owin;
using System.Web.Http;
using System.Fabric.Data;

//(CDLTLL)
using Autofac.Integration.WebApi;

namespace StatefulWebAPIService
{
    public class Startup : IOwinAppBuilder
    {
        private readonly IReliableObjectStateManager objectManager;        

        public Startup(IReliableObjectStateManager objManager)
        {
            this.objectManager = objManager;            
        }

        public void Configuration(IAppBuilder appBuilder)
        {
            //Dependency Injection Configuration
            AutofacContainerConfig.Configure(this.objectManager);

            //Web API config for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            //Configure Web API dependency resolver
            config.DependencyResolver = new AutofacWebApiDependencyResolver(AutofacContainerConfig.Container);

            //Enable attribute based routing
            //http://www.asp.net/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2
            config.MapHttpAttributeRoutes();

            //Configure Formatters for JsonSerializer
            FormatterConfig.ConfigureFormatters(config.Formatters);            

            //Register Default Routes
            RouteConfig.RegisterRoutes(config.Routes);

            appBuilder.UseWebApi(config);
           
        }
    }
}
