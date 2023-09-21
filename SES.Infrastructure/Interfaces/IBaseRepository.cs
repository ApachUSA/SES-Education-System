using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SES.Infrastructure.Interfaces
{
	public interface IBaseRepository<T> where T : class
	{
		Task Create(T entity);

		Task Delete(T entity);

		IQueryable<T> Get();

		Task<T> Update(T entity);

		Task<List<T>> Update(List<T> entity);
	}
}
