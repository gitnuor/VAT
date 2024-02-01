using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class AdjustmentTypeRepository : RepositoryBase<AdjustmentType>, IAdjustmentTypeRepository
{
    private readonly DbContext _context;

    public AdjustmentTypeRepository(DbContext context) : base(context)
    {
        _context = context;
    }

}