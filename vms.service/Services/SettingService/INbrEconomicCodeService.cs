using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;

namespace vms.service.Services.SettingService;

public interface INbrEconomicCodeService : IServiceBase<NbrEconomicCode>
{
    Task<SelectList> GetNbrEconomicCodeSelectList();
}