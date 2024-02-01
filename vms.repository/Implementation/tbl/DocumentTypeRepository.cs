using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class DocumentTypeRepository : RepositoryBase<DocumentType>, IDocumentTypeRepository
{
    private readonly DbContext _context;
    private readonly IDataProtector _dataProtector;

    public DocumentTypeRepository(DbContext context, IDataProtectionProvider protectionProvider, PurposeStringConstants purposeStringConstants) : base(context)
    {
        _context = context;
        _dataProtector = protectionProvider.CreateProtector(purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<DocumentType>> GetDocumentTypes(int orgIdEnc)
    {
        var documentTypes = await Query().Where(w => w.OrganizationId == orgIdEnc).SelectAsync();

        documentTypes.ToList().ForEach(delegate (DocumentType documentType)
        {
            documentType.EncryptedId = _dataProtector.Protect(documentType.DocumentTypeId.ToString());
        });
        return documentTypes;
    }

    public async Task<IEnumerable<DocumentType>> GetDocumentTypes(string pOrgId)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(pOrgId));
        return await Query().Where(w => w.OrganizationId == orgId).SelectAsync();
    }

    public async Task<DocumentType> GetDocumentType(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var documentType = await Query()
            .SingleOrDefaultAsync(x => x.DocumentTypeId == id, System.Threading.CancellationToken.None);
        documentType.EncryptedId = _dataProtector.Protect(documentType.DocumentTypeId.ToString());

        return documentType;
    }

}