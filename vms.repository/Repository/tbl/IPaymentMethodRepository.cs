using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IPaymentMethodRepository : IRepositoryBase<PaymentMethod>
{
    Task<IEnumerable<PaymentMethod>> GetPaymentMethods();
    Task<PaymentMethod> GetPaymentMethod(string idEnc);
}