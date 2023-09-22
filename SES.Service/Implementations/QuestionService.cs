using Microsoft.EntityFrameworkCore;
using SES.Domain.Entity.TestE;
using SES.Domain.Enum;
using SES.Domain.Response;
using SES.Infrastructure.Interfaces;
using SES.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Service.Implementations
{
	public class QuestionService : IQuestionService
	{

		private readonly IBaseRepository<Question> _questionRepository;
		private readonly IOptionService _optionService;

		public QuestionService(IBaseRepository<Question> questionRepository, IOptionService optionService)
		{
			_questionRepository = questionRepository;
			_optionService = optionService;
		}

		public async Task<BaseResponse<bool>> Create(List<Question> model)
		{
			try
			{
				await _questionRepository.Create(model);

				foreach (var item in model)
				{
					if(item.Options != null) await _optionService.Create(item.Options);
				}

				return BaseResponse<bool>.Success(true, "Питання додані");
			}
			catch (Exception e)
			{
				return BaseResponse<bool>.Error($"[Questions Create] : {e.Message}");
			}
		}

		public async Task<BaseResponse<bool>> Create(Question model)
		{
			try
			{
				await _questionRepository.Create(model);

				await _optionService.Create(new List<Option>
				{
					new Option
					{
						Text = string.Empty,
						Question_ID = model.Question_ID
					},

					new Option
					{
						Text = string.Empty,
						Question_ID = model.Question_ID
					},
				});

				return BaseResponse<bool>.Success(true, "Питання додано");
			}
			catch (Exception e)
			{
				return BaseResponse<bool>.Error($"[Question Create] : {e.Message}");
			}
		}

		public async Task<BaseResponse<bool>> Delete(int id)
		{
			try
			{
				var question = await _questionRepository.Get().FirstOrDefaultAsync(x => x.Question_ID == id);
				if (question == null) return BaseResponse<bool>.Fail(ResponseStatus.ItemNotFound, "Питання не знайдено");

				await _questionRepository.Delete(question);

				return BaseResponse<bool>.Success(true, "Питання видалено");
			}
			catch (Exception e)
			{
				return BaseResponse<bool>.Error($"[Question Delete] : {e.Message}");
			}
		}

		public async Task<BaseResponse<List<Question>>> Get(int test_ID)
		{
			try
			{
				var questions = await _questionRepository.Get()
					.Include(x => x.Options)
					.Where(x => x.Test_ID == test_ID)
					.ToListAsync();
				if (questions == null) return BaseResponse<List<Question>>.Fail(ResponseStatus.ItemNotFound, "Питання не знайдено");

				return BaseResponse<List<Question>>.Success(questions, "");
			}
			catch (Exception e)
			{
				return BaseResponse<List<Question>>.Error($"[Question Get] : {e.Message}");
			}
		}
	}
}
