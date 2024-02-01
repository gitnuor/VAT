using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels.ReportsViewModel;
using vms.repository.Repository.sp;
using vms.service.Services.MushakService;

namespace vms.service.ServiceImplementations.MushakService;

public class MushakReturnService : IMushakReturnService
{
    public IMushakReturnRepository Repo { get; }

    public MushakReturnService(IMushakReturnRepository repository)
    {
        Repo = repository;
    }

    public VmMushakReturn GetMushakReturn(int organizationId, int year, int month)
    {
        return Repo.GetMushakReturn(organizationId, year, month);
    }

    public async Task<bool> InsertMushakSubmision(MushakSubmission model)
    {
        return await Repo.InsertMushakSubmision(model);
    }
}