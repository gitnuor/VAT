using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class OverHeadCostRepository : RepositoryBase<OverHeadCost>, IOverHeadCostRepository
{
    private readonly DbContext _context;

    public OverHeadCostRepository(DbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OverHeadCost>> GetOverHeadCost(int orgIdEnc)
    {
        var overHeadCost = await Query().Where(w => w.OrganizationId == orgIdEnc).SelectAsync();

        return overHeadCost;
    }
}