using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.StoredProcedureModel.HighValue;
using vms.entity.StoredProcedureModel.HTMLMushak;
using vms.entity.viewModels;
using vms.entity.viewModels.ReportsViewModel;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class MushakGenerationRepository : RepositoryBase<MushakGeneration>, IMushakGenerationRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;

    public MushakGenerationRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }


    public async Task<IEnumerable<MushakGeneration>> GetMushakGenerations(int orgIdEnc)
    {
        var mushakGenerations = await Query().Where(w => w.OrganizationId == orgIdEnc).SelectAsync();

        mushakGenerations.ToList().ForEach(delegate (MushakGeneration mushakGeneration)
        {
            mushakGeneration.EncryptedId = _dataProtector.Protect(mushakGeneration.MushakGenerationId.ToString());
        });
        return mushakGenerations;
    }
    public async Task<MushakGeneration> GetMushakGeneration(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var mushakGeneration = await Query()
            .SingleOrDefaultAsync(x => x.MushakGenerationId == id, System.Threading.CancellationToken.None);
        mushakGeneration.EncryptedId = _dataProtector.Protect(mushakGeneration.MushakGenerationId.ToString());

        return mushakGeneration;
    }






    public async Task<bool> InsertMushak(SpAddMushakReturnBasicInfo vm)
    { 
        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                $"EXEC [dbo].[SpAddMushakReturnBasicInfo]" +
                $"@OrganizationId," +
                $"@Year," +
                $"@Month," +
                $"@GenerateDate," +
                $"@InterestForDueVat," +
                $"@InterestForDueSd,"+
                $"@FinancialPenalty," +
                $"@ExciseDuty," +
                $"@DevelopmentSurcharge," +
                $"@ItDevelopmentSurcharge," +
                $"@HealthDevelopmentSurcharge," +
                $"@EnvironmentProtectSurcharge," +
                $"@MiscIncrementalAdjustmentAmount," +
                $"@MiscIncrementalAdjustmentDesc," +
                $"@MiscDecrementalAdjustmentAmount," +
                $"@MiscDecrementalAdjustmentDesc," +
                $"@IsWantToGetBackClosingAmount"
                , new SqlParameter("@OrganizationId", (object)vm.OrganizationId ?? DBNull.Value)
                , new SqlParameter("@Year", (object)vm.Year ?? DBNull.Value)
                , new SqlParameter("@Month", (object)vm.Month ?? DBNull.Value)
                , new SqlParameter("@GenerateDate", (object)vm.GenerateDate ?? DBNull.Value)
                , new SqlParameter("@InterestForDueVat", (object)vm.InterestForDueVat ?? DBNull.Value)
                , new SqlParameter("@InterestForDueSd", (object)vm.InterestForDueSd ?? DBNull.Value)
                , new SqlParameter("@FinancialPenalty", (object)vm.FinancialPenalty ?? DBNull.Value)
                , new SqlParameter("@ExciseDuty", (object)vm.ExciseDuty ?? DBNull.Value)
                , new SqlParameter("@DevelopmentSurcharge", (object)vm.DevelopmentSurcharge ?? DBNull.Value)
                , new SqlParameter("@ItDevelopmentSurcharge", (object)vm.ItDevelopmentSurcharge ?? DBNull.Value)
                , new SqlParameter("@HealthDevelopmentSurcharge", (object)vm.HealthDevelopmentSurcharge ?? DBNull.Value)
                , new SqlParameter("@EnvironmentProtectSurcharge", (object)vm.EnvironmentProtectSurcharge ?? DBNull.Value)
                , new SqlParameter("@MiscIncrementalAdjustmentAmount", (object)vm.MiscIncrementalAdjustmentAmount ?? DBNull.Value)
                , new SqlParameter("@MiscIncrementalAdjustmentDesc", (object)vm.MiscIncrementalAdjustmentDesc ?? DBNull.Value)
                , new SqlParameter("@MiscDecrementalAdjustmentAmount", (object)vm.MiscDecrementalAdjustmentAmount ?? DBNull.Value)
                , new SqlParameter("@MiscDecrementalAdjustmentDesc", (object)vm.MiscDecrementalAdjustmentDesc ?? DBNull.Value)
                , new SqlParameter("@IsWantToGetBackClosingAmount", (object)vm.IsWantToGetBackClosingAmount ?? DBNull.Value)
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return await Task.FromResult(true);
    }

    public async Task<bool> InsertReturnPaymentInfo(SpAddMushakReturnPaymentInfo model)
    {
        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                $"EXEC [dbo].[SpAddMushakReturnPaymentInfo]" +
                $"@MushakGenerationId," +
                $"@VatPaymentChallanNo," +
                $"@SuppDutyChallanNo," +
                $"@InterestForDueVatChallanNo," +
                $"@InterestForDueSuppDutyChallanNo," +
                $"@FinancialPenaltyChallanNo," +
                $"@ExciseDutyChallanNo," +
                $"@DevelopmentSurchargeChallanNo," +
                $"@ItDevelopmentSurchargeChallanNo," +
                $"@HealthDevelopmentSurchargeChallanNo," +
                $"@EnvironmentProtectSurchargeChallanNo" 
                , new SqlParameter("@MushakGenerationId", (object)model.MushakGenerationId ?? DBNull.Value)
                , new SqlParameter("@VatPaymentChallanNo", (object)model.VatPaymentChallanNo ?? DBNull.Value)
                , new SqlParameter("@SuppDutyChallanNo", (object)model.SuppDutyChallanNo ?? DBNull.Value)
                , new SqlParameter("@InterestForDueVatChallanNo", (object)model.InterestForDueVatChallanNo ?? DBNull.Value)
                , new SqlParameter("@InterestForDueSuppDutyChallanNo", (object)model.InterestForDueSuppDutyChallanNo ?? DBNull.Value)
                , new SqlParameter("@FinancialPenaltyChallanNo", (object)model.FinancialPenaltyChallanNo ?? DBNull.Value)
                , new SqlParameter("@ExciseDutyChallanNo", (object)model.ExciseDutyChallanNo ?? DBNull.Value)
                , new SqlParameter("@DevelopmentSurchargeChallanNo", (object)model.DevelopmentSurchargeChallanNo ?? DBNull.Value)
                , new SqlParameter("@ItDevelopmentSurchargeChallanNo", (object)model.ItDevelopmentSurchargeChallanNo ?? DBNull.Value)
                , new SqlParameter("@HealthDevelopmentSurchargeChallanNo", (object)model.HealthDevelopmentSurchargeChallanNo ?? DBNull.Value)
                , new SqlParameter("@EnvironmentProtectSurchargeChallanNo", (object)model.EnvironmentProtectSurchargeChallanNo ?? DBNull.Value)
                   
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return await Task.FromResult(true);
    }

    public async Task<bool> InsertReturnPlanToPaymentInfo(SpAddMushakReturnPlanToPaymentInfo model)
    {
        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                $"EXEC [dbo].[SpAddMushakReturnPlanToPaymentInfo]" +
                $"@MushakGenerationId," +
                $"@PaidVatAmount," +
                $"@VatEconomicCodeId," +
                $"@VatPaymentDate," +
                $"@VatPaymentBankBranchId," +
                $"@PaidSuppDutyAmount," +
                $"@SuppDutyEconomicCodeId," +
                $"@SuppDutyPaymentDate," +
                $"@SuppDutyBankBranchId," +
                $"@PaidInterestAmountForDueVat," +
                $"@InterestForDueVatEconomicCodeId," +
                $"@InterestForDueVatPaymentDate," +
                $"@InterestForDueVatBankBranchId," +
                $"@PaidInterestAmountForDueSuppDuty," +
                $"@InterestForDueSuppDutyEconomicCodeId," +
                $"@InterestForDueSuppDutyPaymentDate," +
                $"@InterestForDueSuppDutyBankBranchId," +
                $"@PaidFinancialPenalty," +
                $"@FinancialPenaltyEconomicCodeId," +
                $"@FinancialPenaltyPaymentDate," +
                $"@FinancialPenaltyBankBranchId," +
                $"@PaidExciseDuty," +
                $"@ExciseDutyEconomicCodeId," +
                $"@ExciseDutyPaymentDate," +
                $"@ExciseDutyBankBranchId," +
                $"@PaidDevelopmentSurcharge," +
                $"@DevelopmentSurchargeEconomicCodeId," +
                $"@DevelopmentSurchargePaymentDate," +
                $"@DevelopmentSurchargeBankBranchId," +
                $"@PaidItDevelopmentSurcharge," +
                $"@ItDevelopmentSurchargeEconomicCodeId," +
                $"@ItDevelopmentSurchargePaymentDate," +
                $"@ItDevelopmentSurchargeBankBranchId," +
                $"@PaidHealthDevelopmentSurcharge," +
                $"@HealthDevelopmentSurchargeEconomicCodeId," +
                $"@HealthDevelopmentSurchargePaymentDate," +
                $"@HealthDevelopmentSurchargeBankBranchId," +
                $"@PaidEnvironmentProtectSurcharge," +
                $"@EnvironmentProtectSurchargeEconomicCodeId," +
                $"@EnvironmentProtectSurchargePaymentDate," +
                $"@EnvironmentProtectSurchargeBankBranchId"
                , new SqlParameter("@MushakGenerationId", (object)model.MushakGenerationId ?? DBNull.Value)
                , new SqlParameter("@PaidVatAmount", (object)model.PaidVatAmount ?? DBNull.Value)
                , new SqlParameter("@VatEconomicCodeId", (object)model.VatEconomicCodeId ?? DBNull.Value)
                , new SqlParameter("@VatPaymentDate", (object)model.VatPaymentDate ?? DBNull.Value)
                , new SqlParameter("@VatPaymentBankBranchId", (object)model.VatPaymentBankBranchId ?? DBNull.Value)
                , new SqlParameter("@PaidSuppDutyAmount", (object)model.PaidSuppDutyAmount ?? DBNull.Value)
                , new SqlParameter("@SuppDutyEconomicCodeId", (object)model.SuppDutyEconomicCodeId ?? DBNull.Value)
                , new SqlParameter("@SuppDutyPaymentDate", (object)model.SuppDutyPaymentDate ?? DBNull.Value)
                , new SqlParameter("@SuppDutyBankBranchId", (object)model.SuppDutyBankBranchId ?? DBNull.Value)
                , new SqlParameter("@PaidInterestAmountForDueVat", (object)model.PaidInterestAmountForDueVat ?? DBNull.Value)
                , new SqlParameter("@InterestForDueVatEconomicCodeId", (object)model.InterestForDueVatEconomicCodeId ?? DBNull.Value)
                , new SqlParameter("@InterestForDueVatPaymentDate", (object)model.InterestForDueVatPaymentDate ?? DBNull.Value)
                , new SqlParameter("@InterestForDueVatBankBranchId", (object)model.InterestForDueVatBankBranchId ?? DBNull.Value)

                , new SqlParameter("@PaidInterestAmountForDueSuppDuty", (object)model.PaidInterestAmountForDueSuppDuty ?? DBNull.Value)
                , new SqlParameter("@InterestForDueSuppDutyEconomicCodeId", (object)model.InterestForDueSuppDutyEconomicCodeId ?? DBNull.Value)
                , new SqlParameter("@InterestForDueSuppDutyPaymentDate", (object)model.InterestForDueSuppDutyPaymentDate ?? DBNull.Value)
                , new SqlParameter("@InterestForDueSuppDutyBankBranchId", (object)model.InterestForDueSuppDutyBankBranchId ?? DBNull.Value)
                , new SqlParameter("@PaidFinancialPenalty", (object)model.PaidFinancialPenalty ?? DBNull.Value)
                , new SqlParameter("@FinancialPenaltyEconomicCodeId", (object)model.FinancialPenaltyEconomicCodeId ?? DBNull.Value)
                , new SqlParameter("@FinancialPenaltyPaymentDate", (object)model.FinancialPenaltyPaymentDate ?? DBNull.Value)
                , new SqlParameter("@FinancialPenaltyBankBranchId", (object)model.FinancialPenaltyBankBranchId ?? DBNull.Value)

                , new SqlParameter("@PaidExciseDuty", (object)model.PaidExciseDuty ?? DBNull.Value)
                , new SqlParameter("@ExciseDutyEconomicCodeId", (object)model.ExciseDutyEconomicCodeId ?? DBNull.Value)
                , new SqlParameter("@ExciseDutyPaymentDate", (object)model.ExciseDutyPaymentDate ?? DBNull.Value)
                , new SqlParameter("@ExciseDutyBankBranchId", (object)model.ExciseDutyBankBranchId ?? DBNull.Value)
                , new SqlParameter("@PaidDevelopmentSurcharge", (object)model.PaidDevelopmentSurcharge ?? DBNull.Value)

                , new SqlParameter("@DevelopmentSurchargeEconomicCodeId", (object)model.DevelopmentSurchargeEconomicCodeId ?? DBNull.Value)
                , new SqlParameter("@DevelopmentSurchargePaymentDate", (object)model.DevelopmentSurchargePaymentDate ?? DBNull.Value)
                , new SqlParameter("@DevelopmentSurchargeBankBranchId", (object)model.DevelopmentSurchargeBankBranchId ?? DBNull.Value)
                , new SqlParameter("@PaidItDevelopmentSurcharge", (object)model.PaidItDevelopmentSurcharge ?? DBNull.Value)
                , new SqlParameter("@ItDevelopmentSurchargeEconomicCodeId", (object)model.ItDevelopmentSurchargeEconomicCodeId ?? DBNull.Value)
                , new SqlParameter("@ItDevelopmentSurchargePaymentDate", (object)model.ItDevelopmentSurchargePaymentDate ?? DBNull.Value)
                , new SqlParameter("@ItDevelopmentSurchargeBankBranchId", (object)model.ItDevelopmentSurchargeBankBranchId ?? DBNull.Value)

                , new SqlParameter("@PaidHealthDevelopmentSurcharge", (object)model.PaidHealthDevelopmentSurcharge ?? DBNull.Value)
                , new SqlParameter("@HealthDevelopmentSurchargeEconomicCodeId", (object)model.HealthDevelopmentSurchargeEconomicCodeId ?? DBNull.Value)
                , new SqlParameter("@HealthDevelopmentSurchargePaymentDate", (object)model.HealthDevelopmentSurchargePaymentDate ?? DBNull.Value)
                , new SqlParameter("@HealthDevelopmentSurchargeBankBranchId", (object)model.HealthDevelopmentSurchargeBankBranchId ?? DBNull.Value)


                , new SqlParameter("@PaidEnvironmentProtectSurcharge", (object)model.PaidEnvironmentProtectSurcharge ?? DBNull.Value)
                , new SqlParameter("@EnvironmentProtectSurchargeEconomicCodeId", (object)model.EnvironmentProtectSurchargeEconomicCodeId ?? DBNull.Value)
                , new SqlParameter("@EnvironmentProtectSurchargePaymentDate", (object)model.EnvironmentProtectSurchargePaymentDate ?? DBNull.Value)
                , new SqlParameter("@EnvironmentProtectSurchargeBankBranchId", (object)model.EnvironmentProtectSurchargeBankBranchId ?? DBNull.Value)
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return await Task.FromResult(true);
    }

    public async Task<bool> InsertReturnReturnedAmountInfo(SpAddMushakReturnReturnedAmountInfo vm)
    {
        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                $"EXEC [dbo].[SpAddMushakReturnReturnedAmountInfo]" +
                $"@MushakGenerationId," +
                $"@ReturnAmountFromClosingVat,"+
                $"@ReturnFromClosingVatChequeBankId," +
                $"@ReturnFromClosingVatChequeNo," +
                $"@ReturnFromClosingVatChequeDate," +
                $"@ReturnAmountFromClosingSd," +
                $"@ReturnFromClosingSdChequeBankId," +
                $"@ReturnFromClosingSdChequeNo,"+
                $"@ReturnFromClosingSdChequeDate"
                , new SqlParameter("@MushakGenerationId", (object)vm.MushakGenerationId ?? DBNull.Value)
                , new SqlParameter("@ReturnAmountFromClosingVat", (object)vm.ReturnAmountFromClosingVat ?? DBNull.Value)
                , new SqlParameter("@ReturnFromClosingVatChequeBankId", (object)vm.ReturnFromClosingVatChequeBankId ?? DBNull.Value)
                , new SqlParameter("@ReturnFromClosingVatChequeNo", (object)vm.ReturnFromClosingVatChequeNo ?? DBNull.Value)
                , new SqlParameter("@ReturnFromClosingVatChequeDate", (object)vm.ReturnFromClosingVatChequeDate ?? DBNull.Value)
                , new SqlParameter("@ReturnAmountFromClosingSd", (object)vm.ReturnAmountFromClosingSd ?? DBNull.Value)
                , new SqlParameter("@ReturnFromClosingSdChequeBankId", (object)vm.ReturnFromClosingSdChequeBankId ?? DBNull.Value)
                , new SqlParameter("@ReturnFromClosingSdChequeNo", (object)vm.ReturnFromClosingSdChequeNo ?? DBNull.Value)
                , new SqlParameter("@ReturnFromClosingSdChequeDate", (object)vm.ReturnFromClosingSdChequeDate ?? DBNull.Value)
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return await Task.FromResult(true);
    }

    public async Task<bool> InsertSubmissionInfo(SpAddMushakReturnSubmissionInfo vm)
    {
        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                $"EXEC [dbo].[SpAddMushakReturnSubmissionInfo]" +
                $"@MushakGenerationId," +
                $"@SubmissionDate" 
                , new SqlParameter("@MushakGenerationId", (object)vm.MushakGenerationId ?? DBNull.Value)
                , new SqlParameter("@SubmissionDate", (object)vm.SubmissionDate ?? DBNull.Value)
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return await Task.FromResult(true);
    }


    public async Task<bool> InsertAddMushakReturnCompleteProcess(int id)
    {
        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                $"EXEC [dbo].[SpAddMushakReturnCompleteProcess]" +
                $"@MushakGenerationId"
                   
                , new SqlParameter("@MushakGenerationId", (object)id ?? DBNull.Value)
                    
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return await Task.FromResult(true);
    }

    public async Task<List<Sp4p3>> Mushak4P3(int PriceDeclarId)
    {

        var parameter = new DynamicParameters();
        parameter.Add("@PricedecID", PriceDeclarId);
        var result = await _context.Database.GetDbConnection().QueryAsync<Sp4p3>("SpPriceDeclarationMushak @PricedecID", parameter, commandTimeout: 500);

        return result.ToList();
    }

    public async Task<List<SpPurchaseCalcBook>> Mushak6P1(int productId, int orgId, int orgBranchId, string fromDate, string toDate)
    {
            
        var parameter = new DynamicParameters();
        parameter.Add("@OrganizationId", orgId);
        parameter.Add("@FromDate", fromDate);
        parameter.Add("@ToDate", toDate);
        parameter.Add("@BranchId", orgBranchId);
        parameter.Add("@ProductId", productId);
        var result = await _context.Database.GetDbConnection().QueryAsync<SpPurchaseCalcBook>("SpPurchaseCalcBook @OrganizationId, @FromDate, @ToDate,@BranchId,@ProductId", parameter, commandTimeout: 500);
                        
        return result.ToList();
    }

    public async Task<List<SpPurchaseSaleCalcBook>> Mushak6P2P1(int productId, int orgId, int branchId, int vendorId, int customerId, string fromDate, string toDate)
    {
            
        var parameter = new DynamicParameters();
        parameter.Add("@OrganizationId", orgId);
        parameter.Add("@BranchId", branchId);
        parameter.Add("@FromDate", fromDate);
        parameter.Add("@ToDate", toDate);
        parameter.Add("@VendorId", vendorId);
        parameter.Add("@CustomerId", customerId);
        parameter.Add("@ProductId", productId);
        var result = await _context.Database.GetDbConnection().QueryAsync<SpPurchaseSaleCalcBook>("SpPurchaseSaleCalcBook @OrganizationId, @BranchId, @FromDate, @ToDate,@VendorId,@CustomerId,@ProductId", parameter, commandTimeout: 500);

        return result.ToList();
    }

    public async Task<List<SpSalesCalcBook>> Mushak6P2(int productId, int orgId, int orgBranchId, string fromDate, string toDate)
    {
            
        var parameter = new DynamicParameters();
        parameter.Add("@OrganizationId", orgId);
        parameter.Add("@FromDate", fromDate);
        parameter.Add("@ToDate", toDate);
        parameter.Add("@BranchId", orgBranchId);
        parameter.Add("@ProductId", productId);
        var result = await _context.Database.GetDbConnection().QueryAsync<SpSalesCalcBook>("SpSalesCalcBook @OrganizationId, @FromDate, @ToDate,@BranchId,@ProductId", parameter, commandTimeout: 500);

        return result.ToList();
    }

    public async Task<VmMushakHighValue> Mushak6P10(int organizationId, int year, int month, decimal? lowerLimitOfHighValSale)
    {
        var model = new VmMushakHighValue();
        var parameter = new DynamicParameters();
        parameter.Add("@OrganizationId", organizationId);
        parameter.Add("@Year", year);
        parameter.Add("@Month", month);
        parameter.Add("@LowerLimitOfHighValSale", lowerLimitOfHighValSale);
     
        var result = await _context.Database.GetDbConnection().QueryMultipleAsync("SpPurcSalesChallanForHighValue @OrganizationId, @Year, @Month,@LowerLimitOfHighValSale", parameter, commandTimeout: 500);

        model.SpPurcSalesChallanForHighValuePurchaseList = result.Read<SpPurcSalesChallanForHighValuePurchase>().ToList();
        model.SpPurcSalesChallanForHighValueSaleList = result.Read<SpPurcSalesChallanForHighValueSale>().ToList();

        return model;
    }

    public async Task<List<SpDebitMushak>> Mushak6P8(int DebitNoteID)
    {
        var parameter = new DynamicParameters();
        parameter.Add("@DebitNoteID", DebitNoteID);
        var result = await _context.Database.GetDbConnection().QueryAsync<SpDebitMushak>("SpDebitMushak @DebitNoteID", parameter, commandTimeout: 500);

        return result.ToList();
    }

    public async Task<List<SpSalesTaxInvoice>> Mushak6P3(int SalesId)
    {
        var parameter = new DynamicParameters();
        parameter.Add("@SalesId", SalesId);
        var result = await _context.Database.GetDbConnection().QueryAsync<SpSalesTaxInvoice>("SpSalesTaxInvoice @SalesId", parameter, commandTimeout: 500);

        return result.ToList();
    }

    public async Task<List<SpCreditMushak>> Mushak6P7(int CreditNoteID)
    {
        var parameter = new DynamicParameters();
        parameter.Add("@CreditNoteID", CreditNoteID);
        var result = await _context.Database.GetDbConnection().QueryAsync<SpCreditMushak>("SpCreditMushak @CreditNoteID", parameter, commandTimeout: 500);

        return result.ToList();
    }

    public async Task<List<SpVdsPurchaseCertificate>> Mushak6P6(int PurchaseId)
    {
        var parameter = new DynamicParameters();
        parameter.Add("@PurchaseId", PurchaseId);
        var result = await _context.Database.GetDbConnection().QueryAsync<SpVdsPurchaseCertificate>("spvdspurchasecertificate @PurchaseId", parameter, commandTimeout: 500);

        return result.ToList();
    }

    public async Task<List<SpBranchTransfer>> Mushak6P5(int SaleId)
    {
        var parameter = new DynamicParameters();
        parameter.Add("@SaleId", SaleId);
        var result = await _context.Database.GetDbConnection().QueryAsync<SpBranchTransfer>("spBranchTransfer @SaleId", parameter, commandTimeout: 500);

        return result.ToList();
    }

    public async Task<List<SpGetTdsPurchaseCertificate>> GetTdsCertificate(int purchaseId)
    {
        var parameter = new DynamicParameters();
        parameter.Add("@PurchaseId", purchaseId);
        var result = await _context.Database.GetDbConnection().QueryAsync<SpGetTdsPurchaseCertificate>("SpGetTdsPurchaseCertificate @PurchaseId", parameter, commandTimeout: 500);

        return result.ToList();
    }
}