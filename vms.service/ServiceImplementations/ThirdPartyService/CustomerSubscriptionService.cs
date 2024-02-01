using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using URF.Core.Abstractions;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.viewModels.CustomerViewModel;
using vms.entity.viewModels.SubscriptionAndBilling;
using vms.repository.Repository.tbl;
using vms.service.Services.ThirdPartyService;

namespace vms.service.ServiceImplementations.ThirdPartyService;

public class CustomerSubscriptionService
    (ICustomerSubscriptionRepository repository, ICustomerSubscriptionDetailRepository detailRepository, IUnitOfWork unitOfWork, IMapper mapper) : ServiceBase<CustomerSubscription>(repository),
        ICustomerSubscriptionService
{
    public Task<IEnumerable<ViewCustomerSubscription>> GetCustomerSubscription(int orgId)
    {
        return repository.GetCustomerSubscription(orgId);
    }

    public async Task<bool> SaveCustomerSubscription(CustomerSubscriptionCreateViewModel customerSubscription)
    {
	    var currentTime = DateTime.Now;
	    var subscription = new CustomerSubscription
	    {
		    OrganizationId = 7,
		    OrgBranchId = customerSubscription.OrgBranchId,
		    CollectionOfficeId = customerSubscription.CollectionOfficeId,
		    CustomerId = customerSubscription.CustomerId,
		    Remarks = customerSubscription.Remarks,
		    IsActive = true,
			CreatedBy = 10,
		    CreatedTime = currentTime
	    };
		repository.Insert(subscription);
		await unitOfWork.SaveChangesAsync();
		var subscriptionDetails = new List<CustomerSubscriptionDetail>();

		foreach (var detail in customerSubscription.CustomerSubscriptionDetails.Where(d => d.UnitPrice > 0))
		{
			var subscriptionDetail = new CustomerSubscriptionDetail
			{
				CustomerSubscriptionId = subscription.CustomerSubscriptionId,
				ProductId = detail.ProductId,
				MeasurementUnitId = detail.MeasurementUnitId,
				ConversionRatio = 1,
				Quantity = 1,
				UnitPrice = detail.UnitPrice,
				SupplementaryDutyPercent = detail.SupplementaryDutyPercent,
				ProductVatTypeId = detail.ProductVatTypeId,
				ProductVatPercent = detail.ProductVatPercent,
				CreatedBy = 10,
				CreatedTime = currentTime
			};
			subscriptionDetails.Add(subscriptionDetail);
		}

		await detailRepository.BulkInsertAsync(subscriptionDetails);
		await unitOfWork.SaveChangesAsync();
		return true;
    }

    public Task<IEnumerable<SpGetUnBilledSubscriptions>> GetUnBilledSubscription(int organizationId, int billingOfficeId, int collectionOfficeId, int year, int month)
    {
	    return repository.GetUnBilledSubscription(organizationId, billingOfficeId, collectionOfficeId, year, month);
    }

    public async Task<IEnumerable<SubscriptionBillDetailCreateViewModel>> GetSubscriptionBillDetailCreateViewModel(int organizationId, int billingOfficeId, int collectionOfficeId, int year,
	    int month)
    {
		var unBilledSubscriptions = await repository.GetUnBilledSubscription(organizationId, billingOfficeId, collectionOfficeId, year, month);

		return mapper.Map<IEnumerable<SpGetUnBilledSubscriptions>, IEnumerable<SubscriptionBillDetailCreateViewModel>>(
			unBilledSubscriptions);
    }
}