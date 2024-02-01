using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels.ProductUsedInService;

namespace vms.repository.Repository.tbl
{
    public interface IProductUsedInServicesRepository : IRepositoryBase<ProductUsedInService>
    {
        Task<IEnumerable<ProductUsedInService>> GetProductUsedInServiceList(string pOrgId);
        Task<int> InsertProductUsedInServiceData(VmProductUsedInServicePostModel vmProductUsedInServicePostModel);
    }
}
