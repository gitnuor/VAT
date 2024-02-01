using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using vms.entity.models;
using vms.entity.Utility;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.UploadService;

namespace vms.service.ServiceImplementations.UploadService;

public class DocumentTypeService : ServiceBase<DocumentType>, IDocumentTypeService
{
    private readonly IDocumentTypeRepository _repository;

    public DocumentTypeService(IDocumentTypeRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DocumentType>> GetDocumentTypes(int orgIdEnc)
    {
        return await _repository.GetDocumentTypes(orgIdEnc);
    }

    public async Task<DocumentType> GetDocumentType(string idEnc)
    {
        return await _repository.GetDocumentType(idEnc);
    }


    public async Task<IEnumerable<CustomSelectListItem>> GetDocumentTypeSelectList(string pOrgId)
    {
        var documents = await _repository.GetDocumentTypes(pOrgId);
        return documents.ConvertToCustomSelectList(nameof(DocumentType.DocumentTypeId),
            nameof(DocumentType.Name));
    }
}