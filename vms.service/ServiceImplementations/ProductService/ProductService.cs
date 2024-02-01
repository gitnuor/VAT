using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.Dto.Product;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.ProductService;
using vms.entity.Utility;
using System;
using vms.entity.Dto.Customer;
using vms.entity.Enums;
using vms.service.Services.SettingService;
using URF.Core.Abstractions;
using System.Linq;
using URF.Core.EF;

namespace vms.service.ServiceImplementations.ProductService;

public class ProductService : ServiceBase<Product>, IProductService
{
	private readonly IProductRepository _repository;
	private readonly IMapper _mapper;
	private readonly IIntegratedApplicationRepository _integratedApplicationRepository;
	private readonly IIntegratedApplicationRefDataService _integratedApplicationRefDataService;
	private readonly IUnitOfWork _iUnitOfWork;
	private readonly IProductCategoryService _productCategoryService;
	private readonly IProductGroupService _productGroupService;
	private readonly IMeasurementUnitService _measurementUnitService;

	public ProductService(IProductRepository repository, IMapper mapper, IIntegratedApplicationRepository integratedApplicationRepository, IUnitOfWork iUnitOfWork, IIntegratedApplicationRefDataService integratedApplicationRefDataService, IProductCategoryService productCategoryService, IProductGroupService productGroupService, IMeasurementUnitService measurementUnitService) : base(repository)
	{
		_repository = repository;
		_mapper = mapper;
		_integratedApplicationRepository = integratedApplicationRepository;
		_integratedApplicationRefDataService = integratedApplicationRefDataService;
		_iUnitOfWork = iUnitOfWork;
		_productCategoryService = productCategoryService;
		_productGroupService = productGroupService;
		_measurementUnitService = measurementUnitService;
	}

	public async Task<IEnumerable<Product>> GetProducts(string orgIdEnc)
	{
		return await _repository.GetProducts(orgIdEnc);
	}

	public async Task<Product> GetProduct(string idEnc)
	{
		return await _repository.GetProduct(idEnc);
	}

	public Task<IEnumerable<ViewProduct>> GetProductListByOrg(string orgIdEnc)
	{
		return _repository.GetProductListByOrg(orgIdEnc);
	}

	public async Task<IEnumerable<ProductDto>> GetProductDtoListByOrg(string orgIdEnc)
	{
		var product = await _repository.GetProductListByOrg(orgIdEnc);
		return _mapper.Map<IEnumerable<ViewProduct>, IEnumerable<ProductDto>>(product);
	}

	public vmProduct GetProductAutoComplete()
	{
		return _repository.GetProductAutoComplete();
	}

	public async Task<IEnumerable<CustomSelectListItem>> GetProductsSelectList(string orgId)
	{
		var products = await _repository.GetProducts(orgId);
		return products.ConvertToCustomSelectList(nameof(Product.ProductId),
			nameof(Product.Name));
	}

	public List<SpGetProductForSelfProductionReceive> SpGetProductForSelfProductionReceive(int orgId)
	{
		return _repository.SpGetProductForSelfProductionReceive(orgId);
	}

	public List<SpGetProductForContractualProductionReceive> SpGetProductForContractualProductionReceive(int orgId,
		int conProId)
	{
		return _repository.SpGetProductForContractualProductionReceive(orgId, conProId);
	}

	public async Task<IEnumerable<Product>> GetProductforDamage(int orgId)
	{
		return await _repository.Query().Where(x => x.OrganizationId == orgId && x.ProductType.IsInventory == true)
			.SelectAsync();
	}

	public async Task InsertProduct(ProductPostDto productDto, string appKey)
	{
		#region integrated application validation check

		var integratedApp = await _integratedApplicationRepository.GetIntegratedApplicationByAppKey(appKey);
		if (integratedApp == null)
			throw new Exception("Integrated application not found!");

		if (await _integratedApplicationRefDataService.IsReferenceDataExist(integratedApp.IntegratedApplicationId, EnumReferenceDataType.Product, productDto.Id))
			throw new Exception("Data already exist for this id.!");

		#endregion

		var product = _mapper.Map<ProductPostDto, Product>(productDto);
		product.IsActive = true;
		product.OrganizationId = integratedApp.OrganizationId;
		product.EffectiveFrom = DateTime.Now;

		product.ReferenceKey = appKey;

		#region Product group, type, measurement unit check

		var productGroups = await _productGroupService.GetProductGroupByName(product.OrganizationId, productDto.ProductGroupName);
		if (productGroups == null)
		{
			ProductGroup productGroup = new ProductGroup();
			productGroup.Name = productDto.ProductGroupName;
			productGroup.OrganizationId = product.OrganizationId;
			productGroup.CreatedBy = null;
			productGroup.CreatedTime = DateTime.Now;
			productGroup.IsActive = true;
			productGroup.ReferenceKey = productGroup.ReferenceKey;
			_productGroupService.Insert(productGroup);
			await _iUnitOfWork.SaveChangesAsync();
			product.ProductGroupId = productGroup.ProductGroupId;
		}
		else
		{
			product.ProductGroupId = productGroups.ProductGroupId;
		}

		var productCategories = await _productCategoryService.GetPCatByName(product.OrganizationId, productDto.ProductCategoryName);
		if (productCategories == null)
		{
			ProductCategory productCategory = new ProductCategory();
			productCategory.Name = productDto.ProductCategoryName;
			productCategory.ReferenceKey = productCategory.ReferenceKey;
			productCategory.IsActive = true;
			productCategory.OrganizationId = product.OrganizationId;
			productCategory.CreatedBy = null;
			productCategory.CreatedTime = DateTime.Now;
			_productCategoryService.Insert(productCategory);
			await _iUnitOfWork.SaveChangesAsync();
			product.ProductCategoryId = productCategory.ProductCategoryId;
		}
		else
		{
			product.ProductCategoryId = productCategories.ProductCategoryId;
		}

		var units = await _measurementUnitService.GetMeasurementUnitByName(product.OrganizationId, productDto.MeasurementUnitName);
		if (units == null)
		{
			MeasurementUnit mUnit = new MeasurementUnit();
			mUnit.Name = productDto.MeasurementUnitName;
			mUnit.CreatedBy = null;
			mUnit.CreatedTime = DateTime.Now;
			mUnit.IsActive = true;
			mUnit.OrganizationId = product.OrganizationId;
			_measurementUnitService.Insert(mUnit);
			await _iUnitOfWork.SaveChangesAsync();
			product.MeasurementUnitId = mUnit.MeasurementUnitId;
		}
		else
		{
			product.MeasurementUnitId = units.MeasurementUnitId;
		}

		#endregion

		await _repository.CustomInsertAsync(product);
		await _iUnitOfWork.SaveChangesAsync();
		await _integratedApplicationRefDataService.InsertReferenceData(EnumReferenceDataType.Product,
			integratedApp.IntegratedApplicationId, product.ProductId, productDto.Id);
	}
}