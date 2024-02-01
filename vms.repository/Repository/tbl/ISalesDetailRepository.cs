using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface ISalesDetailRepository : IRepositoryBase<SalesDetail>
{
    Task<IEnumerable<SalesDetail>> GetAllSalesDetails(string idEnc);
    Task<IEnumerable<SalesDetail>> GetSalesDetails(string idEnc);
    Task<IEnumerable<ViewSalesDetail>> GetSalesDetailsBySales(string salesIdEnc);
}