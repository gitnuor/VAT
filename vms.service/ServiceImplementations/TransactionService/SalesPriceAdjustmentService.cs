using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.viewModels.SalesPriceAdjustment;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class SalesPriceAdjustmentService : ServiceBase<SalesPriceAdjustment>, ISalesPriceAdjustmentService
{
    private readonly ISalesPriceAdjustmentRepository _repository;
    private readonly IMapper _mapper;
    public SalesPriceAdjustmentService(ISalesPriceAdjustmentRepository repository, IMapper mapper) : base(repository)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SalesPriceAdjustment>> GetSalesPriceAdjustmentsByOrganization(string orgIdEnc)
    {
        return await _repository.GetSalesPriceAdjustmentsByOrganization(orgIdEnc);
    }

    public async Task<IEnumerable<SalesPriceAdjustment>> GetSalesPriceAdjustmentsByOrganizationAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch)
    {
        return await _repository.GetSalesPriceAdjustmentsByOrganizationAndBranch(orgIdEnc, branchIds, isRequiredBranch);
    }

    public async Task<IEnumerable<SalesPriceAdjustment>> GetSalesPriceDecrementedAdjustmentsByOrganization(string orgIdEnc)
    {
        return await _repository.GetSalesPriceDecrementedAdjustmentsByOrganization(orgIdEnc);
    }

    public async Task<IEnumerable<SalesPriceAdjustment>> GetSalesPriceDecrementedAdjustmentsByOrganizationAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch)
    {
        return await _repository.GetSalesPriceDecrementedAdjustmentsByOrganizationAndBranch(orgIdEnc, branchIds, isRequiredBranch);
    }

    public async Task<IEnumerable<SalesPriceAdjustment>> GetSalesPriceIncrementedAdjustmentsByOrganization(string orgIdEnc)
    {
        return await _repository.GetSalesPriceIncrementedAdjustmentsByOrganization(orgIdEnc);
    }

    public Task<int> InsertCreditNoteAkaDecreasePrice(SalesPriceAdjustmentCreditNotePostViwModel model)
    {
        var creditModel = _mapper.Map<SalesPriceAdjustmentCreditNotePostViwModel, SalesPriceAdjustmentCombineParamsViewModel>(model);
        creditModel.AdjustmentTypeId = 2;
        return _repository.InsertPriceAdjustment(creditModel);
    }

    public Task<int> InsertDebitNoteAkaIncreasePrice(SalesPriceAdjustmentDebitNotePostViwModel model)
    {
        var debitModel = _mapper.Map<SalesPriceAdjustmentDebitNotePostViwModel, SalesPriceAdjustmentCombineParamsViewModel>(model);
        debitModel.AdjustmentTypeId = 2;
        return _repository.InsertPriceAdjustment(debitModel);
    }

    public Task<IEnumerable<SpGetCreditNoteMushakForSalesPriceReduce>> GetCreditNoteMushakForSalesPriceReduce(string priceAdjustmentIdEnc, int userId)
    {
        return _repository.GetCreditNoteMushakForSalesPriceReduce(priceAdjustmentIdEnc, userId);
    }
}