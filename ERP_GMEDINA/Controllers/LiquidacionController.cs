using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERP_GMEDINA.Helpers;
using ERP_GMEDINA.Models;

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
        public JsonResult GetInfoEmpleado(int idEmpleado, DateTime fechaFin)
        {
            int sDias = 0, sMeses = 0, sAnios = 0;
            DateTime fechaInicio;
            object fecha, json;
            using (ERP_GMEDINAEntities db = new ERP_GMEDINAEntities())
            {
                var consulta = from te in db.tbEmpleados
                               join ta in db.tbAreas on te.area_Id equals ta.area_Id into ta_join
                               from ta in ta_join.DefaultIfEmpty()
                               join td in db.tbDepartamentos on te.depto_Id equals td.depto_Id into td_join
                               from td in td_join.DefaultIfEmpty()
                               join tp in db.tbPersonas on te.per_Id equals tp.per_Id into tp_join
                               from tp in tp_join.DefaultIfEmpty()
                               join tc in db.tbCargos on te.car_Id equals tc.car_Id into tc_join
                               from tc in tc_join.DefaultIfEmpty()
                               join ts in db.tbSueldos on te.emp_Id equals ts.emp_Id into ts_join
                               from ts in ts_join.DefaultIfEmpty()
                               join ttm in db.tbTipoMonedas on ts.tmon_Id equals ttm.tmon_Id into ttm_join
                               from ttm in ttm_join.DefaultIfEmpty()
                               where
                                 te.emp_Estado == true &&
                                 ta.area_Estado == true &&
                                 td.depto_Estado == true &&
                                 tp.per_Estado == true &&
                                 tc.car_Estado == true &&
                                 ts.sue_Estado == true &&
                                 ttm.tmon_Estado == true &&
                                 ts.sue_Cantidad != null
                               select new
                               {
                                   numeroIdentidad = tp.per_Identidad,
                                   nombreEmpleado = tp.per_Nombres,
                                   apellidoEmpleado = tp.per_Apellidos,
                                   fechaNacimiento = (DateTime?)tp.per_FechaNacimiento,
                                   sexoEmpleado = tp.per_Sexo,
                                   edadEmpleado = (int?)tp.per_Edad,
                                   descripcionDepartamento = td.depto_Descripcion,
                                   descripcionArea = ta.area_Descripcion,
                                   descripcionCargo = tc.car_Descripcion,
                                   cantidadSueldo = (decimal?)ts.sue_Cantidad,
                                   descripcionMoneda = ttm.tmon_Descripcion
                               };

                fecha = Liquidacion.IntervaloEntreFechas(idEmpleado, fechaFin, ref sDias, ref sMeses, ref sAnios, out fechaInicio);

                json = new { consulta, fecha };

            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public string GetEmpleadosAreas()
        {
            using (Models.ERP_GMEDINAEntities db = new Models.ERP_GMEDINAEntities())
            {
                var jsonAreasEmpleados = db.UDP_Plani_EmpleadosPorAreas_Select();
                var json = "";

                foreach (UDP_Plani_EmpleadosPorAreas_Select_Result result in jsonAreasEmpleados)
                {
                    json = result.json;
                }

                return json;
            }
        }
    }
}