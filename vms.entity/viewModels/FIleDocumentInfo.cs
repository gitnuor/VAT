using Microsoft.AspNetCore.Http;

namespace vms.entity.viewModels;

public class FIleDocumentInfo
{
	public int? DocumentTypeId { get; set; }
	public IFormFile FormFIle { get; set; }
	public string DocumentRemarks { get; set; }
}