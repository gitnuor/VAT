using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class UserLoginHistoryRepository : RepositoryBase<UserLoginHistory>, IUserLoginHistoryRepository
{
    private readonly DbContext _context;

    public UserLoginHistoryRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}