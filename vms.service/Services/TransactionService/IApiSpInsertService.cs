using System.Threading.Tasks;
using vms.entity.viewModels;

namespace vms.service.Services.TransactionService;

public interface IApiSpInsertService
{
    Task<bool> InsertSale(vmSaleOrder saleOrder);

    Task<bool> InsertSaleExcel(DataimportSalesFinal sale, int orgID, int Uid, string security);

    Task<bool> InsertPurchaseExcel(DataimportPurchaseFinal purchase, int orgID, int Uid, string security);

    Task<bool> InsertPurchase(VmPurchase purchase);

    Task<bool> InsertPurchase(vmPurchasePost purchase);

    Task<bool> InsertBulkPurchase(vmPurchaseBulkPost purchase);
    Task<bool> InsertBulkSale(vmSaleBulkPost sale);

    Task<bool> InsertSale(vmSalesPost sales);
}