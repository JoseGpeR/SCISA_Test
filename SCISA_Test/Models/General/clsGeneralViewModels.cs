using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCISA_Test.Models.General
{
    public class clsGeneralViewModels
    {
		public class UserRoles
		{
            public int Int_Id { get; set; }
            public string Chr_Name { get; set; }
            public string Chr_UserName { get; set; }
            public string Chr_Password { get; set; }
            public string Chr_Email { get; set; }
            public bool Bit_Active { get; set; }
            public int Fk_IdRole { get; set; }
            public string Chr_RoleName { get; set; }
        }
        public class clsAllDates
        {
            public int Int_Id { get; set; }
            public DateTime Date_Date { get; set; }
            public string strDate { get; set; }
            public string strDx { get; set; }
            public int Fk_IdDoctor { get; set; }
            public string strDxName { get; set; }
            public string strDxLastName { get; set; }
            public int Fk_IdPatient { get; set; }
            public string strPxName { get; set; }
            public string strPxLastName { get; set; }
            public int Fk_IdUserMaker { get; set; }
            public string Chr_UserName { get; set; }
            public string Chr_MakerName { get; set; }            
            public string Chr_Email { get; set; }            
        }
        public class clsNewDate
        {
            public int Int_Id { get; set; }
            public string Chr_Name { get; set; }
            public string Chr_LastName { get; set; }
            public string Chr_DX { get; set; }
            public bool Bit_Active { get; set; }
            public string Chr_Email { get; set; }
            public DateTime Date_Date { get; set; }
            public int Fk_IdDoctor { get; set; }
            public int Fk_IdPatient { get; set; }
        }
        public class newTbl_Test_Patients
        {
            public int Int_Id { get; set; }
            public string Chr_Name { get; set; }
            public string Chr_LastName { get; set; }
            public string Chr_DX { get; set; }
            public bool Bit_Active { get; set; }
            public string Chr_Email { get; set; }

        }
        public class newTbl_Test_Doctors
        {
            public int Int_Id { get; set; }
            public string Chr_Name { get; set; }
            public string Chr_LastName { get; set; }
            public string Chr_ProfessionalLicense { get; set; }
            public bool Bit_Active { get; set; }
            public string Chr_Email { get; set; }

        }
    }
}