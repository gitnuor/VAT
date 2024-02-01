using vms.entity.models;
using vms.repository.Repository.tbl.acc;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class CoagroupService : ServiceBase<Coagroup>, ICoagroupService
{
    public CoagroupService(ICoagroupRepository repository) : base(repository)
    {
    }
}