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
    
    public partial class Tbl_Test_Dates
    {
        public int Int_Id { get; set; }
        public System.DateTime Date_Date { get; set; }
        public int Fk_IdPatient { get; set; }
        public int Fk_IdDoctor { get; set; }
        public bool Bit_Active { get; set; }
        public int Fk_IdUserMaker { get; set; }
    
        public virtual Tbl_Test_Users Tbl_Test_Users { get; set; }
        public virtual Tbl_Test_Patients Tbl_Test_Patients { get; set; }
        public virtual Tbl_Test_Doctors Tbl_Test_Doctors { get; set; }
    }
}