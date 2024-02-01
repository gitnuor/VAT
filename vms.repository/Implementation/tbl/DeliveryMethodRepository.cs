using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class DeliveryMethodRepository : RepositoryBase<DeliveryMethod>, IDeliveryMethodRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;
    public DeliveryMethodRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }


    public async Task<IEnumerable<DeliveryMethod>> GetDeliveryMethods()
    {
        var deliveryMethods = await Query().SelectAsync();

        deliveryMethods.ToList().ForEach(delegate (DeliveryMethod deliveryMethod)
        {
            deliveryMethod.EncryptedId = _dataProtector.Protect(deliveryMethod.DeliveryMethodId.ToString());
        });
        return deliveryMethods;
    }
    public async Task<DeliveryMethod> GetDeliveryMethod(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var deliveryMethod = await Query()
            .SingleOrDefaultAsync(x => x.DeliveryMethodId == id, System.Threading.CancellationToken.None);
        deliveryMethod.EncryptedId = _dataProtector.Protect(deliveryMethod.DeliveryMethodId.ToString());

        return deliveryMethod;
    }
}