using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.service.Services.MushakService;

public interface IMushakSubmissionsService : IServiceBase<MushakSubmission>
{
    Task<IEnumerable<MushakSubmission>> Get(int organizationId, string searchQuery = null);
}