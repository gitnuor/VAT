using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.Dto.Customer;
using vms.entity.Dto.Product;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.viewModels;

namespace vms.service.Services.ProductService;

public interface IProductService : IServiceBase<Product>
{
    vmProduct GetProductAutoComplete();

    Task<IEnumerable<Product>> GetProducts(string orgIdEnc);
    Task<Product> GetProduct(string idEnc);
    Task<IEnumerable<ViewProduct>> GetProductListByOrg(string orgIdEnc);
    Task<IEnumerable<ProductDto>> GetProductDtoListByOrg(string orgIdEnc);
    Task<IEnumerable<CustomSelectListItem>> GetProductsSelectList(string orgId);
    List<SpGetProductForSelfProductionReceive> SpGetProductForSelfProductionReceive(int orgId);

    List<SpGetProductForContractualProductionReceive> SpGetProductForContractualProductionReceive(int orgId,
        int conProId);

    Task<IEnumerable<Product>> GetProductforDamage(int orgId);
	Task InsertProduct(ProductPostDto product, string appKey);
}