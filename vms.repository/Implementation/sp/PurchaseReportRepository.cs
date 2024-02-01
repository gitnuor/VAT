using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.StoredProcedureModel.Purchase;
using vms.entity.viewModels;
using vms.repository.Repository.sp;

namespace vms.repository.Implementation.sp;

public class PurchaseReportRepository : IPurchaseReportRepository
{
	private readonly DbContext _context;
	private readonly IDataProtector _dataProtector;

	public PurchaseReportRepository(DbContext context, IDataProtectionProvider protectionProvider,
		PurposeStringConstants purposeStringConstants)
	{
		_context = context;
		_dataProtector = protectionProvider.CreateProtector(purposeStringConstants.UserIdQueryString);
	}

	public async Task<IEnumerable<SpGetReportPurchaseModel>> GetPurchaseReportData(int organizationId, int branchId,
		int vendorId, DateTime? fromDate, DateTime? toDate, int userId)
	{
		try
		{
			var parameter = new DynamicParameters();
			parameter.Add("@OrganizationId", organizationId);
			parameter.Add("@BranchId", branchId);
			parameter.Add("@VendorId", vendorId);
			parameter.Add("@FromDate", fromDate);
			parameter.Add("@ToDate", toDate);
			parameter.Add("@UserId", userId);
			using var queryMultiple = await _context.Database.GetDbConnection()
				.QueryMultipleAsync(
					"SpGetReportPurchase @OrganizationId, @BranchId, @VendorId, @FromDate, @ToDate, @UserId", parameter,
					commandTimeout: 500);
			return await queryMultiple.ReadAsync<SpGetReportPurchaseModel>();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public async Task<IEnumerable<ViewPurchaseDetail>> GetPuchaseDetailsListData(string pOrgId)
	{
		var id = int.Parse(_dataProtector.Unprotect(pOrgId));
		var list = await _context.Set<ViewPurchaseDetail>()
			.Where(s => s.OrganizationId == id)
			.OrderByDescending(s => s.PurchaseDetailId)
			.AsNoTracking()
			.ToListAsync();
		return list;
	}

	public async Task<IEnumerable<ViewPurchaseDetail>> GetPuchaseDetailsListDataByOrgAndBranch(string pOrgId,
		List<int> branchIds, bool isRequiredBranch)
	{
		var id = int.Parse(_dataProtector.Unprotect(pOrgId));
		var list = await _context.Set<ViewPurchaseDetail>()
			.Where(s => s.OrganizationId == id)
			.OrderByDescending(s => s.PurchaseDetailId)
			.AsNoTracking()
			.ToListAsync();
		if (isRequiredBranch)
		{
			list = list.Where(s => branchIds.Contains(s.BranchId ?? 0)).ToList();
		}

		return list;
	}

	public async Task<IEnumerable<ViewDebitNoteDetail>> DebitNoteDetailsReport(string pOrgId)
	{
		var id = int.Parse(_dataProtector.Unprotect(pOrgId));
		return await _context.Set<ViewDebitNoteDetail>()
			.Where(s => s.OrganizationId == id)
			.OrderByDescending(s => s.PurchaseId)
			.AsNoTracking()
			.ToListAsync();
	}

	public async Task<IEnumerable<ViewDebitNoteDetail>> DebitNoteDetailsReportByOrgAndBranch(string pOrgId,
		List<int> branchIds, bool isRequiredBranch)
	{
		var id = int.Parse(_dataProtector.Unprotect(pOrgId));
		var list = await _context.Set<ViewDebitNoteDetail>()
			.Where(s => s.OrganizationId == id)
			.OrderByDescending(s => s.PurchaseId)
			.AsNoTracking()
			.ToListAsync();
		if (isRequiredBranch)
		{
			list = list.Where(s => branchIds.Contains(s.BranchId ?? 0)).ToList();
		}

		return list;
	}

	public async Task<IEnumerable<SpGetReportSummeryPurchaseModel>> GetPurchaseSummeryReportData(int organizationId,
		int branchId, int vendorId, DateTime? fromDate, DateTime? toDate, int userId)
	{
		try
		{
			var parameter = new DynamicParameters();
			parameter.Add("@OrganizationId", organizationId);
			parameter.Add("@BranchId", branchId);
			parameter.Add("@VendorId", vendorId);
			parameter.Add("@FromDate", fromDate);
			parameter.Add("@ToDate", toDate);
			parameter.Add("@UserId", userId);
			using var queryMultiple = await _context.Database.GetDbConnection()
				.QueryMultipleAsync(
					"SpGetReportSummeryPurchase @OrganizationId, @BranchId, @VendorId, @FromDate, @ToDate, @UserId",
					parameter, commandTimeout: 500);
			return await queryMultiple.ReadAsync<SpGetReportSummeryPurchaseModel>();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}