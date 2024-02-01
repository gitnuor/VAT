using Microsoft.AspNetCore.Http;
using vms.entity.models;

namespace vms.entity.viewModels;

public class vmFileUpload
{ 


    public Content Content { get; set; }
    public int year { get; set; }
    public string month { get; set; }
    public IFormFile File { get; set; }
    public string path { get; set; }
}