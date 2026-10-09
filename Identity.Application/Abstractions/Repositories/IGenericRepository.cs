using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Abstractions.Repositories
{
	public interface IGenericRepository<T> where T : class
	{
		Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
		Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
		Task AddAsync(T entity, CancellationToken cancellationToken = default);
		void Update(T entity);
		void Delete(T entity);
		Task SaveChangesAsync(CancellationToken cancellationToken = default);
		void Track(T entity);
	}
}
