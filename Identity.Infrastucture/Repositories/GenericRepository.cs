using Identity.Application.Abstractions.Repositories;
using Identity.Infrastucture.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Infrastucture.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		protected readonly AppIdentityDbContext _context;
		protected readonly DbSet<T> _dbSet;

		public GenericRepository(AppIdentityDbContext context)
		{
			_context = context;
			_dbSet = context.Set<T>();
		}

		public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
		{
			return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
		}

		public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			return await _dbSet.ToListAsync(cancellationToken);
		}

		public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
		{
			await _dbSet.AddAsync(entity, cancellationToken);
		}

		public void Update(T entity)
		{
			_dbSet.Update(entity);
		}

		public void Track(T entity)
		{
			_context.Attach(entity);
		}

		public void Delete(T entity)
		{
			_dbSet.Remove(entity);
		}

		public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}
