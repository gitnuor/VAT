using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class ContractVendorTransferRawMaterialRepository : RepositoryBase<ContractualProductionTransferRawMaterial>, IContractVendorTransferRawMaterialRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;

    public ContractVendorTransferRawMaterialRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<ContractualProductionTransferRawMaterial>> GetTransferedRawMaterials(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var contractualProduction = await Query().Where(c => c.ContractualProductionId == id).SelectAsync(CancellationToken.None);
        //contractualProduction.EncryptedId = _dataProtector.Protect(contractualProduction.ContractualProductionId.ToString());

        return contractualProduction;
    }
}