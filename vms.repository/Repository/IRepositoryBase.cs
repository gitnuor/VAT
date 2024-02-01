using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TrackableEntities.Common.Core;
using URF.Core.Abstractions.Trackable;

namespace vms.repository.Repository;

public interface IRepositoryBase<TEntity> : ITrackableRepository<TEntity> where TEntity : class, ITrackable
{
    TEntity Find(object[] keyValues, CancellationToken cancellationToken);
    Task<List<T>> CreateQuery<T>(string sql, CancellationToken cancellationToken);

    void CustomInsert(TEntity item);
    Task CustomInsertAsync(TEntity item);
    void BulkInsert(IEnumerable<TEntity> items);
    Task BulkInsertAsync(IEnumerable<TEntity> items);
    void BulkDelete(IEnumerable<TEntity> items);
}