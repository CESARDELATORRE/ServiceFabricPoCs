

namespace StatefulWebAPIService
{
    using System.Diagnostics.Contracts;
    using System.Web.Http;
    using System.Web.Mvc;
    //using System.Web.Routing;

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
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional },
                constraints: new { }
            );

        }
    }
}
