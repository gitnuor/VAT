using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IPriceSetupRepository : IRepositoryBase<PriceSetup>
{
    Task<IEnumerable<ViewInputOutputCoEfficient>> GetInputOutputCoEfficientReportData(string pOrgId);
}