using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IDocumentTypeRepository : IRepositoryBase<DocumentType>
{
    Task<IEnumerable<DocumentType>> GetDocumentTypes(int orgIdEnc);
    Task<IEnumerable<DocumentType>> GetDocumentTypes(string pOrgId);
    Task<DocumentType> GetDocumentType(string idEnc);
}