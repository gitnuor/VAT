using vms.entity.viewModels;

namespace vms.service.Services.UploadService;

public interface ICRMService
{
    void addFile(vmFileUpload model);
}