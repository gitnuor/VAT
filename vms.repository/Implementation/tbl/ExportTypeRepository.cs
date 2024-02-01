using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class ExportTypeRepository : RepositoryBase<ExportType>, IExportTypeRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;

    public ExportTypeRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<ExportType>> GetExportTypes()
    {
        var exportTypes = await Query().SelectAsync();

        exportTypes.ToList().ForEach(delegate (ExportType exportType)
        {
            exportType.EncryptedId = _dataProtector.Protect(exportType.ExportTypeId.ToString());
        });
        return exportTypes;
    }
    public async Task<ExportType> GetExportType(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var exportType = await Query()
            .SingleOrDefaultAsync(x => x.ExportTypeId == id, System.Threading.CancellationToken.None);
        exportType.EncryptedId = _dataProtector.Protect(exportType.ExportTypeId.ToString());

        return exportType;
    }

}