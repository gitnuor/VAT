using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.Utility;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class PurchaseImportTaxPaymentTypeService : ServiceBase<PurchaseImportTaxPaymentType>, IPurchaseImportTaxPaymentTypeService
{
    private readonly IPurchaseImportTaxPaymentTypeRepository _repository;
    public PurchaseImportTaxPaymentTypeService(IPurchaseImportTaxPaymentTypeRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CustomSelectListItem>> GetPurchaseImportTaxPaymentTypeSelectList()
    {
        var taxPaymentTypeList = await _repository.Query().SelectAsync();
        return taxPaymentTypeList.ConvertToCustomSelectList(nameof(PurchaseImportTaxPaymentType.PurchaseImportTaxPaymentTypeId),
            nameof(PurchaseImportTaxPaymentType.PaymentTypeShortName));
    }
}