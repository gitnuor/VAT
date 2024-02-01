using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class  SaleOrderRepository : RepositoryBase<SaleOrders>, ISaleOrderRepository
{
    private readonly DbContext _context;

    public SaleOrderRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}