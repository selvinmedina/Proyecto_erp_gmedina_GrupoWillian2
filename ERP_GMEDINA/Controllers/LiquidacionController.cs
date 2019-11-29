using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERP_GMEDINA.Helpers;

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
        public JsonResult GetFechaInicioFechaFin(int idEmpleado, DateTime fechaFin)
        {
            int sDias = 0, sMeses = 0, sAnios = 0;
            DateTime fechaInicio;
            //object fecha = Liquidacion.IntervaloEntreFechas(idEmpleado,fechaFin, ref sDias, ref sMeses, ref sAnios, out fechaInicio);
            int diasTrabajados = Liquidacion.BusinessDaysUntil(fechaFin);
            return Json(diasTrabajados, JsonRequestBehavior.AllowGet);
        }
    }
}