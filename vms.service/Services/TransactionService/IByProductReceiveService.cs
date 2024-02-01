using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels.ByProductReceive;

namespace vms.service.Services.TransactionService
{
    public interface IByProductReceiveService : IServiceBase<ByProductReceive>
    {
        Task<IEnumerable<VmByProductReceive>> GetByProductReceiveList(string pOrgId);
        Task<int> InsertByProductReceiveData(VmByProductReceivePostModel vmByProductReceivePostModel);
    }
}
