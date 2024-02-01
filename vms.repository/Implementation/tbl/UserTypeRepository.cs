using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class UserTypeRepository : RepositoryBase<UserType>, IUserTypeRepository
{
    private readonly DbContext _context;

    public UserTypeRepository(DbContext context) : base(context)
    {
        _context = context;
    }

}