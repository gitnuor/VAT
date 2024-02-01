using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using URF.Core.Abstractions;
using vms.entity.Dto.Vendor;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.Utility;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;
using vms.service.Services.ThirdPartyService;

namespace vms.service.ServiceImplementations.ThirdPartyService;

public class VendorService : ServiceBase<Vendor>, IVendorService
{
	private readonly IVendorRepository _repository;
	private readonly IMapper _mapper;
	private readonly IIntegratedApplicationRepository _integratedApplicationRepository;
	private readonly IIntegratedApplicationRefDataService _integratedApplicationRefDataService;
	private readonly IUserRepository _userRepository;
	private readonly IUnitOfWork _unitOfWork;

	public VendorService(IVendorRepository repository, IMapper mapper,
		IIntegratedApplicationRepository integratedApplicationRepository, IUnitOfWork unitOfWork,
		IIntegratedApplicationRefDataService integratedApplicationRefDataService,
		IUserRepository userRepository) : base(repository)
	{
		_repository = repository;
		_mapper = mapper;
		_integratedApplicationRepository = integratedApplicationRepository;
		_integratedApplicationRefDataService = integratedApplicationRefDataService;
		_userRepository = userRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<IEnumerable<CustomSelectListItem>> GetLocalVendorSelectList(string pOrgId)
	{
		var vendors = await _repository.GetLocalVendorsByOrg(pOrgId);
		return vendors.ConvertToCustomSelectList(nameof(Vendor.VendorId),
			nameof(Vendor.Name));
	}

	public async Task<IEnumerable<CustomSelectListItem>> GetLocalForeignSelectList(string pOrgId)
	{
		var vendors = await _repository.GetForeignVendorsByOrg(pOrgId);
		return vendors.ConvertToCustomSelectList(nameof(Vendor.VendorId),
			nameof(Vendor.Name));
	}

    public Task<ViewVendorLocal> GetVendorLocalById(int id)
    {
        return _repository.GetLocalVendorById(id);
    }

    public Task<IEnumerable<ViewVendor>> GetVendorListByOrg(string pOrgId)
	{
		return _repository.GetVendorListByOrg(pOrgId);
	}

	public Task<IEnumerable<ViewVendor>> GetVendorListByOrg(int orgId)
	{
		return _repository.GetVendorListByOrg(orgId);
	}

	public Task<IEnumerable<ViewVendorLocal>> GetVendorLocalListByOrg(string pOrgId)
	{
		return _repository.GetVendorLocalListByOrg(pOrgId);
	}

	public Task<IEnumerable<ViewVendorForeign>> GetVendorForeignListByOrg(string pOrgId)
	{
		return _repository.GetVendorForeignListByOrg(pOrgId);
	}

	public async Task<IEnumerable<VendorDto>> GetVendorDtoListByOrg(string pOrgId)
	{
		var vendor = await _repository.GetVendorListByOrg(pOrgId);
		return _mapper.Map<IEnumerable<ViewVendor>, IEnumerable<VendorDto>>(vendor);
	}

	public async Task<IEnumerable<VendorLocalDto>> GetVendorLocalDtoListByOrg(string pOrgId)
	{
		var vendor = await _repository.GetVendorLocalListByOrg(pOrgId);
		return _mapper.Map<IEnumerable<ViewVendorLocal>, IEnumerable<VendorLocalDto>>(vendor);
	}

	public async Task<IEnumerable<VendorForeignDto>> GetVendorForeignDtoListByOrg(string pOrgId)
	{
		var vendor = await _repository.GetVendorForeignListByOrg(pOrgId);
		return _mapper.Map<IEnumerable<ViewVendorForeign>, IEnumerable<VendorForeignDto>>(vendor);
	}

	public async Task<Vendor> GetVendor(string idEnc)
	{
		return await _repository.GetVendor(idEnc);
	}

	public async Task<IEnumerable<CustomSelectListItem>> GetVendorSelectListItems(string pOrgId)
	{
		var vendors = await _repository.GetVendorsByOrg(pOrgId);
		return vendors.ConvertToCustomSelectList(nameof(Vendor.VendorId),
			nameof(Vendor.Name));
	}

	public async Task<IEnumerable<Vendor>> GetVendors(int orgIdEnc)
	{
		return await _repository.GetVendors(orgIdEnc);
	}


	#region Vendor data insert from API

	public async Task<int> InsertOrUpdateVendorLocalFromApi(VendorLocalPostDto vendorLocalDto, string appKey)
	{
		var integratedApp = await _integratedApplicationRepository.GetIntegratedApplicationByAppKey(appKey);
		if (integratedApp == null)
			throw new Exception("Integrated application not found!");

		if (await _integratedApplicationRefDataService.IsReferenceDataExist(integratedApp.IntegratedApplicationId,
			    EnumReferenceDataType.Vendor, vendorLocalDto.Id))
			throw new Exception("Data already exist for this id.!");

		var user = await _userRepository.GetUserByReference(vendorLocalDto.UserId, integratedApp.OrganizationId);
		var existingVendor = await _repository.GetByReference(vendorLocalDto.Id, integratedApp.OrganizationId);

		int modifiedEntityId;

		if (existingVendor == null)
		{
			var vendorToInsert = _mapper.Map<VendorLocalPostDto, Vendor>(vendorLocalDto);
			vendorToInsert.VendorTypeId = (int)EnumVendorType.Registered;
			vendorToInsert.IsActive = true;
			vendorToInsert.OrganizationId = integratedApp.OrganizationId;
			vendorToInsert.CreatedBy = user?.UserId;
			vendorToInsert.CreatedTime = DateTime.Now;
			await _repository.CustomInsertAsync(vendorToInsert);
			await _unitOfWork.SaveChangesAsync();
			modifiedEntityId = vendorToInsert.VendorId;
		}
		else
		{
			existingVendor = _mapper.Map(vendorLocalDto, existingVendor);
			existingVendor.ModifiedBy = user?.UserId;
			existingVendor.ModifiedTime = DateTime.Now;
			_repository.Update(existingVendor);
			await _unitOfWork.SaveChangesAsync();
			modifiedEntityId = existingVendor.VendorId;
		}

		await _integratedApplicationRefDataService.InsertReferenceData(EnumReferenceDataType.Vendor,
			integratedApp.IntegratedApplicationId, modifiedEntityId, vendorLocalDto.Id);
        return modifiedEntityId;
    }

	#endregion
}