using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.StoredProcedureModel.Sales;
using vms.entity.viewModels;
using vms.repository.Repository.sp;

namespace vms.repository.Implementation.sp;

public class SalesReportRepository : ISalesReportRepository
{
    private readonly DbContext _context;

	private IDataProtector _dataProtector;
	private readonly IDataProtectionProvider _protectionProvider;
	public SalesReportRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants)
    {
        _context = context;
		_protectionProvider = p_protectionProvider;
		_dataProtector = _protectionProvider.CreateProtector(p_purposeStringConstants.UserIdQueryString);
	}

    public async Task<IEnumerable<SpGetReportSalesModel>> GetSalesReportData(int organizationId, int branchId, int customerId, DateTime? fromDate, DateTime? toDate, int userId)
    {
        try
        {
            var parameter = new DynamicParameters();
            parameter.Add("@OrganizationId", organizationId);
            parameter.Add("@BranchId", branchId);
            parameter.Add("@CustomerId", customerId);
            parameter.Add("@FromDate", fromDate);
            parameter.Add("@ToDate", toDate);
            parameter.Add("@UserId", userId);
            using var queryMultiple = await _context.Database.GetDbConnection().QueryMultipleAsync("SpGetReportSales @OrganizationId, @BranchId, @CustomerId, @FromDate, @ToDate, @UserId", parameter, commandTimeout: 500);
            return await queryMultiple.ReadAsync<SpGetReportSalesModel>();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

	public async Task<IEnumerable<ViewSalesDetail>> GetSalesDetailsListData(string pOrgId)
	{
		var id = int.Parse(_dataProtector.Unprotect(pOrgId));
		return await _context.Set<ViewSalesDetail>()
			.Where(s => s.OrganizationId == id)
			.OrderByDescending(s => s.SalesDetailId)
			.AsNoTracking()
			.ToListAsync();
	}

    public async Task<IEnumerable<ViewSalesDetail>> GetSalesDetailsListDataByOrgAndBranch(string pOrgId, List<int> branchIds, bool isRequiredBranch)
    {
        var id = int.Parse(_dataProtector.Unprotect(pOrgId));
        var list = await _context.Set<ViewSalesDetail>()
            .Where(s => s.OrganizationId == id)
            .OrderByDescending(s => s.SalesDetailId)
            .AsNoTracking()
            .ToListAsync();
        if (isRequiredBranch)
        {
            list = list.Where(s => branchIds.Contains(s.BranchId.Value)).ToList();
        }
        return list;
    }

    public async Task<IEnumerable<ViewCreditNoteDetail>> CreditNoteDetailsReportByOrgAndBranch(string pOrgId, List<int> branchIds, bool isRequiredBranch)
    {
        var id = int.Parse(_dataProtector.Unprotect(pOrgId));
        var list = await _context.Set<ViewCreditNoteDetail>()
            .Where(s => s.OrganizationId == id)
            .OrderByDescending(s => s.SalesDetailId)
            .AsNoTracking()
            .ToListAsync();
        if (isRequiredBranch)
        {
            list = list.Where(s => branchIds.Contains(s.BranchId.Value)).ToList();
        }
        return list;
    }

    public async Task<IEnumerable<ViewCreditNoteDetail>> CreditNoteDetailsReport(string pOrgId)
	{
		var id = int.Parse(_dataProtector.Unprotect(pOrgId));
		return await _context.Set<ViewCreditNoteDetail>()
			.Where(s => s.OrganizationId == id)
			.OrderByDescending(s => s.SalesDetailId)
			.AsNoTracking()
			.ToListAsync();
	}

	

	public async Task<IEnumerable<SpGetReportSummerySalesModel>> GetSalesSummeryReportData(int organizationId, int branchId, int customerId, DateTime? fromDate, DateTime? toDate, int userId)
    {
        try
        {
            var parameter = new DynamicParameters();
            parameter.Add("@OrganizationId", organizationId);
            parameter.Add("@BranchId", branchId);
            parameter.Add("@CustomerId", customerId);
            parameter.Add("@FromDate", fromDate);
            parameter.Add("@ToDate", toDate);
            parameter.Add("@UserId", userId);
            using var queryMultiple = await _context.Database.GetDbConnection().QueryMultipleAsync("SpGetReportSummerySales @OrganizationId, @BranchId, @CustomerId, @FromDate, @ToDate, @UserId", parameter, commandTimeout: 500);
            return await queryMultiple.ReadAsync<SpGetReportSummerySalesModel>();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}