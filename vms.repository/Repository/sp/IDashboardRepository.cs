using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.StoredProcedureModel;

namespace vms.repository.Repository.sp;

public interface IDashboardRepository
{


    Task<SpDashboard> GetDashboard(int organizationId);
    Task<SpGetDashBoardInfo> GetDashboardInfo(int organizationId, int year, int month);
    Task<IEnumerable<SpGetYearlyComparison>> GetYearlyComparisonInfo(int organizationId, int fromYear, int toYear, int userId);
    Task<IEnumerable<SpGetMonthlyComparison>> GetMonthlyComparisonInfo(int organizationId, int fromYear, int formMonth, int toYear, int toMonth, int userId);
}