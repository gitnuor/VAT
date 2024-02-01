using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vms.entity.viewModels;
using vms.repository.Repository.sp;

namespace vms.repository.Implementation.sp;

public class ApiSpInsertRepository : IApiSpInsertRepository
{
    private readonly DbContext _context;

    public ApiSpInsertRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<bool> InsertProductionExcel(List<ProductionDataImportViewModel> purchase, int orgID, int Uid, string security)
    {
        var purchaseDetails = Newtonsoft.Json.JsonConvert.SerializeObject(purchase);

        //if (purchase.ContentInfoJson != null)
        //    ContentInfoJson = Newtonsoft.Json.JsonConvert.SerializeObject(purchase.ContentInfoJson);

        //purchase.CreatedBy = purchase.CreatedBy;

        var currentDate = DateTime.Today;
        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                $"EXEC [dbo].[SpInsertBulkProductionReceive]" +
                $"@OrganizationId " +
                $",@SecurityToken" +
                $",@CreatedBy" +

                $",@ProductionReceiveJson"
                , new SqlParameter("@OrganizationId", orgID)
                , new SqlParameter("@SecurityToken", (object)security ?? DBNull.Value)
                , new SqlParameter("@CreatedBy", (object)Uid ?? DBNull.Value)
                , new SqlParameter("@ProductionReceiveJson", (object)purchaseDetails ?? DBNull.Value)
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return await Task.FromResult(true);
    }

    public async Task<bool> InsertPurchaseExcel(DataimportPurchaseFinal purchase, int orgID, int Uid, string security)
    {
        var purchaseDetails = Newtonsoft.Json.JsonConvert.SerializeObject(purchase.purchaseDetails);
        var PurchaseList = Newtonsoft.Json.JsonConvert.SerializeObject(purchase.purchase);

        var purchasePaymentJson = Newtonsoft.Json.JsonConvert.SerializeObject(purchase.Payments);

        //if (purchase.Payments != null)
        //    purchasePaymentJson = Newtonsoft.Json.JsonConvert.SerializeObject(purchase.Payments);

        string ContentInfoJson = null;
        //if (purchase.ContentInfoJson != null)
        //    ContentInfoJson = Newtonsoft.Json.JsonConvert.SerializeObject(purchase.ContentInfoJson);

        //purchase.CreatedBy = purchase.CreatedBy;

        var currentDate = DateTime.Today;
        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                $"EXEC [dbo].[SpInsertBulkPurchase]" +
                $"@OrganizationId " +
                $",@SecurityToken" +
                $",@CreatedBy" +
                $",@PurchaseJson" +
                $",@PurchaseDetailsJson" +
                $",@PurchasePaymentJson" +
                $",@ContentJson"
                , new SqlParameter("@OrganizationId", orgID)
                , new SqlParameter("@SecurityToken", (object)security ?? DBNull.Value)
                , new SqlParameter("@CreatedBy", (object)Uid ?? DBNull.Value)
                , new SqlParameter("@PurchaseJson", (object)PurchaseList ?? DBNull.Value)
                , new SqlParameter("@PurchaseDetailsJson", (object)purchaseDetails ?? DBNull.Value)
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

    public async Task<bool> InsertSaleExcel(DataimportSalesFinal sale, int orgID, int Uid, string security)
    {
        var saleDetails = Newtonsoft.Json.JsonConvert.SerializeObject(sale.SalesDetails);
        var saleList = Newtonsoft.Json.JsonConvert.SerializeObject(sale.Sales);

        var salePaymentJson = Newtonsoft.Json.JsonConvert.SerializeObject(sale.Payments);

        //if (purchase.Payments != null)
        //    purchasePaymentJson = Newtonsoft.Json.JsonConvert.SerializeObject(purchase.Payments);

        string ContentInfoJson = null;
        //if (purchase.ContentInfoJson != null)
        //    ContentInfoJson = Newtonsoft.Json.JsonConvert.SerializeObject(purchase.ContentInfoJson);

        //purchase.CreatedBy = purchase.CreatedBy;

        var currentDate = DateTime.Today;
        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                $"EXEC [dbo].[SpInsertBulkSales]" +
                $"@OrganizationId " +
                $",@SecurityToken" +
                $",@CreatedBy" +
                $",@SalesJson" +
                $",@SalesDetailsJson" +
                $",@PaymentReceiveJson" +
                $",@ContentJson"
                , new SqlParameter("@OrganizationId", orgID)
                , new SqlParameter("@SecurityToken", (object)security ?? DBNull.Value)
                , new SqlParameter("@CreatedBy", (object)Uid ?? DBNull.Value)
                , new SqlParameter("@SalesJson", (object)saleList ?? DBNull.Value)
                , new SqlParameter("@SalesDetailsJson", (object)saleDetails ?? DBNull.Value)
                , new SqlParameter("@PaymentReceiveJson", (object)salePaymentJson ?? DBNull.Value)
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

    public async Task<bool> InsertPurchase(VmPurchase purchase)
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
                $",@ATPBankBranchId" +
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
                , new SqlParameter("@ATPBankBranchId", (object)purchase.ATPBankBranchId ?? DBNull.Value)
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

    public async Task<bool> InsertPurchase(vmPurchasePost purchase)
    {
        var purchaseDetails = Newtonsoft.Json.JsonConvert.SerializeObject(purchase.PurchaseDetails);

        string purchasePaymentJson = null;
        if (purchase.PurchasePayments != null)
            purchasePaymentJson = Newtonsoft.Json.JsonConvert.SerializeObject(purchase.PurchasePayments);
        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                $"EXEC [dbo].[SpInsertPurchaseFromApi]" +
                $"@PurchaseId" +
                $", @OrganizationId" +
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
                $",@IsComplete" +
                $",@CreatedBy" +
                $",@CreatedTime" +
                $",@ApiCreatedBy" +
                $",@ApiCreatedTime" +
                $",@AdvanceTaxPaidAmount" +
                $",@ATPDate" +
                $",@ATPBankBranchId" +
                $",@ATPNbrEconomicCodeId" +
                $",@ATPChallanNo" +
                $",@CustomsAndVATCommissionarateId" +
                $",@SecurityToken" +
                $",@PurchaseOrderDetailsJson" +
                $",@PurchasePaymentJson" +
                $",@ContentJson"
                , new SqlParameter("@PurchaseId", (object)purchase.PurchaseId ?? DBNull.Value)
                , new SqlParameter("@OrganizationId", purchase.OrganizationId)
                , new SqlParameter("@VendorId", (object)purchase.VendorId ?? DBNull.Value)
                , new SqlParameter("@VatChallanNo", (object)purchase.VatChallanNo ?? DBNull.Value)
                , new SqlParameter("@VatChallanIssueDate", (object)purchase.VatChallanIssueDate ?? DBNull.Value)
                , new SqlParameter("@VendorInvoiceNo", (object)purchase.VendorInvoiceNo ?? DBNull.Value)
                , new SqlParameter("@InvoiceNo", (object)purchase.InvoiceNo ?? DBNull.Value)
                , new SqlParameter("@PurchaseDate", purchase.PurchaseDate)
                , new SqlParameter("@PurchaseTypeId", (object)purchase.PurchaseTypeId ?? DBNull.Value)
                , new SqlParameter("@PurchaseReasonId", (object)purchase.PurchaseReasonId ?? DBNull.Value)
                , new SqlParameter("@DiscountOnTotalPrice", purchase.DiscountOnTotalPrice)
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
                , new SqlParameter("@IsComplete", (object)purchase.IsComplete ?? DBNull.Value)
                , new SqlParameter("@CreatedBy", (object)purchase.CreatedBy ?? DBNull.Value)
                , new SqlParameter("@CreatedTime", (object)purchase.CreatedTime ?? DBNull.Value)
                , new SqlParameter("@ApiCreatedBy", (object)purchase.ApiCreatedBy ?? DBNull.Value)
                , new SqlParameter("@ApiCreatedTime", (object)purchase.ApiCreatedTime ?? DBNull.Value)
                , new SqlParameter("@AdvanceTaxPaidAmount", (object)purchase.AdvanceTaxPaidAmount ?? DBNull.Value)
                , new SqlParameter("@ATPDate", (object)purchase.Atpdate ?? DBNull.Value)
                , new SqlParameter("@ATPBankBranchId", (object)purchase.AtpbankBranchId ?? DBNull.Value)
                , new SqlParameter("@ATPNbrEconomicCodeId", (object)purchase.AtpnbrEconomicCodeId ?? DBNull.Value)
                , new SqlParameter("@ATPChallanNo", (object)purchase.AtpchallanNo ?? DBNull.Value)
                , new SqlParameter("@CustomsAndVATCommissionarateId", (object)purchase.CustomsAndVatcommissionarateId ?? DBNull.Value)
                , new SqlParameter("@SecurityToken", (object)purchase.SecurityToken ?? DBNull.Value)
                , new SqlParameter("@PurchaseOrderDetailsJson", (object)purchaseDetails ?? DBNull.Value)
                , new SqlParameter("@PurchasePaymentJson", (object)purchasePaymentJson ?? DBNull.Value)
                , new SqlParameter("@ContentJson", DBNull.Value)
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return await Task.FromResult(true);
    }

    public async Task<bool> InsertBulkPurchase(vmPurchaseBulkPost purchase)
    {
        string purchaseJson = null;
        if (purchase.PurchaseList.Any())
            purchaseJson = Newtonsoft.Json.JsonConvert.SerializeObject(purchase.PurchaseList);
        string purchaseDetailJson = null;
        if (purchase.PurchaseList.Any())
            purchaseDetailJson = Newtonsoft.Json.JsonConvert.SerializeObject(purchase.PurchaseDetailList);
        string purchasePaymentJson = null;
        if (purchase.PurchasePaymentList.Any())
            purchasePaymentJson = Newtonsoft.Json.JsonConvert.SerializeObject(purchase.PurchasePaymentList);

        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                $"EXEC [dbo].[SpInsertBulkPurchase] " +
                $" @OrganizationId" +
                $",@SecurityToken" +
                $",@CreatedBy" +
                $",@CreatedTime" +
                $",@PurchaseJson" +
                $",@PurchaseDetailsJson" +
                $",@PurchasePaymentJson" +
                $",@ContentJson"
                , new SqlParameter("@OrganizationId", purchase.OrganizationId)
                , new SqlParameter("@SecurityToken", (object)purchase.SecurityToken ?? DBNull.Value)
                , new SqlParameter("@CreatedBy", (object)purchase.CreatedBy ?? DBNull.Value)
                , new SqlParameter("@CreatedTime", (object)purchase.CreatedTime ?? DBNull.Value)
                , new SqlParameter("@PurchaseJson", (object)purchaseJson ?? DBNull.Value)
                , new SqlParameter("@PurchaseDetailsJson", (object)purchaseDetailJson ?? DBNull.Value)
                , new SqlParameter("@PurchasePaymentJson", (object)purchasePaymentJson ?? DBNull.Value)
                , new SqlParameter("@ContentJson", DBNull.Value)
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return await Task.FromResult(true);
    }

    public async Task<bool> InsertSale(vmSaleOrder saleOrder)
    {
        var saleDetailsJson = Newtonsoft.Json.JsonConvert.SerializeObject(saleOrder.SalesDetailList);
        string PaymentReceiveJson = null;
        if (saleOrder.SalesPaymentReceiveJson != null)
            PaymentReceiveJson = Newtonsoft.Json.JsonConvert.SerializeObject(saleOrder.SalesPaymentReceiveJson);
        string ContentJson = null;//
        if (saleOrder.ContentInfoJson != null)
        {
            ContentJson = Newtonsoft.Json.JsonConvert.SerializeObject(saleOrder.ContentInfoJson);
        }
        saleOrder.CreatedTime = DateTime.Now;
        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                $"EXEC [dbo].[SpInsertSale]  " +
                $"@InvoiceNo," +
                $"@VatChallanNo," +
                $"@OrganizationId," +
                $"@DiscountOnTotalPrice," +
                $"@IsVatDeductedInSource," +
                $"@VDSAmount," +
                $"@CustomerId," +
                $"@ReceiverName," +
                $"@ReceiverContactNo," +
                $"@ShippingAddress," +
                $"@ShippingCountryId," +
                $"@SalesTypeId," +
                $"@SalesDeliveryTypeId," +
                $"@WorkOrderNo ," +
                $"@SalesDate," +
                $"@ExpectedDeliveryDate," +
                $"@DeliveryDate ," +
                $"@DeliveryMethodId," +
                $"@ExportTypeId ," +
                $"@LcNo," +
                $"@LcDate," +
                $"@BillOfEntry," +
                $"@BillOfEntryDate," +
                $"@DueDate," +
                $"@TermsOfLc," +
                $"@CustomerPoNumber," +
                $"@OtherBranchOrganizationId," +
                $"@IsComplete," +
                $"@IsTaxInvoicePrined," +
                $"@TaxInvoicePrintedTime," +
                $"@ReferenceKey," +
                $"@CreatedBy," +
                $"@CreatedTime," +
                $"@SalesDetailsJson," +
                $"@PaymentReceiveJson," +
                $"@ContentJson"
                , new SqlParameter("@InvoiceNo", (object)saleOrder.InvoiceNo ?? DBNull.Value)
                , new SqlParameter("@VatChallanNo", (object)saleOrder.VatChallanNo ?? DBNull.Value)
                , new SqlParameter("@OrganizationId", (object)saleOrder.OrganizationId ?? DBNull.Value)
                , new SqlParameter("@DiscountOnTotalPrice", (object)saleOrder.DiscountOnTotalPrice ?? DBNull.Value)
                , new SqlParameter("@IsVatDeductedInSource", (object)saleOrder.IsVatDeductedInSource ?? DBNull.Value)
                , new SqlParameter("@VDSAmount", (object)saleOrder.VDSAmount ?? DBNull.Value)
                , new SqlParameter("@CustomerId", (object)saleOrder.CustomerId ?? DBNull.Value)
                , new SqlParameter("@ReceiverName", (object)saleOrder.ReceiverName ?? DBNull.Value)
                , new SqlParameter("@ReceiverContactNo", (object)saleOrder.ReceiverContactNo ?? DBNull.Value)
                , new SqlParameter("@ShippingAddress", (object)saleOrder.ShippingAddress ?? DBNull.Value)

                , new SqlParameter("@ShippingCountryId", (object)saleOrder.ShippingCountryId ?? DBNull.Value)
                , new SqlParameter("@SalesTypeId", (object)saleOrder.SalesTypeId ?? DBNull.Value)
                , new SqlParameter("@SalesDeliveryTypeId", saleOrder.SalesDeliveryTypeId)
                , new SqlParameter("@WorkOrderNo", (object)saleOrder.WorkOrderNo ?? DBNull.Value)
                , new SqlParameter("@SalesDate", saleOrder.SalesDate)
                , new SqlParameter("@ExpectedDeliveryDate", (object)saleOrder.ExpectedDeliveryDate ?? DBNull.Value)
                , new SqlParameter("@DeliveryDate", (object)saleOrder.DeliveryDate ?? DBNull.Value)
                , new SqlParameter("@DeliveryMethodId", (object)saleOrder.DeliveryMethodId ?? DBNull.Value)
                , new SqlParameter("@ExportTypeId", (object)saleOrder.ExportTypeId ?? DBNull.Value)
                , new SqlParameter("@LcNo", (object)saleOrder.LcNo ?? DBNull.Value)
                , new SqlParameter("@LcDate", (object)saleOrder.LcDate ?? DBNull.Value)
                , new SqlParameter("@BillOfEntry", (object)saleOrder.BillOfEntry ?? DBNull.Value)
                , new SqlParameter("@BillOfEntryDate", (object)saleOrder.BillOfEntryDate ?? DBNull.Value)
                , new SqlParameter("@DueDate", (object)saleOrder.DueDate ?? DBNull.Value)
                , new SqlParameter("@TermsOfLc", (object)saleOrder.TermsOfLc ?? DBNull.Value)
                , new SqlParameter("@CustomerPoNumber", (object)saleOrder.CustomerPoNumber ?? DBNull.Value)
                , new SqlParameter("@OtherBranchOrganizationId", (object)saleOrder.OtherBranchOrganizationId ?? DBNull.Value)
                , new SqlParameter("@IsComplete", (object)saleOrder.IsComplete ?? DBNull.Value)
                , new SqlParameter("@IsTaxInvoicePrined", (object)saleOrder.IsTaxInvoicePrined ?? DBNull.Value)
                , new SqlParameter("@TaxInvoicePrintedTime", (object)saleOrder.TaxInvoicePrintedTime ?? DBNull.Value)
                , new SqlParameter("@ReferenceKey", (object)saleOrder.ReferenceKey ?? DBNull.Value)
                , new SqlParameter("@CreatedBy", (object)saleOrder.CreatedBy ?? DBNull.Value)
                , new SqlParameter("@CreatedTime", (object)saleOrder.CreatedTime ?? DBNull.Value)
                , new SqlParameter("@SalesDetailsJson", (object)saleDetailsJson ?? DBNull.Value)
                , new SqlParameter("@PaymentReceiveJson", (object)PaymentReceiveJson ?? DBNull.Value)
                , new SqlParameter("@ContentJson", (object)ContentJson ?? DBNull.Value)
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return await Task.FromResult(true);
    }

    public async Task<bool> InsertSale(vmSalesPost sales)
    {
        var saleDetailsJson = Newtonsoft.Json.JsonConvert.SerializeObject(sales.SalesDetails);
        string PaymentReceiveJson = null;
        if (sales.SalesPaymentReceives != null)
            PaymentReceiveJson = Newtonsoft.Json.JsonConvert.SerializeObject(sales.SalesPaymentReceives);
        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                $"EXEC [dbo].[SpInsertSaleFromApi]  " +
                $"@InvoiceNo," +
                $"@VatChallanNo," +
                $"@OrganizationId," +
                $"@DiscountOnTotalPrice," +
                $"@IsVatDeductedInSource," +
                $"@VDSAmount," +
                $"@CustomerId," +
                $"@ReceiverName," +
                $"@ReceiverContactNo," +
                $"@ShippingAddress," +
                $"@ShippingCountryId," +
                $"@SalesTypeId," +
                $"@SalesDeliveryTypeId," +
                $"@WorkOrderNo ," +
                $"@SalesDate," +
                $"@ExpectedDeliveryDate," +
                $"@DeliveryDate ," +
                $"@DeliveryMethodId," +
                $"@ExportTypeId ," +
                $"@LcNo," +
                $"@LcDate," +
                $"@BillOfEntry," +
                $"@BillOfEntryDate," +
                $"@DueDate," +
                $"@TermsOfLc," +
                $"@CustomerPoNumber," +
                $"@OtherBranchOrganizationId," +
                $"@IsComplete," +
                $"@IsTaxInvoicePrined," +
                $"@TaxInvoicePrintedTime," +
                $"@ReferenceKey," +
                $"@CreatedBy," +
                $"@CreatedTime," +
                $"@SecurityToken," +
                $"@SalesDetailsJson," +
                $"@PaymentReceiveJson," +
                $"@ContentJson"
                , new SqlParameter("@InvoiceNo", (object)sales.InvoiceNo ?? DBNull.Value)
                , new SqlParameter("@VatChallanNo", (object)sales.VatChallanNo ?? DBNull.Value)
                , new SqlParameter("@OrganizationId", (object)sales.OrganizationId ?? DBNull.Value)
                , new SqlParameter("@DiscountOnTotalPrice", (object)sales.DiscountOnTotalPrice ?? DBNull.Value)
                , new SqlParameter("@IsVatDeductedInSource", (object)sales.IsVatDeductedInSource ?? DBNull.Value)
                , new SqlParameter("@VDSAmount", (object)sales.Vdsamount ?? DBNull.Value)
                , new SqlParameter("@CustomerId", (object)sales.CustomerId ?? DBNull.Value)
                , new SqlParameter("@ReceiverName", (object)sales.ReceiverName ?? DBNull.Value)
                , new SqlParameter("@ReceiverContactNo", (object)sales.ReceiverContactNo ?? DBNull.Value)
                , new SqlParameter("@ShippingAddress", (object)sales.ShippingAddress ?? DBNull.Value)
                , new SqlParameter("@ShippingCountryId", (object)sales.ShippingCountryId ?? DBNull.Value)
                , new SqlParameter("@SalesTypeId", (object)sales.SalesTypeId ?? DBNull.Value)
                , new SqlParameter("@SalesDeliveryTypeId", sales.SalesDeliveryTypeId)
                , new SqlParameter("@WorkOrderNo", (object)sales.WorkOrderNo ?? DBNull.Value)
                , new SqlParameter("@SalesDate", sales.SalesDate)
                , new SqlParameter("@ExpectedDeliveryDate", (object)sales.ExpectedDeliveryDate ?? DBNull.Value)
                , new SqlParameter("@DeliveryDate", (object)sales.DeliveryDate ?? DBNull.Value)
                , new SqlParameter("@DeliveryMethodId", (object)sales.DeliveryMethodId ?? DBNull.Value)
                , new SqlParameter("@ExportTypeId", (object)sales.ExportTypeId ?? DBNull.Value)
                , new SqlParameter("@LcNo", (object)sales.LcNo ?? DBNull.Value)
                , new SqlParameter("@LcDate", (object)sales.LcDate ?? DBNull.Value)
                , new SqlParameter("@BillOfEntry", (object)sales.BillOfEntry ?? DBNull.Value)
                , new SqlParameter("@BillOfEntryDate", (object)sales.BillOfEntryDate ?? DBNull.Value)
                , new SqlParameter("@DueDate", (object)sales.DueDate ?? DBNull.Value)
                , new SqlParameter("@TermsOfLc", (object)sales.TermsOfLc ?? DBNull.Value)
                , new SqlParameter("@CustomerPoNumber", (object)sales.CustomerPoNumber ?? DBNull.Value)
                , new SqlParameter("@OtherBranchOrganizationId", (object)sales.OtherBranchOrganizationId ?? DBNull.Value)
                , new SqlParameter("@IsComplete", (object)sales.IsComplete ?? DBNull.Value)
                , new SqlParameter("@IsTaxInvoicePrined", (object)sales.IsTaxInvoicePrined ?? DBNull.Value)
                , new SqlParameter("@TaxInvoicePrintedTime", (object)sales.TaxInvoicePrintedTime ?? DBNull.Value)
                , new SqlParameter("@ReferenceKey", (object)sales.ReferenceKey ?? DBNull.Value)
                , new SqlParameter("@CreatedBy", (object)sales.CreatedBy ?? DBNull.Value)
                , new SqlParameter("@CreatedTime", (object)sales.CreatedTime ?? DBNull.Value)
                , new SqlParameter("@SecurityToken", (object)sales.SecurityToken ?? DBNull.Value)
                , new SqlParameter("@SalesDetailsJson", (object)saleDetailsJson ?? DBNull.Value)
                , new SqlParameter("@PaymentReceiveJson", (object)PaymentReceiveJson ?? DBNull.Value)
                , new SqlParameter("@ContentJson", DBNull.Value)
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return await Task.FromResult(true);
    }

    public async Task<bool> InsertBulkSale(vmSaleBulkPost sales)
    {
        string saleJson = null;
        if (sales.SaleList.Any())
            saleJson = Newtonsoft.Json.JsonConvert.SerializeObject(sales.SaleList);
        string saleDetailJson = null;
        if (sales.SaleDetailList.Any())
            saleDetailJson = Newtonsoft.Json.JsonConvert.SerializeObject(sales.SaleDetailList);
        string salePaymentJson = null;
        if (sales.SalePaymentList.Any())
            salePaymentJson = Newtonsoft.Json.JsonConvert.SerializeObject(sales.SalePaymentList);

        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                $"EXEC [dbo].[SpInsertBulkSales] " +
                $" @OrganizationId" +
                $",@SecurityToken" +
                $",@CreatedBy" +
                $",@CreatedTime" +
                $",@SalesJson" +
                $",@SalesDetailsJson" +
                $",@PaymentReceiveJson" +
                $",@ContentJson"
                , new SqlParameter("@OrganizationId", sales.OrganizationId)
                , new SqlParameter("@SecurityToken", (object)sales.SecurityToken ?? DBNull.Value)
                , new SqlParameter("@CreatedBy", (object)sales.CreatedBy ?? DBNull.Value)
                , new SqlParameter("@CreatedTime", (object)sales.CreatedTime ?? DBNull.Value)
                , new SqlParameter("@SalesJson", (object)saleJson ?? DBNull.Value)
                , new SqlParameter("@SalesDetailsJson", (object)saleDetailJson ?? DBNull.Value)
                , new SqlParameter("@PaymentReceiveJson", (object)salePaymentJson ?? DBNull.Value)
                , new SqlParameter("@ContentJson", DBNull.Value)
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return await Task.FromResult(true);
    }
}