using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class ContractVendorTransferRawMaterialDetailsRepository : RepositoryBase<ContractualProductionTransferRawMaterialDetail>, IContractVendorTransferRawMaterialDetailsRepository
{
    private readonly DbContext _context;

    public ContractVendorTransferRawMaterialDetailsRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}