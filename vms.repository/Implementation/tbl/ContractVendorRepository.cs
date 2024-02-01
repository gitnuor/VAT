using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class ContractVendorRepository : RepositoryBase<ContractualProduction>, IContractVendorRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;

    public ContractVendorRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }


    public async Task<IEnumerable<ContractualProduction>> GetContractualProductions(int orgIdEnc)
    {
        var contractualProductions = await Query().Include(c => c.Vendor).Include(c => c.ContractType).Where(w => w.OrganizationId == orgIdEnc).OrderByDescending(c=>c.ContractualProductionId).SelectAsync();

        contractualProductions.ToList().ForEach(delegate (ContractualProduction contractualProduction)
        {
            contractualProduction.EncryptedId = _dataProtector.Protect(contractualProduction.ContractualProductionId.ToString());
        });
        return contractualProductions;
    }
    public async Task<ContractualProduction> GetTransferContract(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var contractualProduction = await Query().Include(c => c.Vendor).Include("ContractualProductionProductDetails.Product.MeasurementUnit")
            .SingleOrDefaultAsync(x => x.ContractualProductionId == id, CancellationToken.None);
        contractualProduction.EncryptedId = _dataProtector.Protect(contractualProduction.ContractualProductionId.ToString());

        return contractualProduction;
    }


    



    public async  Task<IEnumerable<ContractualProduction>> GetAll()
    {
        return await  Query()//.Include(c => c.ContractVendorProductDetails)
            //.Include(c => c.ContractVendorProductDetails.Select(x => x.Product))
            .Include("ContractualProductionProductDetails.Product")
            .SelectAsync(CancellationToken.None);
    }

    public async Task<IEnumerable<ViewContractualProduction>> ViewContractualProduction(string orgIdEnc)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
        var list = await _context.Set<ViewContractualProduction>()
            .Where(s => s.OrganizationId == orgId)
            .OrderByDescending(s => s.ApiTransactionId)
            .AsNoTracking()
            .ToListAsync();
        list.ForEach(s => s.EncryptedId = _dataProtector.Protect(s.ContractualProductionId.ToString()));
        return list;
    }
}