using vms.entity.viewModels.ReportsViewModel;

namespace vms.repository.Repository.sp;

public interface IMushakHighValueRepository
{
    VmMushakHighValue GetMushakReturn(int organizationId, int year, int month, decimal low);
}