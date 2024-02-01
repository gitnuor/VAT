using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.StoredProcedureModel;

namespace vms.repository.Repository.tbl;

public interface ICustomerSubscriptionRepository : IRepositoryBase<CustomerSubscription>
{
    Task<IEnumerable<ViewCustomerSubscription>> GetCustomerSubscription(int orgId);
    Task<IEnumerable<SpGetUnBilledSubscriptions>> GetUnBilledSubscription(int organizationId, int billingOfficeId, int collectionOfficeId, int year, int month);
}