using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class CustomsAndVatcommissionarateRepository : RepositoryBase<CustomsAndVatcommissionarate>, ICustomsAndVatcommissionarateRepository
{
    private readonly DbContext _context;

    public CustomsAndVatcommissionarateRepository(DbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CustomsAndVatcommissionarate>> CustomsAndVatcommissionarates()
    {
        return await Query()
            .SelectAsync();
    }
}