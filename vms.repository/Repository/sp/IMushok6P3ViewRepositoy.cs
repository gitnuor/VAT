using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.StoredProcedureModel;
using vms.entity.viewModels.ReportsViewModel;

namespace vms.repository.Repository.sp;

public interface IMushok6P3ViewRepositoy
{
    Task<List<spGet6P3View>> GetMushok6p3(vmSalesDetails vm);

}