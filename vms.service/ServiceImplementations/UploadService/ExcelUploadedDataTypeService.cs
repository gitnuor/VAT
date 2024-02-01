using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.UploadService;

namespace vms.service.ServiceImplementations.UploadService;

public class ExcelUploadedDataTypeService : ServiceBase<ExcelUploadedDataType>, IExcelUploadedDataTypeService
{
    private IExcelUploadedDataTypeRepository _repository;
    public ExcelUploadedDataTypeService(IExcelUploadedDataTypeRepository repository) : base(repository)
    {
        _repository = repository;
    }
}