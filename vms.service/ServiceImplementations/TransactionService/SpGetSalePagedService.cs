using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.StoredProcedureModel;
using vms.repository.Repository.sp;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class SpGetSalePagedService : ISpGetSalePagedService
{
    private readonly ISpGetSalePagedRepository _repository;
    public SpGetSalePagedService(ISpGetSalePagedRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<spGetSalePaged>> GetSalePaged(int PageSize, int PageNumber, int OrganizationId)
    {
        return await _repository.GetSalePaged(PageSize, PageNumber, OrganizationId);
    }
}