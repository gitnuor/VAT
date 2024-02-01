using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.StoredProcedureModel;
using vms.entity.StoredProcedureModel.HTMLMushak;

namespace vms.service.Services.ReportService;

public interface ISpPurchaseCalcBookService
{
    Task<List<SpPurchaseCalcBook>> GetSpPurchaseCalcBook(int organizationId, DateTime fromDate, DateTime toDate,
        int vendorId, int productId);

    Task<List<SpSalesCalcBook>> GetSpSaleCalcBook(int organizationId, DateTime fromDate, DateTime toDate,
        int customerId, int productId);

    Task<List<SpPurchaseSaleCalcBook>> GetSpPurchaseSaleCalcBook(int organizationId, DateTime fromDate,
        DateTime toDate, int customerId, int vendorId, int productId);

    Task<List<Sp6p6>> GetSpVds(int purchaseId);
    Task<List<SpCreditNoteMushak>> GetSpCreditNote(int creditNoteId);
    Task<List<SpDebiNotetMushak>> GetSpDebitNote(int debitNoteId);
    Task<List<SpSalesTaxInvoice>> GetChalan(int salesId);
    Task<List<Sp6p4>> Get6P4Raw(int id);
    Task<List<Sp6p4>> Get6P4Finish(int id);
    Task<List<Sp6p5>> Get6P5(int id);
    Task<List<Sp4p3>> Get4P3(int id);
}