using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class ProductVatRepository : RepositoryBase<ProductVat>, IProductVatRepository
{
    private readonly DbContext _context;

    public ProductVatRepository(DbContext context) : base(context)
    {
        _context = context;
    }

}