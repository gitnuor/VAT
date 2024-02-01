using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.repository.Repository.tbl;

public interface IProductionRepository : IRepositoryBase<ProductionReceive>
{
    Task<string> InsertData(vmProductionReceive production);

    Task<IEnumerable<ProductionReceive>> GetProductions(int orgIdEnc);
    Task<IEnumerable<ViewProductionReceive>> ViewProductionReceive(string orgIdEnc);
    Task<IEnumerable<ViewProductionReceive>> ViewProductionReceiveByOrgAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch);
}