using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.viewModels.CustomerViewModel;
using vms.entity.viewModels.SubscriptionAndBilling;

namespace vms.service.Services.ThirdPartyService;

public interface ICustomerSubscriptionService : IServiceBase<CustomerSubscription>
{
    Task<IEnumerable<ViewCustomerSubscription>> GetCustomerSubscription(int orgId);
    Task<bool> SaveCustomerSubscription(CustomerSubscriptionCreateViewModel customerSubscription); 
    Task<IEnumerable<SpGetUnBilledSubscriptions>> GetUnBilledSubscription(int organizationId, int billingOfficeId, int collectionOfficeId, int year, int month);
    Task<IEnumerable<SubscriptionBillDetailCreateViewModel>> GetSubscriptionBillDetailCreateViewModel(int organizationId, int billingOfficeId, int collectionOfficeId, int year, int month);
}