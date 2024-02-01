using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class PurchaseOrderDetailRepository : RepositoryBase<PurchaseDetail>, IPurchaseOrderDetailsRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;

    public PurchaseOrderDetailRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<PurchaseDetail>> GetPurchaseDetails(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var purchase = await Query()
                 .Include(sd => sd.Product.ProductType)
                 .Include(sd => sd.Product.ProductCategory)
                 .Include(sd => sd.Product.Brand)
                .Include(sd => sd.MeasurementUnit)
                .Include(c => c.ProductVattype)
                .Include(c => c.DebitNoteDetails)
                .Include(c => c.ProductTransactionBooks)
                .Where(sd => sd.PurchaseId == id).SelectAsync();
        purchase.ToList().ForEach(delegate (PurchaseDetail pur)
        {
            pur.EncryptedId = _dataProtector.Protect(pur.PurchaseId.ToString());
        });
        return purchase;
    }
}