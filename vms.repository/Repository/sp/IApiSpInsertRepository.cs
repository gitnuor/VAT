using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.viewModels;

namespace vms.repository.Repository.sp;

public interface IApiSpInsertRepository
{
    Task<bool> InsertSale(vmSaleOrder saleOrder);

    Task<bool> InsertSaleExcel(DataimportSalesFinal sale, int orgID, int Uid, string security);

    Task<bool> InsertPurchaseExcel(DataimportPurchaseFinal purchase, int orgID, int Uid, string security);

    Task<bool> InsertPurchase(VmPurchase purchase);

    Task<bool> InsertPurchase(vmPurchasePost purchase);

    Task<bool> InsertBulkPurchase(vmPurchaseBulkPost purchase);
    Task<bool> InsertBulkSale(vmSaleBulkPost sale);

    Task<bool> InsertProductionExcel(List<ProductionDataImportViewModel> purchase, int orgID, int Uid, string security);

    Task<bool> InsertSale(vmSalesPost sales);
}