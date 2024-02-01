using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.StoredProcedureModel.ParamModel;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.utility;

namespace vms.repository.Implementation.tbl;

public class PurchaseRepository : RepositoryBase<Purchase>, IPurchaseRepository
{
	private readonly DbContext _context;

	private readonly IDataProtector _dataProtector;

	public PurchaseRepository(DbContext context, IDataProtectionProvider protectionProvider,
		PurposeStringConstants purposeStringConstants) : base(context)
	{
		_context = context;
		_dataProtector = protectionProvider.CreateProtector(purposeStringConstants.UserIdQueryString);
	}

	public async Task<Purchase> GetPurchaseDetails(string pEncryptedId)
	{
		int id = int.Parse(_dataProtector.Unprotect(pEncryptedId));
		var purchase = await Query().Include(c => c.Vendor).Include(c => c.Organization).Include(c => c.PurchaseType)
			.Include(p => p.PurchaseReason).Include(c => c.PurchaseDetails)
			.SingleOrDefaultAsync(x => x.PurchaseId == id, CancellationToken.None);
		purchase.EncryptedId = _dataProtector.Protect(purchase.PurchaseId.ToString());

		return purchase;
	}

	public async Task<IEnumerable<Purchase>> GetPurchaseDue(int orgId)
	{
		var purchaseDue = await Query().Where(c => c.OrganizationId == orgId && c.DueAmount > 0)
			.Include(c => c.Organization).Include(c => c.PurchaseType).Include(c => c.Vendor)
			.OrderByDescending(c => c.PurchaseId).SelectAsync(CancellationToken.None);
		var purchaseDueList = purchaseDue.ToList();
		purchaseDueList.ToList().ForEach(delegate(Purchase pur)
		{
			pur.EncryptedId = _dataProtector.Protect(pur.PurchaseId.ToString());
		});
		return purchaseDueList;
	}

	public async Task<bool> InsertTransferReceive(vmTransferReceive vm)
	{
		if (vm.ContentJson != null)
			Newtonsoft.Json.JsonConvert.SerializeObject(vm.ContentJson);
		try
		{
			await _context.Database.ExecuteSqlRawAsync(
				$"EXEC [dbo].[SpInsertTransferReceive]" +
				$"@TransferSalesId " +
				$",@OrganizationId " +
				$",@InvoiceNo " +
				$",@PurchaseDate " +
				$",@PurchaseReasonId ," +
				$" @DeliveryDate " +
				$",@IsComplete " +
				$",@CreatedBy " +
				$",@CreatedTime" +
				$",@ContentJson"
				, new SqlParameter("@TransferSalesId", vm.TransferSalesId)
				, new SqlParameter("@OrganizationId", vm.OrganizationId)
				, new SqlParameter("@InvoiceNo", (object)vm.InvoiceNo ?? DBNull.Value)
				, new SqlParameter("@PurchaseDate", vm.PurchaseDate)
				, new SqlParameter("@PurchaseReasonId", vm.PurchaseReasonId)
				, new SqlParameter("@DeliveryDate", vm.DeliveryDate)
				, new SqlParameter("@IsComplete", (object)vm.IsComplete ?? DBNull.Value)
				, new SqlParameter("@CreatedBy", vm.CreatedBy)
				, new SqlParameter("@CreatedTime", vm.CreatedTime)
				, new SqlParameter("@ContentJson", (object)vm.ContentJson ?? DBNull.Value)
			);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}

		return await Task.FromResult(true);
	}

	public async Task<IEnumerable<Purchase>> GetVdsPurchasesWithVdsPayment(string orgIdEnc)
	{
		var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
		var purchases = await Query()
			.Where(p => p.OrganizationId == orgId && p.PurchaseTypeId == (int)EnumPurchaseType.PurchaseTypeLocal &&
			            p.MushakReturnPaymentForVds.Any())
			.Include(p => p.OrgBranch)
			.Include(p => p.PurchaseType)
			.Include(p => p.Vendor)
			.Include(p => p.Organization)
			.OrderByDescending(p => p.PurchaseId)
			.SelectAsync();

		var purchaseByOrganization = purchases.ToList();
		purchaseByOrganization.ForEach(delegate(Purchase purchase)
		{
			purchase.EncryptedId = _dataProtector.Protect(purchase.PurchaseId.ToString());
		});
		return purchaseByOrganization;
	}

    public async Task<IEnumerable<Purchase>> GetVdsPurchasesWithVdsPaymentAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
        var purchases = await Query()
            .Where(p => p.OrganizationId == orgId && p.PurchaseTypeId == (int)EnumPurchaseType.PurchaseTypeLocal &&
                        p.MushakReturnPaymentForVds.Any())
            .Include(p => p.OrgBranch)
            .Include(p => p.PurchaseType)
            .Include(p => p.Vendor)
            .Include(p => p.Organization)
            .OrderByDescending(p => p.PurchaseId)
            .SelectAsync();

        var purchaseByOrganization = purchases.ToList();
        if (isRequiredBranch)
        {
            purchaseByOrganization = purchaseByOrganization.Where(s => branchIds.Contains(s.OrgBranchId.Value)).ToList();
        }
        purchaseByOrganization.ForEach(delegate (Purchase purchase)
        {
            purchase.EncryptedId = _dataProtector.Protect(purchase.PurchaseId.ToString());
        });
        return purchaseByOrganization;
    }

    public async Task<IEnumerable<Purchase>> GetTdsPurchasesWithDueTdsPayment(int pOrgId)
	{
		var purchases = await Query().Where(x => x.IsTds == true)
			.Where(w => w.OrganizationId == pOrgId && !w.TdsPaymentForPurchases.Any()).Include(b => b.OrgBranch)
			.Include(c => c.Organization).Include(c => c.PurchaseType).Include(c => c.DebitNotes).Include(c => c.Vendor)
			.OrderByDescending(c => c.PurchaseId).SelectAsync();

		var purchaseList = purchases.ToList();
		purchaseList.ToList().ForEach(delegate(Purchase purchase)
		{
			purchase.EncryptedId = _dataProtector.Protect(purchase.PurchaseId.ToString());
		});
		return purchaseList;
	}

	public async Task<IEnumerable<Purchase>> GetTdsPurchasesWithTdsPayment(string orgIdEnc)
	{
		var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
		var purchases = await Query()
			.Where(p => p.OrganizationId == orgId && p.PurchaseTypeId == (int)EnumPurchaseType.PurchaseTypeLocal &&
			            p.TdsPaymentForPurchases.Any())
			.Include(p => p.OrgBranch)
			.Include(p => p.PurchaseType)
			.Include(p => p.Vendor)
			.Include(p => p.Organization)
			.OrderByDescending(p => p.PurchaseId)
			.SelectAsync();

		var purchaseByOrganization = purchases.ToList();
		purchaseByOrganization.ForEach(delegate(Purchase purchase)
		{
			purchase.EncryptedId = _dataProtector.Protect(purchase.PurchaseId.ToString());
		});
		return purchaseByOrganization;
	}

	public async Task<int> InsertPurchase(SpInsertPurchaseCombinedParam purchase)
	{
		var purchaseDetails = Newtonsoft.Json.JsonConvert.SerializeObject(purchase.PurchaseDetailList);
		//var purchasePaymentJson = purchase.PurchasePayments == null
		//	? null
		//	: Newtonsoft.Json.JsonConvert.SerializeObject(purchase.PurchasePayments);
		//var contentInfoJson = purchase.ContentInfoList == null
		//	? null
		//	: Newtonsoft.Json.JsonConvert.SerializeObject(purchase.ContentInfoList);
		//var taxPaymentJson = purchase.ImportTaxPayments == null
		//	? null
		//	: Newtonsoft.Json.JsonConvert.SerializeObject(purchase.ImportTaxPayments);


		var currentTime = DateTime.Today;
		try
		{
			const string sql = "EXEC [dbo].[SpInsertPurchaseCombined]" +
			                   "@OrganizationId " +
			                   ",@OrgBranchId " +
			                   ",@VendorId" +
			                   ",@VatChallanNo" +
			                   ",@VatChallanIssueDate" +
			                   ",@VendorInvoiceNo " +
			                   ",@InvoiceNo" +
			                   ",@VoucherNo" +
			                   ",@PurchaseDate" +
			                   ",@PurchaseTypeId " +
			                   ",@PurchaseReasonId " +
			                   ",@DiscountOnTotalPrice" +
			                   ",@IsVatDeductedInSource" +
			                   ",@VDSAmount" +
							   ",@IsTds" +
							   ",@TdsAmount" +
							   ",@ExpectedDeliveryDate " +
			                   ",@DeliveryDate" +
			                   ",@LcNo" +
			                   ",@LcDate" +
			                   ",@BillOfEntry" +
			                   ",@BillOfEntryDate" +
			                   ",@DueDate" +
			                   ",@TermsOfLc" +
			                   ",@PoNumber" +
			                   ",@MushakGenerationId" +
			                   ",@IsComplete" +
			                   ",@CreatedBy" +
			                   ",@CreatedTime" +
			                   ",@AdvanceTaxPaidAmount" +
			                   ",@ATPDate" +
			                   ",@ATPBankId" +
			                   ",@ATPBankBranchName" +
			                   ",@ATPChallanNo" +
			                   ",@CustomsAndVATCommissionarateId" +
			                   ",@PurchaseOrderDetailsJson" +
			                   ",@PurchasePaymentJson" +
			                   ",@ContentJson" +
			                   ",@TaxPaymentJson";

			var parameter = new DynamicParameters();
			parameter.Add("@OrganizationId", purchase.OrganizationId);
			parameter.Add("@OrgBranchId", purchase.OrgBranchId);
			parameter.Add("@VendorId", purchase.VendorId);
			parameter.Add("@VatChallanNo", purchase.VatChallanNo);
			parameter.Add("@VatChallanIssueDate", purchase.VatChallanIssueDate);
			parameter.Add("@VendorInvoiceNo", purchase.VendorInvoiceNo);
			parameter.Add("@InvoiceNo", purchase.InvoiceNo);
			parameter.Add("@VoucherNo", purchase.VoucherNo);
			parameter.Add("@PurchaseDate",
				StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(purchase.PurchaseDate));
			parameter.Add("@PurchaseTypeId", purchase.PurchaseTypeId);
			parameter.Add("@PurchaseReasonId", purchase.PurchaseReasonId);
			parameter.Add("@DiscountOnTotalPrice", purchase.DiscountOnTotalPrice);
			parameter.Add("@IsVatDeductedInSource", purchase.IsVatDeductedInSource);
			parameter.Add("@VDSAmount", purchase.Vdsamount);
			parameter.Add("@IsTds", purchase.IsTds);
			parameter.Add("@TdsAmount", purchase.TdsAmount);
			parameter.Add("@ExpectedDeliveryDate",
				StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(purchase.ExpectedDeliveryDate));
			parameter.Add("@DeliveryDate",
				StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(purchase.DeliveryDate ?? currentTime));
			parameter.Add("@LcNo", purchase.LcNo);
			parameter.Add("@LcDate", StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(purchase.LcDate));
			parameter.Add("@BillOfEntry", purchase.BillOfEntry);
			parameter.Add("@BillOfEntryDate",
				StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(purchase.BillOfEntryDate));
			parameter.Add("@DueDate", StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(purchase.DueDate));
			parameter.Add("@TermsOfLc", purchase.TermsOfLc);
			parameter.Add("@PoNumber", purchase.PoNumber);
			parameter.Add("@MushakGenerationId", purchase.MushakGenerationId);
			parameter.Add("@IsComplete", purchase.IsComplete);
			parameter.Add("@CreatedBy", purchase.CreatedBy);
			parameter.Add("@CreatedTime",
				StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(purchase.CreatedTime ?? currentTime));
			parameter.Add("@AdvanceTaxPaidAmount", purchase.AdvanceTaxPaidAmount);
			parameter.Add("@ATPDate", purchase.Atpdate);
			parameter.Add("@ATPBankId", purchase.AtpbankId);
			parameter.Add("@ATPBankBranchName", purchase.AtpbankBranchName);
			parameter.Add("@ATPChallanNo", purchase.AtpchallanNo);
			parameter.Add("@CustomsAndVATCommissionarateId", purchase.CustomsAndVatcommissionarateId);
			parameter.Add("@PurchaseOrderDetailsJson", purchaseDetails);
			parameter.Add("@PurchasePaymentJson", null);
			parameter.Add("@ContentJson", null);
			parameter.Add("@TaxPaymentJson", null);

			using var queryMultiple = await _context.Database.GetDbConnection()
				.QueryMultipleAsync(sql, parameter, commandTimeout: 500);
			return await queryMultiple.ReadFirstAsync<int>();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public async Task<int> InsertApiPurchase(SpInsertPurchaseFromApiCombinedParam purchase, string apiData = null)
	{
		var purchaseDetails = Newtonsoft.Json.JsonConvert.SerializeObject(purchase.Details);
		var purchasePaymentJson = purchase.PurchasePayments == null
			? null
			: Newtonsoft.Json.JsonConvert.SerializeObject(purchase.PurchasePayments);
		var taxPaymentJson = purchase.ImportTaxPayments == null
			? null
			: Newtonsoft.Json.JsonConvert.SerializeObject(purchase.ImportTaxPayments);


		var currentTime = DateTime.Today;
		try
		{
			const string sql = "EXEC [dbo].[SpInsertPurchaseFromApiCombined]" +
							   "@SecurityToken " +
			                   ",@OrganizationId " +
							   ",@BranchId " +
			                   ",@VendorId" +
                               ",@VendorName" +
                               ",@VendorBin" +
                               ",@VendorNid" +
                               ",@VendorAddress" +
                               ",@VendorContactNo" +
			                   ",@VatChallanNo" +
			                   ",@VatChallanIssueDate" +
			                   ",@VendorInvoiceNo " +
			                   ",@InvoiceNo" +
			                   ",@VoucherNo" +
			                   ",@PurchaseDate" +
			                   ",@PurchaseTypeId " +
			                   ",@PurchaseReasonId " +
			                   ",@DiscountOnTotalPrice" +
			                   ",@IsVatDeductedInSource" +
			                   ",@VDSAmount" +
							   ",@IsTds" +
							   ",@TdsAmount" +
							   ",@ExpectedDeliveryDate " +
			                   ",@DeliveryDate" +
			                   ",@LcNo" +
			                   ",@LcDate" +
			                   ",@BillOfEntry" +
			                   ",@BillOfEntryDate" +
			                   ",@DueDate" +
			                   ",@TermsOfLc" +
			                   ",@PoNumber" +
			                   ",@MushakGenerationId" +
			                   ",@IsComplete" +
			                   ",@CreatedBy" +
			                   ",@CreatedTime" +
			                   ",@AdvanceTaxPaidAmount" +
			                   ",@ATPDate" +
			                   ",@ATPBankId" +
			                   ",@ATPBankBranchName" +
			                   ",@ATPChallanNo" +
			                   ",@CustomsAndVATCommissionarateId" +
			                   ",@PurchaseDetailsJson" +
			                   ",@PurchasePaymentJson" +
							   ",@TaxPaymentJson" +
							   ",@ApiData";

			var parameter = new DynamicParameters();
			parameter.Add("@SecurityToken", purchase.Token);
			parameter.Add("@OrganizationId", purchase.OrganizationId);
			parameter.Add("@BranchId", purchase.BranchId);
			parameter.Add("@VendorId", purchase.VendorId);

			parameter.Add("@VendorName", purchase.VendorName);
			parameter.Add("@VendorBin", purchase.VendorBin);
			parameter.Add("@VendorNid", purchase.VendorNid);
			parameter.Add("@VendorAddress", purchase.VendorAddress);
			parameter.Add("@VendorContactNo", purchase.VendorContactNo);

			parameter.Add("@VatChallanNo", purchase.VatChallanNo);
			parameter.Add("@VatChallanIssueDate", StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(purchase.VatChallanIssueDate));
			parameter.Add("@VendorInvoiceNo", purchase.VendorInvoiceNo);
			parameter.Add("@InvoiceNo", purchase.InvoiceNo);
			parameter.Add("@VoucherNo", purchase.VoucherNo);
			parameter.Add("@PurchaseDate",
				StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(purchase.PurchaseDate));
			parameter.Add("@PurchaseTypeId", purchase.PurchaseTypeId);
			parameter.Add("@PurchaseReasonId", purchase.PurchaseReasonId);
			parameter.Add("@DiscountOnTotalPrice", purchase.DiscountOnTotalPrice);
			parameter.Add("@IsVatDeductedInSource", purchase.IsVatDeductedInSource);
			parameter.Add("@VDSAmount", purchase.Vdsamount);
			parameter.Add("@IsTds", purchase.IsTds);
			parameter.Add("@TdsAmount", purchase.TdsAmount);
			parameter.Add("@ExpectedDeliveryDate",
				StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(purchase.ExpectedDeliveryDate));
			parameter.Add("@DeliveryDate",
				StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(purchase.DeliveryDate ?? currentTime));
			parameter.Add("@LcNo", purchase.LcNo);
			parameter.Add("@LcDate", StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(purchase.LcDate));
			parameter.Add("@BillOfEntry", purchase.BillOfEntry);
			parameter.Add("@BillOfEntryDate",
				StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(purchase.BillOfEntryDate));
			parameter.Add("@DueDate", StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(purchase.DueDate));
			parameter.Add("@TermsOfLc", purchase.TermsOfLc);
			parameter.Add("@PoNumber", purchase.PoNumber);
			parameter.Add("@MushakGenerationId", purchase.MushakGenerationId);
			parameter.Add("@IsComplete", purchase.IsComplete);
			parameter.Add("@CreatedBy", purchase.CreatedBy);
			parameter.Add("@CreatedTime",
				StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(purchase.CreatedTime ?? currentTime));
			parameter.Add("@AdvanceTaxPaidAmount", purchase.AdvanceTaxPaidAmount);
			parameter.Add("@ATPDate", purchase.Atpdate);
			parameter.Add("@ATPBankId", purchase.AtpbankId);
			parameter.Add("@ATPBankBranchName", purchase.AtpbankBranchName);
			parameter.Add("@ATPChallanNo", purchase.AtpchallanNo);
			parameter.Add("@CustomsAndVATCommissionarateId", purchase.CustomsAndVatcommissionarateId);
			parameter.Add("@PurchaseDetailsJson", purchaseDetails);
			parameter.Add("@PurchasePaymentJson", purchasePaymentJson);
			parameter.Add("@TaxPaymentJson", taxPaymentJson);
			parameter.Add("@ApiData", apiData);

			using var queryMultiple = await _context.Database.GetDbConnection()
				.QueryMultipleAsync(sql, parameter, commandTimeout: 500);
			return await queryMultiple.ReadFirstAsync<int>();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public async Task<bool> InsertDebitNote(vmDebitNote vmDebitNote)
	{
		var debitNoteDetails = Newtonsoft.Json.JsonConvert.SerializeObject(vmDebitNote.DebitNoteDetails);
		try
		{
			await _context.Database.ExecuteSqlRawAsync(
				$"EXEC [dbo].[spInsertDebitNote]  " +
				$"@PurchaseId," +
				$"@VoucherNo," +
				$"@ReasonOfReturn," +
				$"@ReturnDate," +
				$"@VehicleTypeId," +
				$"@VehicleName," +
				$"@VehicleRegNo," +
				$"@VehicleDriverName, " +
				$"@VehicleDriverContactNo," +
				$"@CreatedBy," +
				$"@CreatedTime," +
				$"@DebitNoteDetails"
				, new SqlParameter("@PurchaseId", vmDebitNote.PurchaseId)
				, new SqlParameter("@VoucherNo", (object)vmDebitNote.VoucherNo ?? DBNull.Value)
				, new SqlParameter("@ReasonOfReturn", (object)vmDebitNote.ReasonOfReturn ?? DBNull.Value)
				, new SqlParameter("@ReturnDate", vmDebitNote.ReturnDate)
				, new SqlParameter("@VehicleTypeId", vmDebitNote.VehicleTypeId)
				, new SqlParameter("@VehicleName", (object)vmDebitNote.VehicleName ?? DBNull.Value)
				, new SqlParameter("@VehicleRegNo", (object)vmDebitNote.VehicleRegNo ?? DBNull.Value)
				, new SqlParameter("@VehicleDriverName", (object)vmDebitNote.VehicleDriverName ?? DBNull.Value)
				, new SqlParameter("@VehicleDriverContactNo",
					(object)vmDebitNote.VehicleDriverContactNo ?? DBNull.Value)
				, new SqlParameter("@CreatedBy", (object)vmDebitNote.CreatedBy ?? DBNull.Value)
				, new SqlParameter("@CreatedTime", (object)vmDebitNote.CreatedTime ?? DBNull.Value)
				, new SqlParameter("@DebitNoteDetails", debitNoteDetails)
			);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}

		return await Task.FromResult(true);
	}

	public async Task<bool> ManagePurchaseDue(vmPurchasePayment vmPurchase)
	{
		try
		{
			await _context.Database.ExecuteSqlRawAsync(
				$"EXEC [dbo].[SPManagePurchaseDue]" +
				$"@PurchaseId " +
				$",@PaymentMethodId" +
				$",@PaidAmount" +
				$",@CreatedBy "
				, new SqlParameter("@PurchaseId", vmPurchase.PurchaseId)
				, new SqlParameter("@PaymentMethodId", vmPurchase.PaymentMethodId)
				, new SqlParameter("@PaidAmount", vmPurchase.PaidAmount)
				, new SqlParameter("@CreatedBy", vmPurchase.CreatedBy)
			);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}

		return await Task.FromResult(true);
	}


	public async Task<IEnumerable<Purchase>> GetPurchases(int pOrgId)
	{
		var purchases = await Query().Where(w => w.OrganizationId == pOrgId).Include(c => c.Organization)
			.Include(c => c.PurchaseType).Include(c => c.DebitNotes).Include(c => c.Vendor)
			.OrderByDescending(c => c.PurchaseId).SelectAsync();

		var purchaseList = purchases.ToList();
		purchaseList.ToList().ForEach(delegate(Purchase purchase)
		{
			purchase.EncryptedId = _dataProtector.Protect(purchase.PurchaseId.ToString());
		});
		return purchaseList;
	}

	public async Task<IEnumerable<Purchase>> GetPurchaseByOrganization(string orgIdEnc)
	{
		var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
		var purchases = await Query()
			.Where(p => p.OrganizationId == orgId && p.PurchaseTypeId != 3)
			.Include(p => p.OrgBranch)
			.Include(p => p.PurchaseType)
			.Include(p => p.Vendor)
			.Include(p => p.Organization)
			.OrderByDescending(p => p.PurchaseId)
			.SelectAsync();

		var purchaseByOrganization = purchases.ToList();
		purchaseByOrganization.ForEach(delegate(Purchase sale)
		{
			sale.EncryptedId = _dataProtector.Protect(sale.PurchaseId.ToString());
		});
		return purchaseByOrganization;
	}

	public async Task<IEnumerable<ViewPurchase>> GetPurchaseListByOrganization(string orgIdEnc)
	{
		var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
		var list = await _context.Set<ViewPurchase>()
			.Where(s => s.OrganizationId == orgId)
			.OrderByDescending(s => s.PurchaseId)
			.AsNoTracking()
			.ToListAsync();
		list.ForEach(s => s.EncryptedId = _dataProtector.Protect(s.PurchaseId.ToString()));
		return list;
	}

    public async Task<IEnumerable<ViewPurchase>> GetPurchaseListByOrganizationAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
        var list = await _context.Set<ViewPurchase>()
            .Where(s => s.OrganizationId == orgId)
            .OrderByDescending(s => s.PurchaseId)
            .AsNoTracking()
            .ToListAsync();
        if (isRequiredBranch)
        {
            list = list.Where(s => branchIds.Contains(s.OrgBranchId.Value)).ToList();
        }
        list.ForEach(s => s.EncryptedId = _dataProtector.Protect(s.PurchaseId.ToString()));
        return list;
    }

    public async Task<IEnumerable<ViewVdsPurchase>> GetVdsPurchaseListByOrganization(string orgIdEnc)
	{
		var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
		var list = await _context.Set<ViewVdsPurchase>()
			.Where(s => s.OrganizationId == orgId)
			.OrderByDescending(s => s.PurchaseId)
			.AsNoTracking()
			.ToListAsync();
		list.ForEach(s => s.EncryptedId = _dataProtector.Protect(s.PurchaseId.ToString()));
		return list;
	}

    public async Task<IEnumerable<ViewVdsPurchase>> GetVdsPurchaseListByOrganizationAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
        var list = await _context.Set<ViewVdsPurchase>()
            .Where(s => s.OrganizationId == orgId)
            .OrderByDescending(s => s.PurchaseId)
            .AsNoTracking()
            .ToListAsync();
        if (isRequiredBranch)
        {
            list = list.Where(s => branchIds.Contains(s.OrgBranchId.Value)).ToList();
        }
        list.ForEach(s => s.EncryptedId = _dataProtector.Protect(s.PurchaseId.ToString()));
        return list;
    }



    public async Task<IEnumerable<ViewPurchaseLocal>> GetPurchaseLocalListByOrganization(string orgIdEnc)
	{
		var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
		var list = await _context.Set<ViewPurchaseLocal>()
			.Where(s => s.OrganizationId == orgId)
			.OrderByDescending(s => s.PurchaseId)
			.AsNoTracking()
			.ToListAsync();
		list.ForEach(s => s.EncryptedId = _dataProtector.Protect(s.PurchaseId.ToString()));
		return list;
	}

    public async Task<IEnumerable<ViewPurchaseLocal>> GetPurchaseLocalListByOrganizationAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
        var list = await _context.Set<ViewPurchaseLocal>()
            .Where(s => s.OrganizationId == orgId)
            .OrderByDescending(s => s.PurchaseId)
            .AsNoTracking()
            .ToListAsync();
        if (isRequiredBranch)
        {
            list = list.Where(s => branchIds.Contains(s.OrgBranchId.Value)).ToList();
        }
        list.ForEach(s => s.EncryptedId = _dataProtector.Protect(s.PurchaseId.ToString()));
        return list;
    }

    public async Task<IEnumerable<ViewPurchaseImport>> GetPurchaseImportListByOrganization(string orgIdEnc)
	{
		var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
		var list = await _context.Set<ViewPurchaseImport>()
			.Where(s => s.OrganizationId == orgId)
			.OrderByDescending(s => s.PurchaseId)
			.AsNoTracking()
			.ToListAsync();
		list.ForEach(s => s.EncryptedId = _dataProtector.Protect(s.PurchaseId.ToString()));
		return list;
	}

	public async Task<IEnumerable<Purchase>> GetVdsPurchases(int pOrgId)
	{
		var purchases = await Query().Where(x => x.IsVatDeductedInSource).Where(w => w.OrganizationId == pOrgId)
			.Include(b => b.OrgBranch).Include(c => c.Organization).Include(c => c.PurchaseType)
			.Include(c => c.DebitNotes).Include(c => c.Vendor).OrderByDescending(c => c.PurchaseId).SelectAsync();

		var purchaseList = purchases.ToList();
		purchaseList.ToList().ForEach(delegate(Purchase purchase)
		{
			purchase.EncryptedId = _dataProtector.Protect(purchase.PurchaseId.ToString());
		});
		return purchaseList;
	}

	public async Task<IEnumerable<Purchase>> GetVdsPurchasesWithDueVdsPayment(int pOrgId)
	{
		var purchases = await Query().Where(x => x.IsVatDeductedInSource)
			.Where(w => w.OrganizationId == pOrgId && !w.MushakReturnPaymentForVds.Any()).Include(b => b.OrgBranch)
			.Include(c => c.Organization).Include(c => c.PurchaseType).Include(c => c.DebitNotes).Include(c => c.Vendor)
			.OrderByDescending(c => c.PurchaseId).SelectAsync();

		var purchaseList = purchases.ToList();
		purchaseList.ToList().ForEach(delegate(Purchase purchase)
		{
			purchase.EncryptedId = _dataProtector.Protect(purchase.PurchaseId.ToString());
		});
		return purchaseList;
	}

	public async Task<List<SpMonthlyPurchaseReport>> MonthlyPurchaseReport(int userId,int PurReason = 0,  int organizationId = 0,
		int vendorId = 0, string invoiceNo = null, DateTime? fromDate = null, DateTime? toDate = null)
	{
		var parameter = new DynamicParameters();
		parameter.Add("@PurchaseReasonId ", PurReason);
		parameter.Add("@VendorId ", vendorId);
		parameter.Add("@OrganizationId ", organizationId);
        parameter.Add("@UserId ", userId);
        if (string.IsNullOrEmpty(invoiceNo))
		{
			parameter.Add("@InvoiceNo", null);
			parameter.Add("@FromDate", fromDate);
			parameter.Add("@ToDate", toDate);
		}
		else
		{
			parameter.Add("@InvoiceNo ", invoiceNo);
			parameter.Add("@FromDate", null);
			parameter.Add("@ToDate", null);
		}

		var result = await _context.Database.GetDbConnection().QueryAsync<SpMonthlyPurchaseReport>(
			"SpPurchaseReport @PurchaseReasonId, @VendorId, @OrganizationId, @InvoiceNo, @FromDate, @ToDate, @UserId", parameter,
			commandTimeout: 500);

		return result.ToList();
	}

	public async Task<List<SpGetPurchaseReportByProduct>> GetPurchaseReportByProduct(int purReason, int organizationId,
		int productId, string invoiceNo, DateTime? fromDate, DateTime? toDate, int userId)
	{
		var parameter = new DynamicParameters();
		parameter.Add("@PurchaseReasonId ", purReason);
		parameter.Add("@ProductId ", productId);
		parameter.Add("@OrganizationId ", organizationId);
        parameter.Add("@UserId", userId);
        if (string.IsNullOrEmpty(invoiceNo))
		{
			parameter.Add("@InvoiceNo", null);
			parameter.Add("@FromDate", fromDate);
			parameter.Add("@ToDate", toDate);
		}
		else
		{
			parameter.Add("@InvoiceNo ", invoiceNo);
			parameter.Add("@FromDate", null);
			parameter.Add("@ToDate", null);
		}

		var result = await _context.Database.GetDbConnection().QueryAsync<SpGetPurchaseReportByProduct>(
            "SpGetPurchaseReportByProduct @PurchaseReasonId, @ProductId, @OrganizationId, @InvoiceNo, @FromDate, @ToDate, @UserId",
			parameter, commandTimeout: 500);

		return result.ToList();
	}

    public async Task<List<ViewPurchaseLocal>> GetPurchaseVdsList(int organizationId, DateTime? fromDate, DateTime? toDate)
    {
        var list = await _context.Set<ViewPurchaseLocal>()
            .Where(s => s.OrganizationId == organizationId 
				&& s.PurchaseDate >= fromDate 
				&& s.PurchaseDate <= toDate)
            .OrderByDescending(s => s.PurchaseId)
            .AsNoTracking()
            .ToListAsync();
        return list;
    }

    public async Task<List<ViewPurchaseLocal>> GetPurchaseVdsListByOrgAndBranch(int organizationId, DateTime? fromDate, DateTime? toDate, List<int> branchIds, bool isRequiredBranch)
    {
        var list = await _context.Set<ViewPurchaseLocal>()
            .Where(s => s.OrganizationId == organizationId
                && s.PurchaseDate >= fromDate
                && s.PurchaseDate <= toDate)
            .OrderByDescending(s => s.PurchaseId)
            .AsNoTracking()
            .ToListAsync();
        if (isRequiredBranch)
        {
            list = list.Where(s => branchIds.Contains(s.OrgBranchId.Value)).ToList();
        }
        return list;
    }

    public async Task<List<SpGetProductPurchase>> ProductPurchaseListReport(int organizationId,
        int branchId,DateTime? fromDate, DateTime? toDate)
    {
        var parameter = new DynamicParameters();
        parameter.Add("@OrganizationId ", organizationId);
        parameter.Add("@BranchId", branchId);
        parameter.Add("@FromDate", fromDate);
        parameter.Add("@ToDate", toDate);

        var result = await _context.Database.GetDbConnection().QueryAsync<SpGetProductPurchase>(
            "SpGetProductPurchase @OrganizationId, @BranchId, @FromDate, @ToDate",
            parameter, commandTimeout: 500);

        return result.ToList();
    }

    public async Task<bool> ProcessUploadedSimplifiedPurchase(long fileUploadId, int organizationId)
	{
		var parameter = new DynamicParameters();
		parameter.Add("@ExcelDataUploadId", fileUploadId);
		parameter.Add("@OrganizationId", organizationId);

		return await _context.Database.GetDbConnection().QueryFirstOrDefaultAsync<bool>(
			"SpProcessSimplifiedPurchaseFromExcel @ExcelDataUploadId, @OrganizationId ", parameter,
			commandTimeout: 500);
	}

	public async Task<bool> ProcessUploadedSimplifiedLocalPurchase(long fileUploadId, int organizationId)
	{
		var parameter = new DynamicParameters();
		parameter.Add("@ExcelDataUploadId", fileUploadId);
		parameter.Add("@OrganizationId", organizationId);

		return await _context.Database.GetDbConnection().QueryFirstOrDefaultAsync<bool>(
			"SpProcessSimplifiedLocalPurchaseFromExcel @ExcelDataUploadId, @OrganizationId ", parameter,
			commandTimeout: 500);
	}
}