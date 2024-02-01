using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.SettingService;

public interface IPurchaseReasonService : IServiceBase<PurchaseReason>
{
    Task<IEnumerable<PurchaseReason>> GetPurchaseReasons();
    Task<IEnumerable<CustomSelectListItem>> GetSelectList();
    Task<PurchaseReason> GetPurchaseReason(string idEnc);
}