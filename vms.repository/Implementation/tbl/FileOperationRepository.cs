using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class FileOperationRepository : IFileOperationRepository
{
	public async Task<FileSaveFeedbackDto> SingleFileSaveOnDisk(IFormFile file, FileSaveDto fileSaveDto)
	{
		var fdto = new FileSaveFeedbackDto();
		var fileExtenstion = Path.GetExtension(file.FileName);
		var fileName = Guid.NewGuid().ToString();
		fileName += fileExtenstion;
		var uploadFilePath = Path.Combine(fileSaveDto.FileRootPath, fileSaveDto.OrganizationId.ToString(),
			fileSaveDto.FileModulePath);

		if (fileSaveDto.TransactionId != null)
		{
			uploadFilePath = Path.Combine(uploadFilePath, fileSaveDto.TransactionId.ToString());
		}

		fdto.MimeType = fileExtenstion;

		var exists = Directory.Exists(uploadFilePath);

		if (!exists)
		{
			Directory.CreateDirectory(uploadFilePath);
		}

		if (file.Length > 0)
		{
			uploadFilePath = Path.Combine(uploadFilePath, fileName);
			await using var fileStream = new FileStream(uploadFilePath, FileMode.Create);
			await file.CopyToAsync(fileStream);
		}

		fdto.FileUrl = uploadFilePath;
		return fdto;
	}

	public async Task<List<FileSaveFeedbackDto>> SaveFileInDisk(FileSaveDto fileSaveDto)
	{
		var dtos = new List<FileSaveFeedbackDto>();
		foreach (var item in fileSaveDto.FormFileList)
		{
			var fileSaveFeedbackDto = await SingleFileSaveOnDisk(item.FormFIle, fileSaveDto);
			var content = new FileSaveFeedbackDto
			{
				FileUrl = fileSaveFeedbackDto.FileUrl,
				MimeType = fileSaveFeedbackDto.MimeType,
				DocumentTypeId = item.DocumentTypeId,
				DocumentRemarks = item.DocumentRemarks
			};
			dtos.Add(content);
		}

		return dtos;
	}
}