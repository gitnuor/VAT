using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.UploadService;

namespace vms.service.ServiceImplementations.UploadService;

public class ContentService : ServiceBase<Content>, IContentService
{
    private readonly IContentRepository _repository;
    private readonly IFileOperationService _fileOperationService;
    public ContentService(IContentRepository repository, IFileOperationService fileOperationService) : base(repository)
    {
        _repository = repository;
        _fileOperationService = fileOperationService;
    }

    public Task InsertSaleContents(IEnumerable<FileSaveFeedbackDto> fileSaveFeedbackDtos, int orgId, int createdBy, int saleId)
    {
        return InsertContents(fileSaveFeedbackDtos, orgId, EnumObjectType.Sale, createdBy, saleId);
    }

    public Task InsertCustomerContents(IEnumerable<FileSaveFeedbackDto> fileSaveFeedbackDtos, int orgId, int createdBy, int customerId)
    {
        return InsertContents(fileSaveFeedbackDtos, orgId, EnumObjectType.Customer, createdBy, customerId);
    }

    private Task InsertContents(IEnumerable<FileSaveFeedbackDto> fileSaveFeedbackDtos, int orgId, EnumObjectType objectType, int createdBy, int objectId)
    {
        var contents = fileSaveFeedbackDtos.Select(dto => new Content
        {
            DocumentTypeId = dto.DocumentTypeId,
            OrganizationId = orgId,
            FileUrl = dto.FileUrl,
            MimeType = dto.MimeType,
            Node = null,
            ObjectId = (int)objectType,
            ObjectPrimaryKey = objectId,
            IsActive = true,
            CreatedBy = createdBy,
            CreatedTime = DateTime.Now,
            Remarks = dto.DocumentRemarks,
        })
            .ToList();

        return _repository.BulkInsertAsync(contents);
    }
}