using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using vms.entity.StoredProcedureModel.Sales;
using vms.entity.viewModels.SalesReport;
using vms.repository.Repository.sp;
using vms.service.Services.ReportService;

namespace vms.service.ServiceImplementations.ReportService;

public class SalesSummeryReportService : ISalesSummeryReportService
{
    private readonly IMapper _mapper;
    private readonly ISalesReportRepository _repository;

    public SalesSummeryReportService(IMapper mapper, ISalesReportRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<IEnumerable<SalesSummeryReportViewModel>> GetByBranch(int organizationId, int branchId, DateTime? fromDate, DateTime? toDate, int userId)
    {
        var sales = await _repository.GetSalesSummeryReportData(organizationId, branchId, 0, fromDate, toDate, userId);
        return _mapper.Map<IEnumerable<SpGetReportSummerySalesModel>, IEnumerable<SalesSummeryReportViewModel>>(sales);
    }

    public async Task<IEnumerable<SalesSummeryReportViewModel>> GetByCustomer(int organizationId, int customerId, DateTime? fromDate, DateTime? toDate, int userId)
    {
        var sales = await _repository.GetSalesSummeryReportData(organizationId, 0, customerId, fromDate, toDate, userId);
        return _mapper.Map<IEnumerable<SpGetReportSummerySalesModel>, IEnumerable<SalesSummeryReportViewModel>>(sales);
    }

    public async Task<IEnumerable<SalesSummeryReportViewModel>> GetAll(int organizationId, DateTime? fromDate, DateTime? toDate, int userId)
    {
        var sales = await _repository.GetSalesSummeryReportData(organizationId, 0, 0, fromDate, toDate, userId);
        return _mapper.Map<IEnumerable<SpGetReportSummerySalesModel>, IEnumerable<SalesSummeryReportViewModel>>(sales);
    }
}