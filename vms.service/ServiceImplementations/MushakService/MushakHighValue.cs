using vms.entity.viewModels.ReportsViewModel;
using vms.repository.Repository.sp;
using vms.service.Services.MushakService;

namespace vms.service.ServiceImplementations.MushakService;

public class MushakHighValue : IMushakHighValue
{
    public IMushakHighValueRepository Repo { get; }

    public MushakHighValue(IMushakHighValueRepository repository)
    {
        Repo = repository;
    }

    public VmMushakHighValue GetMushakReturn(int organizationId, int year, int month, decimal low)
    {
        return Repo.GetMushakReturn(organizationId, year, month, low);
    }
}