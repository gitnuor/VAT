using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class ContractVendorProductDetailsRepository : RepositoryBase<ContractualProductionProductDetail>, IContractVendorProductDetailsRepository
{
    private readonly DbContext _context;

    public ContractVendorProductDetailsRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}