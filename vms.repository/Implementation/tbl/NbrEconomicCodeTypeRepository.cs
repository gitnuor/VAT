using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class NbrEconomicCodeTypeRepository : RepositoryBase<NbrEconomicCodeType>, INbrEconomicCodeTypeRepository
{
    private readonly DbContext _context;

    public NbrEconomicCodeTypeRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}