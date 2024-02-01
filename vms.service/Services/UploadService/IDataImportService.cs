using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.viewModels;
using vms.entity.viewModels.ProductViewModel;

namespace vms.service.Services.UploadService;

public interface IDataImportService
{
    Task<List<ProductDataImportViewModel>> LoadProduct(string location);

    Task<bool> InsertBulk(List<ProductDataImportViewModel> products, int organizationId, int createBy);

    Task<List<ProductionDataImportViewModel>> LoadProduction(string location);

    Task<bool> InsertBulkProduction(List<ProductionDataImportViewModel> products, int organizationId, int createBy);

    Task<DataimportPurchaseFinal> LoadPurchase(string location);

    Task<vmPurchaseBulkPost> LoadPurchaseFromExcel(string location);
    Task<vmSaleBulkPost> LoadSaleFromExcel(string location);

    Task<bool> InsertBulkpurchase(DataimportPurchaseFinal purchase, int orgID, int Uid, string security);

    Task<DataimportSalesFinal> LoadSales(string location);

    Task<bool> InsertBulkSales(DataimportSalesFinal sales, int orgID, int Uid, string security);

    Task<vmStaticData> LoadStaticDataFromExcel(string location);

}