using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IContentRepository : IRepositoryBase<Content>
{
	Task<IEnumerable<ViewUploadedContent>> GetContentByObject(int objectId, int objectPrimaryKey);
}