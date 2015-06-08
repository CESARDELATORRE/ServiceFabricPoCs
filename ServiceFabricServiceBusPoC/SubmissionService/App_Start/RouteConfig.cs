
namespace SubmissionService
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
                name: "SubmitCommand",
                routeTemplate: "SubmitCommand",
                defaults: new { controller = "Default", action = "SubmitCommandMessageAzureServiceBus" },
                constraints: new { }
            );

            //routes.MapHttpRoute(
            //    name: "SendValue",
            //    routeTemplate: "SendValue/{word}",
            //    defaults: new { controller = "Default", action = "SendValue" },
            //    constraints: new { }
            //);

            routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "{action}",
                defaults: new { controller = "Default", action = "Index" },
                constraints: new { }
            );
            
        }

    }
}
