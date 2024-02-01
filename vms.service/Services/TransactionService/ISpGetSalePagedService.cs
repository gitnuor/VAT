using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.StoredProcedureModel;

namespace vms.service.Services.TransactionService;

public interface ISpGetSalePagedService
{
    Task<List<spGetSalePaged>> GetSalePaged(int PageSize, int PageNumber, int OrganizationId);

}