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
    
    public partial class tbRamaActividad
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbRamaActividad()
        {
            this.tbSalarioPorHora = new HashSet<tbSalarioPorHora>();
        }
    
        public int rama_Id { get; set; }
        public string rama_Descripcion { get; set; }
        public int rama_UsuarioCrea { get; set; }
        public System.DateTime rama_FechaCrea { get; set; }
        public Nullable<int> rama_UsuarioModifica { get; set; }
        public Nullable<System.DateTime> rama_FechaModifica { get; set; }
        public bool rama_Activo { get; set; }
    
        public virtual tbUsuario tbUsuario { get; set; }
        public virtual tbUsuario tbUsuario1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbSalarioPorHora> tbSalarioPorHora { get; set; }
    }
}
