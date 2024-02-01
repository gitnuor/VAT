using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class MushakReturnPaymentTypeRepository : RepositoryBase<MushakReturnPaymentType>, IMushakReturnPaymentTypeRepository
{
    private readonly DbContext _context;

    public MushakReturnPaymentTypeRepository(DbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MushakReturnPaymentType>> GetPaymentType()
    {
        return await Query().SelectAsync();
    }

    public async Task<IEnumerable<MushakReturnPaymentType>> MushakReturnPaymentTypeWithCode()
    {
        return await Query().Include(x=>x.NbrEconomicCode).SelectAsync();
    }
}