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
	public class VendorCategoryService: ServiceBase<VendorCategory>, IVendorCategoryService
	{
		private readonly IVendorCategoryRepository _repository;
		private readonly IDataProtectionProvider _protectionProvider;
		private readonly PurposeStringConstants _purposeStringConstants;
		private IDataProtector _dataProtector;
		public VendorCategoryService(IVendorCategoryRepository repository, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(repository)
		{
			_repository = repository;
			_protectionProvider = p_protectionProvider;
			_purposeStringConstants = p_purposeStringConstants;
			_dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
		}

		public async Task<IEnumerable<VendorCategory>> GetVendorCategory(string orgIdEnc)
		{
			var vendorCategorys = await _repository.GetVendorCategory(orgIdEnc);
			vendorCategorys.ToList().ForEach(delegate (VendorCategory vendorCategory)
			{
				vendorCategory.EncryptedId = _dataProtector.Protect(vendorCategory.VendorCategoryId.ToString());
			});
			return vendorCategorys;
		}
	}
}
