using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using vms.entity.viewModels;

namespace vms.repository.Repository.tbl;

public interface IFileOperationRepository
{
    Task<FileSaveFeedbackDto> SingleFileSaveOnDisk(IFormFile file, FileSaveDto fileSaveDto);
    Task<List<FileSaveFeedbackDto>> SaveFileInDisk(FileSaveDto fileSaveDto);
}