using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using vms.entity.Dto.SalesCreate;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.viewModels;
using vms.entity.viewModels.ReportsViewModel;
using vms.entity.viewModels.VmSalesCombineParamsModels;
using vms.repository.Repository.tbl;
using vms.utility;

namespace vms.repository.Implementation.tbl;

public class SaleRepository : RepositoryBase<Sale>, ISaleRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private readonly IDataProtector _dataProtector;

    public SaleRepository(DbContext context, IDataProtectionProvider p_protectionProvider,
        PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<Sale>> GetSalesByOrganization(string orgIdEnc)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
        var sales = await Query()
            .Where(s => s.OrganizationId == orgId && s.SalesTypeId != 3)
            .Include(s => s.OrgBranch)
            .Include(s => s.SalesType)
            .Include(s => s.SalesDeliveryType)
            .Include(s => s.ExportType)
            .Include(s => s.Customer)
            .Include(s => s.Organization)
            .OrderByDescending(s => s.SalesId)
            .SelectAsync();

        var salesByOrganization = sales.ToList();
        salesByOrganization.ForEach(delegate(Sale sale)
        {
            sale.EncryptedId = _dataProtector.Protect(sale.SalesId.ToString());
        });
        return salesByOrganization;
    }

    public async Task<IEnumerable<Sale>> GetSalesByOrganizationAndBranch(string orgIdEnc, List<int> branchIds,
        bool isRequiredBranch)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
        var sales = await Query()
            .Where(s => s.OrganizationId == orgId && s.SalesTypeId != 3)
            .Include(s => s.OrgBranch)
            .Include(s => s.SalesType)
            .Include(s => s.SalesDeliveryType)
            .Include(s => s.ExportType)
            .Include(s => s.Customer)
            .Include(s => s.Organization)
            .OrderByDescending(s => s.SalesId)
            .SelectAsync();

        var salesByOrganization = sales.ToList();
        if (isRequiredBranch)
        {
            salesByOrganization = salesByOrganization.Where(s => branchIds.Contains(s.OrgBranchId.Value)).ToList();
        }

        salesByOrganization.ForEach(delegate(Sale sale)
        {
            sale.EncryptedId = _dataProtector.Protect(sale.SalesId.ToString());
        });
        return salesByOrganization;
    }

    public async Task<IEnumerable<ViewSale>> GetSalesListByOrganization(string orgIdEnc)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
        var list = await _context.Set<ViewSale>()
            .Where(s => s.OrganizationId == orgId)
            .OrderByDescending(s => s.SalesId)
            .AsNoTracking()
            .ToListAsync();
        list.ForEach(s => s.EncryptedId = _dataProtector.Protect(s.SalesId.ToString()));
        return list;
    }

    public async Task<IEnumerable<ViewSale>> GetSalesListByOrganizationByBranch(string orgIdEnc, List<int> branchIds,
        bool isRequiredBranch)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
        var list = await _context.Set<ViewSale>()
            .Where(s => s.OrganizationId == orgId)
            .OrderByDescending(s => s.SalesId)
            .AsNoTracking()
            .ToListAsync();
        if (isRequiredBranch)
        {
            list = list.Where(s => branchIds.Contains(s.OrgBranchId.Value)).ToList();
        }

        list.ForEach(s => s.EncryptedId = _dataProtector.Protect(s.SalesId.ToString()));
        return list;
    }

    public async Task<IEnumerable<ViewSalesLocal>> GetSalesLocalListByOrganization(string orgIdEnc)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
        var list = await _context.Set<ViewSalesLocal>()
            .Where(s => s.OrganizationId == orgId)
            .OrderByDescending(s => s.SalesId)
            .AsNoTracking()
            .ToListAsync();
        list.ForEach(s => s.EncryptedId = _dataProtector.Protect(s.SalesId.ToString()));
        return list;
    }

    public async Task<IEnumerable<ViewSalesExport>> GetSalesExportListByOrganization(string orgIdEnc)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
        var list = await _context.Set<ViewSalesExport>()
            .Where(s => s.OrganizationId == orgId)
            .OrderByDescending(s => s.SalesId)
            .AsNoTracking()
            .ToListAsync();
        list.ForEach(s => s.EncryptedId = _dataProtector.Protect(s.SalesId.ToString()));
        return list;
    }

    public async Task<Sale> GetSaleData(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var sales = await Query().Include(c => c.SalesType)
            .Include(c => c.SalesDeliveryType).Include(c => c.ExportType)
            .Include(p => p.Customer).Include(p => p.Organization)
            .SingleOrDefaultAsync(c => c.SalesId == id, CancellationToken.None);
        sales.EncryptedId = _dataProtector.Protect(sales.SalesId.ToString());

        return sales;
    }

    public async Task<ViewSale> GetViewSale(string idEnc)
    {
        var id = int.Parse(_dataProtector.Unprotect(idEnc));
        var sale = await _context.Set<ViewSale>().FirstOrDefaultAsync(s => s.SalesId == id);
        if (sale != null)
        {
            sale.EncryptedId = idEnc;
        }

        return sale;
    }

    public async Task<ViewSalesLocal> GetViewSaleLocal(string idEnc)
    {
        var id = int.Parse(_dataProtector.Unprotect(idEnc));
        var sale = await _context.Set<ViewSalesLocal>().FirstOrDefaultAsync(s => s.SalesId == id);
        if (sale != null)
        {
            sale.EncryptedId = idEnc;
        }

        return sale;
    }

    public async Task<ViewSalesExport> GetViewSaleExport(string idEnc)
    {
        var id = int.Parse(_dataProtector.Unprotect(idEnc));
        var sale = await _context.Set<ViewSalesExport>().FirstOrDefaultAsync(s => s.SalesId == id);
        if (sale != null)
        {
            sale.EncryptedId = idEnc;
        }

        return sale;
    }

    public async Task<IEnumerable<ViewVdsSale>> GetSalesViewByOrgAndBranch(string orgIdEnc, List<int> branchIds,
        bool isRequiredBranch)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
        var list = await _context.Set<ViewVdsSale>()
            .Where(s => s.OrganizationId == orgId)
            .OrderByDescending(s => s.SalesId)
            .AsNoTracking()
            .ToListAsync();
        if (isRequiredBranch)
        {
            list = list.Where(s => branchIds.Contains(s.OrgBranchId.Value)).ToList();
        }

        return list;
    }

    public async Task<IEnumerable<Sale>> GetSalesDetails(int orgId)
    {
        var sales = await Query().Where(c => c.OrganizationId == orgId && c.SalesTypeId != 3)
            .Include(p => p.Customer).Include(c => c.CreditNotes).Include(p => p.Organization)
            .OrderByDescending(c => c.SalesId).SelectAsync();
        sales.ToList().ForEach(delegate(Sale sale)
        {
            sale.EncryptedId = _dataProtector.Protect(sale.SalesId.ToString());
        });
        return sales;
    }

    public async Task<int> InsertSaleFromApi(SalesCombinedInsertParamDto sale, string apiData = null)
    {
        try
        {
            const string sql = "EXEC [dbo].[SpInsertSaleFromApiCombined]  " +
                               "@SecurityToken," +
                               "@OrganizationId," +
                               "@BranchId," +
                               "@InvoiceNo," +
                               "@InvoiceDate," +
                               "@VatChallanNo," +
                               "@DiscountOnTotalPrice," +
                               "@IsVatDeductedInSource," +
                               "@VDSAmount," +
                               "@CustomerId," +
                               "@CustomerName," +
                               "@CustomerBin," +
                               "@CustomerNid," +
                               "@CustomerAddress," +
                               "@CustomerPhoneNo," +
                               "@ReceiverName," +
                               "@ReceiverContactNo," +
                               "@ShippingAddress," +
                               "@ShippingCountryId," +
                               "@SalesTypeId," +
                               "@SalesDeliveryTypeId," +
                               "@WorkOrderNo ," +
                               "@SalesDate," +
                               "@DeliveryDate ," +
                               "@DeliveryMethodId," +
                               "@ExportTypeId ," +
                               "@LcNo," +
                               "@LcDate," +
                               "@BillOfEntry," +
                               "@BillOfEntryDate," +
                               "@DueDate," +
                               "@TermsOfLc," +
                               "@CustomerPoNumber," +
                               "@IsComplete," +
                               "@IsTaxInvoicePrinted," +
                               "@TaxInvoicePrintedTime," +
                               "@VehicleName," +
                               "@VehicleRegNo," +
                               "@VehicleDriverName," +
                               "@VehicleDriverContactNo," +
                               "@SalesRemarks," +
                               "@ReferenceKey," +
                               "@CreatedBy," +
                               "@CreatedTime," +
                               "@SalesDetailsJson," +
                               "@ApiData";
            var saleDetailsJson = JsonConvert.SerializeObject(sale.Details);

            sale.CreatedTime = DateTime.Now;

            var parameter = new DynamicParameters();

            parameter.Add("@SecurityToken", sale.Token);
            parameter.Add("@OrganizationId", sale.OrganizationId);
            parameter.Add("@BranchId", sale.BranchId);
            parameter.Add("@InvoiceNo", sale.InvoiceNo);
            parameter.Add("@InvoiceDate", StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(sale.InvoiceDate));
            parameter.Add("@VatChallanNo", sale.VatChallanNo);
            parameter.Add("@DiscountOnTotalPrice", sale.DiscountOnTotalPrice);
            parameter.Add("@IsVatDeductedInSource", sale.IsVatDeductedInSource);
            parameter.Add("@VDSAmount", sale.VdsAmount);
            parameter.Add("@CustomerId", sale.CustomerId);
            parameter.Add("@CustomerName", sale.CustomerName);
            parameter.Add("@CustomerBin", sale.CustomerBin);
            parameter.Add("@CustomerNid", sale.CustomerNid);
            parameter.Add("@CustomerAddress", sale.CustomerAddress);
            parameter.Add("@CustomerPhoneNo", sale.CustomerPhoneNo);

            parameter.Add("@ReceiverName", sale.ReceiverName);
            parameter.Add("@ReceiverContactNo", sale.ReceiverContactNo);
            parameter.Add("@ShippingAddress", sale.ShippingAddress);
            parameter.Add("@ShippingCountryId", sale.ShippingCountryId);
            parameter.Add("@SalesTypeId", sale.SalesTypeId);
            parameter.Add("@SalesDeliveryTypeId", sale.SalesDeliveryTypeId);
            parameter.Add("@WorkOrderNo", sale.WorkOrderNo);
            parameter.Add("@SalesDate",
                StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(sale.SalesDate));
            parameter.Add("@DeliveryDate",
                StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(sale.DeliveryDate));
            parameter.Add("@DeliveryMethodId", sale.DeliveryMethodId);
            parameter.Add("@ExportTypeId", sale.ExportTypeId);
            parameter.Add("@LcNo", sale.LcNo);
            parameter.Add("@LcDate", sale.LcDate);
            parameter.Add("@BillOfEntry", sale.BillOfEntry);
            parameter.Add("@BillOfEntryDate", sale.BillOfEntryDate);
            parameter.Add("@DueDate", sale.DueDate);
            parameter.Add("@TermsOfLc", sale.TermsOfLc);
            parameter.Add("@CustomerPoNumber", sale.CustomerPoNumber);
            parameter.Add("@IsComplete", sale.IsComplete);
            parameter.Add("@IsTaxInvoicePrinted", sale.IsTaxInvoicePrinted);
            parameter.Add("@TaxInvoicePrintedTime", sale.TaxInvoicePrintedTime);
            parameter.Add("@VehicleName", sale.VehicleName);
            parameter.Add("@VehicleRegNo", sale.VehicleRegistrationNo);
            parameter.Add("@VehicleDriverName", sale.DriverName);
            parameter.Add("@VehicleDriverContactNo", sale.DriverMobile);
            parameter.Add("@SalesRemarks", sale.SalesRemarks);
            parameter.Add("@ReferenceKey", sale.SalesId);
            parameter.Add("@CreatedBy", sale.CreatedBy);
            parameter.Add("@CreatedTime",
                StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(sale.CreatedTime));
            parameter.Add("@SalesDetailsJson", saleDetailsJson);
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

    public Task<Sale> GetSalesDetailsAsync(vmSalesDetails salesDetails)
    {
        try
        {
            var parameter = new DynamicParameters();
            parameter.Add("@FromDate", salesDetails.FromDate);
            parameter.Add("@ToDate", salesDetails.ToDate);
            parameter.Add("@InvoiceNo", salesDetails.InvoiceNo);
            parameter.Add("@CustomerName", salesDetails.CustomerName);

            //var data =  _context.Database.(
            //      $"EXEC [dbo].[GetMushok6.3Info]  " +
            //      $"@FromDate," +
            //      $"@ToDate," +
            //      $"@InvoiceNo," +
            //      $"@CustomerName"

            //      , new SqlParameter("@FromDate",Convert.ToDateTime(salesDetails.FromDate))
            //      , new SqlParameter("@ToDate", Convert.ToDateTime(salesDetails.ToDate))
            //      , new SqlParameter("@InvoiceNo", salesDetails.InvoiceNo)
            //      , new SqlParameter("@CustomerName", salesDetails.CustomerId)

            //  );

            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<Sale>> GetSalesDue(int orgId)
    {
        var salesDue = await Query().Include(c => c.SalesType).Include(c => c.SalesDeliveryType)
            .Include(c => c.ExportType).Include(p => p.Customer).Include(p => p.Organization)
            .Where(w => w.PaymentDueAmount > 0 && w.OrganizationId == orgId && w.SalesTypeId != 3).SelectAsync();
        salesDue.ToList().ForEach(delegate(Sale sale)
        {
            sale.EncryptedId = _dataProtector.Protect(sale.SalesId.ToString());
        });
        return salesDue;
    }

    public async Task<bool> InsertCreditNote(vmCreditNote vmCreditNote)
    {
        var creditNoteDetails = Newtonsoft.Json.JsonConvert.SerializeObject(vmCreditNote.CreditNoteDetails);
        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                $"EXEC [dbo].[SpCreditNote]  " +
                $"@SalesId," +
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
                $"@CreditNoteDetails"
                , new SqlParameter("@SalesId", (object)vmCreditNote.SalesId ?? DBNull.Value)
                , new SqlParameter("@VoucherNo", (object)vmCreditNote.VoucherNo ?? DBNull.Value)
                , new SqlParameter("@ReasonOfReturn", (object)vmCreditNote.ReasonOfReturn ?? DBNull.Value)
                , new SqlParameter("@ReturnDate", (object)vmCreditNote.ReturnDate ?? DBNull.Value)
                , new SqlParameter("@VehicleTypeId", (object)vmCreditNote.VehicleTypeId ?? DBNull.Value)
                , new SqlParameter("@VehicleName", (object)vmCreditNote.VehicleName ?? DBNull.Value)
                , new SqlParameter("@VehicleRegNo", (object)vmCreditNote.VehicleRegNo ?? DBNull.Value)
                , new SqlParameter("@VehicleDriverName", (object)vmCreditNote.VehicleDriverName ?? DBNull.Value)
                , new SqlParameter("@VehicleDriverContactNo",
                    (object)vmCreditNote.VehicleDriverContactNo ?? DBNull.Value)
                , new SqlParameter("@CreatedBy", (object)vmCreditNote.CreatedBy ?? DBNull.Value)
                , new SqlParameter("@CreatedTime", (object)vmCreditNote.CreatedTime ?? DBNull.Value)
                , new SqlParameter("@CreditNoteDetails", (object)creditNoteDetails ?? DBNull.Value)
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return await Task.FromResult(true);
    }

    public async Task<int> InsertSale(VmSalesCombineParamsModel saleOrder)
    {
        try
        {
            string sql = $"EXEC [dbo].[SpInsertSaleCombined]  " +
                         $"@InvoiceNo," +
                         $"@InvoiceDate," +
                         $"@VatChallanNo," +
                         $"@OrganizationId," +
                         $"@OrgBranchId," +
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
                         $"@VehicleTypeId," +
                         $"@VehicleName," +
                         $"@VehicleRegNo," +
                         $"@VehicleDriverName," +
                         $"@VehicleDriverContactNo," +
                         $"@SalesRemarks," +
                         $"@ReferenceKey," +
                         $"@CreatedBy," +
                         $"@CreatedTime," +
                         $"@SalesDetailsJson," +
                         $"@PaymentReceiveJson," +
                         $"@SalesBreakDownJson," +
                         $"@ContentJson";
            var saleDetailsJson = Newtonsoft.Json.JsonConvert.SerializeObject(saleOrder.SalesDetailList);
            string paymentReceiveJson = null;
            if (saleOrder.SalesPaymentReceiveJson != null)
                paymentReceiveJson = Newtonsoft.Json.JsonConvert.SerializeObject(saleOrder.SalesPaymentReceiveJson);
            string contentJson = null; //
            if (saleOrder.ContentInfoJson != null)
            {
                contentJson = Newtonsoft.Json.JsonConvert.SerializeObject(saleOrder.ContentInfoJson);
            }

            string breakDownJson = null;
            if (saleOrder.SaleBreakDowns.Any())
            {
                breakDownJson = Newtonsoft.Json.JsonConvert.SerializeObject(saleOrder.SaleBreakDowns);
            }

            saleOrder.CreatedTime = DateTime.Now;

            var parameter = new DynamicParameters();

            parameter.Add("@InvoiceNo", saleOrder.InvoiceNo);
            parameter.Add("@InvoiceDate", saleOrder.InvoiceDate);
            parameter.Add("@VatChallanNo", saleOrder.VatChallanNo);
            parameter.Add("@OrganizationId", saleOrder.OrganizationId);
            parameter.Add("@OrgBranchId", saleOrder.OrgBranchId);
            parameter.Add("@DiscountOnTotalPrice", saleOrder.DiscountOnTotalPrice);
            parameter.Add("@IsVatDeductedInSource", saleOrder.IsVatDeductedInSource);
            parameter.Add("@VDSAmount", saleOrder.VDSAmount);
            parameter.Add("@CustomerId", saleOrder.CustomerId);
            parameter.Add("@ReceiverName", saleOrder.ReceiverName);
            parameter.Add("@ReceiverContactNo", saleOrder.ReceiverContactNo);
            parameter.Add("@ShippingAddress", saleOrder.ShippingAddress);
            parameter.Add("@ShippingCountryId", saleOrder.ShippingCountryId);
            parameter.Add("@SalesTypeId", saleOrder.SalesTypeId);
            parameter.Add("@SalesDeliveryTypeId", saleOrder.SalesDeliveryTypeId);
            parameter.Add("@WorkOrderNo", saleOrder.WorkOrderNo);
            parameter.Add("@SalesDate",
                StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(saleOrder.SalesDate));
            parameter.Add("@ExpectedDeliveryDate",
                StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(saleOrder.ExpectedDeliveryDate));
            parameter.Add("@DeliveryDate",
                StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(saleOrder.DeliveryDate));
            parameter.Add("@DeliveryMethodId", saleOrder.DeliveryMethodId);
            parameter.Add("@ExportTypeId", saleOrder.ExportTypeId);
            parameter.Add("@LcNo", saleOrder.LcNo);
            parameter.Add("@LcDate", saleOrder.LcDate);
            parameter.Add("@BillOfEntry", saleOrder.BillOfEntry);
            parameter.Add("@BillOfEntryDate", saleOrder.BillOfEntryDate);
            parameter.Add("@DueDate", saleOrder.DueDate);
            parameter.Add("@TermsOfLc", saleOrder.TermsOfLc);
            parameter.Add("@CustomerPoNumber", saleOrder.CustomerPoNumber);
            parameter.Add("@OtherBranchOrganizationId", saleOrder.OtherBranchOrganizationId);
            parameter.Add("@IsComplete", saleOrder.IsComplete);
            parameter.Add("@IsTaxInvoicePrined", saleOrder.IsTaxInvoicePrined);
            parameter.Add("@TaxInvoicePrintedTime", saleOrder.TaxInvoicePrintedTime);
            parameter.Add("@VehicleTypeId", saleOrder.VehicleTypeId);
            parameter.Add("@VehicleName", saleOrder.VehicleName);
            parameter.Add("@VehicleRegNo", saleOrder.VehicleRegNo);
            parameter.Add("@VehicleDriverName", saleOrder.VehicleDriverName);
            parameter.Add("@VehicleDriverContactNo", saleOrder.VehicleDriverContactNo);
            parameter.Add("@SalesRemarks", saleOrder.SalesRemarks);
            parameter.Add("@ReferenceKey", saleOrder.ReferenceKey);
            parameter.Add("@CreatedBy", saleOrder.CreatedBy);
            parameter.Add("@CreatedTime",
                StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(saleOrder.CreatedTime));
            parameter.Add("@SalesDetailsJson", saleDetailsJson);
            parameter.Add("@PaymentReceiveJson", paymentReceiveJson);
            parameter.Add("@SalesBreakDownJson", breakDownJson);
            parameter.Add("@ContentJson", contentJson);
            using (var queryMultiple = _context.Database.GetDbConnection()
                       .QueryMultiple(sql, parameter, commandTimeout: 500))
            {
                return await queryMultiple.ReadFirstAsync<int>();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<int> InsertSaleLocalData(VmSalesCombineParamsModel saleOrder)
    {
        int result = 0;
        var saleDetailsJson = Newtonsoft.Json.JsonConvert.SerializeObject(saleOrder.SalesDetailList);
        //var PaymentReceiveJson = Newtonsoft.Json.JsonConvert.SerializeObject(saleOrder.SalesPaymentReceiveJson);
        string paymentReceiveJson = null;
        if (saleOrder.SalesPaymentReceiveJson != null)
            paymentReceiveJson = Newtonsoft.Json.JsonConvert.SerializeObject(saleOrder.SalesPaymentReceiveJson);
        string contentJson = null; //
        if (saleOrder.ContentInfoJson != null)
        {
            contentJson = Newtonsoft.Json.JsonConvert.SerializeObject(saleOrder.ContentInfoJson);
        }

        string breakDownJson = null;
        if (saleOrder.SaleBreakDowns.Any())
        {
            breakDownJson = Newtonsoft.Json.JsonConvert.SerializeObject(saleOrder.SaleBreakDowns);
        }

        saleOrder.CreatedTime = DateTime.Now;
        try
        {
            result = await _context.Database.ExecuteSqlRawAsync(
                $"EXEC [dbo].[SpInsertSaleCombined]  " +
                $"@InvoiceNo," +
                $"@InvoiceDate," +
                $"@VatChallanNo," +
                $"@OrganizationId," +
                $"@OrgBranchId," +
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
                $"@VehicleTypeId," +
                $"@VehicleName," +
                $"@VehicleRegNo," +
                $"@VehicleDriverName," +
                $"@VehicleDriverContactNo," +
                $"@ReferenceKey," +
                $"@CreatedBy," +
                $"@CreatedTime," +
                $"@SalesDetailsJson," +
                $"@PaymentReceiveJson," +
                $"@SalesBreakDownJson," +
                $"@ContentJson"
                , new SqlParameter("@InvoiceNo", (object)saleOrder.InvoiceNo ?? DBNull.Value)
                , new SqlParameter("@InvoiceDate", (object)saleOrder.InvoiceDate ?? DBNull.Value)
                , new SqlParameter("@VatChallanNo", (object)saleOrder.VatChallanNo ?? DBNull.Value)
                , new SqlParameter("@OrganizationId", (object)saleOrder.OrganizationId ?? DBNull.Value)
                , new SqlParameter("@OrgBranchId", (object)saleOrder.OrgBranchId ?? DBNull.Value)
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
                , new SqlParameter("@SalesDate", StringGenerator.DateTimeToSqlCompatibleString(saleOrder.SalesDate))
                , new SqlParameter("@ExpectedDeliveryDate",
                    StringGenerator.DateTimeToSqlCompatibleString(saleOrder.ExpectedDeliveryDate))
                , new SqlParameter("@DeliveryDate",
                    StringGenerator.DateTimeToSqlCompatibleString(saleOrder.DeliveryDate))
                , new SqlParameter("@DeliveryMethodId", (object)saleOrder.DeliveryMethodId ?? DBNull.Value)
                , new SqlParameter("@ExportTypeId", (object)saleOrder.ExportTypeId ?? DBNull.Value)
                , new SqlParameter("@LcNo", (object)saleOrder.LcNo ?? DBNull.Value)
                , new SqlParameter("@LcDate", (object)saleOrder.LcDate ?? DBNull.Value)
                , new SqlParameter("@BillOfEntry", (object)saleOrder.BillOfEntry ?? DBNull.Value)
                , new SqlParameter("@BillOfEntryDate", (object)saleOrder.BillOfEntryDate ?? DBNull.Value)
                , new SqlParameter("@DueDate", (object)saleOrder.DueDate ?? DBNull.Value)
                , new SqlParameter("@TermsOfLc", (object)saleOrder.TermsOfLc ?? DBNull.Value)
                , new SqlParameter("@CustomerPoNumber", (object)saleOrder.CustomerPoNumber ?? DBNull.Value)
                , new SqlParameter("@OtherBranchOrganizationId",
                    (object)saleOrder.OtherBranchOrganizationId ?? DBNull.Value)
                , new SqlParameter("@IsComplete", (object)saleOrder.IsComplete ?? DBNull.Value)
                , new SqlParameter("@IsTaxInvoicePrined", (object)saleOrder.IsTaxInvoicePrined ?? DBNull.Value)
                , new SqlParameter("@TaxInvoicePrintedTime", (object)saleOrder.TaxInvoicePrintedTime ?? DBNull.Value)
                , new SqlParameter("@VehicleTypeId", (object)saleOrder.VehicleTypeId ?? DBNull.Value)
                , new SqlParameter("@VehicleName", (object)saleOrder.VehicleName ?? DBNull.Value)
                , new SqlParameter("@VehicleRegNo", (object)saleOrder.VehicleRegNo ?? DBNull.Value)
                , new SqlParameter("@VehicleDriverName", (object)saleOrder.VehicleDriverName ?? DBNull.Value)
                , new SqlParameter("@VehicleDriverContactNo", (object)saleOrder.VehicleDriverContactNo ?? DBNull.Value)
                , new SqlParameter("@ReferenceKey", (object)saleOrder.ReferenceKey ?? DBNull.Value)
                , new SqlParameter("@CreatedBy", (object)saleOrder.CreatedBy ?? DBNull.Value)
                , new SqlParameter("@CreatedTime", StringGenerator.DateTimeToSqlCompatibleString(saleOrder.CreatedTime))
                , new SqlParameter("@SalesDetailsJson", (object)saleDetailsJson ?? DBNull.Value)
                , new SqlParameter("@PaymentReceiveJson", (object)paymentReceiveJson ?? DBNull.Value)
                , new SqlParameter("@SalesBreakDownJson", (object)breakDownJson ?? DBNull.Value)
                , new SqlParameter("@ContentJson", (object)contentJson ?? DBNull.Value)
            );
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> InsertData(vmSaleOrder saleOrder)
    {
        var saleDetailsJson = Newtonsoft.Json.JsonConvert.SerializeObject(saleOrder.SalesDetailList);
        //var PaymentReceiveJson = Newtonsoft.Json.JsonConvert.SerializeObject(saleOrder.SalesPaymentReceiveJson);
        string paymentReceiveJson = null;
        if (saleOrder.SalesPaymentReceiveJson != null)
            paymentReceiveJson = Newtonsoft.Json.JsonConvert.SerializeObject(saleOrder.SalesPaymentReceiveJson);
        string contentJson = null; //
        if (saleOrder.ContentInfoJson != null)
        {
            contentJson = Newtonsoft.Json.JsonConvert.SerializeObject(saleOrder.ContentInfoJson);
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
                $"@VehicleTypeId," +
                $"@VehicleName," +
                $"@VehicleRegNo," +
                $"@VehicleDriverName," +
                $"@VehicleDriverContactNo," +
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
                , new SqlParameter("@SalesDate", StringGenerator.DateTimeToSqlCompatibleString(saleOrder.SalesDate))
                , new SqlParameter("@ExpectedDeliveryDate",
                    StringGenerator.DateTimeToSqlCompatibleString(saleOrder.ExpectedDeliveryDate))
                , new SqlParameter("@DeliveryDate",
                    StringGenerator.DateTimeToSqlCompatibleString(saleOrder.DeliveryDate))
                , new SqlParameter("@DeliveryMethodId", (object)saleOrder.DeliveryMethodId ?? DBNull.Value)
                , new SqlParameter("@ExportTypeId", (object)saleOrder.ExportTypeId ?? DBNull.Value)
                , new SqlParameter("@LcNo", (object)saleOrder.LcNo ?? DBNull.Value)
                , new SqlParameter("@LcDate", (object)saleOrder.LcDate ?? DBNull.Value)
                , new SqlParameter("@BillOfEntry", (object)saleOrder.BillOfEntry ?? DBNull.Value)
                , new SqlParameter("@BillOfEntryDate", (object)saleOrder.BillOfEntryDate ?? DBNull.Value)
                , new SqlParameter("@DueDate", (object)saleOrder.DueDate ?? DBNull.Value)
                , new SqlParameter("@TermsOfLc", (object)saleOrder.TermsOfLc ?? DBNull.Value)
                , new SqlParameter("@CustomerPoNumber", (object)saleOrder.CustomerPoNumber ?? DBNull.Value)
                , new SqlParameter("@OtherBranchOrganizationId",
                    (object)saleOrder.OtherBranchOrganizationId ?? DBNull.Value)
                , new SqlParameter("@IsComplete", (object)saleOrder.IsComplete ?? DBNull.Value)
                , new SqlParameter("@IsTaxInvoicePrined", (object)saleOrder.IsTaxInvoicePrined ?? DBNull.Value)
                , new SqlParameter("@TaxInvoicePrintedTime", (object)saleOrder.TaxInvoicePrintedTime ?? DBNull.Value)
                , new SqlParameter("@VehicleTypeId", (object)saleOrder.VehicleTypeId ?? DBNull.Value)
                , new SqlParameter("@VehicleName", (object)saleOrder.VehicleName ?? DBNull.Value)
                , new SqlParameter("@VehicleRegNo", (object)saleOrder.VehicleRegNo ?? DBNull.Value)
                , new SqlParameter("@VehicleDriverName", (object)saleOrder.VehicleDriverName ?? DBNull.Value)
                , new SqlParameter("@VehicleDriverContactNo", (object)saleOrder.VehicleDriverContactNo ?? DBNull.Value)
                , new SqlParameter("@ReferenceKey", (object)saleOrder.ReferenceKey ?? DBNull.Value)
                , new SqlParameter("@CreatedBy", (object)saleOrder.CreatedBy ?? DBNull.Value)
                , new SqlParameter("@CreatedTime", StringGenerator.DateTimeToSqlCompatibleString(saleOrder.CreatedTime))
                , new SqlParameter("@SalesDetailsJson", (object)saleDetailsJson ?? DBNull.Value)
                , new SqlParameter("@PaymentReceiveJson", (object)paymentReceiveJson ?? DBNull.Value)
                , new SqlParameter("@ContentJson", (object)contentJson ?? DBNull.Value)
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }


        return await Task.FromResult(true);
    }

    public Task<bool> ProcessUploadedSimplifiedSale(long fileUploadId, int organizationId)
    {
        var parameter = new DynamicParameters();
        parameter.Add("@ExcelDataUploadId", fileUploadId);
        parameter.Add("@OrganizationId", organizationId);

        return _context.Database.GetDbConnection().QueryFirstOrDefaultAsync<bool>(
            "SpProcessSimplifiedLocalSaleFromExcel @ExcelDataUploadId, @OrganizationId ", parameter,
            commandTimeout: 500);
    }

    public Task<bool> ProcessUploadedSimplifiedLocalSaleCalculatedByVat(long fileUploadId, int organizationId)
	{
		var parameter = new DynamicParameters();
		parameter.Add("@ExcelDataUploadId", fileUploadId);
		parameter.Add("@OrganizationId", organizationId);

		return _context.Database.GetDbConnection().QueryFirstOrDefaultAsync<bool>(
			"SpProcessSimplifiedLocalSaleCalculatedByVatFromExcel @ExcelDataUploadId, @OrganizationId ", parameter,
			commandTimeout: 500);
	}

    public async Task<IEnumerable<Sale>> GetSalesDueByOrganization(string orgIdEnc)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
        var sales = await Query()
            .Where(s => s.OrganizationId == orgId && s.SalesTypeId != 3 && s.PaymentDueAmount > 0)
            .Include(s => s.OrgBranch)
            .Include(s => s.SalesType)
            .Include(s => s.SalesDeliveryType)
            .Include(s => s.ExportType)
            .Include(s => s.Customer)
            .Include(s => s.Organization)
            .OrderByDescending(s => s.SalesId)
            .SelectAsync();

        var salesByOrganization = sales.ToList();
        salesByOrganization.ForEach(delegate(Sale sale)
        {
            sale.EncryptedId = _dataProtector.Protect(sale.SalesId.ToString());
        });
        return salesByOrganization;
    }

    public async Task<IEnumerable<Sale>> GetSalesDueByOrganizationAndBranch(string orgIdEnc, List<int> branchIds,
        bool isRequiredBranch)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
        var sales = await Query()
            .Where(s => s.OrganizationId == orgId && s.SalesTypeId != 3 && s.PaymentDueAmount > 0)
            .Include(s => s.OrgBranch)
            .Include(s => s.SalesType)
            .Include(s => s.SalesDeliveryType)
            .Include(s => s.ExportType)
            .Include(s => s.Customer)
            .Include(s => s.Organization)
            .OrderByDescending(s => s.SalesId)
            .SelectAsync();
        if (isRequiredBranch)
        {
            sales = sales.Where(s => branchIds.Contains(s.OrgBranchId.Value)).ToList();
        }

        var salesByOrganization = sales.ToList();
        salesByOrganization.ForEach(delegate(Sale sale)
        {
            sale.EncryptedId = _dataProtector.Protect(sale.SalesId.ToString());
        });
        return salesByOrganization;
    }

    public async Task<IEnumerable<ViewSalesPaymentAgingReport>> GetSalesAgingReport(string orgIdEnc)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
        return await _context.Set<ViewSalesPaymentAgingReport>()
            .Where(x => x.OrganizationId == orgId)
            .ToListAsync();
    }

    public async Task<List<SpGetProductSale>> ProductSalesListReport(int organizationId,
        int branchId, DateTime? fromDate, DateTime? toDate, int userId)
    {
        var parameter = new DynamicParameters();
        parameter.Add("@OrganizationId ", organizationId);
        parameter.Add("@BranchId", branchId);
        parameter.Add("@FromDate", fromDate);
        parameter.Add("@ToDate", toDate);
        parameter.Add("@UserId", userId);

        var result = await _context.Database.GetDbConnection().QueryAsync<SpGetProductSale>(
            "SpGetProductSale @OrganizationId, @BranchId, @FromDate, @ToDate, @UserId",
            parameter, commandTimeout: 500);

        return result.ToList();
    }
}