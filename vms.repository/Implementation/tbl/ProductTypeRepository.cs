using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class ProductTypeRepository : RepositoryBase<ProductType>, IProductTypeRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;

    public ProductTypeRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<ProductType>> GetProductTypes()
    {
        var productTypes = await Query().SelectAsync();

        //pCategories.ToList().ForEach(delegate (ProductCategory pCategory)
        //{
        //    pCategory.EncryptedId = _dataProtector.Protect(pCategory.ProductCategoryId.ToString());
        //});
        return productTypes;
    }



}