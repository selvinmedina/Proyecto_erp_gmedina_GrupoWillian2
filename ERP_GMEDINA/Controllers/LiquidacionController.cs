using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP_GMEDINA.Controllers
{
    public class LiquidacionController : Controller
    {
        // GET: Liquidacion
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetFechaInicioFechaFin(string fechaInicio, string fechaFin)
        {
            DateTime dFechaIncio = DateTime.Parse(fechaInicio);
            DateTime dFechaFin= DateTime.Parse(fechaFin);

            object json = new { dFechaIncio, dFechaFin };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}