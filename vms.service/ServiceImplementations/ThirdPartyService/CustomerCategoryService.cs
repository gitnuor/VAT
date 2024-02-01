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
	public class CustomerCategoryService: ServiceBase<CustomerCategory>, ICustomerCategoryService
	{
		private readonly ICustomerCategoryRepository _repository;
		private readonly IDataProtectionProvider _protectionProvider;
		private readonly PurposeStringConstants _purposeStringConstants;
		private IDataProtector _dataProtector;
		public CustomerCategoryService(ICustomerCategoryRepository repository, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(repository)
		{
			_repository = repository;
			_protectionProvider = p_protectionProvider;
			_purposeStringConstants = p_purposeStringConstants;
			_dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
		}

		public async Task<IEnumerable<CustomerCategory>> GetCustomerCategory(string orgIdEnc)
		{
			var custCategorys = await _repository.GetCustomerCategory(orgIdEnc);
			custCategorys.ToList().ForEach(delegate (CustomerCategory custCategory)
			{
				custCategory.EncryptedId = _dataProtector.Protect(custCategory.CustomerCategoryId.ToString());
			});
			return custCategorys;
		}
        //public async Task<CustomerCategory> GetPCat(string idEnc)
        //{
        //    return await _repository.GetCategory(idEnc);
        //}
    }
}
