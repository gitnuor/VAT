using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class ProductProductTypeMappingRepository : RepositoryBase<ProductProductTypeMapping>, IProductProductTypeMappingRepository
{
    private readonly DbContext _context;

    public ProductProductTypeMappingRepository(DbContext context) : base(context)
    {
        _context = context;
    }

}