using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TrackableEntities.Common.Core;
using URF.Core.Services;
using vms.repository.Repository;

namespace vms.service;

public class ServiceBase<TEntity> : Service<TEntity>, IServiceBase<TEntity> where TEntity : class, ITrackable
{
	private readonly IRepositoryBase<TEntity> _repository;

	protected ServiceBase(IRepositoryBase<TEntity> repository) : base(repository)
	{
		_repository = repository;
	}

	public void BulkDelete(IEnumerable<TEntity> items)
	{
		_repository.BulkDelete(items);
	}

	public void BulkInsert(IEnumerable<TEntity> items)
	{
		_repository.BulkInsert(items);
	}

	public async Task BulkInsertAsync(IEnumerable<TEntity> items)
	{
		await _repository.BulkInsertAsync(items);
	}

	public async Task<List<T>> CreateQuery<T>(string sql, CancellationToken cancellationToken)
	{
		return await _repository.CreateQuery<T>(sql, cancellationToken);
	}

	public void CustomInsert(TEntity item)
	{
		_repository.CustomInsert(item);
	}

	public async Task CustomInsertAsync(TEntity item)
	{
		await _repository.CustomInsertAsync(item);
	}

	public TEntity Find(object[] keyValues, CancellationToken cancellationToken)
	{
		return _repository.Find(keyValues, cancellationToken);
	}
}