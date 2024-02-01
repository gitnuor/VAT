using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;
    public UserRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<User>> GetUsers(int orgIdEnc)
    {
        return await Query()
	        .Where(w => w.OrganizationId == orgIdEnc)
	        .Include(p => p.Role)
            .Include(p => p.UserType)
            .SelectAsync();
    }
    public async Task<User> GetUser(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var user = await Query()
            .Include(p => p.Role)
            .Include(p => p.UserType)
            .SingleOrDefaultAsync(x => x.UserId == id, System.Threading.CancellationToken.None);
        user.EncryptedId = _dataProtector.Protect(user.UserId.ToString());

        return user;
    }

    public async Task<User> GetUserByUsername(string userName)
    {
        if (string.IsNullOrEmpty(userName))
        {
            throw new Exception("Empty username is not allowed!");
        }

        userName = userName.Trim().ToLower();
        var user = await Query()
            .Include(u => u.Organization)
            .Include(u => u.UserType)
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.UserName.ToLower() == userName);
        if (user != null)
        {
            user.EncryptedId = _dataProtector.Protect(user.UserId.ToString());
        }
            
        return user;
    }

    public async Task<User> GetUserByUsernameAndEncPass(string userName, string encPassword)
    {
        if (string.IsNullOrEmpty(userName))
        {
            throw new Exception("Empty username is not allowed!");
        }

        userName = userName.Trim().ToLower();
        var user = await Query()
            .Include(u => u.Organization)
            .Include(u => u.UserType)
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.UserName.ToLower() == userName && u.Password == encPassword);
        if (user != null)
        {
            user.EncryptedId = _dataProtector.Protect(user.UserId.ToString());
        }
        return user;
    }

    public async Task<IEnumerable<User>> GetAllByOrganization(string encOrganizationId)
    {
        var organizationId = int.Parse(_dataProtector.Unprotect(encOrganizationId));
        var usersMain = await Query().Where(u => u.OrganizationId == organizationId).Include(u => u.Role).SelectAsync();
        var userList = usersMain.ToList();
        userList.ForEach(delegate(User user)
        {
            user.EncryptedId = _dataProtector.Protect(user.UserId.ToString());
        });
        return userList;
    }

    public async Task<IEnumerable<ViewUser>> GetAllUserByOrganization(string encOrganizationId)
    {
		var organizationId = int.Parse(_dataProtector.Unprotect(encOrganizationId));
		var users = await _context.Set<ViewUser>()
            .Where(s => s.OrganizationId == organizationId)
            .AsNoTracking()
            .ToListAsync();
        return users;
    }

    public Task<List<ViewUser>> GetAllUserByOrganization(int orgId)
    {
	    return _context.Set<ViewUser>()
		    .Where(s => s.OrganizationId == orgId)
		    .AsNoTracking()
		    .ToListAsync();
	}

    public Task<ViewUser> GetUserByReference(string referenceId, int orgId)
    {
        return _context.Set<ViewUser>()
            .FirstOrDefaultAsync(x => x.ReferenceKey == referenceId && x.OrganizationId == orgId);
    }
}