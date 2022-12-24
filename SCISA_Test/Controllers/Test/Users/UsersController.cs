using SCISA_Test.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static SCISA_Test.Models.General.clsGeneralViewModels;

namespace SCISA_Test.Controllers.Test.Users
{
    public class UsersController : Encrypt.EncryptController
    {
        protected SCISADBEntities db = new SCISADBEntities();
        [HttpPost]
        public JsonResult getUsers()
        {
            try
            {
                //get all users with roles and access by role
                string strTempCookie = Request.Cookies["coUsername"].ToString().ToUpper().Trim();
                List<UserRoles> lUsers = (from t1 in db.Tbl_Test_Users
                                          join t2 in db.Tbl_Test_Roles on t1.Fk_IdRole equals t2.Int_Id
                                          where t1.Bit_Active == true && t1.Chr_UserName.ToUpper().Trim() != strTempCookie && t1.Chr_UserName.ToUpper().Trim() != "ADMIN"
                                          select new UserRoles
                                          {
                                              Int_Id = t1.Int_Id,
                                              Chr_Name = t1.Chr_Name,
                                              Chr_UserName = t1.Chr_UserName,
                                              Chr_Password = t1.Chr_Password,
                                              Chr_Email = t1.Chr_Email,
                                              Bit_Active = t1.Bit_Active,
                                              Fk_IdRole = t1.Fk_IdRole,
                                              Chr_RoleName = t2.Chr_Name
                                          }).ToList();
                List<Tbl_Test_Roles> lRoles = db.Tbl_Test_Roles.Where(x => x.Bit_Active == true).Select(x => new Tbl_Test_Roles
                {
                    Int_Id = x.Int_Id,
                    Chr_Name = x.Chr_Name
                }).ToList();
                List<int> lIdsRoles = lRoles.Select(x => x.Int_Id).ToList();
                List<Tbl_Test_Access> lAccess = (from t1 in db.Tbl_Test_Access
                                                 where lIdsRoles.Contains(t1.Fk_IdRole)
                                                 select t1).ToList();
                return Json(new { Success = true, lUsers = lUsers, lRoles = lRoles, lAccess = lAccess });
            }
            catch (Exception ex)
            {
                Log.Log.Error("UsersController" + ex.ToString());
                return Json(new { Success = true, message = ex.ToString() });
            }

        }
        [HttpPost]
        public JsonResult saveUser(Tbl_Test_Users _newUser)
        {
            try
            {
                //save new user
                if (db.Tbl_Test_Users.Where(x => x.Chr_UserName.ToUpper().Trim() == _newUser.Chr_UserName.ToUpper().Trim()).FirstOrDefault() != null)
                {
                    return Json(new { Success = true, message = "Nombre de usuario ya existente" });
                }
                Tbl_Test_Users _user = new Tbl_Test_Users();
                _user.Chr_UserName = _newUser.Chr_UserName;
                _user.Chr_Name = _newUser.Chr_Name;
                //encrypt password on MD5
                _user.Chr_Password = strEncrypt(_newUser.Chr_UserName);
                _user.Bit_Active = true;
                _user.Chr_Email = _newUser.Chr_Email;
                _user.Fk_IdRole = db.Tbl_Test_Roles.Where(x => x.Chr_Name.ToUpper().Trim() == "ADMINISTRADOR").Select(x => x.Int_Id).FirstOrDefault();
                db.Tbl_Test_Users.Add(_user);
                db.SaveChanges();
                return Json(new { Success = true });
            }
            catch (Exception ex)
            {
                Log.Log.Error("UsersController" + ex.ToString());
                return Json(new { Success = true, message = ex.ToString() });
            }

        }
    }
}