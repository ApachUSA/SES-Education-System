﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Domain.ViewModel
{
	public class ChangePasswordVM
	{
		public int User_ID { get; set; }

		public required string Password { get; set; }

		public required string ConfirmPassword { get; set; }
	}
}
