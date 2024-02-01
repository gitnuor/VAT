using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.Dto.Vendor;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.ThirdPartyService;

public interface IVendorService : IServiceBase<Vendor>
{
    Task<IEnumerable<Vendor>> GetVendors(int orgIdEnc);
    Task<IEnumerable<CustomSelectListItem>> GetLocalVendorSelectList(string pOrgId);
    Task<IEnumerable<CustomSelectListItem>> GetLocalForeignSelectList(string pOrgId);

    Task<IEnumerable<ViewVendor>> GetVendorListByOrg(int orgId);
    Task<ViewVendorLocal> GetVendorLocalById(int id);
    Task<IEnumerable<ViewVendor>> GetVendorListByOrg(string pOrgId);
    Task<IEnumerable<ViewVendorLocal>> GetVendorLocalListByOrg(string pOrgId);
    Task<IEnumerable<ViewVendorForeign>> GetVendorForeignListByOrg(string pOrgId);

    Task<IEnumerable<VendorDto>> GetVendorDtoListByOrg(string pOrgId);
    Task<IEnumerable<VendorLocalDto>> GetVendorLocalDtoListByOrg(string pOrgId);
    Task<IEnumerable<VendorForeignDto>> GetVendorForeignDtoListByOrg(string pOrgId);

    Task<Vendor> GetVendor(string idEnc);
    Task<IEnumerable<CustomSelectListItem>> GetVendorSelectListItems(string pOrgId);

	Task<int> InsertOrUpdateVendorLocalFromApi(VendorLocalPostDto vendorLocal, string appKey);
}