using System.Web.Mvc;
using System.Web.Routing;

namespace GeobaseWebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("ByIp", "ip/location", new { controller = "Geobase", action = "GetByIp"});
            routes.MapRoute("ByCity", "city/locations",new { controller = "Geobase", action = "GetByCity" });
        }
    }
}
