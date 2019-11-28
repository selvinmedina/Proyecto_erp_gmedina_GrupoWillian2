using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP_GMEDINA.Helpers
{
    public class Liquidacion
    {
        public static string IntervaloEntreFechas(DateTime fechaInicial, DateTime fechaFinal, ref string sDias, ref string sMeses, ref string sAnios)
        {
            DateTime dFechaInicial = fechaFinal,
                dFechaFinal = fechaFinal;

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
                diasFechaInicio += 30;
                mesFechaFinal -= 1;
            }

            //Calcula dias de antiguedad
            dias = diasFechaFinal - diasFechaInicio;

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
            sDias = dias.ToString();
            sMeses = meses.ToString();
            sAnios = anios.ToString();

            return String.Format("{0} {1} {2} {3} {4} {5}",
                anios,
                (anios == 1) ? "Año" : "Años",
                (meses == 1) ? "Mes" : "Meses",
                (dias == 1) ? "Dia" : "Dias");
        }
    }
}