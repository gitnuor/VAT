using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class PaymentMethodRepository : RepositoryBase<PaymentMethod>, IPaymentMethodRepository
{
        
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;

    public PaymentMethodRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<PaymentMethod>> GetPaymentMethods()
    {
        var paymentMethods = await Query().SelectAsync();

        paymentMethods.ToList().ForEach(delegate (PaymentMethod paymentMethod)
        {
            paymentMethod.EncryptedId = _dataProtector.Protect(paymentMethod.PaymentMethodId.ToString());
        });
        return paymentMethods;
    }
    public async Task<PaymentMethod> GetPaymentMethod(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var paymentMethod = await Query()
            .SingleOrDefaultAsync(x => x.PaymentMethodId == id, System.Threading.CancellationToken.None);
        paymentMethod.EncryptedId = _dataProtector.Protect(paymentMethod.PaymentMethodId.ToString());

        return paymentMethod;
    }

}