using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class PriceSetupProductCostRepository : RepositoryBase<PriceSetupProductCost>, IPriceSetupProductCostRepository
{
    private readonly DbContext _context;

    public PriceSetupProductCostRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}