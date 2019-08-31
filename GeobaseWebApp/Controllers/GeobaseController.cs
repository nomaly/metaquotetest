using System.Web.Mvc;
using GeobaseModel;
using GeobaseWebApp.Utils;

namespace GeobaseWebApp.Controllers
{
    public class GeobaseController : Controller
    {
        private readonly IGeobase Geo;

        public GeobaseController()
        {
            Geo = DependencyLocator.Get<IGeobase>("Geobase");
        }

        [HttpGet]
        public ActionResult GetByIp(string ip)
            => Json(Geo.FindByIp(ip), JsonRequestBehavior.AllowGet);

        [HttpGet]
        public ActionResult GetByCity(string city)
            => Json(Geo.FindByCity(city), JsonRequestBehavior.AllowGet);
    }
}
