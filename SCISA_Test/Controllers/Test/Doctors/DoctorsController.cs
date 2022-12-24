using SCISA_Test.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static SCISA_Test.Models.General.clsGeneralViewModels;

namespace SCISA_Test.Controllers.Test.Doctors
{
    public class DoctorsController : Controller
    {
        protected SCISADBEntities db = new SCISADBEntities();
        [HttpPost]
        public JsonResult getPxDx()
        {
            try
            {
                //search all patients
                List<newTbl_Test_Patients> lPatients = db.Tbl_Test_Patients.Where(x => x.Bit_Active == true).Select(x => new newTbl_Test_Patients
                {
                    Int_Id = x.Int_Id,
                    Chr_Name = x.Chr_Name,
                    Chr_LastName = x.Chr_LastName,
                    Chr_DX = x.Chr_DX,
                    Bit_Active = x.Bit_Active,
                    Chr_Email = x.Chr_Email
                }).ToList();
                //search all doctors
                List<newTbl_Test_Doctors> lDoctors = db.Tbl_Test_Doctors.Where(x => x.Bit_Active == true).Select(x => new newTbl_Test_Doctors
                {
                    Int_Id = x.Int_Id,
                    Chr_Name = x.Chr_Name,
                    Chr_LastName = x.Chr_LastName,
                    Chr_ProfessionalLicense = x.Chr_ProfessionalLicense,
                    Bit_Active = x.Bit_Active,
                    Chr_Email = x.Chr_Email
                }).ToList();
                return Json(new { Success = true, lDoctors = lDoctors });
            }
            catch (Exception ex)
            {
                Log.Log.Error("DoctorsController" + ex.ToString());
                return Json(new { Success = true, message = ex.ToString() });
            }

        }
        [HttpPost]
        public JsonResult saveDoctor(Tbl_Test_Doctors _newDoctor)
        {
            try
            {
                //create new doctor
                Tbl_Test_Doctors _doc = new Tbl_Test_Doctors()
                {
                    Chr_Name = _newDoctor.Chr_Name,
                    Chr_LastName = _newDoctor.Chr_LastName,
                    Chr_ProfessionalLicense = _newDoctor.Chr_ProfessionalLicense,
                    Bit_Active = true,
                    Chr_Email = _newDoctor.Chr_Email
                };
                db.Tbl_Test_Doctors.Add(_doc);
                db.SaveChanges();
                newTbl_Test_Doctors doc_return = new newTbl_Test_Doctors
                {
                    Int_Id = _doc.Int_Id,
                    Chr_Name = _doc.Chr_Name,
                    Chr_LastName = _doc.Chr_LastName,
                    Chr_ProfessionalLicense = _doc.Chr_ProfessionalLicense,
                    Bit_Active = _doc.Bit_Active,
                    Chr_Email = _doc.Chr_Email
                };
                return Json(new { Success = true, _doc = doc_return });
            }
            catch (Exception ex)
            {
                Log.Log.Error("DoctorsController" + ex.ToString());
                return Json(new { Success = false, message = ex.ToString() });
            }

        }
    }
}