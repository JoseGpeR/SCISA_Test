//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SCISA_Test.Models.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_Test_Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Test_Users()
        {
            this.Tbl_Test_Dates = new HashSet<Tbl_Test_Dates>();
        }
    
        public int Int_Id { get; set; }
        public string Chr_Name { get; set; }
        public string Chr_UserName { get; set; }
        public string Chr_Password { get; set; }
        public string Chr_Email { get; set; }
        public bool Bit_Active { get; set; }
        public int Fk_IdRole { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Test_Dates> Tbl_Test_Dates { get; set; }
        public virtual Tbl_Test_Roles Tbl_Test_Roles { get; set; }
    }
}