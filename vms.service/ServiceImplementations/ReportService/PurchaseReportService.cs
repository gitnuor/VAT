using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using vms.entity.models;
using vms.entity.StoredProcedureModel.Purchase;
using vms.entity.viewModels.PurchaseReport;
using vms.repository.Repository.sp;
using vms.service.Services.ReportService;

namespace vms.service.ServiceImplementations.ReportService;

public class PurchaseReportService : IPurchaseReportService
{
    private readonly IMapper _mapper;
    private readonly IPurchaseReportRepository _repository;

    public PurchaseReportService(IMapper mapper, IPurchaseReportRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }
    public async Task<IEnumerable<PurchaseReportViewModel>> GetPurchaseReportByBranchAndVendor(int organizationId, int branchId, int vendorId, DateTime? fromDate,
        DateTime? toDate, int userId)
    {
        var purchases = await _repository.GetPurchaseReportData(organizationId, branchId, vendorId, fromDate, toDate, userId);
        return _mapper.Map<IEnumerable<SpGetReportPurchaseModel>, IEnumerable<PurchaseReportViewModel>>(purchases);
    }

    public async Task<IEnumerable<PurchaseReportViewModel>> GetPurchaseReportByBranch(int organizationId, int branchId, DateTime? fromDate, DateTime? toDate, int userId)
    {
        var purchases = await _repository.GetPurchaseReportData(organizationId, branchId, 0, fromDate, toDate, userId);
        return _mapper.Map<IEnumerable<SpGetReportPurchaseModel>, IEnumerable<PurchaseReportViewModel>>(purchases);
    }

    public async Task<IEnumerable<PurchaseReportViewModel>> GetPurchaseReportByVendor(int organizationId, int vendorId, DateTime? fromDate, DateTime? toDate, int userId)
    {
        var purchases = await _repository.GetPurchaseReportData(organizationId, 0, vendorId, fromDate, toDate, userId);
        return _mapper.Map<IEnumerable<SpGetReportPurchaseModel>, IEnumerable<PurchaseReportViewModel>>(purchases);
    }

    public async Task<IEnumerable<PurchaseReportViewModel>> GetPurchaseReportAll(int organizationId, DateTime? fromDate, DateTime? toDate, int userId)
    {
        var purchases = await _repository.GetPurchaseReportData(organizationId, 0, 0, fromDate, toDate, userId);
        return _mapper.Map<IEnumerable<SpGetReportPurchaseModel>, IEnumerable<PurchaseReportViewModel>>(purchases);
    }

    public async Task<IEnumerable<ViewPurchaseDetail>> GetPuchaseDetailsListData(string organizationId)
    {
        return await _repository.GetPuchaseDetailsListData(organizationId);
    }

    public async Task<IEnumerable<ViewPurchaseDetail>> GetPuchaseDetailsListDataByOrgAndBranch(string organizationId, List<int> branchIds, bool isRequiredBranch)
    {
        return await _repository.GetPuchaseDetailsListDataByOrgAndBranch(organizationId, branchIds, isRequiredBranch);
    }

    public async Task<IEnumerable<ViewDebitNoteDetail>> DebitNoteDetailsReport(string organizationId)
    {
        return await _repository.DebitNoteDetailsReport(organizationId);
    }

    public async Task<IEnumerable<ViewDebitNoteDetail>> DebitNoteDetailsReportByOrgAndBranch(string organizationId, List<int> branchIds, bool isRequiredBranch)
    {
        return await _repository.DebitNoteDetailsReportByOrgAndBranch(organizationId, branchIds, isRequiredBranch);
    }
}