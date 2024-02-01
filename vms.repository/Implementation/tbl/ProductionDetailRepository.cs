using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class ProductionDetailRepository : RepositoryBase<ProductionDetail>, IProductionDetailRepository
{
    private readonly DbContext _context;

    public ProductionDetailRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}