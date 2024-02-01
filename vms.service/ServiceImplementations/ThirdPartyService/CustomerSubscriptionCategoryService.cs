using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.ThirdPartyService;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.ThirdPartyService
{
	public class CustomerSubscriptionCategoryService : ServiceBase<CustomerSubscriptionCategory>, ICustomerSubscriptionCategoryService
    {
		private readonly ICustomerSubscriptionCategoryRepository _repository;
		private readonly IDataProtectionProvider _protectionProvider;
		private readonly PurposeStringConstants _purposeStringConstants;
		private IDataProtector _dataProtector;
		public CustomerSubscriptionCategoryService(ICustomerSubscriptionCategoryRepository repository, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(repository)
		{
			_repository = repository;
			_protectionProvider = p_protectionProvider;
			_purposeStringConstants = p_purposeStringConstants;
			_dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
		}

		public async Task<IEnumerable<CustomerSubscriptionCategory>> GetCustomerCategory(string orgIdEnc)
		{
			var custCategorys = await _repository.GetCustomerCategory(orgIdEnc);
			custCategorys.ToList().ForEach(delegate (CustomerSubscriptionCategory custCategory)
			{
				custCategory.EncryptedId = _dataProtector.Protect(custCategory.CustomerSubscriptionCategoryId.ToString());
			});
			return custCategorys;
		}
    }
}
