using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IMushakReturnPaymentTypeRepository : IRepositoryBase<MushakReturnPaymentType>
{
    Task<IEnumerable<MushakReturnPaymentType>> GetPaymentType();
    Task<IEnumerable<MushakReturnPaymentType>> MushakReturnPaymentTypeWithCode();
}