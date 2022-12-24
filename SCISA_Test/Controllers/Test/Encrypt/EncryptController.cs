using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static SCISA_Test.Models.General.clsGeneralViewModels;

namespace SCISA_Test.Controllers.Test.Encrypt
{
    public class EncryptController : Controller
    {
        [HttpPost]
        public string strEncrypt(string strText)
        {
            //encrypt string to MD5
            byte[] btAllBytes = new UnicodeEncoding().GetBytes(strText);
            byte[] btEncodeBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(btAllBytes);
            string strEncondeText = BitConverter.ToString(btEncodeBytes);
            return strEncondeText;
        }
    }
}