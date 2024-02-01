using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class RoleRightRepository : RepositoryBase<RoleRight>, IRoleRightRepository
{
    private readonly DbContext _context;
    private readonly IDataProtector _dataProtector;

    public RoleRightRepository(DbContext context, IDataProtectionProvider protectionProvider, PurposeStringConstants purposeStringConstants) : base(context)
    {
        _context = context;
        _dataProtector = protectionProvider.CreateProtector(purposeStringConstants.UserIdQueryString);
    }

    public  void DeleteObjectList(List<RoleRight> list)
    {
        _context.RemoveRange(list);
    }
    public void InsertObjectList(List<RoleRight> list)
    {
        _context.AddRange(list);
    }

    public async Task<IEnumerable<RoleRight>> GetByRole(int roleId)
    {
        var roleRights = (await Query().Where(rr => rr.RoleId == roleId).Include(rr => rr.Right).SelectAsync()).ToList();
        roleRights.ForEach(delegate (RoleRight roleRight)
        {
            roleRight.EncryptedId = _dataProtector.Protect(roleRight.RoleRightId.ToString());
        });
        return roleRights;
    }
}