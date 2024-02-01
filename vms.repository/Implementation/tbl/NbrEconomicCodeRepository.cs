using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class NbrEconomicCodeRepository : RepositoryBase<NbrEconomicCode>, INbrEconomicCodeRepository
{
    private readonly DbContext _context;

    public NbrEconomicCodeRepository(DbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<NbrEconomicCode>> GetNbrEconomicCode()
    {
        var nbrEconomicCode = await Query().SelectAsync();
        return nbrEconomicCode;
    }
}