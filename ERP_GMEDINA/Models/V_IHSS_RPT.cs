//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ERP_GMEDINA.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class V_IHSS_RPT
    {
        public int emp_emp_Id { get; set; }
        public Nullable<int> cpla_cpla_IdPlanilla { get; set; }
        public string cpla_DescripcionPlanilla { get; set; }
        public string NombreCompleto { get; set; }
        public string per_Identidad { get; set; }
        public string per_Sexo { get; set; }
        public Nullable<int> per_Edad { get; set; }
        public string per_Direccion { get; set; }
        public string per_Telefono { get; set; }
        public string per_CorreoElectronico { get; set; }
        public string per_EstadoCivil { get; set; }
        public Nullable<decimal> hipa_SueldoNeto { get; set; }
        public Nullable<decimal> CantidadIHSSEmpresa { get; set; }
        public Nullable<decimal> CantidadIHSSColaborador { get; set; }
        public Nullable<decimal> TotalIHSS { get; set; }
        public System.DateTime hipa_FechaPago { get; set; }
        public bool emp_Estado { get; set; }
    }
}
