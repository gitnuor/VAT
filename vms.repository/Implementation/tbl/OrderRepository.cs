using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class OrderRepository : RepositoryBase<Order>, IOrderRepository
{
    private readonly DbContext _context;

    public OrderRepository(DbContext context) : base(context)
    {
        _context = context;
    }

}