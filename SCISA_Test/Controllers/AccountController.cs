using Newtonsoft.Json;
using SCISA_Test.Controllers.Test.Encrypt;
using SCISA_Test.Models.Database;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static SCISA_Test.Models.General.clsGeneralViewModels;
using static SCISA_Test.Models.Login.clsLoginModel;

namespace SCISA_Test.Controllers
{
    public class AccountController : EncryptController
    {
        protected SCISADBEntities db = new SCISADBEntities();
        [HttpGet]
        public ActionResult Lobby()
        {
            //if session not expired yet
            if (Request.Cookies["coUsername"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult Login()
        {
            //if session not expired yet
            if (Request.Cookies["coUsername"] == null)
            {
                return View();
            } else
            {
                return RedirectToAction("Lobby", "Account");
            }
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            try
            {
                //create a session to user
                Tbl_Test_Users existUser = db.Tbl_Test_Users.Where(m => m.Chr_UserName.ToUpper().Trim() == model.inpUserName.ToUpper().Trim()).FirstOrDefault();
                if (existUser == null || existUser.Bit_Active == false)
                {
                    ViewBag.Error = "Usuario incorrecto o desactivado.";
                    return View();
                }
                //valid the model
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                //encrypt password on MD5
                if (strEncrypt(model.inpPassword.ToString()) == existUser.Chr_Password)
                {
                    DateTime now = DateTime.Now;
                    //Cookie username
                    createCookie(now, "coUsername", model.inpUserName.ToUpper().Trim());
                    return RedirectToAction("Lobby", "Account");
                }
                else
                {
                    ViewBag.Error = "Contraseña incorrecta.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                Log.Log.Error("AccountController" + ex.ToString());
                ViewBag.Error = "Error no reconocido.";
                return View();
            }
        }
        protected void createCookie(DateTime date, string strCookieName, string strValue)
        {
            //create a cookie to use on other processes
            HttpCookie _cookie = new HttpCookie(strCookieName);
            _cookie.Value = strValue;
            _cookie.Expires = date.AddDays(1);
            Response.Cookies.Add(_cookie);
        }
        public ActionResult LogOut()
        {
            //delete all cookies from this site
            string cookieName;
            HttpCookie aCookie;
            int limit = Request.Cookies.Count;
            for (int i = 0; i < limit; i++)
            {
                cookieName = Request.Cookies[i].Name;
                aCookie = new HttpCookie(cookieName);
                //Expiration last day
                aCookie.Expires = DateTime.Now.AddDays(-1);
                //Set cookie and automatically this was deleted
                Response.Cookies.Add(aCookie);
            }
            return RedirectToAction("Login", "Account");
        }
    }
}