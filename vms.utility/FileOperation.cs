namespace vms.Utility;

public static class FileOperation
{

    //private static async Task<FileSaveFeedbackDto> FileSave(IFormFile File, FileSaveDto fileSaveDto)
    //{
    //    FileSaveFeedbackDto fdto = new FileSaveFeedbackDto();
    //    var fileExtenstion = Path.GetExtension(File.FileName);
    //    string fileName = Guid.NewGuid().ToString();
    //    fileName += fileExtenstion;
    //    var uploadFilePath = Path.Combine(fileSaveDto.fileRootPath, fileSaveDto.organizationId.ToString(), fileSaveDto.fileModulePath, fileSaveDto.TransactionId.ToString(), fileName);
    //    fdto.MimeType = fileExtenstion;
    //    bool exists = Directory.Exists(uploadFilePath);

    //    if (!exists)
    //    {
    //        Directory.CreateDirectory(uploadFilePath);
    //    }
    //    if (File.Length > 0)
    //    {
    //        using var fileStream = new FileStream(uploadFilePath, FileMode.Create);
    //        await File.CopyToAsync(fileStream);
    //    }
    //    return fdto;
    //}

    //public static async Task<List<FileSaveFeedbackDto>> SaveFileInDisk(FileSaveDto fileSaveDto)
    //{
    //    var dtos = new List<FileSaveFeedbackDto>();
    //    foreach (var item in fileSaveDto.FormFileList)
    //    {
    //        var fileSaveFeedbackDto = await FileSave(item.FormFIle, fileSaveDto);
    //        var content = new FileSaveFeedbackDto
    //        {
    //            FileUrl = fileSaveFeedbackDto.FileUrl,
    //            MimeType = fileSaveFeedbackDto.MimeType,
    //            DocumentTypeId = item.DocumentTypeId
    //        };
    //        dtos.Add(content);
    //    }
    //    return dtos;
    //}
}