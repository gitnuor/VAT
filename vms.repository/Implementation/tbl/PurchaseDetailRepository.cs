using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class PurchaseDetailRepository : RepositoryBase<PurchaseDetail>, IPurchaseDetailRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;

    private IDataProtector _dataProtector;

    public PurchaseDetailRepository(DbContext context,IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }


    public async Task<IEnumerable<PurchaseDetail>> GetPurchaseDetails(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var purchase = await Queryable()
            .Include(x => x.DamageDetails)
            .Include(x => x.Product)
            .Where(x => x.PurchaseId == id).ToListAsync();
        //.ToListAync();
        // purchase.EncryptedId = _dataProtector.Protect(purchase.PurchaseId.ToString());

        return purchase;
        // throw new NotImplementedException();
    }
}