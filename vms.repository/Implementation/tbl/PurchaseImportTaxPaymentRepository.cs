using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class PurchaseImportTaxPaymentRepository : RepositoryBase<PurchaseImportTaxPayment>, IPurchaseImportTaxPaymentRepository
{
    private readonly DbContext _context;

    public PurchaseImportTaxPaymentRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}