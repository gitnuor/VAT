using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IVendorRepository : IRepositoryBase<Vendor>
{
	Task<IEnumerable<Vendor>> GetVendors(int orgIdEnc);
	Task<IEnumerable<Vendor>> GetVendorsByOrg(string pOrgId);
	Task<IEnumerable<ViewVendor>> GetVendorListByOrg(string pOrgId);
	Task<IEnumerable<ViewVendor>> GetVendorListByOrg(int orgId);
	Task<IEnumerable<ViewVendorLocal>> GetVendorLocalListByOrg(string pOrgId);
	Task<IEnumerable<ViewVendorForeign>> GetVendorForeignListByOrg(string pOrgId);
	Task<IEnumerable<Vendor>> GetLocalVendorsByOrg(string pOrgId);
	Task<IEnumerable<Vendor>> GetForeignVendorsByOrg(string pOrgId);
	Task<Vendor> GetVendor(string idEnc);
	Task<Vendor> GetByReference(string refKey, int orgId);
    Task<ViewVendorLocal> GetLocalVendorById(int id);
}