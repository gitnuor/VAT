using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.StoredProcedureModel.HTMLMushak;
using vms.repository.Repository.sp;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace vms.repository.Implementation.sp;

public class SpPurchaseCalcBookRepository : ISpPurchaseCalcBookRepository
{
    private readonly DbContext _context;

    public SpPurchaseCalcBookRepository(DbContext context)
    {
        _context = context;
    }

    //public async Task<List<SpPurchaseCalcBook>> GetSpPurchaseCalcBook(int organizationId, DateTime fromDate,
    //    DateTime toDate, int vendorId, int productId)
    //{
    //    return await _context.Set<SpPurchaseCalcBook>()
    //        .FromSqlRaw("SpPurchaseCalcBook @OrganizationId={0},@FromDate={1},@ToDate={2},@VendorId={3},@ProductId={4}",
    //            organizationId, fromDate, toDate, vendorId, productId).ToListAsync();
    //}

    public async Task<List<SpPurchaseCalcBook>> GetSpPurchaseCalcBook(int organizationId, DateTime fromDate,
       DateTime toDate, int vendorId, int productId)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OrganizationId", organizationId);
            parameters.Add("@FromDate", fromDate);
            parameters.Add("@ToDate", toDate);
            parameters.Add("@VendorId", vendorId);
            parameters.Add("@ProductId", productId);
            string sql = "EXEC SpPurchaseCalcBook @OrganizationId, @FromDate, @ToDate, @VendorId, @ProductId";

            var result = await _context.Database.GetDbConnection().QueryAsync<SpPurchaseCalcBook>(sql, parameters, commandType: CommandType.StoredProcedure, commandTimeout: 500);
            return result.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<SpSalesCalcBook>> GetSpSaleCalcBook(int organizationId, DateTime fromDate,
        DateTime toDate, int customerId, int productId)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OrganizationId", organizationId);
            parameters.Add("@FromDate", fromDate);
            parameters.Add("@ToDate", toDate);
            parameters.Add("@CustomerId", customerId);
            parameters.Add("@ProductId", productId);
            string sql = "EXEC SpSalesCalcBook @OrganizationId, @FromDate, @ToDate, @CustomerId, @ProductId";

            var result = await _context.Database.GetDbConnection().QueryAsync<SpSalesCalcBook>(sql, parameters, commandType: CommandType.StoredProcedure, commandTimeout: 500);
            return result.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<List<SpPurchaseSaleCalcBook>> GetSpPurchaseSaleCalcBook(int organizationId, DateTime fromDate,
        DateTime toDate, int customerId, int vendorId, int productId)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OrganizationId", organizationId);
            parameters.Add("@FromDate", fromDate);
            parameters.Add("@ToDate", toDate);
            parameters.Add("@VendorId", vendorId);
            parameters.Add("@CustomerId", customerId);
            parameters.Add("@ProductId", productId);
            string sql = "EXEC SpPurchaseSaleCalcBook @OrganizationId, @FromDate, @ToDate, @VendorId, @CustomerId, @ProductId";
            
            var result = await _context.Database.GetDbConnection().QueryAsync<SpPurchaseSaleCalcBook>(sql, parameters, commandType: CommandType.StoredProcedure, commandTimeout: 500);
            return result.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<Sp6p6>> GetSpVds(int purchaseId)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@PurchaseId", purchaseId);
            string sql = "EXEC spvdspurchasecertificate @PurchaseId";

            var result = await _context.Database.GetDbConnection().QueryAsync<Sp6p6>(sql, parameters, commandType: CommandType.StoredProcedure, commandTimeout: 500);
            return result.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<SpCreditNoteMushak>> GetSpCreditNote(int creditNoteId)
    {
        var parameter = new DynamicParameters();
        parameter.Add("@CreditNoteID", creditNoteId);
        try
        {
            return (await _context.Database.GetDbConnection()
                .QueryAsync<SpCreditNoteMushak>("SpCreditNoteMushak @CreditNoteID",
                    parameter,
                    commandTimeout: 500)).ToList();
        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message);
        }
    }

    public async Task<List<SpDebiNotetMushak>> GetSpDebitNote(int DebitNoteID)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@DebitNoteId", DebitNoteID);
            string sql = "EXEC SpDebiNotetMushak @DebitNoteId";

            var result = await _context.Database.GetDbConnection().QueryAsync<SpDebiNotetMushak>(sql, parameters, commandType: CommandType.StoredProcedure, commandTimeout: 500);
            return result.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<SpSalesTaxInvoice>> GetChalan(int salesId)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@SalesId", salesId);
            string sql = "EXEC SpSalesTaxInvoice @SalesId";

            var result = await _context.Database.GetDbConnection().QueryAsync<SpSalesTaxInvoice>(sql, parameters, commandType: CommandType.StoredProcedure, commandTimeout: 500);
            return result.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<Sp6p4>> Get6P4Raw(int id)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ContractVendorTransferRawMateriallId", id);
            string sql = "EXEC SpContractualrawMetaria @ContractVendorTransferRawMateriallId";

            var result = await _context.Database.GetDbConnection().QueryAsync<Sp6p4>(sql, parameters, commandType: CommandType.StoredProcedure, commandTimeout: 500);
            return result.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
   
    public async Task<List<Sp6p4>> Get6P4Finish(int id)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ContractVendorTransferRawMateriallId", id);
            string sql = "EXEC SpContractualFinishedGood @ContractVendorTransferRawMateriallId";

            var result = await _context.Database.GetDbConnection().QueryAsync<Sp6p4>(sql, parameters, commandType: CommandType.StoredProcedure, commandTimeout: 500);
            return result.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<Sp6p5>> Get6P5(int id)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@SaleId", id);
            string sql = "EXEC spBranchTransfer @SaleId";

            var result = await _context.Database.GetDbConnection().QueryAsync<Sp6p5>(sql, parameters, commandType: CommandType.StoredProcedure, commandTimeout: 500);
            return result.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<Sp4p3>> Get4P3(int id)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@PricedecID", id);
            string sql = "EXEC SpPriceDeclarationMushak @PricedecID";

            var result = await _context.Database.GetDbConnection().QueryAsync<Sp4p3>(sql, parameters, commandType: CommandType.StoredProcedure, commandTimeout: 500);
            return result.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}