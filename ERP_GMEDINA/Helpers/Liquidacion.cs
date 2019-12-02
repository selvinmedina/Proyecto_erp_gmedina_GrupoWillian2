﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERP_GMEDINA.Models;

namespace ERP_GMEDINA.Helpers
{
    public class Liquidacion
    {
        public static object IntervaloEntreFechas(int idEmpleado, DateTime fechaFinal, ref int sDias, ref int sMeses, ref int sAnios, out DateTime dFechaInicial)
        {
            DateTime dFechaFinal = fechaFinal;
            dFechaInicial = DateTime.MinValue;
            using (ERP_GMEDINAEntities db = new ERP_GMEDINAEntities())
            {
                try
                {
                    dFechaInicial = db.tbEmpleados.Where(x => x.emp_Id == idEmpleado).Select(x => x.emp_Fechaingreso).FirstOrDefault();
                }
                catch (Exception ex)
                {
                }
            }

            TimeSpan difFechas = dFechaFinal - dFechaInicial;
            

            if(dFechaInicial > dFechaFinal)
                return new { sDias = 0, sMeses = 0, sAnios = 0};

            int dias = 0,
                meses = 0,
                anios = 0,

                //Numero de dias, numero del mes, y numero del año en de la fecha inicial
                diasFechaInicio = dFechaInicial.Day,
                mesFechaInicio = dFechaInicial.Month,
                anioFechaInicio = dFechaInicial.Year,

                //Numero de dias, numero del mes, y numero del año en de la fecha final
                diasFechaFinal = dFechaFinal.Day,
                mesFechaFinal = dFechaFinal.Month,
                anioFechaFinal = dFechaFinal.Year;


            if (diasFechaFinal < diasFechaInicio)
            {
                
                mesFechaFinal -= 1;
            }

            //Calcula dias de antiguedad)

            dias = (diasFechaFinal - diasFechaInicio) + 1;

            if (mesFechaFinal < mesFechaInicio)
            {
                mesFechaFinal += 12;
                anioFechaFinal -= 1;
            }

            //Calcula meses de antiguedad
            meses = mesFechaFinal - mesFechaInicio;

            //Calcula años de antiguedad
            anios = anioFechaFinal - anioFechaInicio;

            //Asignar los dias meses y años a las variables ref
            sDias = (dias);
            sMeses = meses;
            sAnios = anios;

            if (sDias < 0)
                sDias += 30;

            return new { sDias, sMeses, sAnios };

        }
    }
}