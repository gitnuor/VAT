using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.UploadService;

public interface IDocumentTypeService : IServiceBase<DocumentType>
{
    Task<IEnumerable<DocumentType>> GetDocumentTypes(int orgIdEnc);
    Task<DocumentType> GetDocumentType(string idEnc);


    Task<IEnumerable<CustomSelectListItem>> GetDocumentTypeSelectList(string pOrgId);
}