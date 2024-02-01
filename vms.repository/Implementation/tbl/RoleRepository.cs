using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class RoleRepository : RepositoryBase<Role>, IRoleRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;

    public RoleRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<Role>> GetRoles(int orgIdEnc)
    {
        var roles = await Query().Where(w => w.OrganizationId == orgIdEnc).SelectAsync();
            
        roles.ToList().ForEach(delegate (Role role)
        {
            role.EncryptedId = _dataProtector.Protect(role.RoleId.ToString());
        });
        return roles;
    }
    public async Task<Role> GetRole(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var role = await Query()               
            .SingleOrDefaultAsync(x => x.RoleId == id, System.Threading.CancellationToken.None);
        role.EncryptedId = _dataProtector.Protect(role.RoleId.ToString());

        return role;
    }

	public async Task<Role> GetRoleByName(int organizationId, string roleName)
	{
		var role = await Query()
			.SingleOrDefaultAsync(x => x.OrganizationId == organizationId && x.RoleName == roleName, System.Threading.CancellationToken.None);
		return role;
	}

}