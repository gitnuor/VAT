using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class CustomerDeliveryAddressRepository : RepositoryBase<CustomerDeliveryAddress>, ICustomerDeliveryAddressRepository
{
    private readonly DbContext _context;

    public CustomerDeliveryAddressRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}