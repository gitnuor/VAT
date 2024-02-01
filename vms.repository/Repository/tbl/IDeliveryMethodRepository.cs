using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IDeliveryMethodRepository : IRepositoryBase<DeliveryMethod>
{
    Task<IEnumerable<DeliveryMethod>> GetDeliveryMethods();
    Task<DeliveryMethod> GetDeliveryMethod(string idEnc);
}