using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class SaleOrderDetailRepository : RepositoryBase<SalesDetail>, ISaleOrderDetailsRepository
{
    private readonly DbContext _context;

    public SaleOrderDetailRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}