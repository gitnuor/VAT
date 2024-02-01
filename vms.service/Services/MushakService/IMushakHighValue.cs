using vms.entity.viewModels.ReportsViewModel;

namespace vms.service.Services.MushakService;

public interface IMushakHighValue
{
    VmMushakHighValue GetMushakReturn(int organizationId, int year, int month, decimal low);
}