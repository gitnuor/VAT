using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class PurchaseImportTaxPaymentTypeRepository : RepositoryBase<PurchaseImportTaxPaymentType>, IPurchaseImportTaxPaymentTypeRepository
{
    private readonly DbContext _context;

    public PurchaseImportTaxPaymentTypeRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}