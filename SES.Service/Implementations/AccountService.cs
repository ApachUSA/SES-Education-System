using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SES.Domain.Entity.UserE;
using SES.Domain.Enum;
using SES.Domain.Response;
using SES.Domain.ViewModel;
using SES.Infrastructure.Interfaces;
using SES.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SES.Service.Implementations
{
	public class AccountService : IAccountService
	{

		private readonly IMapper _mapper;
		private readonly IBaseRepository<User> _userRespository;

		public AccountService(IMapper mapper, IBaseRepository<User> userRespository)
		{
			_mapper = mapper;
			_userRespository = userRespository;
		}

		public async Task<BaseResponse<ClaimsIdentity>> Login(LoginVM model)
		{
			try
			{
				var user = await _userRespository.Get().Include(x => x.Department).FirstOrDefaultAsync(x => x.Login == model.Login && x.Password == model.Password);
				if (user == null)
				{
					return BaseResponse<ClaimsIdentity>.Fail(ResponseStatus.UserNotFound, "Неправельний логін або пароль");
				}

				return BaseResponse<ClaimsIdentity>.Success(Auth(user), "Користувач аторизований");
			}
			catch (Exception e)
			{
				return BaseResponse<ClaimsIdentity>.Error($"[Login] : {e.Message}");
			}
		}

		public async Task<BaseResponse<bool>> Register(RegisterVM model)
		{
			try
			{
				var user = _mapper.Map<User>(model);

				if (_userRespository.Get().Any(x => x.Surname == user.Surname && x.Name == user.Name && x.Position_ID == user.Position_ID))
				{
					return BaseResponse<bool>.Fail(ResponseStatus.UserAlreadyExists, "Такий користувач вже є.");
				}

				await _userRespository.Create(user);

				return BaseResponse<bool>.Success(true, "Користувач зареєстрований");
			}
			catch (Exception e)
			{
				return BaseResponse<bool>.Error($"[Register] : {e.Message}");
			}
		}



		private static ClaimsIdentity Auth(User user)
		{
			var claims = new List<Claim>()
			{
				new Claim(ClaimsIdentity.DefaultNameClaimType, user.Surname + " " + user.Name + " " + user.Patronymic),
				new Claim(ClaimsIdentity.DefaultRoleClaimType,user.Role_ID.ToString()),
				new Claim("UserId", user.User_ID.ToString()),
				new Claim("Department", user.Department.Number.ToString())
			};

			return new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

		}
	}
}
