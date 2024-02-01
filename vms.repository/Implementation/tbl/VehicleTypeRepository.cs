using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class VehicleTypeRepository : RepositoryBase<VehicleType>, IVehicleTypeRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;

    public VehicleTypeRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<VehicleType>> GetVehicleTypes(int orgId)
    {
        var types = await Query().Where(w => w.OrganizationId == orgId).SelectAsync();

        types.ToList().ForEach(delegate (VehicleType type)
        {
            type.EncryptedId = _dataProtector.Protect(type.VehicleTypeId.ToString());
        });
        return types.ToList();
    }
    public async Task<VehicleType> GetVehicleType(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var type = await Query()
            .SingleOrDefaultAsync(x => x.VehicleTypeId == id, System.Threading.CancellationToken.None);
        type.EncryptedId = _dataProtector.Protect(type.VehicleTypeId.ToString());

        return type;
    }
}