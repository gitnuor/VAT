using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.ProductService;

public interface IPriceSetupService : IServiceBase<PriceSetup>
{
    Task InsertProductPriceAsync(VmInputOutputCoEfficientPost vmInputOutputCoEfficientPost, VmUserSession userSession);
    Task<IEnumerable<PriceSetup>> GetPriceSetupByProductId(int productId);
    Task<IEnumerable<ViewInputOutputCoEfficient>> GetInputOutputCoEfficientReportData(string pOrgId);
}