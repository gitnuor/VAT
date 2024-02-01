using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl
{
    public class UserBranchRepository : RepositoryBase<UserBranch>, IUserBranchRepository
    {
        private readonly DbContext _context;

        public UserBranchRepository(DbContext context) : base(context)
        {
            _context = context;
        }
    }
}
