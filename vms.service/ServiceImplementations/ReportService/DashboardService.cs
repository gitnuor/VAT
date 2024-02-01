using System.Threading.Tasks;
using vms.entity.StoredProcedureModel;
using System;
using System.Collections.Generic;
using vms.repository.Repository.sp;
using vms.service.Services.ReportService;

namespace vms.service.ServiceImplementations.ReportService;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _repository;

    public DashboardService(IDashboardRepository repository
    )
    {
        _repository = repository;
    }

    public async Task<SpDashboard> GetDashboard(int organizationId)
    {
        try
        {
            return await _repository.GetDashboard(organizationId);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);

        }
    }

    public Task<SpGetDashBoardInfo> GetDashboardInfo(int organizationId, int year, int month)
    {
        return _repository.GetDashboardInfo(organizationId, year, month);
    }

    public Task<IEnumerable<SpGetYearlyComparison>> GetYearlyComparisonInfo(int organizationId, int fromYear, int toYear, int userId)
    {
        return _repository.GetYearlyComparisonInfo(organizationId, fromYear, toYear, userId);
    }

    public Task<IEnumerable<SpGetMonthlyComparison>> GetMonthlyComparisonInfo(int organizationId, int fromYear, int formMonth, int toYear, int toMonth, int userId)
    {
        return _repository.GetMonthlyComparisonInfo(organizationId, fromYear, formMonth, toYear, toMonth, userId);
    }
}