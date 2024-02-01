using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using vms.entity.StoredProcedureModel.Purchase;
using vms.entity.viewModels.PurchaseReport;
using vms.repository.Repository.sp;
using vms.service.Services.ReportService;

namespace vms.service.ServiceImplementations.ReportService;

public class PurchaseSummeryReportService : IPurchaseSummeryReportService
{
    private readonly IMapper _mapper;
    private readonly IPurchaseReportRepository _repository;

    public PurchaseSummeryReportService(IMapper mapper, IPurchaseReportRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<IEnumerable<PurchaseSummeryReportViewModel>> GetByBranch(int organizationId, int branchId, DateTime? fromDate, DateTime? toDate, int userId)
    {
        var purchases = await _repository.GetPurchaseSummeryReportData(organizationId, branchId, 0, fromDate, toDate, userId);
        return _mapper.Map<IEnumerable<SpGetReportSummeryPurchaseModel>, IEnumerable<PurchaseSummeryReportViewModel>>(purchases);
    }

    public async Task<IEnumerable<PurchaseSummeryReportViewModel>> GetByVendor(int organizationId, int vendorId, DateTime? fromDate, DateTime? toDate, int userId)
    {
        var purchases = await _repository.GetPurchaseSummeryReportData(organizationId, 0, vendorId, fromDate, toDate, userId);
        return _mapper.Map<IEnumerable<SpGetReportSummeryPurchaseModel>, IEnumerable<PurchaseSummeryReportViewModel>>(purchases);
    }

    public async Task<IEnumerable<PurchaseSummeryReportViewModel>> GetAll(int organizationId, DateTime? fromDate, DateTime? toDate, int userId)
    {
        var purchases = await _repository.GetPurchaseSummeryReportData(organizationId, 0, 0, fromDate, toDate, userId);
        return _mapper.Map<IEnumerable<SpGetReportSummeryPurchaseModel>, IEnumerable<PurchaseSummeryReportViewModel>>(purchases);
    }
}