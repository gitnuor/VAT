using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class ProductCategoryRepository : RepositoryBase<ProductCategory>, IProductCategoryRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;

    public ProductCategoryRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<ProductCategory>> GetPCategories(int orgIdEnc)
    {
        var pCategories = await Query().Where(w => w.OrganizationId == orgIdEnc).SelectAsync();

        pCategories.ToList().ForEach(delegate (ProductCategory pCategory)
        {
            pCategory.EncryptedId = _dataProtector.Protect(pCategory.ProductCategoryId.ToString());
        });
        return pCategories;
    }
    public async Task<ProductCategory> GetPCat(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var pCategory = await Query()
            .SingleOrDefaultAsync(x => x.ProductCategoryId == id, System.Threading.CancellationToken.None);
        pCategory.EncryptedId = _dataProtector.Protect(pCategory.ProductCategoryId.ToString());

        return pCategory;

    }

	public async Task<ProductCategory> GetPCatByName(int organizationId, string categoryName)
	{
		var pCategory = await Query()
			.SingleOrDefaultAsync(x => x.OrganizationId == organizationId && x.Name == categoryName, System.Threading.CancellationToken.None);

		return pCategory;

	}

	public async Task<IEnumerable<ProductCategory>> GetPCategoriesByOrg(string orgIdEnc)
	{
		int id = int.Parse(_dataProtector.Unprotect(orgIdEnc));
		var pCategories = await Query().Where(w => w.OrganizationId == id).SelectAsync();

		pCategories.ToList().ForEach(delegate (ProductCategory pCategory)
		{
			pCategory.EncryptedId = _dataProtector.Protect(pCategory.ProductCategoryId.ToString());
		});
		return pCategories;
	}
}