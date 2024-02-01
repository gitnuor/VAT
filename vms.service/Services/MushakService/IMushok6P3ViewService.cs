using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.StoredProcedureModel;
using vms.entity.viewModels.ReportsViewModel;

namespace vms.service.Services.MushakService;

public interface IMushok6P3ViewService
{
    Task<List<spGet6P3View>> GetMushok6p3(vmSalesDetails vm);
}