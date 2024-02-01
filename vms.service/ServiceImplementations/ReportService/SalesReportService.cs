using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using vms.entity.models;
using vms.entity.StoredProcedureModel.Sales;
using vms.entity.viewModels.SalesReport;
using vms.repository.Repository.sp;
using vms.service.Services.ReportService;

namespace vms.service.ServiceImplementations.ReportService;

public class SalesReportService : ISalesReportService
{
    private readonly IMapper _mapper;
    private readonly ISalesReportRepository _repository;

    public SalesReportService(IMapper mapper, ISalesReportRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }
    public async Task<IEnumerable<SalesReportViewModel>> GetSalesReportByBranchAndCustomer(int organizationId, int branchId, int customerId, DateTime? fromDate,
        DateTime? toDate, int userId)
    {
        var sales = await _repository.GetSalesReportData(organizationId, branchId, customerId, fromDate, toDate, userId);
        return _mapper.Map<IEnumerable<SpGetReportSalesModel>, IEnumerable<SalesReportViewModel>>(sales);
    }

    public async Task<IEnumerable<SalesReportViewModel>> GetSalesReportByBranch(int organizationId, int branchId, DateTime? fromDate, DateTime? toDate, int userId)
    {
        var sales = await _repository.GetSalesReportData(organizationId, branchId, 0, fromDate, toDate, userId);
        return _mapper.Map<IEnumerable<SpGetReportSalesModel>, IEnumerable<SalesReportViewModel>>(sales);
    }

    public async Task<IEnumerable<SalesReportViewModel>> GetSalesReportByCustomer(int organizationId, int customerId, DateTime? fromDate, DateTime? toDate, int userId)
    {
        var sales = await _repository.GetSalesReportData(organizationId, 0, customerId, fromDate, toDate, userId);
        return _mapper.Map<IEnumerable<SpGetReportSalesModel>, IEnumerable<SalesReportViewModel>>(sales);
    }

    public async Task<IEnumerable<SalesReportViewModel>> GetSalesReportAll(int organizationId, DateTime? fromDate, DateTime? toDate, int userId)
    {
        var sales = await _repository.GetSalesReportData(organizationId, 0, 0, fromDate, toDate, userId);
        return _mapper.Map<IEnumerable<SpGetReportSalesModel>, IEnumerable<SalesReportViewModel>>(sales);
    }

    public async Task<IEnumerable<ViewSalesDetail>> GetSalesDetailsListData(string organizationId)
    {
        return await _repository.GetSalesDetailsListData(organizationId);
    }
    public async Task<IEnumerable<ViewSalesDetail>> GetSalesDetailsListDataByOrgAndBranch(string organizationId, List<int> branchIds, bool isRequiredBranch)
    {
        return await _repository.GetSalesDetailsListDataByOrgAndBranch(organizationId, branchIds, isRequiredBranch);
    }


    public async Task<IEnumerable<ViewCreditNoteDetail>> CreditNoteDetailsReport(string organizationId)
    {
        return await _repository.CreditNoteDetailsReport(organizationId);
    }

    public async Task<IEnumerable<ViewCreditNoteDetail>> CreditNoteDetailsReportByOrgAndBranch(string organizationId, List<int> branchIds, bool isRequiredBranch)
    {
        return await _repository.CreditNoteDetailsReportByOrgAndBranch(organizationId, branchIds, isRequiredBranch);
    }

}