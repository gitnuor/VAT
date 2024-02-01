using System.Collections.Generic;

namespace vms.entity.viewModels;

public class FileSaveDto
{
    public string FileSaveDirectory { get; set; }
    public string FileRootPath { get; set; }
    public string FileModulePath { get; set; }
    public int OrganizationId { get; set; }
    public int? TransactionId { get; set; }
    public IList<FIleDocumentInfo> FormFileList { get; set; } = new List<FIleDocumentInfo>();
}