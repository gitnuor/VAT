using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.Dto.Customer;
using vms.entity.models;
using vms.entity.viewModels;
using vms.entity.viewModels.CustomerViewModel;

namespace vms.service.Services.ThirdPartyService;

public interface ICustomerService : IServiceBase<Customer>
{
    Task<IEnumerable<Customer>> GetCustomers(int orgIdEnc);
    Task<IEnumerable<Customer>> GetCustomerByExportType(int orgIdEnc, int exportTypeId);
    Task<Customer> GetCustomer(string idEnc);
    Task<SelectList> GetLocalCustomersSelectList(string pOrgId);
    Task<SelectList> GetForeignCustomersSelectList(string pOrgId);
    Task<List<SelectListItem>> GetCustomerSelectListItems(string pOrgId);
    Task<CustomerManageViewModel> GetCustomerManageModel(string encId);

    Task<Customer> InsertLocalCustomer(CustomerLocalCreatePostViewModel model, OperatingUserViewModel operatingUser);
    Task<IEnumerable<ViewCustomer>> GetCustomerListByOrg(string pOrgId);
    Task<ViewCustomerLocal> GetCustomerLocalById(int id);
    Task<IEnumerable<ViewCustomerLocal>> GetCustomerLocalListByOrg(string pOrgId);
    Task<IEnumerable<ViewCustomerForeign>> GetCustomerForeignListByOrg(string pOrgId);
    Task<IEnumerable<CustomerDto>> GetCustomerDtoListByOrg(string pOrgId);
    Task<IEnumerable<CustomerLocalDto>> GetCustomerLocalDtoListByOrg(string pOrgId);
    Task<IEnumerable<CustomerForeignDto>> GetCustomerForeignDtoListByOrg(string pOrgId);
    Task<IEnumerable<ViewCustomerWithBranch>> GetCustomerWithBranchListByOrg(string pOrgId);
    Task<IEnumerable<ViewCustomerWithBranch>> GetCustomerWithBranchListByBranch(int orgId, int branchId);

    Task InsertCustomerBranches(IEnumerable<CustomerBranchCreateViewModel> customerBranches, int customerId,
        OperatingUserViewModel operatingUser);

    Task InsertCustomerDocuments(IEnumerable<VmsDocumentPostViewModel> documents, int customerId,
        OperatingUserViewModel operatingUser);

    Task<int> InsertOrUpdateCustomerLocalFromApi(CustomerLocalPostDto customer, string appKey);
}