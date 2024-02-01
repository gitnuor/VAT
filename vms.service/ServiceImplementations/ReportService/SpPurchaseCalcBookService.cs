using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.StoredProcedureModel;
using vms.entity.StoredProcedureModel.HTMLMushak;
using vms.repository.Repository.sp;
using vms.service.Services.ReportService;

namespace vms.service.ServiceImplementations.ReportService;

public class SpPurchaseCalcBookService : ISpPurchaseCalcBookService
{
    private readonly ISpPurchaseCalcBookRepository _repository;

    public SpPurchaseCalcBookService(ISpPurchaseCalcBookRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<SpPurchaseCalcBook>> GetSpPurchaseCalcBook(int organizationId, DateTime fromDate,
        DateTime toDate, int vendorId, int productId)
    {
        return await _repository.GetSpPurchaseCalcBook(organizationId, fromDate, toDate, vendorId, productId);
    }

    public async Task<List<SpSalesCalcBook>> GetSpSaleCalcBook(int organizationId, DateTime fromDate,
        DateTime toDate, int customerId, int productId)
    {
        return await _repository.GetSpSaleCalcBook(organizationId, fromDate, toDate, customerId, productId);
    }

    public async Task<List<SpPurchaseSaleCalcBook>> GetSpPurchaseSaleCalcBook(int organizationId, DateTime fromDate,
        DateTime toDate, int customerId, int vendorId, int productId)
    {
        return await _repository.GetSpPurchaseSaleCalcBook(organizationId, fromDate, toDate, customerId, vendorId,
            productId);
    }

    public async Task<List<Sp6p6>> GetSpVds(int purchaseId)
    {
        return await _repository.GetSpVds(purchaseId);
    }

    public async Task<List<SpCreditNoteMushak>> GetSpCreditNote(int creditNoteId)
    {
        return await _repository.GetSpCreditNote(creditNoteId);
    }

    public async Task<List<SpDebiNotetMushak>> GetSpDebitNote(int debitNoteId)
    {
        return await _repository.GetSpDebitNote(debitNoteId);
    }

    public async Task<List<SpSalesTaxInvoice>> GetChalan(int salesId)
    {
        return await _repository.GetChalan(salesId);
    }

    public async Task<List<Sp6p4>> Get6P4Raw(int id)
    {
        return await _repository.Get6P4Raw(id);
    }

    public async Task<List<Sp6p4>> Get6P4Finish(int id)
    {
        return await _repository.Get6P4Finish(id);
    }

    public async Task<List<Sp6p5>> Get6P5(int id)
    {
        return await _repository.Get6P5(id);
    }

    public async Task<List<Sp4p3>> Get4P3(int id)
    {
        return await _repository.Get4P3(id);
    }
}