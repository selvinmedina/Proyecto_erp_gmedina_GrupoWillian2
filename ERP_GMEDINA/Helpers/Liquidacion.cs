using System;
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
            using (ERP_GMEDINAEntities db = new ERP_GMEDINAEntities())
            {
                dFechaInicial = db.tbEmpleados.Where(x => x.emp_Id == 1).Select(x => x.emp_Fechaingreso).First();
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
                diasFechaInicio += 30;
                mesFechaFinal -= 1;
            }

            //Calcula dias de antiguedad)

            dias = (diasFechaFinal - diasFechaInicio) + 1;

            if (dias == -30)
                dias = 0;
            if (dias == -31)
                dias = 29;
            if (dias == -32)
                dias = 28;


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

            return new { sDias, sMeses, sAnios };

        }

        /// <summary>
        /// Calculates number of business days, taking into account:
        ///  - weekends (Saturdays and Sundays)
        ///  - bank holidays in the middle of the week
        /// </summary>
        /// <param name="fechaInicial">First day in the time interval</param>
        /// <param name="fechaFinal">Last day in the time interval</param>
        /// <param name="bankHolidays">List of bank holidays excluding weekends</param>
        /// <returns>Number of business days during the 'span'</returns>
        public static int BusinessDaysUntil(DateTime fechaFinal)
        {

            DateTime dFechaFinal = fechaFinal, fechaInicial;
            using (ERP_GMEDINAEntities db = new ERP_GMEDINAEntities())
            {
                fechaInicial = db.tbEmpleados.Where(x => x.emp_Id == 1).Select(x => x.emp_Fechaingreso).First();
            }

            fechaInicial = fechaInicial.Date;
            fechaFinal = fechaFinal.Date;
            if (fechaInicial > fechaFinal)
                throw new ArgumentException("Incorrect last day " + fechaFinal);

            TimeSpan span = fechaFinal - fechaInicial;
            int businessDays = span.Days + 1;
            int fullWeekCount = businessDays / 7;
            // find out if there are weekends during the time exceedng the full weeks
            if (businessDays > fullWeekCount * 7)
            {
                // we are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                int firstDayOfWeek = (int)fechaInicial.DayOfWeek;
                int lastDayOfWeek = (int)fechaFinal.DayOfWeek;
                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += 7;
                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                        businessDays -= 2;
                    else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                        businessDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                    businessDays -= 1;
            }

            return businessDays;
        }
    }
}