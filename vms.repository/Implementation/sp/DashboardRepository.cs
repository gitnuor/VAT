using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using vms.entity.StoredProcedureModel;
using vms.repository.Repository.sp;

namespace vms.repository.Implementation.sp;

public class DashboardRepository : IDashboardRepository
{
    private readonly DbContext _context;
    public DashboardRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<SpDashboard> GetDashboard(int organizationId)
    {
        try
        {
            var parameter = new DynamicParameters();
            var output = new SpDashboard();
            parameter.Add("@OrganizationId", organizationId);
            using (var queryMultiple = _context.Database.GetDbConnection().QueryMultiple("SpGetDashBoard @OrganizationId", parameter, commandTimeout: 500))
            {
                output.VatCurrentStatus = queryMultiple.ReadFirst<SpVatCurrent>();
                output.VatPaymentList = queryMultiple.Read<SpVatPayment>().ToList();
            }
            output.strVatPaymentList = JsonConvert.SerializeObject(output.VatPaymentList);
            output.VatPaymentList = null;
            return await Task.FromResult(output); 
        }
        catch (Exception ex)
        {
	        throw new Exception(ex.Message);
        }

    }

    public async Task<SpGetDashBoardInfo> GetDashboardInfo(int organizationId, int year, int month)
    {
        try
        {
            var parameter = new DynamicParameters();
            parameter.Add("@OrganizationId", organizationId);
            parameter.Add("@Year", year);
            parameter.Add("@Month", month);
            var output = new SpGetDashBoardInfo();
            using var queryMultiple = await _context.Database.GetDbConnection().QueryMultipleAsync("SpGetDashBoardInfo @OrganizationId, @Year, @Month", parameter, commandTimeout: 500);
            output.Summery = await queryMultiple.ReadFirstAsync<SpGetDashBoardInfoSummery>();
            // output.DailyPurchases = await queryMultiple.ReadAsync<SpGetDashBoardInfoDailyPurchase>();
            return output;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IEnumerable<SpGetYearlyComparison>> GetYearlyComparisonInfo(int organizationId, int fromYear, int toYear, int userId)
    {
        try
        {
            var parameter = new DynamicParameters();
            parameter.Add("@OrganizationId", organizationId);
            parameter.Add("@FromYear", fromYear);
            parameter.Add("@ToYear", toYear);
            parameter.Add("@UserId", userId);
            using var queryMultiple = await _context.Database.GetDbConnection().QueryMultipleAsync("SpGetYearlyComparison @OrganizationId, @FromYear, @ToYear, @UserId", parameter, commandTimeout: 500);
            return await queryMultiple.ReadAsync<SpGetYearlyComparison>();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IEnumerable<SpGetMonthlyComparison>> GetMonthlyComparisonInfo(int organizationId, int fromYear, int fromMonth, int toYear, int toMonth, int userId)
    {
        try
        {
            var parameter = new DynamicParameters();
            parameter.Add("@OrganizationId", organizationId);
            parameter.Add("@FromYear", fromYear);
            parameter.Add("@ToYear", toYear);
            parameter.Add("@FromMonth", fromMonth);
            parameter.Add("@ToMonth", toMonth);
            parameter.Add("@UserId", userId);
            using var queryMultiple = await _context.Database.GetDbConnection().QueryMultipleAsync("SpGetMonthlyComparison @OrganizationId, @FromYear, @FromMonth, @ToYear, @ToMonth, @UserId", parameter, commandTimeout: 500);
            return await queryMultiple.ReadAsync<SpGetMonthlyComparison>();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}