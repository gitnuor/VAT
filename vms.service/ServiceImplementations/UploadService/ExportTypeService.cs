using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.Utility;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.UploadService;

namespace vms.service.ServiceImplementations.UploadService;

public class ExportTypeService : ServiceBase<ExportType>, IExportTypeService
{
    private readonly IExportTypeRepository _repository;

    public ExportTypeService(IExportTypeRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ExportType>> GetExportTypes()
    {
        return await _repository.GetExportTypes();
    }

    public async Task<ExportType> GetExportType(string idEnc)
    {
        return await _repository.GetExportType(idEnc);
    }

    public async Task<IEnumerable<CustomSelectListItem>> GetExportTypesSelectList()
    {
		var exportTypesList = await _repository.GetExportTypes();
		return exportTypesList.ConvertToCustomSelectList(nameof(ExportType.ExportTypeId),
			nameof(ExportType.ExportTypeName));
	}
}