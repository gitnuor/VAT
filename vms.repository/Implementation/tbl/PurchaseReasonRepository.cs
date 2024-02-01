using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class PurchaseReasonRepository : RepositoryBase<PurchaseReason>, IPurchaseReasonRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;

    public PurchaseReasonRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<PurchaseReason>> GetPurchaseReasons()
    {
        var purchaseReasons = await Query().SelectAsync();

        purchaseReasons.ToList().ForEach(delegate (PurchaseReason purchaseReason)
        {
            purchaseReason.EncryptedId = _dataProtector.Protect(purchaseReason.PurchaseReasonId.ToString());
        });
        return purchaseReasons;
    }
    public async Task<PurchaseReason> GetPurchaseReason(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var purchaseReason = await Query()
            .SingleOrDefaultAsync(x => x.PurchaseReasonId == id, System.Threading.CancellationToken.None);
        purchaseReason.EncryptedId = _dataProtector.Protect(purchaseReason.PurchaseReasonId.ToString());

        return purchaseReason;
    }

}