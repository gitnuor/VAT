using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.viewModels.SalesPriceAdjustment;

namespace vms.service.Services.TransactionService;

public interface ISalesPriceAdjustmentService : IServiceBase<SalesPriceAdjustment>
{
    Task<IEnumerable<SalesPriceAdjustment>> GetSalesPriceAdjustmentsByOrganization(string orgIdEnc);
    Task<IEnumerable<SalesPriceAdjustment>> GetSalesPriceAdjustmentsByOrganizationAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch);
    Task<IEnumerable<SalesPriceAdjustment>> GetSalesPriceDecrementedAdjustmentsByOrganization(string orgIdEnc);
    Task<IEnumerable<SalesPriceAdjustment>> GetSalesPriceDecrementedAdjustmentsByOrganizationAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch);
    Task<IEnumerable<SalesPriceAdjustment>> GetSalesPriceIncrementedAdjustmentsByOrganization(string orgIdEnc);
    Task<int> InsertCreditNoteAkaDecreasePrice(SalesPriceAdjustmentCreditNotePostViwModel model);
    Task<int> InsertDebitNoteAkaIncreasePrice(SalesPriceAdjustmentDebitNotePostViwModel model);
    Task<IEnumerable<SpGetCreditNoteMushakForSalesPriceReduce>> GetCreditNoteMushakForSalesPriceReduce(string priceAdjustmentIdEnc, int userId);
}