using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.UploadService;

public interface IExportTypeService : IServiceBase<ExportType>
{
    Task<IEnumerable<ExportType>> GetExportTypes();
    Task<ExportType> GetExportType(string idEnc);
	Task<IEnumerable<CustomSelectListItem>> GetExportTypesSelectList();
}