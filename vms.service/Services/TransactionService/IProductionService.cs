using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.TransactionService;

public interface IProductionService : IServiceBase<ProductionReceive>
{
    Task<string> InsertData(vmProductionReceive production);
    Task<string> InserSelfProductiontData(VmSelfProduction production, VmUserSession userSession);
    Task<string> InsertContractualProductionData(VmContractualProduction production, VmUserSession userSession);
    Task<IEnumerable<ProductionReceive>> GetProductions(int orgIdEnc);
    Task<IEnumerable<ViewProductionReceive>> ViewProductionReceive(string orgIdEnc);
    Task<IEnumerable<ViewProductionReceive>> ViewProductionReceiveByOrgAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch);
}