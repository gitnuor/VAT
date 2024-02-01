using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class BankRepository : RepositoryBase<Bank>, IBankRepository
{
    private readonly DbContext _context;

    public BankRepository(DbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Bank>> GetBank()
    {
        return await Query()
            .SelectAsync();
    }
}