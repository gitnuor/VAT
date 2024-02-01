using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class DamageTypeRepository : RepositoryBase<DamageType>, IDamageTypeRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;

    public DamageTypeRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<DamageType>> GetDamageTypes(int orgIdEnc)
    {
        var damageTypes = await Query()//.Where(w => w.OrganizationId == orgIdEnc)
            .SelectAsync();

        damageTypes.ToList().ForEach(delegate (DamageType damageType)
        {
            damageType.EncryptedId = _dataProtector.Protect(damageType.DamageTypeId.ToString());
        });
        return damageTypes;
    }
    public async Task<DamageType> GetDamageType(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var damageType = await Query()
            .SingleOrDefaultAsync(x => x.DamageTypeId == id, System.Threading.CancellationToken.None);
        damageType.EncryptedId = _dataProtector.Protect(damageType.DamageTypeId.ToString());

        return damageType;
    }

}