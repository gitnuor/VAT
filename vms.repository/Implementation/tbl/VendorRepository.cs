using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class VendorRepository : RepositoryBase<Vendor>, IVendorRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;

    public VendorRepository(DbContext context, IDataProtectionProvider p_protectionProvider,
        PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<Vendor>> GetVendorsByOrg(string pOrgId)
    {
        var id = int.Parse(_dataProtector.Unprotect(pOrgId));
        return await Query()
            .Where(v => v.OrganizationId == id)
            .SelectAsync();
    }

    public async Task<IEnumerable<ViewVendor>> GetVendorListByOrg(string pOrgId)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(pOrgId));
        var vendors = await _context.Set<ViewVendor>()
            .Where(s => s.OrganizationId == orgId)
            .OrderByDescending(s => s.VendorId)
            .AsNoTracking()
            .ToListAsync();
        vendors.ForEach(s => s.EncryptedId = _dataProtector.Protect(s.VendorId.ToString()));
        return vendors;
    }

    public async Task<IEnumerable<ViewVendor>> GetVendorListByOrg(int orgId)
    {
        var vendors = await _context.Set<ViewVendor>()
            .Where(s => s.OrganizationId == orgId)
            .OrderByDescending(s => s.VendorId)
            .AsNoTracking()
            .ToListAsync();
        vendors.ForEach(s => s.EncryptedId = _dataProtector.Protect(s.VendorId.ToString()));
        return vendors;
    }

    public async Task<IEnumerable<ViewVendorLocal>> GetVendorLocalListByOrg(string pOrgId)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(pOrgId));
        var vendors = await _context.Set<ViewVendorLocal>()
            .Where(s => s.OrganizationId == orgId)
            .OrderByDescending(s => s.VendorId)
            .AsNoTracking()
            .ToListAsync();
        vendors.ForEach(s => s.EncryptedId = _dataProtector.Protect(s.VendorId.ToString()));
        return vendors;
    }

    public async Task<IEnumerable<ViewVendorForeign>> GetVendorForeignListByOrg(string pOrgId)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(pOrgId));
        var vendors = await _context.Set<ViewVendorForeign>()
            .Where(s => s.OrganizationId == orgId)
            .OrderByDescending(s => s.VendorId)
            .AsNoTracking()
            .ToListAsync();
        vendors.ForEach(s => s.EncryptedId = _dataProtector.Protect(s.VendorId.ToString()));
        return vendors;
    }

    public async Task<IEnumerable<Vendor>> GetLocalVendorsByOrg(string pOrgId)
    {
        var id = int.Parse(_dataProtector.Unprotect(pOrgId));
        var vendors = await Query()
            .Where(v => v.OrganizationId == id && v.VendorTypeId != (int)EnumVendorType.Foreign)
            .SelectAsync();
        var vendorList = vendors.ToList();
        vendorList.ForEach(delegate(Vendor vendor)
        {
            vendor.EncryptedId = _dataProtector.Protect((vendor.VendorId.ToString()));
        });

        return vendorList;
    }

    public async Task<IEnumerable<Vendor>> GetForeignVendorsByOrg(string pOrgId)
    {
        var id = int.Parse(_dataProtector.Unprotect(pOrgId));
        var vendors = await Query()
            .Where(v => v.OrganizationId == id && v.VendorTypeId == (int)EnumVendorType.Foreign)
            .SelectAsync();
        var vendorList = vendors.ToList();
        vendorList.ForEach(delegate(Vendor vendor)
        {
            vendor.EncryptedId = _dataProtector.Protect((vendor.VendorId.ToString()));
        });

        return vendorList;
    }

    public async Task<Vendor> GetVendor(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var vendor = await Query()
            .SingleOrDefaultAsync(x => x.VendorId == id, System.Threading.CancellationToken.None);
        vendor.EncryptedId = _dataProtector.Protect(vendor.VendorId.ToString());

        return vendor;
    }

    public Task<Vendor> GetByReference(string refKey, int orgId)
    {
        return Query().FirstOrDefaultAsync(x => x.OrganizationId == orgId && x.ReferenceKey == refKey);
    }

    public Task<ViewVendorLocal> GetLocalVendorById(int id)
    {
        return _context.Set<ViewVendorLocal>().FirstOrDefaultAsync(x => x.VendorId == id);
    }

    public async Task<IEnumerable<Vendor>> GetVendors(int orgIdEnc)
    {
        var vendors = await Query().Where(w => w.OrganizationId == orgIdEnc).SelectAsync();

        vendors.ToList().ForEach(delegate(Vendor vendor)
        {
            vendor.EncryptedId = _dataProtector.Protect(vendor.VendorId.ToString());
        });
        return vendors;
    }
}