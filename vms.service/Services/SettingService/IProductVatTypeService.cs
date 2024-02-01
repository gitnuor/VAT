using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.Dto.Product;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.SettingService;

public interface IProductVatTypeService : IServiceBase<ProductVattype>
{
    Task<IEnumerable<ProductVattype>> GetProductVattypes();
    Task<IEnumerable<ProductVatTypeDto>> GetProductVatTypeListDto();
    Task<IEnumerable<ProductVattype>> GetLocalPurchaseProductVatTypes();
    Task<IEnumerable<ProductVattype>> GetForeignPurchaseProductVatTypes();
    Task<IEnumerable<ProductVattype>> GetLocalSaleProductVatTypes();
    Task<IEnumerable<ProductVattype>> GetForeignSaleProductVatTypes();
    Task<ProductVattype> GetProductVattype(string idEnc);
    Task<IEnumerable<CustomSelectListItem>> GetProductVattypesSelectList();
}