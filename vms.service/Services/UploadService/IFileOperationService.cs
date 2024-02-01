using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using vms.entity.viewModels;

namespace vms.service.Services.UploadService;

public interface IFileOperationService
{
    Task<List<FileSaveFeedbackDto>> SaveFiles(FileSaveDto fileSaveDto);
    Task<FileSaveFeedbackDto> SaveFile(IFormFile file, FileSaveDto fileSaveDto);
}