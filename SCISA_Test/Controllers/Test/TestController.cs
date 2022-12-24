using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using SCISA_Test.Models.Database;
using static SCISA_Test.Models.General.clsGeneralViewModels;

namespace SCISA_Test.Controllers.Test
{
    public class TestController : Controller
    {

        
        protected object getInfo(string api)
        {
            try
            {
                string urlWebApi = ConfigurationManager.AppSettings["urlWebApi"].ToString();
                if (urlWebApi[urlWebApi.Length - 1].ToString() != "/")
                {
                    urlWebApi += "/";
                }
                JavaScriptSerializer js = new JavaScriptSerializer();
                //string json = JsonConvert.SerializeObject(new clsText() { strText = strText });
                WebRequest request = WebRequest.Create(urlWebApi + api);
                request.Method = "POST";
                request.PreAuthenticate = true;
                request.ContentType = "application/json;charset=utf-8'";
                using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    //streamWriter.Write(json);
                    streamWriter.Flush();
                }
                var httpResponse = request.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    return streamReader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}