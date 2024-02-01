using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class ProductOpeningBalanceRepository : RepositoryBase<ProductOpeningBalance>, IProductOpeningBalanceRepository
{
    private readonly DbContext _context;

    public ProductOpeningBalanceRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}