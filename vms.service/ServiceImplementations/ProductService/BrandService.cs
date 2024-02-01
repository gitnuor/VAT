using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using vms.entity.models;
using vms.entity.Utility;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.ProductService;

namespace vms.service.ServiceImplementations.ProductService;

public class BrandService : ServiceBase<Brand>, IBrandService
{
	private readonly IBrandRepository _repository;
	private readonly IDataProtector _dataProtector;

	public BrandService(IBrandRepository repository, IDataProtectionProvider protectionProvider,
		PurposeStringConstants purposeStringConstants) : base(repository)
	{
		_repository = repository;
		_dataProtector = protectionProvider.CreateProtector(purposeStringConstants.UserIdQueryString);
	}


	public async Task<IEnumerable<CustomSelectListItem>> GetBrandSelectList(string orgId)
	{
		var brands = await _repository.GetBrandsByOrg(orgId);
		return brands.ConvertToCustomSelectList(nameof(Brand.BrandId),
			nameof(Brand.Name));
	}

	public async Task<IEnumerable<Brand>> GetBrandList(string pOrgId)
	{
		var brands = (await _repository.GetBrandsByOrg(pOrgId)).ToList();
		brands.ForEach(brand => { brand.EncryptedId = _dataProtector.Protect(brand.BrandId.ToString()); });
		return brands;
	}
}