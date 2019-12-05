using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERP_GMEDINA.Models;

namespace ERP_GMEDINA.Helpers
{
    public class Liquidacion
    {
        #region Calcular las fechas, año de 360 días
        public static double Dias360Mes(DateTime fechaFin, int idEmpleado)
        {
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

        #region Hecho, o Haciendo
        public static decimal Salario(int idEmpleado)
        {
            //Salario: Es en base a los ultimos 6 meses de pago, se hace el promedio

            decimal? salario = 0;
            using (ERP_GMEDINAEntities db = new ERP_GMEDINAEntities())
            {
                var historialSalariosDB = ObtenerSalarios(idEmpleado, db).Take(6).ToList();
                decimal dHistorialSalariosDB = 0;

                if (historialSalariosDB.Count() > 0)
                {
                    //Hacer el promedio de x cantidad de meses meses de pago para el "Salario"
                    foreach (var item in historialSalariosDB)
                    {
                        dHistorialSalariosDB += item;
                    }
                    salario = (dHistorialSalariosDB / historialSalariosDB.Count());
                }
                else
                {
                    //Salario es el sueldo
                    salario = db.tbSueldos.Where(x => x.emp_Id == idEmpleado).Select(x => x.sue_Cantidad).FirstOrDefault();
                }
            }

            return (decimal)salario;
        }

        public static decimal SalarioOrdinarioDiario(decimal salario)
        {
            //Salario Ordinario Diario: salario/30
            return (salario / 30);
        }

        public static decimal SalarioOrdinarioPromedioDiario(decimal salario)
        {
            //Salario ordinario promedio diario = (salario * 14)/360
            return ((salario * 14) / 360);
        }

        public decimal SalarioPromedioDiaro(decimal salario, decimal promedioHorasExtras, decimal promedioBonificaciones)
        {
            //Salario promedio diario = salario + promedio Horas Extras + promedio Bonificaciones + promedio de Comisiones

            return 0;
        }

        public decimal AlimentacionOVivienda(decimal salario)
        {
            return (salario * 0.20M);
        }

        public decimal AlimentacionYVivienda(decimal salario)
        {
            return (salario * 0.30M);
        }

        public decimal PromedioComisiones(int idEmpleado)
        {
            //Comisiones en los ultimos 6 meses / 6 
            decimal comision = 0;
            using (ERP_GMEDINAEntities db = new ERP_GMEDINAEntities())
            {
                decimal totalComisionesMeses = 0;

                var ultimos6MesesComisiones = ((DateTime)db.tbEmpleadoComisiones
                    .Where(x => x.emp_Id == idEmpleado && x.cc_Activo == true)
                    .OrderByDescending(x => x.cc_FechaPagado)
                    .Select(x => x.cc_FechaPagado).FirstOrDefault()).AddMonths(-6);

                var promedioComisiones = db.tbEmpleadoComisiones
                    .Where(x => x.emp_Id == idEmpleado && x.cc_Activo == true && x.cc_FechaPagado.Value != null && x.cc_Pagado == true && x.cc_FechaPagado >= ultimos6MesesComisiones)
                    .Select(x => new { porcentajeComision = x.cc_PorcentajeComision, totalVenta = x.cc_TotalVenta }).ToList();

                int cantidadPromedioComisiones = promedioComisiones.Count;

                if (cantidadPromedioComisiones > 0) foreach (var item in promedioComisiones) totalComisionesMeses += (item.porcentajeComision * item.totalVenta);

                comision = (totalComisionesMeses / cantidadPromedioComisiones);

            }
            return comision;
        }

        public decimal PromedioBonificacines(int idEmpleado)
        {
            //Bonificaciones en los últimos 6 meses / 6
            decimal totalPromedioBonificaciones = 0;
            using (ERP_GMEDINAEntities db = new ERP_GMEDINAEntities())
            {
                decimal totalBonificaciones = 0;

                var fechaPagadoMenos6Meses = ((DateTime)db.tbEmpleadoBonos
                    .Where(x => x.emp_Id == idEmpleado && x.cb_Activo == true)
                    .OrderByDescending(x => x.cb_FechaPagado)
                    .Select(x => x.cb_FechaPagado).FirstOrDefault()).AddMonths(-6);

                var promedioBonificaciones = db.tbEmpleadoBonos
                    .Where(x => x.emp_Id == idEmpleado && x.cb_Activo == true && x.cb_FechaPagado.Value != null && x.cb_Pagado == true && x.cb_FechaPagado >= fechaPagadoMenos6Meses)
                    .Select(x => new { montoBonificacion = x.cb_Monto }).ToList();

                int cantidadPromedioBonificaciones = promedioBonificaciones.Count;

                if (cantidadPromedioBonificaciones > 0) foreach (var item in promedioBonificaciones) totalBonificaciones += (Decimal)(item.montoBonificacion);

                totalPromedioBonificaciones = (totalBonificaciones / cantidadPromedioBonificaciones);

            }

            return totalPromedioBonificaciones;
        }

        public bool mas10Empleados()
        {
            //Ya se esta validando eso en la base de datos
            return true;
        }

        public void EsEmpresaDomestica()
        {
            //No se tomara en cuenta esta validación para este sistema
        }

        public void Cesantia(decimal salarioPromedioDiario, double dias, ref double salarioCesantiaProporcional, ref decimal salarioCesantia)
        {
            double anios = 0, meses = 0;

            CalcularAniosMesesDias(ref anios, ref meses, ref dias);

            if (anios >= 1)
            {
                //Calcular la cesantia, por cada año son 30 dias de pago
                while (anios >= 1)
                {
                    anios -= 1;
                    salarioCesantia += (salarioPromedioDiario * 30);
                }

                //Calcular la cesantia proporcional
                // Si ya le deducí todos los años, ahora que le deduzca los meses
                if (meses >= 1)
                {
                    double cantidadDiasMes = 0;
                    //Convertir los meses a dias
                    while (meses >= 1)
                    {
                        meses -= 1;
                        cantidadDiasMes += 30;
                    }

                    //Aqui ya tengo la cantidad total de dias que le pagara, luego lo multiplico por 30
                    //Mas info: https://www.toptrabajos.com/blog/hn/calculo-indemnizacion-honduras/
                    salarioCesantiaProporcional = ((((cantidadDiasMes += dias) * 30) / 360) * Decimal.ToDouble(salarioPromedioDiario));
                }
                return;
            }

            if (meses >= 6 && anios < 1)
            {
                salarioCesantia = (salarioPromedioDiario * 20);
                return;
            }

            if (meses >= 3 && meses < 6)
            {
                salarioCesantia = (salarioPromedioDiario * 10);
                return;
            }

        }

        public static decimal HorasExtras(int idEmpleado)
        {
            //(Horas extras en los últimos 6 meses / 30) / 8

            int totalHorasExtras = 0, promedioHorasExtras = 0;

            using (ERP_GMEDINAEntities db = new ERP_GMEDINAEntities())
            {
                var fechaUltimos6MesesHorasExtras = ((DateTime)db.tbHistorialHorasTrabajadas
                    .Where(x => x.htra_Estado == true && x.emp_Id == idEmpleado)
                    .OrderByDescending(x => x.htra_Fecha)
                    .Select(x => x.htra_Fecha).FirstOrDefault())
                    .AddMonths(-6);

                var historialHorasExtras = db.tbHistorialHorasTrabajadas.
                    Where(x => x.tbTipoHoras.tiho_Recargo > 0 && x.tbTipoHoras.tiho_Estado == true && x.emp_Id == idEmpleado && x.htra_Fecha >= fechaUltimos6MesesHorasExtras)
                    .Select(x => x.htra_CantidadHoras).ToList();

                int cantidadHorasExtras = historialHorasExtras.Count;

                if (cantidadHorasExtras > 0) foreach (var item in historialHorasExtras) totalHorasExtras += item;

                promedioHorasExtras = (totalHorasExtras / cantidadHorasExtras);
            }

            return promedioHorasExtras;
        }

        public static decimal Preaviso(decimal salarioPromedioDiario, double dias)
        {
            #region Declaracion de Variables
            decimal totalPreaviso = 0;
            double meses = 0, anios = 0;
            #endregion

            CalcularAniosMesesDias(ref anios, ref meses, ref dias);

            #region Validaciones
            //Mas de 2 años: 2 meses de preaviso (60 dias de pago).
            if (anios >= 2) return (totalPreaviso = (salarioPromedioDiario * 60));

            //1 a 2 años: 1 mes de preaviso (30 dias de pago)
            if (anios >= 1 && anios <= 2) return (totalPreaviso = (salarioPromedioDiario * 30));

            //De 6 meses a 1 año: dos semanas de pago 14 dias de pago
            if (meses > 6 && anios < 1) return (totalPreaviso = (salarioPromedioDiario * 14));

            //De 3 a 6 meses, una semana de pago (7 dias de pago)
            if (meses >= 3 && meses < 6) return (totalPreaviso = (salarioPromedioDiario * 7));

            //Menos de 3 meses, 24 horas de salario promedio diario (1 dia de pago)
            if (meses < 3 && meses > 2) return (totalPreaviso = salarioPromedioDiario);
            #endregion

            //Si esta en el período de prueba no se le otorga preaviso
            return 0;
        }

        #endregion

        #region Por hacer

        public void AguinaldoProporcional()
        {
            //https://www.toptrabajos.com/blog/hn/calculo-de-aguinaldo-honduras/
        }

        public void DecimoCuartoProporcional()
        {

        }

        #endregion

        #region Utilitarios
        public static void CalcularAniosMesesDias(ref double anios, ref double meses, ref double dias)
        {
            while (dias >= 360)
            {
                dias -= 360;
                anios += 1;
            }

            while (dias >= 30)
            {
                dias -= 30;
                meses += 1;
            }
        }

        private static IQueryable<decimal> ObtenerSalarios(int Emp_Id, ERP_GMEDINAEntities db)
        {
            return db.tbHistorialDePago.Where(p => p.emp_Id == Emp_Id).Select(x => (decimal)x.hipa_SueldoNeto);
        }

        //Ejecutar calculos
        public static object EjecutarCalculosSalarios(int idEmpleado)
        {
            decimal salario = Salario(idEmpleado);
            decimal salarioOrdinarioDiario = SalarioOrdinarioDiario(salario);
            decimal salarioOrdinarioPromedioDiario = SalarioOrdinarioPromedioDiario(salario);
            decimal salarioPromedioDiario = salarioOrdinarioPromedioDiario;

            salario = Math.Round((Decimal)salario, 2);
            salarioOrdinarioDiario = Math.Round((Decimal)salarioOrdinarioDiario, 2);
            salarioOrdinarioPromedioDiario = Math.Round((Decimal)salarioOrdinarioPromedioDiario, 2);
            salarioPromedioDiario = Math.Round((Decimal)salarioPromedioDiario, 2);

            return new { salario, salarioOrdinarioDiario, salarioOrdinarioPromedioDiario, salarioPromedioDiario };
        }
        #endregion
    }
}