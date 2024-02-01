using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels.ByProductReceive;

namespace vms.repository.Repository.tbl
{
    public interface IByProductReceiveRepository : IRepositoryBase<ByProductReceive>
	{
		Task<IEnumerable<ByProductReceive>> GetByProductReceiveList(string pOrgId);
		Task<int> InsertByProductReceiveData(VmByProductReceivePostModel vmByProductReceivePostModel);

    }
}
