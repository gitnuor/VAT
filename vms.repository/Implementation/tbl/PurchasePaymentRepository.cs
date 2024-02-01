using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class PurchasePaymentRepository : RepositoryBase<PurchasePayment>, IPurchasePaymentRepository
{
    private readonly DbContext _context;

    public PurchasePaymentRepository(DbContext context) : base(context)
    {
        _context = context;
    }

}