using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.StoredProcedureModel;
using vms.entity.viewModels.ReportsViewModel;
using vms.repository.Repository.sp;
using vms.service.Services.MushakService;

namespace vms.service.ServiceImplementations.MushakService;

public class Mushok6P3ViewService : IMushok6P3ViewService
{
    private readonly IMushok6P3ViewRepositoy _repositoy;

    public Mushok6P3ViewService(IMushok6P3ViewRepositoy repositoy)
    {
        _repositoy = repositoy;
    }
    public async Task<List<spGet6P3View>> GetMushok6p3(vmSalesDetails vm)
    {
        return await _repositoy.GetMushok6p3(vm);

    }

}