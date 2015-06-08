
namespace StatelessGatewayWebAPIService
{
    using System.Web.Http;

    public static class RouteConfig
    {
        /// <summary>
        /// Routing registration.
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(HttpRouteCollection routes)
        {
            routes.MapHttpRoute(
                name: "Count",
                routeTemplate: "Count",
                defaults: new { controller = "Default", action = "Count" },
                constraints: new { }
            );
           
            routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "{action}",
                defaults: new { controller = "Default", action = "Index" },
                constraints: new { }
            );
            
        }

    }
}
