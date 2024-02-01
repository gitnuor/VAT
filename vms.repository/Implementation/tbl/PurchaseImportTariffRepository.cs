using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class PurchaseImportTariffRepository : RepositoryBase<PurchaseImportTariff>, IPurchaseImportTariffRepository
{
    private readonly DbContext _context;

    public PurchaseImportTariffRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}