using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class OrgBranchTypeRepository : RepositoryBase<OrgBranchType>, IOrgBranchTypeRepository
{
    private readonly DbContext _context;

    public OrgBranchTypeRepository(DbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OrgBranchType>> GetOrgBranchType()
    {
        return await Query()
            .SelectAsync();
    }
}