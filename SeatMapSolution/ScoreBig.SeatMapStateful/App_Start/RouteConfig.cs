
namespace ScoreBig.SeatMapStateful
{
    using System.Web.Http;

    public static class RouteConfig
    {
        /// <summary>
        /// Routing registration.
        /// </summary>
        /// <param name="routes">The Http routes</param>
        public static void RegisterRoutes(HttpRouteCollection routes)
        {
            routes.MapHttpRoute(
                name: "AddorUpdateSeatMap",
                routeTemplate: "AddorUpdateSeatMap",
                defaults: new {controller = "Default", action = "AddOrUpdateSeatMap" },
                constraints: new {}
                );

            routes.MapHttpRoute(
                name: "MapTicketBlock",
                routeTemplate: "MapTicketBlock/{seatMapId}/{section}/{row}",
                defaults: new {controller = "Default", action = "MapTicketBlock" },
                constraints: new {}
                );
        }
    }
}