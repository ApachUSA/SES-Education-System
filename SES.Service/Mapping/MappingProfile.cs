﻿using AutoMapper;
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
			.ForMember(opt => opt.User_ID, opt => opt.MapFrom(src => src.User_ID_VM))
			.ForMember(opt => opt.Surname, opt => opt.MapFrom(src => SplitSnp(src.Snp, 0)))
			.ForMember(opt => opt.Name, opt => opt.MapFrom(src => SplitSnp(src.Snp, 1)))
			.ForMember(opt => opt.Patronymic, opt => opt.MapFrom(src => SplitSnp(src.Snp, 2)))
			.ForMember(opt => opt.Department_ID, opt => opt.MapFrom(src => src.Department_ID_VM))
			.ForMember(opt => opt.Rang_ID, opt => opt.MapFrom(src => src.Rang_ID_VM))
			.ForMember(opt => opt.Position_ID, opt => opt.MapFrom(src => src.Position_ID_VM))
			.ForMember(opt => opt.Login, opt => opt.MapFrom(src => src.Login_VM))
			.ForMember(opt => opt.Password, opt => opt.MapFrom(src => src.Password_VM))
			.ForMember(opt => opt.PasswordConfirm, opt => opt.MapFrom(src => src.Password_VM))
			.ForMember(opt => opt.Role_ID, opt => opt.MapFrom(src => src.Role_ID_VM));

			CreateMap<User, RegisterVM>()
			.ForMember(dest => dest.User_ID_VM, opt => opt.MapFrom(src => src.User_ID))
			.ForMember(dest => dest.Snp, opt => opt.MapFrom(src => $"{src.Surname} {src.Name} {src.Patronymic}"))
			.ForMember(dest => dest.Department_ID_VM, opt => opt.MapFrom(src => src.Department_ID))
			.ForMember(dest => dest.Rang_ID_VM, opt => opt.MapFrom(src => src.Rang_ID))
			.ForMember(dest => dest.Position_ID_VM, opt => opt.MapFrom(src => src.Position_ID))
			.ForMember(dest => dest.Login_VM, opt => opt.MapFrom(src => src.Login))
			.ForMember(dest => dest.Password_VM, opt => opt.MapFrom(src => src.Password))
			.ForMember(dest => dest.Role_ID_VM, opt => opt.MapFrom(src => src.Role_ID));

			CreateMap<User, UserApiVM>()
				.ForMember(dest => dest.User_ID, opt => opt.MapFrom(src => src.User_ID))
				.ForMember(dest => dest.Snp, opt => opt.MapFrom(src => $"{src.Surname} {src.Name} {src.Patronymic}"))
				.ForMember(dest => dest.Rang, opt => opt.MapFrom(src => src.Rang.Name))
				.ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position.Name));


			CreateMap<Test_Result, Test_History>();
		}

		private static string? SplitSnp(string snp, int index)
		{
			string[] words = snp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			return index >= 0 && index < words.Length ? words[index] : null;
		}
	}
}
