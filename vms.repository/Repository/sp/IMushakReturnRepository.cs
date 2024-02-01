using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels.ReportsViewModel;

namespace vms.repository.Repository.sp;

public interface IMushakReturnRepository
{
    VmMushakReturn GetMushakReturn(int organizationId, int year, int month);
    Task<bool> InsertMushakSubmision(MushakSubmission model);

}