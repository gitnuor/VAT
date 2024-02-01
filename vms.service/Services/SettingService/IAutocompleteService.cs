using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.StoredProcedureModel;

namespace vms.service.Services.SettingService;

public interface IAutocompleteService
{
    Task<List<SpGetProductAutocompleteForPurchase>> GetProductAutocompleteForPurchases(int organizationId,
        string searchTerm);

    Task<List<SpGetProductAutocompleteForSale>> GetProductAutocompleteForSales(int organizationId,
        string searchTerm);

    Task<List<SpGetProductAutocompleteForProductionReceive>> ProductionReceiveAutoComplete(int organizationId,
        string searchTerm, int contructId = 0);
    Task<List<SpGetProductAutocompleteForBom>> ProductionReceiveAutoCompleteBOM(int organizationId,
        string searchTerm);
    Task<List<SpGetProductAutocompleteForPriceSetu>> ProductionReceiveAutoCompletePriceSetup(int organizationId,
        string searchTerm);
    Task<List<SpGetVatType>> GetProductVatType(bool IsLocalSale, bool IsExport, bool IsLocalPurchase, bool IsImport, bool IsVds);
    Task<List<SpGetRawMaterialForProduction>> GetRawMaterialForProduction(int orgId, int branchId, int prodId);


}