using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class ContractTypeRepository : RepositoryBase<ContractType>, IContractTypeRepository
{
    private readonly DbContext _context;

    public ContractTypeRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}