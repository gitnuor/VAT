using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.UploadService;

public interface IContentService : IServiceBase<Content>
{
    public Task InsertSaleContents(IEnumerable<FileSaveFeedbackDto> fileSaveFeedbackDtos, int orgId, int createdBy, int saleId);
    public Task InsertCustomerContents(IEnumerable<FileSaveFeedbackDto> fileSaveFeedbackDtos, int orgId, int createdBy, int customerId);
}