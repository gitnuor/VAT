using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace vms.service.Services.UploadService;

public interface IVmsExcelService
{
    string ReadUnknownExcel(IFormFile excelFile);
    List<T> ReadExcel<T>(IFormFile excelFile) where T : class;
}