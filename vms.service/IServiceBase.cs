using TrackableEntities.Common.Core;
using URF.Core.Abstractions.Services;
using vms.repository.Repository;

namespace vms.service;

public interface IServiceBase<TEntity> : IService<TEntity>, IRepositoryBase<TEntity> where TEntity : class, ITrackable
{
}