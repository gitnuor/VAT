using Microsoft.AspNetCore.Http;

namespace vms.Models;

public class ContentInfo
{
    public int DocumentTypeId { get; set; }
    public IFormFile UploadFile { get; set; }
}