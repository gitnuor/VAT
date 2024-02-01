using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels.ProductUsedInService;

namespace vms.service.Services.TransactionService
{
    public interface IProductUsedInServicesService : IServiceBase<ProductUsedInService>
    {
        Task<IEnumerable<VmProductUsedInService>> GetProductUsedInServiceList(string pOrgId);
        Task<int> InsertProductUsedInServiceData(VmProductUsedInServicePostModel vmProductUsedInServicePostModel);
    }
}
