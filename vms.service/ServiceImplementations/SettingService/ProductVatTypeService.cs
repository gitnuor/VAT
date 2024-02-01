using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using vms.entity.Dto.Product;
using vms.entity.models;
using vms.entity.Utility;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class ProductVatTypeService : ServiceBase<ProductVattype>, IProductVatTypeService
{
    private readonly IProductVatTypeRepository _repository;
    private readonly IMapper _mapper;

    public ProductVatTypeService(IProductVatTypeRepository repository, IMapper mapper) : base(repository)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductVattype>> GetProductVattypes()
    {
        return await _repository.GetProductVattypes();
    }

    public async Task<IEnumerable<ProductVatTypeDto>> GetProductVatTypeListDto()
    {
        var vatTypes = await _repository.GetProductVattypes();
        return _mapper.Map<IEnumerable<ProductVattype>, IEnumerable<ProductVatTypeDto>>(vatTypes);
    }

    public async Task<IEnumerable<ProductVattype>> GetLocalPurchaseProductVatTypes()
    {
        var vatTypes = await _repository.GetProductVattypes();
        return vatTypes.Where(vt => vt.IsApplicableForLocalPurchase).ToList();
    }

    public async Task<IEnumerable<ProductVattype>> GetForeignPurchaseProductVatTypes()
    {
        var vatTypes = await _repository.GetProductVattypes();
        return vatTypes.Where(vt => vt.IsApplicableForImport).ToList();
    }

    public async Task<IEnumerable<ProductVattype>> GetLocalSaleProductVatTypes()
    {
        var vatTypes = await _repository.GetProductVattypes();
        return vatTypes.Where(vt => vt.IsApplicableForLocalSale).ToList();
    }

    public async Task<IEnumerable<ProductVattype>> GetForeignSaleProductVatTypes()
    {
        var vatTypes = await _repository.GetProductVattypes();
        return vatTypes.Where(vt => vt.IsApplicableForExport).ToList();
    }

    public async Task<ProductVattype> GetProductVattype(string idEnc)
    {
        return await _repository.GetProductVattype(idEnc);
    }

    public async Task<IEnumerable<CustomSelectListItem>> GetProductVattypesSelectList()
    {
        var vatTypes = await _repository.GetProductVattypes();
        return vatTypes.ConvertToCustomSelectList(nameof(ProductVattype.ProductVattypeId),
            nameof(ProductVattype.Name));
    }
}