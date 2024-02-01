using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using vms.entity.viewModels;
using vms.service.Services.UploadService;

namespace vms.service.ServiceImplementations.UploadService;

public class CRMService : ICRMService
{

    private readonly IHostingEnvironment hostingEnvironment;


    public CRMService(IHostingEnvironment hostingEnvironment)
    {

        this.hostingEnvironment = hostingEnvironment;

    }
    public void addFile(vmFileUpload model)
    {
        model.path = Path.Combine(hostingEnvironment.WebRootPath, model.path);

        string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.File.FileName;
        string filePath = Path.Combine(model.path, uniqueFileName);

        if (!Directory.Exists(model.path))
            Directory.CreateDirectory(model.path);

        model.File.CopyTo(new FileStream(filePath, FileMode.Create));

    }

}