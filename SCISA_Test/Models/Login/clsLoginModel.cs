using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCISA_Test.Models.Login
{
    public class clsLoginModel
    {
		public class LoginViewModel
		{
			[Required]
			public string inpUserName { get; set; }
			[Required]
			[DataType(DataType.Password)]
			[Display(Name = "pass")]
			public string inpPassword { get; set; }
		}
	}
}