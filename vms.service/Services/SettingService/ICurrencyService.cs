using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;

namespace vms.service.Services.SettingService;

public interface ICurrencyService : IServiceBase<Currency>
{
    Task<SelectList> GetCurrencySelectList();
    Task<IEnumerable<Currency>> GetCurrencyList();
}