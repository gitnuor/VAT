using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.service.Services.TransactionService;

public interface ISalesDetailService : IServiceBase<SalesDetail>
{
    Task<IEnumerable<SalesDetail>> GetAllSalesDetails(string idEnc);

    Task<IEnumerable<SalesDetail>> GetSalesDetails(string idEnc);
}