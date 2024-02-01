using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.StoredProcedureModel;
using vms.repository.Repository.sp;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class AutocompleteService : IAutocompleteService
{
    private readonly IAutocompleteRepository _autocompleteRepository;

    public AutocompleteService(IAutocompleteRepository autocompleteRepository)
    {
        _autocompleteRepository = autocompleteRepository;
    }

    public async Task<List<SpGetProductAutocompleteForPurchase>> GetProductAutocompleteForPurchases(int organizationId, string searchTerm)
    {
        return await _autocompleteRepository.GetProductAutocompleteForPurchases(organizationId, searchTerm);
    }
    public async Task<List<SpGetProductAutocompleteForProductionReceive>> ProductionReceiveAutoComplete(int organizationId, string searchTerm, int contructId = 0)
    {
        return await _autocompleteRepository.ProductionReceiveAutoComplete(organizationId, searchTerm, contructId);
    }

    public async Task<List<SpGetProductAutocompleteForSale>> GetProductAutocompleteForSales(int organizationId, string searchTerm)
    {
        return await _autocompleteRepository.GetProductAutocompleteForSales(organizationId, searchTerm);
    }
    public async Task<List<SpGetProductAutocompleteForBom>> ProductionReceiveAutoCompleteBOM(int organizationId, string searchTerm)
    {
        return await _autocompleteRepository.ProductionReceiveAutoCompleteBOM(organizationId, searchTerm);
    }

    public async Task<List<SpGetProductAutocompleteForPriceSetu>> ProductionReceiveAutoCompletePriceSetup(int organizationId,
        string searchTerm)
    {
        return await _autocompleteRepository.ProductionReceiveAutoCompletePriceSetup(organizationId, searchTerm);
    }

    public async Task<List<SpGetVatType>> GetProductVatType(bool IsLocalSale, bool IsExport, bool IsLocalPurchase, bool IsImport, bool IsVds)
    {
        return await _autocompleteRepository.GetProductVatType(IsLocalSale, IsExport, IsLocalPurchase, IsImport, IsVds);
    }

    public async Task<List<SpGetRawMaterialForProduction>> GetRawMaterialForProduction(int orgId, int branchId, int prodId)
    {
        return await _autocompleteRepository.GetRawMaterialForProduction(orgId, branchId, prodId);
    }
}