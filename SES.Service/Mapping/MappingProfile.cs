using AutoMapper;
using SES.Domain.Entity.TestE;
using SES.Domain.Entity.UserE;
using SES.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Service.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<RegisterVM, User>()
			.ForMember(opt => opt.Surname, opt => opt.MapFrom(src => SplitSnp(src.Snp, 0)))
			.ForMember(opt => opt.Name, opt => opt.MapFrom(src => SplitSnp(src.Snp, 1)))
			.ForMember(opt => opt.Patronymic, opt => opt.MapFrom(src => SplitSnp(src.Snp, 2)));

			CreateMap<Test_Result, Test_History>();
		}

		private static string? SplitSnp(string snp, int index)
		{
			var parts = snp.Split(' ');
			return index >= 0 && index < parts.Length ? parts[index] : null;
		}
	}
}
