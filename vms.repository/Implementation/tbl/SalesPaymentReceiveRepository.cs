using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class SalesPaymentReceiveRepository : RepositoryBase<SalesPaymentReceive>, ISalesPaymentReceiveRepository
{
    private readonly DbContext _context;

    public SalesPaymentReceiveRepository(DbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> ManageSalesDueAsync(VmSalesPaymentReceive vmSales)
    {
        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                $"EXEC [dbo].[SpInsertReceivedSalesPayment]" +
                $"@SalesId" +
                $",@ReceivedPaymentMethodId" +
                $",@BankId" +
                $",@WalletNo" +
                $",@BankAccountNo" +
                $",@DocumentNoOrTransId" +
                $",@DocumentOrTransDate" +
                $",@ReceiveAmount" +
                $",@ReceiveDate" +
                $",@ReferenceKey"+
                $",@PaymentRemarks" +
                $",@CreatedBy" +
                $",@CreatedTime " 

                , new SqlParameter("@SalesId", vmSales.SalesId)
                , new SqlParameter("@ReceivedPaymentMethodId", vmSales.PaymentMethodId)
                , new SqlParameter("@BankId", vmSales.BankId)
                , new SqlParameter("@WalletNo", (object)vmSales.MobilePaymentWalletNo ?? DBNull.Value)
                , new SqlParameter("@BankAccountNo", (object)vmSales.BankAccountNo ?? DBNull.Value)
                , new SqlParameter("@DocumentNoOrTransId", (object)vmSales.DocumentNoOrTransactionId ?? DBNull.Value)
                , new SqlParameter("@DocumentOrTransDate", (object)vmSales.PaymentDocumentOrTransDate ?? DBNull.Value)
                , new SqlParameter("@ReceiveAmount", vmSales.PaidAmount)
                , new SqlParameter("@ReceiveDate", vmSales.PaymentDate)
                , new SqlParameter("@ReferenceKey", (object)vmSales.ReferenceKey ?? DBNull.Value)
                , new SqlParameter("@PaymentRemarks", (object)vmSales.PaymentRemarks ?? DBNull.Value)
                , new SqlParameter("@CreatedBy", vmSales.CreatedBy)
                , new SqlParameter("@CreatedTime", vmSales.CreatedTime)
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