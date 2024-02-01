using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface ISalesDeliveryTypeRepository : IRepositoryBase<SalesDeliveryType>
{
    Task<IEnumerable<SalesDeliveryType>> GetSalesDeliveryTypes();
    Task<SalesDeliveryType> GetSalesDeliveryType(string idEnc);
}