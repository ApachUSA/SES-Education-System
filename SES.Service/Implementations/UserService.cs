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
	public class UserService : IUserService
	{
		private readonly IBaseRepository<User> _userRepository;

		public UserService(IBaseRepository<User> userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<BaseResponse<bool>> ChangePassword(ChangePasswordVM model)
		{
			try
			{
				var user = await _userRepository.Get().FirstOrDefaultAsync(x => x.User_ID == model.User_ID);
				if (user == null) return BaseResponse<bool>.Fail(ResponseStatus.UserNotFound, "Користувача не знайдено");

				user.Password = model.Password;
				await _userRepository.Update(user);

				return BaseResponse<bool>.Success(true, "");
			}
			catch (Exception e)
			{
				return BaseResponse<bool>.Error($"[User ChangePassword] : {e.Message}");
			}
		}

		public async Task<BaseResponse<bool>> Delete(int id)
		{
			try
			{
				var user = await _userRepository.Get().FirstOrDefaultAsync(x => x.User_ID == id);
				if (user == null) return BaseResponse<bool>.Fail(ResponseStatus.UserNotFound, "Користувача не знайдено");

				await _userRepository.Delete(user);
				return BaseResponse<bool>.Success(true, "Користувача видалено");
			}
			catch (Exception e)
			{
				return BaseResponse<bool>.Error($"[User Delete] : {e.Message}");
			}
		}

		public async Task<BaseResponse<List<User>>> Get()
		{
			try
			{
				var users = await _userRepository.Get()
					.Include(x => x.Department)
					.ThenInclude(x => x.City)
					.Include(x => x.Position)
					.Include(x => x.Test_Histories)
					.ToListAsync();

				if (users == null) return BaseResponse<List<User>>.Fail(ResponseStatus.UserNotFound, "Користувачів не знайдено");

				return BaseResponse<List<User>>.Success(users, "");
			}
			catch (Exception e)
			{
				return BaseResponse<List<User>>.Error($"[User Get (List)] : {e.Message}");
			}
		}

		public async Task<BaseResponse<User>> Get(int id)
		{
			try
			{
				var user = await _userRepository.Get()
					.Include(x => x.Department)
					.ThenInclude(x => x.City)
					.Include(x => x.Rang)
					.Include(x => x.Position)
					.Include(x => x.Test_Histories)
					.FirstOrDefaultAsync(x => x.User_ID == id);

				if (user == null) return BaseResponse<User>.Fail(ResponseStatus.UserNotFound, "Користувача не знайдено");

				return BaseResponse<User>.Success(user, "");
			}
			catch (Exception e)
			{
				return BaseResponse<User>.Error($"[User Get] : {e.Message}");
			}
		}

		public async Task<BaseResponse<User>> Update(User model)
		{
			try
			{
				await _userRepository.Update(model);
				return BaseResponse<User>.Success(model, "");
			}
			catch (Exception e)
			{
				return BaseResponse<User>.Error($"[User Update] : {e.Message}");
			}
		}
	}
}
