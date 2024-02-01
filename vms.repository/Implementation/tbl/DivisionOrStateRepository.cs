using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class DivisionOrStateRepository : RepositoryBase<DivisionOrState>, IDivisionOrStateRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;

    public DivisionOrStateRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<DivisionOrState>> GetDivisionOrStates(int orgId)
    {
        var divisionOrStates = await Query().Include(x=>x.Country).Where(w => w.OrganizationId == orgId).SelectAsync();

        divisionOrStates.ToList().ForEach(delegate (DivisionOrState type)
        {
            type.EncryptedId = _dataProtector.Protect(type.DivisionOrStateId.ToString());
        });
        return divisionOrStates.ToList();
    }
    public async Task<DivisionOrState> GetDivisionOrStateById(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var divisionOrState = await Query()
            .SingleOrDefaultAsync(x => x.DivisionOrStateId == id, System.Threading.CancellationToken.None);
        divisionOrState.EncryptedId = _dataProtector.Protect(divisionOrState.DivisionOrStateId.ToString());

        return divisionOrState;
    }

    public async Task<IEnumerable<DivisionOrState>> GetDivisionOrStatesByCountryId(int orgId, int countryId)
    {
        var divisionOrStates = await Query().Include(x => x.Country).Where(w => w.OrganizationId == orgId && w.CountryId==countryId).SelectAsync();
        return divisionOrStates;
    }
}