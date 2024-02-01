namespace vms.entity.viewModels;

public class FileSaveFeedbackDto
{
    public int? DocumentTypeId { get; set; }
    public string FileUrl { get; set; }
    public string MimeType { get; set; }
    public string DocumentRemarks { get; set; }
}