//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AppOrdenCompra.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Concepto
    {
        public int id { get; set; }
        public Nullable<int> id_orden { get; set; }
        public string descripcion { get; set; }
        public Nullable<int> cantidad { get; set; }
        public Nullable<decimal> valor_unitario { get; set; }
    
        public virtual Oden Oden { get; set; }
    }
}
