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
    
    public partial class Tbl_Test_Access
    {
        public int Int_Id { get; set; }
        public int Fk_IdRole { get; set; }
        public string Chr_Access { get; set; }
        public bool Bit_Active { get; set; }
    
        public virtual Tbl_Test_Roles Tbl_Test_Roles { get; set; }
    }
}