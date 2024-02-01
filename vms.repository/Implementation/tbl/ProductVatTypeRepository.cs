using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class ProductVatTypeRepository : RepositoryBase<ProductVattype>, IProductVatTypeRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;

    public ProductVatTypeRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<ProductVattype>> GetProductVattypes()
    {
        var productVatTypes = await Query()
            //                .Include(p => p.PurchaseType)
            //                .Include(p => p.SalesType)
            //                .Include(p => p.TransactionType)
            .SelectAsync();

        productVatTypes.ToList().ForEach(delegate (ProductVattype productVatType)
        {
            productVatType.EncryptedId = _dataProtector.Protect(productVatType.ProductVattypeId.ToString());
        });
        return productVatTypes;
    }

    public async Task<ProductVattype> GetProductVattype(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var productVatType = await Query()
            //                .Include(p => p.PurchaseType)
            //                .Include(p => p.SalesType)
            //                .Include(p => p.TransactionType)
            .SingleOrDefaultAsync(x => x.ProductVattypeId == id, System.Threading.CancellationToken.None);
        productVatType.EncryptedId = _dataProtector.Protect(productVatType.ProductVattypeId.ToString());

        return productVatType;
    }
}