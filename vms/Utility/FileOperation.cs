using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using vms.entity.viewModels;

namespace vms.Utility;

public static class FileOperation
{

    private static async Task<FileSaveFeedbackDto> FileSave(IFormFile file, FileSaveDto fileSaveDto)
    {
        var fdto = new FileSaveFeedbackDto();
        var fileExtenstion = Path.GetExtension(file.FileName);
        var fileName = Guid.NewGuid().ToString();
        fileName += fileExtenstion;
        var uploadFilePath = Path.Combine(fileSaveDto.FileRootPath, fileSaveDto.OrganizationId.ToString(), fileSaveDto.FileModulePath, fileSaveDto.TransactionId.ToString(), fileName);
        fdto.MimeType = fileExtenstion;
        var exists = Directory.Exists(uploadFilePath);

        if (!exists)
        {
            Directory.CreateDirectory(uploadFilePath);
        }

        if (file.Length <= 0) return fdto;
        await using var fileStream = new FileStream(uploadFilePath, FileMode.Create);
        await file.CopyToAsync(fileStream);
        return fdto;
    }

    public static async Task<List<FileSaveFeedbackDto>> SaveFileInDisk(FileSaveDto fileSaveDto)
    {
        var dtos = new List<FileSaveFeedbackDto>();
        foreach (var item in fileSaveDto.FormFileList)
        {
            var fileSaveFeedbackDto = await FileSave(item.FormFIle, fileSaveDto);
            var content = new FileSaveFeedbackDto
            {
                FileUrl = fileSaveFeedbackDto.FileUrl,
                MimeType = fileSaveFeedbackDto.MimeType,
                DocumentTypeId = item.DocumentTypeId
            };
            dtos.Add(content);
        }
        return dtos;
    }
}