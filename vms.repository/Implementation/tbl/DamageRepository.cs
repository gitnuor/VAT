using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class DamageRepository : RepositoryBase<Damage>, IDamageRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;

    public DamageRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<Damage>> GetDamage(int orgIdEnc)
    {
        var damage = await Query().Where(w => w.OrganizationId == orgIdEnc)
            .SelectAsync();

        return damage;
    }
    public async Task<Damage> GetDamage(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var damageType = await Query()
            .SingleOrDefaultAsync(x => x.DamageTypeId == id, System.Threading.CancellationToken.None);

        return damageType;
    }

    public async Task<Damage> GetDamagewithDetails(int orgId, int damageId)
    {
        var damage = await Query().Where(x => x.OrganizationId == orgId && x.DamageId == damageId)
            .FirstOrDefaultAsync();
        return damage;
    }

}