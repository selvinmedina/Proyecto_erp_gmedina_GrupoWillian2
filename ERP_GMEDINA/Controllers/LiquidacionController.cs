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
        public JsonResult GetFechaInicioFechaFin(int idEmpleado, DateTime fechaFin)
        {
            int sDias = 0, sMeses = 0, sAnios = 0;
            DateTime fechaInicio;
            object fecha = Liquidacion.IntervaloEntreFechas(idEmpleado, fechaFin, ref sDias, ref sMeses, ref sAnios, out fechaInicio);
            return Json(fecha, JsonRequestBehavior.AllowGet);
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


                #region Comentarios
                //var consulta = (from ta in db.tbAreas
                //                select new SelectAreasEmpleadosViewModel
                //                {
                //                    text = ta.area_Descripcion,
                //                    children =
                //                    {
                //                        id = (from te in db.tbAreas
                //                            join emp in db.tbEmpleados
                //                            on te.area_Id equals emp.area_Id
                //                            where
                //                            ta.area_Id == te.area_Id
                //                            select new
                //                            {
                //                                id = emp.emp_Id
                //                            }).First().id,
                //                      text = (from te in db.tbAreas
                //                             join emp in db.tbEmpleados
                //                             on te.area_Id equals emp.area_Id
                //                             join per in db.tbPersonas
                //                             on emp.per_Id equals per.per_Id
                //                             where
                //                             ta.area_Id == te.area_Id
                //                             select new
                //                             {
                //                                 text = per.per_Nombres + "" + per.per_Apellidos
                //                             }).First().text
                //                    }
                //                }).ToList();

                #endregion

                //TODO: Hacer la filtracion de Empleados por Areas en SQL

                //Retornar como un elemento padre la Area, y elemento hijo los empleados

                //Filtrar que los empleados no se hayan salido
                return json;
            }
        }
    }
}