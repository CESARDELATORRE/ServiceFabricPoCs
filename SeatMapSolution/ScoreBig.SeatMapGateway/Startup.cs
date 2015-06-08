
namespace ScoreBig.SeatMapGateway
{
    using System.Web.Http;
    using Owin;
    using ScoreBig.Common;

    public class Startup : IOwinAppBuilder
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 256;

            HttpConfiguration config = new HttpConfiguration();

            FormatterConfig.ConfigureFormatters(config.Formatters);
            RouteConfig.RegisterRoutes(config.Routes);

            appBuilder.UseWebApi(config);
        }
    }
}