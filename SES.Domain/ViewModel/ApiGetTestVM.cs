﻿using SES.Domain.Entity.TestE;
using SES.Domain.Entity.UserE;
using SES.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Domain.ViewModel
{
	public class ApiGetTestVM
	{
		public ResultStatus? Status { get; set; }

		public int TestResult_ID { get; set; }

		public UserApiVM User { get; set; }

		public List<Question> Test { get; set; }
	}
}
