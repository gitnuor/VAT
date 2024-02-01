using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class ProductGroupRepository : RepositoryBase<ProductGroup>, IProductGroupRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;

    public ProductGroupRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<ProductGroup>> GetProductGroups()
    { 
        var productGroups = await Query().SelectAsync();

        productGroups.ToList().ForEach(delegate (ProductGroup productGroup)
        {
            productGroup.EncryptedId = _dataProtector.Protect(productGroup.ProductGroupId.ToString());
        });
        return productGroups;
    }

	public async Task<IEnumerable<ProductGroup>> GetProductGroupsByOrg(string pOrgid)
	{
		int id = int.Parse(_dataProtector.Unprotect(pOrgid));
		var productGroups = await Query().Where(x => x.OrganizationId == id).SelectAsync();

		productGroups.ToList().ForEach(delegate (ProductGroup productGroup)
		{
			productGroup.EncryptedId = _dataProtector.Protect(productGroup.ProductGroupId.ToString());
		});
		return productGroups;
	}
	public async Task<ProductGroup> GetProductGroup(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var productGroup = await Query()
            .SingleOrDefaultAsync(x => x.ProductGroupId == id, System.Threading.CancellationToken.None);
        productGroup.EncryptedId = _dataProtector.Protect(productGroup.ProductGroupId.ToString());

        return productGroup;
    }

	public async Task<ProductGroup> GetProductGroupByName(int organizationId, string groupName)
	{
		var productGroup = await Query()
			.SingleOrDefaultAsync(x => x.OrganizationId == organizationId && x.Name == groupName, System.Threading.CancellationToken.None);
		return productGroup;
	}

}