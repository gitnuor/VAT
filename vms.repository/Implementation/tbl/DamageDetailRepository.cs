using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class DamageDetailRepository : RepositoryBase<DamageDetail>, IDamageDetailRepository
{
    private readonly DbContext _context;

    public DamageDetailRepository(DbContext context) : base(context)
    {
        _context = context;
    }


    public async Task<IEnumerable<DamageDetail>> GetDamageDetails(int damageId)
    {
        var damageDetails= await Query().Where(x => x.DamageId == damageId).Include(x => x.Product).SelectAsync();
        return damageDetails;
    }
}