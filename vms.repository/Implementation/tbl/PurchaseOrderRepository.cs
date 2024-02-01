using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.StoredProcedureModel.ParamModel;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class PurchaseOrderRepository : RepositoryBase<Purchase>, IPurchaseOrderRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;

    public PurchaseOrderRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }

    public async Task<Purchase> GetPurchaseDetails(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        //int id = int.Parse(IvatDataProtector.Unprotect(idEnc));
        var purchase = await Query().Include(c => c.Vendor).Include(c => c.Organization).
            Include(c => c.PurchaseType).Include(p => p.PurchaseReason).Include(c => c.PurchaseDetails)
            .SingleOrDefaultAsync(x => x.PurchaseId == id, CancellationToken.None);
        purchase.EncryptedId = _dataProtector.Protect(purchase.PurchaseId.ToString());

        return purchase;
    }

    public async Task<IEnumerable<Purchase>> GetPurchaseDue(int orgId)
    {
        var purchaseDue = await Query().Where(c => c.OrganizationId == orgId && c.DueAmount > 0).
            Include(c => c.Organization).Include(c => c.PurchaseType).Include(c => c.Vendor)
            .OrderByDescending(c => c.PurchaseId).SelectAsync(CancellationToken.None);
        purchaseDue.ToList().ForEach(delegate (Purchase pur)
        {
            pur.EncryptedId = _dataProtector.Protect(pur.PurchaseId.ToString());
        });
        return purchaseDue;
    }

    public async Task<IEnumerable<Purchase>> GetPurchaseDueByOrgAndBranch(int orgId, List<int> branchIds, bool isRequiredBranch)
    {
        var purchaseDue = await Query().Where(c => c.OrganizationId == orgId && c.DueAmount > 0).
            Include(c => c.Organization).Include(c => c.PurchaseType).Include(c => c.Vendor)
            .OrderByDescending(c => c.PurchaseId).SelectAsync(CancellationToken.None);
        if (isRequiredBranch)
        {
            purchaseDue = purchaseDue.Where(s => branchIds.Contains(s.OrgBranchId.Value)).ToList();
        }
        purchaseDue.ToList().ForEach(delegate (Purchase pur)
        {
            pur.EncryptedId = _dataProtector.Protect(pur.PurchaseId.ToString());
        });
        return purchaseDue;
    }

    public async Task<bool> InsertTransferReceive(vmTransferReceive vm)
    {
        string ContentInfoJson = null;
        if (vm.ContentJson != null)
            ContentInfoJson = Newtonsoft.Json.JsonConvert.SerializeObject(vm.ContentJson);
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
                , new SqlParameter("@TransferSalesId", (object)vm.TransferSalesId ?? DBNull.Value)
                , new SqlParameter("@OrganizationId", (object)vm.OrganizationId ?? DBNull.Value)
                , new SqlParameter("@InvoiceNo", (object)vm.InvoiceNo ?? DBNull.Value)
                , new SqlParameter("@PurchaseDate", (object)vm.PurchaseDate ?? DBNull.Value)
                , new SqlParameter("@PurchaseReasonId", (object)vm.PurchaseReasonId ?? DBNull.Value)
                , new SqlParameter("@DeliveryDate", (object)vm.DeliveryDate ?? DBNull.Value)
                , new SqlParameter("@IsComplete", (object)vm.IsComplete ?? DBNull.Value)
                , new SqlParameter("@CreatedBy", (object)vm.CreatedBy ?? DBNull.Value)
                , new SqlParameter("@CreatedTime", (object)vm.CreatedTime ?? DBNull.Value)
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

    public async Task<bool> InsertData(VmPurchase purchase)
    {
        var purchaseDetails = Newtonsoft.Json.JsonConvert.SerializeObject(purchase.PurchaseOrderDetailList);

        string purchasePaymentJson = null;
        if (purchase.PurchasePaymenJson != null)
            purchasePaymentJson = Newtonsoft.Json.JsonConvert.SerializeObject(purchase.PurchasePaymenJson);

        string ContentInfoJson = null;
        if (purchase.ContentInfoJson != null)
            ContentInfoJson = Newtonsoft.Json.JsonConvert.SerializeObject(purchase.ContentInfoJson);

        purchase.CreatedBy = purchase.CreatedBy;

        var currentDate = DateTime.Today;
        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                $"EXEC [dbo].[SpInsertPurchase]" +
                $"@OrganizationId " +
                $",@VendorId" +
                $",@VatChallanNo" +
                $",@VatChallanIssueDate" +
                $",@VendorInvoiceNo " +
                $",@InvoiceNo" +
                $",@PurchaseDate" +
                $",@PurchaseTypeId " +
                $",@PurchaseReasonId " +
                $",@DiscountOnTotalPrice" +
                $",@IsVatDeductedInSource" +
                $",@VDSAmount" +
                $",@ExpectedDeliveryDate " +
                $",@DeliveryDate" +
                $",@LcNo" +
                $",@LcDate" +
                $",@BillOfEntry" +
                $",@BillOfEntryDate" +
                $",@DueDate" +
                $",@TermsOfLc" +
                $",@PoNumber" +
                $",@MushakGenerationId" +
                $",@IsComplete" +
                $",@CreatedBy" +
                $",@CreatedTime" +

                $",@AdvanceTaxPaidAmount" +
                $",@ATPDate" +
                $",@ATPBankId" +
                $",@ATPBankBranchName" +
                $",@ATPNbrEconomicCodeId" +
                $",@ATPChallanNo" +
                $",@CustomsAndVATCommissionarateId" +
                $",@ReferenceKey" +

                $",@PurchaseOrderDetailsJson" +
                $",@PurchasePaymentJson" +
                $",@ContentJson"
                , new SqlParameter("@OrganizationId", purchase.OrganizationId)
                , new SqlParameter("@VendorId", (object)purchase.VendorId ?? DBNull.Value)
                , new SqlParameter("@VatChallanNo", (object)purchase.VatChallanNo ?? DBNull.Value)
                , new SqlParameter("@VatChallanIssueDate", (object)purchase.VatChallanIssueDate ?? DBNull.Value)
                , new SqlParameter("@VendorInvoiceNo", (object)purchase.VendorInvoiceNo ?? DBNull.Value)
                , new SqlParameter("@InvoiceNo", (object)purchase.InvoiceNo ?? DBNull.Value)
                , new SqlParameter("@PurchaseDate", (object)currentDate ?? DBNull.Value)
                , new SqlParameter("@PurchaseTypeId", (object)purchase.PurchaseTypeId ?? DBNull.Value)
                , new SqlParameter("@PurchaseReasonId", (object)purchase.PurchaseReasonId ?? DBNull.Value)
                , new SqlParameter("@DiscountOnTotalPrice", (object)purchase.DiscountOnTotalPrice ?? DBNull.Value)
                , new SqlParameter("@IsVatDeductedInSource", (object)purchase.IsVatDeductedInSource ?? DBNull.Value)
                , new SqlParameter("@VDSAmount", (object)purchase.VDSAmount ?? DBNull.Value)
                , new SqlParameter("@ExpectedDeliveryDate", (object)purchase.ExpectedDeliveryDate ?? DBNull.Value)
                , new SqlParameter("@DeliveryDate", (object)purchase.DeliveryDate ?? DBNull.Value)
                , new SqlParameter("@LcNo", (object)purchase.LcNo ?? DBNull.Value)
                , new SqlParameter("@LcDate", (object)purchase.LcDate ?? DBNull.Value)
                , new SqlParameter("@BillOfEntry", (object)purchase.BillOfEntry ?? DBNull.Value)
                , new SqlParameter("@BillOfEntryDate", (object)purchase.BillOfEntryDate ?? DBNull.Value)
                , new SqlParameter("@DueDate", (object)purchase.DueDate ?? DBNull.Value)
                , new SqlParameter("@TermsOfLc", (object)purchase.TermsOfLc ?? DBNull.Value)
                , new SqlParameter("@PoNumber", (object)purchase.PoNumber ?? DBNull.Value)
                , new SqlParameter("@MushakGenerationId", (object)purchase.MushakGenerationId ?? DBNull.Value)
                , new SqlParameter("@IsComplete", (object)purchase.IsComplete ?? DBNull.Value)
                , new SqlParameter("@CreatedBy", (object)purchase.CreatedBy ?? DBNull.Value)
                , new SqlParameter("@CreatedTime", (object)purchase.CreatedTime ?? DBNull.Value)

                , new SqlParameter("@AdvanceTaxPaidAmount", (object)purchase.AdvanceTaxPaidAmount ?? DBNull.Value)
                , new SqlParameter("@ATPDate", (object)purchase.ATPDate ?? DBNull.Value)
                , new SqlParameter("@ATPBankId", (object)purchase.ATPBankBranchId ?? DBNull.Value)
                , new SqlParameter("@ATPBankBranchName", (object)purchase.ATPBankBranchName ?? DBNull.Value)
                , new SqlParameter("@ATPNbrEconomicCodeId", (object)purchase.ATPNbrEconomicCodeId ?? DBNull.Value)
                , new SqlParameter("@ATPChallanNo", (object)purchase.ATPChallanNo ?? DBNull.Value)
                , new SqlParameter("@CustomsAndVATCommissionarateId", (object)purchase.CustomsAndVATCommissionarateId ?? DBNull.Value)
                , new SqlParameter("@ReferenceKey", (object)purchase.ReferenceKey ?? DBNull.Value)

                , new SqlParameter("@PurchaseOrderDetailsJson", (object)purchaseDetails ?? DBNull.Value)
                , new SqlParameter("@PurchasePaymentJson", (object)purchasePaymentJson ?? DBNull.Value)
                , new SqlParameter("@ContentJson", (object)ContentInfoJson ?? DBNull.Value)
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return await Task.FromResult(true);
    }


    public async Task<int> InsertLocalPurchase(SpInsertPurchaseCombinedParam purchase)
    {
        int Result = 0;
        var purchaseDetails = Newtonsoft.Json.JsonConvert.SerializeObject(purchase.PurchaseDetailList);

        string purchasePaymentJson = null;
        if (purchase.PurchasePayments != null)
            purchasePaymentJson = Newtonsoft.Json.JsonConvert.SerializeObject(purchase.PurchasePayments);

        string ContentInfoJson = null;
        if (purchase.ContentInfoList != null)
            ContentInfoJson = Newtonsoft.Json.JsonConvert.SerializeObject(purchase.ContentInfoList);

        purchase.CreatedBy = purchase.CreatedBy;

        var currentDate = DateTime.Today;
        try
        {
            Result=await _context.Database.ExecuteSqlRawAsync(
                $"EXEC [dbo].[SpInsertPurchase]" +
                $"@OrganizationId " +
                $",@VendorId" +
                $",@VatChallanNo" +
                $",@VatChallanIssueDate" +
                $",@VendorInvoiceNo " +
                $",@InvoiceNo" +
                $",@PurchaseDate" +
                $",@PurchaseTypeId " +
                $",@PurchaseReasonId " +
                $",@DiscountOnTotalPrice" +
                $",@IsVatDeductedInSource" +
                $",@VDSAmount" +
                $",@ExpectedDeliveryDate " +
                $",@DeliveryDate" +
                $",@LcNo" +
                $",@LcDate" +
                $",@BillOfEntry" +
                $",@BillOfEntryDate" +
                $",@DueDate" +
                $",@TermsOfLc" +
                $",@PoNumber" +
                $",@MushakGenerationId" +
                $",@IsComplete" +
                $",@CreatedBy" +
                $",@CreatedTime" +

                $",@AdvanceTaxPaidAmount" +
                $",@ATPDate" +
                $",@ATPBankId" +
                $",@ATPBankBranchName" +
                $",@ATPNbrEconomicCodeId" +
                $",@ATPChallanNo" +
                $",@CustomsAndVATCommissionarateId" +
                $",@ReferenceKey" +

                $",@PurchaseOrderDetailsJson" +
                $",@PurchasePaymentJson" +
                $",@ContentJson"
                , new SqlParameter("@OrganizationId", purchase.OrganizationId)
                , new SqlParameter("@VendorId", (object)purchase.VendorId ?? DBNull.Value)
                , new SqlParameter("@VatChallanNo", (object)purchase.VatChallanNo ?? DBNull.Value)
                , new SqlParameter("@VatChallanIssueDate", (object)purchase.VatChallanIssueDate ?? DBNull.Value)
                , new SqlParameter("@VendorInvoiceNo", (object)purchase.VendorInvoiceNo ?? DBNull.Value)
                , new SqlParameter("@InvoiceNo", (object)purchase.InvoiceNo ?? DBNull.Value)
                , new SqlParameter("@PurchaseDate", (object)currentDate ?? DBNull.Value)
                , new SqlParameter("@PurchaseTypeId", (object)purchase.PurchaseTypeId ?? DBNull.Value)
                , new SqlParameter("@PurchaseReasonId", (object)purchase.PurchaseReasonId ?? DBNull.Value)
                , new SqlParameter("@DiscountOnTotalPrice", (object)purchase.DiscountOnTotalPrice ?? DBNull.Value)
                , new SqlParameter("@IsVatDeductedInSource", (object)purchase.IsVatDeductedInSource ?? DBNull.Value)
                , new SqlParameter("@VDSAmount", (object)purchase.Vdsamount ?? DBNull.Value)
                , new SqlParameter("@ExpectedDeliveryDate", (object)purchase.ExpectedDeliveryDate ?? DBNull.Value)
                , new SqlParameter("@DeliveryDate", (object)purchase.DeliveryDate ?? DBNull.Value)
                , new SqlParameter("@LcNo", (object)purchase.LcNo ?? DBNull.Value)
                , new SqlParameter("@LcDate", (object)purchase.LcDate ?? DBNull.Value)
                , new SqlParameter("@BillOfEntry", (object)purchase.BillOfEntry ?? DBNull.Value)
                , new SqlParameter("@BillOfEntryDate", (object)purchase.BillOfEntryDate ?? DBNull.Value)
                , new SqlParameter("@DueDate", (object)purchase.DueDate ?? DBNull.Value)
                , new SqlParameter("@TermsOfLc", (object)purchase.TermsOfLc ?? DBNull.Value)
                , new SqlParameter("@PoNumber", (object)purchase.PoNumber ?? DBNull.Value)
                , new SqlParameter("@MushakGenerationId", (object)purchase.MushakGenerationId ?? DBNull.Value)
                , new SqlParameter("@IsComplete", (object)purchase.IsComplete ?? DBNull.Value)
                , new SqlParameter("@CreatedBy", (object)purchase.CreatedBy ?? DBNull.Value)
                , new SqlParameter("@CreatedTime", (object)purchase.CreatedTime ?? DBNull.Value)

                , new SqlParameter("@AdvanceTaxPaidAmount", (object)purchase.AdvanceTaxPaidAmount ?? DBNull.Value)
                , new SqlParameter("@ATPDate", (object)purchase.Atpdate ?? DBNull.Value)
                , new SqlParameter("@ATPBankId", (object)purchase.AtpbankId ?? DBNull.Value)
                , new SqlParameter("@ATPBankBranchName", (object)purchase.AtpbankBranchName ?? DBNull.Value)
                , new SqlParameter("@ATPNbrEconomicCodeId", (object)purchase.AtpnbrEconomicCodeId ?? DBNull.Value)
                , new SqlParameter("@ATPChallanNo", (object)purchase.AtpchallanNo ?? DBNull.Value)
                , new SqlParameter("@CustomsAndVATCommissionarateId", (object)purchase.CustomsAndVatcommissionarateId ?? DBNull.Value)
                , new SqlParameter("@ReferenceKey", (object)purchase.ReferenceKey ?? DBNull.Value)

                , new SqlParameter("@PurchaseOrderDetailsJson", (object)purchaseDetails ?? DBNull.Value)
                , new SqlParameter("@PurchasePaymentJson", (object)purchasePaymentJson ?? DBNull.Value)
                , new SqlParameter("@ContentJson", (object)ContentInfoJson ?? DBNull.Value)
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return Result;
    }

    public async Task<int> InsertPurchase(SpInsertPurchaseCombinedParam purchase)
    {
        var purchaseDetails = Newtonsoft.Json.JsonConvert.SerializeObject(purchase.PurchaseDetailList);

        string purchasePaymentJson = null;
        if (purchase.PurchasePayments != null)
            purchasePaymentJson = Newtonsoft.Json.JsonConvert.SerializeObject(purchase.PurchasePayments);

        string contentInfoJson = null;
        if (purchase.ContentInfoList != null)
            contentInfoJson = Newtonsoft.Json.JsonConvert.SerializeObject(purchase.ContentInfoList);

        purchase.CreatedBy = purchase.CreatedBy;

        var currentDate = DateTime.Today;
        try
        {
            string sql = $"EXEC [dbo].[SpInsertPurchaseCombined]" +
                         $"@OrganizationId " +
                         $",@VendorId" +
                         $",@VatChallanNo" +
                         $",@VatChallanIssueDate" +
                         $",@VendorInvoiceNo " +
                         $",@InvoiceNo" +
                         $",@PurchaseDate" +
                         $",@PurchaseTypeId " +
                         $",@PurchaseReasonId " +
                         $",@DiscountOnTotalPrice" +
                         $",@IsVatDeductedInSource" +
                         $",@VDSAmount" +
                         $",@ExpectedDeliveryDate " +
                         $",@DeliveryDate" +
                         $",@LcNo" +
                         $",@LcDate" +
                         $",@BillOfEntry" +
                         $",@BillOfEntryDate" +
                         $",@DueDate" +
                         $",@TermsOfLc" +
                         $",@PoNumber" +
                         $",@MushakGenerationId" +
                         $",@IsComplete" +
                         $",@CreatedBy" +
                         $",@CreatedTime" +
                         $",@AdvanceTaxPaidAmount" +
                         $",@ATPDate" +
                         $",@ATPBankId" +
                         $",@ATPBankBranchName" +
                         $",@ATPNbrEconomicCodeId" +
                         $",@ATPChallanNo" +
                         $",@CustomsAndVATCommissionarateId" +
                         $",@ReferenceKey" +
                         $",@PurchaseOrderDetailsJson" +
                         $",@PurchasePaymentJson" +
                         $",@ContentJson";
            var parameter = new DynamicParameters();
            parameter.Add("@OrganizationId", purchase.OrganizationId);
            parameter.Add("@VendorId", purchase.VendorId);
            parameter.Add("@VatChallanNo", purchase.VatChallanNo);
            parameter.Add("@VatChallanIssueDate", purchase.VatChallanIssueDate);
            parameter.Add("@VendorInvoiceNo", purchase.VendorInvoiceNo);
            parameter.Add("@InvoiceNo", purchase.InvoiceNo);
            parameter.Add("@PurchaseDate", currentDate);
            parameter.Add("@PurchaseTypeId", purchase.PurchaseTypeId);
            parameter.Add("@PurchaseReasonId", purchase.PurchaseReasonId);
            parameter.Add("@DiscountOnTotalPrice", purchase.DiscountOnTotalPrice);
            parameter.Add("@IsVatDeductedInSource", purchase.IsVatDeductedInSource);
            parameter.Add("@VDSAmount", purchase.Vdsamount);
            parameter.Add("@ExpectedDeliveryDate", purchase.ExpectedDeliveryDate);
            parameter.Add("@DeliveryDate", purchase.DeliveryDate);
            parameter.Add("@LcNo", purchase.LcNo);
            parameter.Add("@LcDate", purchase.LcDate);
            parameter.Add("@BillOfEntry", purchase.BillOfEntry);
            parameter.Add("@BillOfEntryDate", purchase.BillOfEntryDate);
            parameter.Add("@DueDate", purchase.DueDate);
            parameter.Add("@TermsOfLc", purchase.TermsOfLc);
            parameter.Add("@PoNumber", purchase.PoNumber);
            parameter.Add("@MushakGenerationId", purchase.MushakGenerationId);
            parameter.Add("@IsComplete", purchase.IsComplete);
            parameter.Add("@CreatedBy", purchase.CreatedBy);
            parameter.Add("@CreatedTime", purchase.CreatedTime);
            parameter.Add("@AdvanceTaxPaidAmount", purchase.AdvanceTaxPaidAmount);
            parameter.Add("@ATPDate", purchase.Atpdate);
            parameter.Add("@ATPBankId", purchase.AtpbankId);
            parameter.Add("@ATPBankBranchName", purchase.AtpbankBranchName);
            parameter.Add("@ATPNbrEconomicCodeId", purchase.AtpnbrEconomicCodeId);
            parameter.Add("@ATPChallanNo", purchase.AtpchallanNo);
            parameter.Add("@CustomsAndVATCommissionarateId", purchase.CustomsAndVatcommissionarateId);
            parameter.Add("@ReferenceKey", purchase.ReferenceKey);
            parameter.Add("@PurchaseOrderDetailsJson", purchaseDetails);
            parameter.Add("@PurchasePaymentJson", purchasePaymentJson);
            parameter.Add("@ContentJson", contentInfoJson);
            using (var queryMultiple = _context.Database.GetDbConnection()
                       .QueryMultiple(sql, parameter, commandTimeout: 500))
            {
                return await queryMultiple.ReadFirstAsync<int>();
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
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
                , new SqlParameter("@PurchaseId", (object)vmDebitNote.PurchaseId ?? DBNull.Value)
                , new SqlParameter("@VoucherNo", (object)vmDebitNote.VoucherNo ?? DBNull.Value)
                , new SqlParameter("@ReasonOfReturn", (object)vmDebitNote.ReasonOfReturn ?? DBNull.Value)
                , new SqlParameter("@ReturnDate", (object)vmDebitNote.ReturnDate ?? DBNull.Value)
                , new SqlParameter("@VehicleTypeId", (object)vmDebitNote.VehicleTypeId ?? DBNull.Value)
                , new SqlParameter("@VehicleName", (object)vmDebitNote.VehicleName ?? DBNull.Value)
                , new SqlParameter("@VehicleRegNo", (object)vmDebitNote.VehicleRegNo ?? DBNull.Value)
                , new SqlParameter("@VehicleDriverName", (object)vmDebitNote.VehicleDriverName ?? DBNull.Value)
                , new SqlParameter("@VehicleDriverContactNo", (object)vmDebitNote.VehicleDriverContactNo ?? DBNull.Value)
                , new SqlParameter("@CreatedBy", (object)vmDebitNote.CreatedBy ?? DBNull.Value)
                , new SqlParameter("@CreatedTime", (object)vmDebitNote.CreatedTime ?? DBNull.Value)
                , new SqlParameter("@DebitNoteDetails", (object)debitNoteDetails ?? DBNull.Value)
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
                $"EXEC [dbo].[SpInsertPurchasePayment]" +
                $"@PurchaseId " +
                $",@PaymentMethodId" +
                $",@BankId" +
                $",@WalletNo" +
                $",@BankAccountNo" +
                $",@DocumentNoOrTransId" +
                $",@DocumentOrTransDate" +
                $",@PaidAmount" +
                $",@PaymentDate" +
                $",@ReferenceKey" +
                $",@PaymentRemarks" +
                $",@CreatedBy "+
                $",@CreatedTime "
                , new SqlParameter("@PurchaseId", vmPurchase.PurchaseId)
                , new SqlParameter("@PaymentMethodId", vmPurchase.PaymentMethodId)
                , new SqlParameter("@BankId", vmPurchase.BankId)
                , new SqlParameter("@WalletNo", (object)vmPurchase.MobilePaymentWalletNo?? DBNull.Value)
                , new SqlParameter("@BankAccountNo", (object)vmPurchase.BankAccountNo ?? DBNull.Value)
                , new SqlParameter("@DocumentNoOrTransId", (object)vmPurchase.DocumentNoOrTransactionId ?? DBNull.Value)
                , new SqlParameter("@DocumentOrTransDate", (object)vmPurchase.PaymentDocumentOrTransDate ?? DBNull.Value)
                , new SqlParameter("@PaidAmount", vmPurchase.PaidAmount)
                , new SqlParameter("@PaymentDate", vmPurchase.PaymentDate)
                , new SqlParameter("@ReferenceKey", (object)vmPurchase.ReferenceKey ?? DBNull.Value)
                , new SqlParameter("@PaymentRemarks", (object)vmPurchase.PaymentRemarks ?? DBNull.Value)
                , new SqlParameter("@CreatedBy", vmPurchase.CreatedBy)
                , new SqlParameter("@CreatedTime", vmPurchase.CreatedTime)
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return await Task.FromResult(true);
    }


    public async Task<IEnumerable<Purchase>> GetPurchases(int orgIdEnc)
    {
        var purchases = await Query().Where(w => w.OrganizationId == orgIdEnc).Include(c => c.Organization).Include(c => c.PurchaseType).Include(c => c.DebitNotes).Include(c => c.Vendor).OrderByDescending(c => c.PurchaseId).SelectAsync();

        purchases.ToList().ForEach(delegate (Purchase purchase)
        {
            purchase.EncryptedId = _dataProtector.Protect(purchase.PurchaseId.ToString());
        });
        return purchases;
    }


}