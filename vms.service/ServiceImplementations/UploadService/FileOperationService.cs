using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.UploadService;

namespace vms.service.ServiceImplementations.UploadService;

public class FileOperationService : IFileOperationService
{
    private readonly IFileOperationRepository _repo;

    public FileOperationService(IFileOperationRepository repository)
    {
        _repo = repository;
    }

    public async Task<List<FileSaveFeedbackDto>> SaveFiles(FileSaveDto fileSaveDto)
    {
        return await _repo.SaveFileInDisk(fileSaveDto);
    }

    public async Task<FileSaveFeedbackDto> SaveFile(IFormFile file, FileSaveDto fileSaveDto)
    {
        return await _repo.SingleFileSaveOnDisk(file, fileSaveDto);
    }
}