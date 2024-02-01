using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class SupplimentaryDutyRepository : RepositoryBase<SupplimentaryDuty>, ISupplimentaryDutyRepository
{
    private readonly DbContext _context;

    public SupplimentaryDutyRepository(DbContext context) : base(context)
    {
        _context = context;
    }

}