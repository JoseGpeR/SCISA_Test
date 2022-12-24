using log4net;
using SCISA_Test.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static SCISA_Test.Models.General.clsGeneralViewModels;

namespace SCISA_Test.Controllers.Test.Dates
{
    public class DatesController : Controller
    {
        protected SCISADBEntities db = new SCISADBEntities();
        //private Log.Log Log { get; set; }
        [HttpPost]
        public JsonResult getAllDates()
        {
            try
            {
                //Query to search all dates
                List<clsAllDates> lAllDates = (from t1 in db.Tbl_Test_Dates
                                               join t2 in db.Tbl_Test_Doctors on t1.Fk_IdDoctor equals t2.Int_Id
                                               join t3 in db.Tbl_Test_Patients on t1.Fk_IdPatient equals t3.Int_Id
                                               join t4 in db.Tbl_Test_Users on t1.Fk_IdUserMaker equals t4.Int_Id
                                               where t1.Bit_Active == true && t2.Bit_Active == true && t3.Bit_Active == true
                                               select new clsAllDates
                                               {
                                                   Int_Id = t1.Int_Id,
                                                   Date_Date = t1.Date_Date,
                                                   Fk_IdDoctor = t1.Fk_IdDoctor,
                                                   strDxName = t2.Chr_Name,
                                                   strDxLastName = t2.Chr_LastName,
                                                   Fk_IdPatient = t1.Fk_IdPatient,
                                                   strDx = t3.Chr_DX,
                                                   strPxName = t3.Chr_Name,
                                                   strPxLastName = t3.Chr_LastName,
                                                   Fk_IdUserMaker = t4.Int_Id,
                                                   Chr_UserName = t4.Chr_UserName,
                                                   Chr_MakerName = t4.Chr_Name,
                                                   Chr_Email = t3.Chr_Email
                                               }).OrderBy(x => x.Date_Date).ToList();
                foreach (var item in lAllDates)
                {
                    //set string date to grid on ag grid
                    item.strDate = item.Date_Date.ToString("yyyy-MM-dd");
                }
                return Json(new { Success = true, lAllDates = lAllDates });
            }
            catch (Exception ex)
            {
                Log.Log.Error("DatesController" + ex.ToString());
                return Json(new { Success = false, message = ex.ToString() });
            }
           
        }
        [HttpPost]
        public JsonResult saveDate(clsNewDate newDate)
        {
            try
            {
                //verify if it has a doctor
                if (newDate.Fk_IdDoctor == 0)
                {
                    return Json(new { Success = false, message = "No se puede guardar una cita sin Doctor." });
                }
                //insert or update patient
                Tbl_Test_Patients patient = new Tbl_Test_Patients();
                if (newDate.Fk_IdPatient != 0)
                {
                    patient = db.Tbl_Test_Patients.SingleOrDefault(x => x.Int_Id == newDate.Fk_IdPatient);
                    patient.Chr_Name = newDate.Chr_Name;
                    patient.Chr_LastName = newDate.Chr_LastName;
                    patient.Chr_DX = newDate.Chr_DX;
                    patient.Chr_Email = newDate.Chr_Email;
                    patient.Bit_Active = true;
                }
                else
                {
                    patient.Chr_Name = newDate.Chr_Name;
                    patient.Chr_LastName = newDate.Chr_LastName;
                    patient.Chr_DX = newDate.Chr_DX;
                    patient.Chr_Email = newDate.Chr_Email;
                    patient.Bit_Active = true;
                    db.Tbl_Test_Patients.Add(patient);
                }
                db.SaveChanges();
                //insert or update date
                string tempCookieUserName = Request.Cookies["coUsername"].Value.ToString();
                //search organizer
                Tbl_Test_Users existUser = db.Tbl_Test_Users.Where(m => m.Chr_UserName.ToUpper().Trim() == tempCookieUserName.ToUpper().Trim()).FirstOrDefault();
                Tbl_Test_Dates tblDate = new Tbl_Test_Dates();
                if (newDate.Int_Id != 0)
                {
                    tblDate = db.Tbl_Test_Dates.SingleOrDefault(x => x.Int_Id == newDate.Int_Id);
                    tblDate.Date_Date = newDate.Date_Date;
                    tblDate.Fk_IdDoctor = newDate.Fk_IdDoctor;
                    tblDate.Fk_IdPatient = patient.Int_Id;
                    tblDate.Bit_Active = true;
                    tblDate.Fk_IdUserMaker = existUser.Int_Id;
                }
                else
                {
                    tblDate.Date_Date = newDate.Date_Date;
                    tblDate.Fk_IdDoctor = newDate.Fk_IdDoctor;
                    tblDate.Fk_IdPatient = patient.Int_Id;
                    tblDate.Bit_Active = true;
                    tblDate.Fk_IdUserMaker = existUser.Int_Id;
                    db.Tbl_Test_Dates.Add(tblDate);
                }
                db.SaveChanges();
                //build object to work it on js
                Tbl_Test_Doctors doctor = db.Tbl_Test_Doctors.Where(x => x.Int_Id == tblDate.Fk_IdDoctor).FirstOrDefault();
                clsAllDates newDateSaved = new clsAllDates()
                {
                    Int_Id = tblDate.Int_Id,
                    Date_Date = tblDate.Date_Date,
                    strDate = tblDate.Date_Date.ToString("yyyy-MM-dd"),
                    Fk_IdDoctor = doctor.Int_Id,
                    strDxName = doctor.Chr_Name,
                    strDxLastName = doctor.Chr_LastName,
                    Fk_IdPatient = patient.Int_Id,
                    strDx = patient.Chr_DX,
                    strPxName = patient.Chr_Name,
                    strPxLastName = patient.Chr_LastName,
                    Fk_IdUserMaker = existUser.Int_Id,
                    Chr_UserName = existUser.Chr_UserName,
                    Chr_MakerName = existUser.Chr_Name,
                    Chr_Email = patient.Chr_Email
                };
                return Json(new { Success = true, newDateSaved = newDateSaved });
            }
            catch (Exception ex)
            {
                Log.Log.Error("DatesController" + ex.ToString());
                return Json(new { Success = false, message = ex.ToString() });
            }
            
        }
        [HttpPost]
        public JsonResult deleteDate(int Int_Id)
        {
            try
            {
                //search and supress date
                Tbl_Test_Dates _date = db.Tbl_Test_Dates.SingleOrDefault(x => x.Int_Id == Int_Id);
                if (_date != null)
                {
                    _date.Bit_Active = false;
                    db.SaveChanges();
                }
                return Json(new { Success = true });
            }
            catch (Exception ex)
            {
                Log.Log.Error("DatesController" + ex.ToString());
                return Json(new { Success = false, message = ex.ToString() });
            }

        }
    }
}