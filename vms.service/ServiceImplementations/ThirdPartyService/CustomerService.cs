using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using URF.Core.Abstractions;
using vms.entity.Dto.Customer;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.entity.viewModels.CustomerViewModel;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;
using vms.service.Services.ThirdPartyService;
using vms.service.Services.UploadService;
using vms.utility.StaticData;

namespace vms.service.ServiceImplementations.ThirdPartyService;

public class CustomerService : ServiceBase<Customer>, ICustomerService
{
	private readonly ICustomerRepository _repository;
	private readonly ICustomerBranchService _customerBranchService;
	private readonly IFileOperationService _fileOperationService;
	private readonly IContentService _contentService;
	private readonly IContentRepository _contentRepository;
	private readonly IIntegratedApplicationRepository _integratedApplicationRepository;
	private readonly IIntegratedApplicationRefDataService _integratedApplicationRefDataService;
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;


	public CustomerService(ICustomerRepository repository, ICustomerBranchService customerBranchService,
		IFileOperationService fileOperationService, IContentService contentService,
		IContentRepository contentRepository, IMapper mapper,
		IIntegratedApplicationRepository integratedApplicationRepository,
		IUnitOfWork unitOfWork,
		IIntegratedApplicationRefDataService integratedApplicationRefDataService,
		IUserRepository userRepository) : base(repository)
	{
		_repository = repository;
		_customerBranchService = customerBranchService;
		_fileOperationService = fileOperationService;
		_contentService = contentService;
		_contentRepository = contentRepository;
		_mapper = mapper;
		_integratedApplicationRepository = integratedApplicationRepository;
		_integratedApplicationRefDataService = integratedApplicationRefDataService;
		_userRepository = userRepository;
		_unitOfWork = unitOfWork;
	}

	public async Task<Customer> GetCustomer(string idEnc)
	{
		return await _repository.GetCustomer(idEnc);
	}

	public async Task<IEnumerable<Customer>> GetCustomers(int orgIdEnc)
	{
		return await _repository.GetCustomers(orgIdEnc);
	}


	public async Task<IEnumerable<Customer>> GetCustomerByExportType(int orgIdEnc, int exportTypeId)
	{
		return await _repository.GetCustomerByExportType(orgIdEnc, exportTypeId);
	}

	public async Task<SelectList> GetLocalCustomersSelectList(string pOrgId)
	{
		return new(await _repository.GetLocalCustomersByOrg(pOrgId),
			nameof(Customer.CustomerId),
			nameof(Customer.Name));
	}

	public async Task<SelectList> GetForeignCustomersSelectList(string pOrgId)
	{
		return new(await _repository.GetForeignCustomersByOrg(pOrgId),
			nameof(Customer.CustomerId),
			nameof(Customer.Name));
	}

	public async Task<List<SelectListItem>> GetCustomerSelectListItems(string pOrgId)
	{
		var customers = await _repository.GetAllCustomersByOrg(pOrgId);
		return customers.Select(x => new SelectListItem
		{
			Text = x.Name,
			Value = x.CustomerId.ToString()
		}).ToList();
	}

	public async Task<CustomerManageViewModel> GetCustomerManageModel(string encId)
	{
		var model = new CustomerManageViewModel
		{
			Customer = await _repository.GetCustomer(encId)
		};
		model.Contents = await _contentRepository.Query().Where(x =>
			x.OrganizationId == model.Customer.OrganizationId && x.ObjectId == (int)EnumObjectType.Customer &&
			x.ObjectPrimaryKey == model.Customer.CustomerId).SelectAsync();


		return model;
	}

	public async Task<Customer> InsertLocalCustomer(CustomerLocalCreatePostViewModel model,
		OperatingUserViewModel operatingUser)
	{
		var customer = _mapper.Map<CustomerLocalCreatePostViewModel, Customer>(model);
		customer.IsActive = true;
		customer.CreatedBy = operatingUser.UserId;
		customer.OrganizationId = operatingUser.OrganizationId;
		customer.CreatedTime = operatingUser.OperationTime;
		await _repository.CustomInsertAsync(customer);

		return customer;
	}

	public async Task<IEnumerable<CustomerDto>> GetCustomerDtoListByOrg(string pOrgId)
	{
		var customer = await _repository.GetCustomerListByOrg(pOrgId);
		return _mapper.Map<IEnumerable<ViewCustomer>, IEnumerable<CustomerDto>>(customer);
	}

	public async Task<IEnumerable<CustomerLocalDto>> GetCustomerLocalDtoListByOrg(string pOrgId)
	{
		var customer = await _repository.GetCustomerLocalListByOrg(pOrgId);
		return _mapper.Map<IEnumerable<ViewCustomerLocal>, IEnumerable<CustomerLocalDto>>(customer);
	}

	public async Task<IEnumerable<CustomerForeignDto>> GetCustomerForeignDtoListByOrg(string pOrgId)
	{
		var customer = await _repository.GetCustomerForeignListByOrg(pOrgId);
		return _mapper.Map<IEnumerable<ViewCustomerForeign>, IEnumerable<CustomerForeignDto>>(customer);
	}

	public Task<IEnumerable<ViewCustomerWithBranch>> GetCustomerWithBranchListByOrg(string pOrgId)
	{
		return _repository.GetCustomerWithBranchListByOrg(pOrgId);
	}

	public Task<IEnumerable<ViewCustomerWithBranch>> GetCustomerWithBranchListByBranch(int orgId, int branchId)
	{
		return _repository.GetCustomerWithBranchListByBranch(branchId);
	}

	public Task InsertCustomerBranches(IEnumerable<CustomerBranchCreateViewModel> customerBranches, int customerId,
		OperatingUserViewModel operatingUser)
	{
		var branches = GetCustomerBranchesToSave(customerBranches, customerId, operatingUser);
		return branches.Any() ? _customerBranchService.BulkInsertAsync(branches) : Task.CompletedTask;
	}

	public async Task InsertCustomerDocuments(IEnumerable<VmsDocumentPostViewModel> documents, int customerId,
		OperatingUserViewModel operatingUser)
	{
		var documentToSave = GetFilesToSave(documents, customerId, operatingUser.OrganizationId);
		var documentSaveFeedBack = await _fileOperationService.SaveFiles(documentToSave);
		await _contentService.InsertCustomerContents(documentSaveFeedBack, operatingUser.OrganizationId,
			operatingUser.UserId, customerId);
	}

	public async Task<int> InsertOrUpdateCustomerLocalFromApi(CustomerLocalPostDto customer, string appKey)
	{
		var integratedApp = await _integratedApplicationRepository.GetIntegratedApplicationByAppKey(appKey);
		if (integratedApp == null)
			throw new Exception("Integrated application not found!");

		if (await _integratedApplicationRefDataService.IsReferenceDataExist(integratedApp.IntegratedApplicationId,
			    EnumReferenceDataType.Customer, customer.Id))
			throw new Exception("Data already exist for this id.!");

		var user = await _userRepository.GetUserByReference(customer.UserId, integratedApp.OrganizationId);
		var existingCustomer = await _repository.GetByReference(customer.Id, integratedApp.OrganizationId);
		int modifiedEntityId;
		if (existingCustomer == null)
		{
			var customerToInsert = _mapper.Map<CustomerLocalPostDto, Customer>(customer);
			customerToInsert.IsActive = true;
			customerToInsert.OrganizationId = integratedApp.OrganizationId;
			customerToInsert.CreatedBy = user?.UserId;
			customerToInsert.CreatedTime = DateTime.Now;
			await _repository.CustomInsertAsync(customerToInsert);
			await _unitOfWork.SaveChangesAsync();
			modifiedEntityId = customerToInsert.CustomerId;
		}
		else
		{
			existingCustomer = _mapper.Map(customer, existingCustomer);
			existingCustomer.ModifiedBy = user?.UserId;
			existingCustomer.ModifiedTime = DateTime.Now;
			_repository.Update(existingCustomer);
			await _unitOfWork.SaveChangesAsync();
			modifiedEntityId = existingCustomer.CustomerId;
		}

		await _integratedApplicationRefDataService.InsertReferenceData(EnumReferenceDataType.Customer,
			integratedApp.IntegratedApplicationId, modifiedEntityId, customer.Id);
		return modifiedEntityId;
	}

	public Task<IEnumerable<ViewCustomer>> GetCustomerListByOrg(string pOrgId)
	{
		return _repository.GetCustomerListByOrg(pOrgId);
	}

	public Task<ViewCustomerLocal> GetCustomerLocalById(int id)
	{
		return _repository.GetLocalCustomersById(id);
	}

	public Task<IEnumerable<ViewCustomerLocal>> GetCustomerLocalListByOrg(string pOrgId)
	{
		return _repository.GetCustomerLocalListByOrg(pOrgId);
	}

	public Task<IEnumerable<ViewCustomerForeign>> GetCustomerForeignListByOrg(string pOrgId)
	{
		return _repository.GetCustomerForeignListByOrg(pOrgId);
	}

	private List<CustomerBranch> GetCustomerBranchesToSave(IEnumerable<CustomerBranchCreateViewModel> customerBranches,
		int customerId,
		OperatingUserViewModel operatingUser)
	{
		return customerBranches.Where(b => b.IsSelected)
			.Select(branchCreateViewModel => new CustomerBranch
			{
				OrganizationId = operatingUser.OrganizationId,
				OrgBranchId = branchCreateViewModel.OrgBranchId,
				IsActive = true,
				CreatedBy = operatingUser.UserId,
				CreatedTime = DateTime.Now,
				CustomerId = customerId
			})
			.ToList();
	}

	private FileSaveDto GetFilesToSave(IEnumerable<VmsDocumentPostViewModel> documents, int customerId,
		int organizationId)
	{
		var dto = new FileSaveDto
		{
			FileRootPath = ControllerStaticData.FileRootPath,
			FileModulePath = ControllerStaticData.FileCustomerPath,
			OrganizationId = organizationId,
			TransactionId = customerId
		};

		foreach (var doc in documents)
		{
			var fileDocument = new FIleDocumentInfo
			{
				FormFIle = doc.UploadedFile,
				DocumentTypeId = doc.DocumentTypeId,
				DocumentRemarks = doc.DocumentRemarks
			};
			dto.FormFileList.Add(fileDocument);
		}

		return dto;
	}
}