using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace vms.entity.viewModels.BranchTransferReceive;

public class BranchTransferReceiveDocumentPostViewModel
{
	[DisplayName("Type")]
	[Required(ErrorMessage = "DocumentType is Required")]
	public int DocumentTypeId { get; set; }
	[Required(ErrorMessage = "File is Required")]
	public IFormFile UploadedFile { get; set; }
	public string FileName => UploadedFile.FileName;
	public string FileUrl => UploadedFile.FileName;
	public string FilePath { get; set; }
	[DisplayName("Remarks")]
	[MaxLength(50)]
	public string DocumentRemarks { get; set; }
}