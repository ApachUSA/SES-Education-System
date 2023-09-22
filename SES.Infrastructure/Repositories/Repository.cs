using SES.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Infrastructure.Repositories
{
	public class Repository<T> : IBaseRepository<T> where T : class
	{
		private readonly SES_DB_Context _dbContext;

		public Repository(SES_DB_Context dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task Create(T entity)
		{
			await _dbContext.Set<T>().AddAsync(entity);
			await _dbContext.SaveChangesAsync();
		}

		public async Task Create(List<T> entity)
		{
			await _dbContext.Set<T>().AddRangeAsync(entity);
			await _dbContext.SaveChangesAsync();
		}

		public async Task Delete(T entity)
		{
			_dbContext.Set<T>().Remove(entity);
			await _dbContext.SaveChangesAsync();
		}

		public async Task Delete(List<T> entity)
		{
			_dbContext.Set<T>().RemoveRange(entity);
			await _dbContext.SaveChangesAsync();
		}

		public IQueryable<T> Get()
		{
			return _dbContext.Set<T>();
		}

		public async Task<T> Update(T entity)
		{
			_dbContext.Set<T>().Update(entity);
			await _dbContext.SaveChangesAsync();

			return entity;
		}

		public async Task<List<T>> Update(List<T> entity)
		{
			_dbContext.Set<T>().UpdateRange(entity);
			await _dbContext.SaveChangesAsync();

			return entity;
		}
	}
}
