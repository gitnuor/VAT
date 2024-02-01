using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.StoredProcedureModel;

namespace vms.repository.Repository.sp;

public interface ISpGetSalePagedRepository
{
    Task<List<spGetSalePaged>> GetSalePaged(int PageSize,int PageNumber,int OrganizationId);

}