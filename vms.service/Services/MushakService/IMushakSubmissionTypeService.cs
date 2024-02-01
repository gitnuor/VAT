using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.service.Services.MushakService;

public interface IMushakSubmissionTypeService : IServiceBase<MushakSubmissionType>
{
    Task<IEnumerable<MushakSubmissionType>> Get(int organizationId, string searchQuery = null);
}