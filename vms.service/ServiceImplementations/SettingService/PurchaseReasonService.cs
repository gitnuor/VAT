using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.Utility;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class PurchaseReasonService : ServiceBase<PurchaseReason>, IPurchaseReasonService
{
    private readonly IPurchaseReasonRepository _repository;

    public PurchaseReasonService(IPurchaseReasonRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PurchaseReason>> GetPurchaseReasons()
    {
        return await _repository.GetPurchaseReasons();
    }

    public async Task<IEnumerable<CustomSelectListItem>> GetSelectList()
    {
        var reasons = await _repository.GetPurchaseReasons();
        return reasons.ConvertToCustomSelectList(nameof(PurchaseReason.PurchaseReasonId),
            nameof(PurchaseReason.Reason));
    }

    public async Task<PurchaseReason> GetPurchaseReason(string idEnc)
    {
        return await _repository.GetPurchaseReason(idEnc);
    }
}