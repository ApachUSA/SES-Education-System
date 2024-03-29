﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Domain.Entity.TestE
{
	public class Question
	{
		public int Question_ID { get; set; }

		[DisplayName("Текст запитання")]
		[Required(ErrorMessage = "Введіть текст запитання.")]
		public required string Text { get; set; }

		public required int Test_ID { get; set; }

		public Test? Test { get; set; }

		public List<Option>? Options { get; set; }
	}
}
