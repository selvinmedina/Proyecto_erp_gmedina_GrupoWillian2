using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERP_GMEDINA.Models;

namespace ERP_GMEDINA.Helpers
{
    public class Liquidacion
    {
        public static double Dias360Mes(DateTime fechaFin, int idEmpleado)
        {
            #region Calcular las fechas, año de 360 días
            DateTime fechaInicio = DateTime.MinValue;
            using (ERP_GMEDINAEntities db = new ERP_GMEDINAEntities())
            {
                try
                {
                    fechaInicio = db.tbEmpleados.Where(x => x.emp_Id == idEmpleado).Select(x => x.emp_Fechaingreso).FirstOrDefault();
                }
                catch (Exception ex)
                {

                }
            }

            if (fechaInicio > fechaFin)
                return 0;

            int diaInicio = fechaInicio.Day;
            int mesInicio = fechaInicio.Month;
            int anioInicio = fechaInicio.Year;
            int diaFin = fechaFin.Day;
            int mesFin = fechaFin.Month;
            int anioFin = fechaFin.Year;

            if (diaInicio == 31 || EsElUltimoDiaDeFebrero(fechaInicio))
            {
                diaInicio = 30;
            }

            if (diaInicio == 30 && diaFin == 31)
            {
                diaFin = 30;
            }

            return ((anioFin - anioInicio) * 360) + ((mesFin - mesInicio) * 30) + (diaFin - diaInicio);
        }

        private static bool EsElUltimoDiaDeFebrero(DateTime date)
        {
            return date.Month == 2 && date.Day == DateTime.DaysInMonth(date.Year, date.Month);
        }
        #endregion

        public decimal Salario(int emp_Id)
        { 
            //Cacular el salario en base a los ultimos 6 mesesde pago
            //Si tiene mas de 6 meses que lo calcule en base a eso
            //Hacer el promedio de los ultimos 6 meses de pago para el "Salario"

            //No se pueden dejar salarios vacios
            //Si no entonces usar el salario Mensual

            return 0;
        }

        public decimal SalarioOrdinarioDiario(decimal salario)
        {
            //Salario Ordinario Diario: 
            //Salario%30


            return 0;
        }

        /*
            No puede qeudar ninguno vacio o sin valor en los promedios de los ultimos 6 meses 
        */

        public decimal SalarioOrdinarioPromedioDiario(decimal salario)
        {
            //Salario*14
            //Salario/360


            return 0;
        }

        public decimal SalarioPromedioDiaro(decimal salario)
        {
            //SalarioOrdinarioPromedioDiario + Horas Extras + Bonificaciones..

            return 0;
        }

        public decimal ComisionesUltimos6Meses()
        {
            //Promedio de 6 comisiones / 6  ... Si no hay comisiones dejarlo en 0
            return 0;
        }



        #region MALCOM_MEDINA

        //CALCULO DE SALARIO ORDINARIO PROMEDIO MENSUAL
        public decimal SalarioOrdinarioMensual(int Emp_Id)
        {
            //Captura de años de antiguedad
            decimal SalarioPromedio;
            using (ERP_GMEDINAEntities db = new ERP_GMEDINAEntities())
            {
                IQueryable<decimal> SalariosUlt6Meses = db.tbHistorialDePago.Where(p => p.emp_Id == Emp_Id).Select(x => (decimal)x.hipa_SueldoNeto).Take(6);
                //CAPTURA DEL SALARIO PROMEDIO Y CONVERSIÓN A STRING PARA EL RETORNO 
                SalarioPromedio = SalariosUlt6Meses.Average();
            }
            return SalarioPromedio;
        }
        #endregion
    }
}