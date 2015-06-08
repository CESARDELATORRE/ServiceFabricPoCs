
namespace ScoreBig.SeatMapGateway
{
    using System.Web.Http;

    public static class RouteConfig
    {
        /// <summary>
        /// Routing registration.
        /// </summary>
        /// <param name="routes">The http routes.</param>
        public static void RegisterRoutes(HttpRouteCollection routes)
        {
            routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "{action}",
                defaults: new {controller = "Default", action = "Index"},
                constraints: new {}
                );

            routes.MapHttpRoute(
                name: "Files",
                routeTemplate: "Files/{name}",
                defaults: new {controller = "File", action = "Get"},
                constraints: new {}
                );

            routes.MapHttpRoute(
                name: "AddorUpdateSeatMap",
                routeTemplate: "AddorUpdateSeatMap",
                defaults: new { controller = "Default", action = "AddOrUpdateSeatMap" },
                constraints: new { }
                );

            routes.MapHttpRoute(
                name: "MapTicketBlock",
                routeTemplate: "MapTicketBlock/{seatMapId}/{section}/{row}",
                defaults: new { controller = "Default", action = "MapTicketBlock" },
                constraints: new { }
                );
        }
    }
}