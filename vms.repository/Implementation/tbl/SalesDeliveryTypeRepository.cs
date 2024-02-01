using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class SalesDeliveryTypeRepository : RepositoryBase<SalesDeliveryType>, ISalesDeliveryTypeRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;

    public SalesDeliveryTypeRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<SalesDeliveryType>> GetSalesDeliveryTypes()
    {
        var salesDeliveryTypes = await Query().SelectAsync();

        salesDeliveryTypes.ToList().ForEach(delegate (SalesDeliveryType salesDeliveryType)
        {
            salesDeliveryType.EncryptedId = _dataProtector.Protect(salesDeliveryType.SalesDeliveryTypeId.ToString());
        });
        return salesDeliveryTypes;
    }
    public async Task<SalesDeliveryType> GetSalesDeliveryType(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var salesDeliveryType = await Query()
            .SingleOrDefaultAsync(x => x.SalesDeliveryTypeId == id, System.Threading.CancellationToken.None);
        salesDeliveryType.EncryptedId = _dataProtector.Protect(salesDeliveryType.SalesDeliveryTypeId.ToString());

        return salesDeliveryType;

    }
}