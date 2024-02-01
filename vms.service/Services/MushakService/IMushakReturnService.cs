using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels.ReportsViewModel;

namespace vms.service.Services.MushakService;

public interface IMushakReturnService
{
    VmMushakReturn GetMushakReturn(int organizationId, int year, int month);
    Task<bool> InsertMushakSubmision(MushakSubmission model);

}