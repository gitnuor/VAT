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

public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
{
	private readonly DbContext _context;
	private readonly IDataProtectionProvider _protectionProvider;
	private readonly PurposeStringConstants _purposeStringConstants;
	private readonly IDataProtector _dataProtector;

	public CustomerRepository(DbContext context, IDataProtectionProvider p_protectionProvider,
		PurposeStringConstants p_purposeStringConstants) : base(context)
	{
		_context = context;
		_protectionProvider = p_protectionProvider;
		_purposeStringConstants = p_purposeStringConstants;
		_dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
	}

	public async Task<Customer> GetCustomer(string idEnc)
	{
		int id = int.Parse(_dataProtector.Unprotect(idEnc));
		var customer = await Query()
			.SingleOrDefaultAsync(x => x.CustomerId == id, System.Threading.CancellationToken.None);
		customer.EncryptedId = _dataProtector.Protect(customer.CustomerId.ToString());

		return customer;
	}

	public async Task<IEnumerable<Customer>> GetAllCustomersByOrg(string pOrgId)
	{
		var id = int.Parse(_dataProtector.Unprotect(pOrgId));
		return await Query()
			.Where(v => v.OrganizationId == id)
			.SelectAsync();
	}

	public IQueryable<ViewCustomer> GetAllCustomer()
	{
		return _context.Set<ViewCustomer>().AsNoTracking();
	}

	public async Task<IEnumerable<ViewCustomer>> GetCustomerListByOrg(string pOrgId)
	{
		var orgId = int.Parse(_dataProtector.Unprotect(pOrgId));
		var customers = await _context.Set<ViewCustomer>()
			.Where(s => s.OrganizationId == orgId)
			.OrderByDescending(s => s.CustomerId)
			.AsNoTracking()
			.ToListAsync();
		customers.ForEach(s => s.EncryptedId = _dataProtector.Protect(s.CustomerId.ToString()));
		return customers;
	}

	public async Task<IEnumerable<ViewCustomerLocal>> GetCustomerLocalListByOrg(string pOrgId)
	{
		var orgId = int.Parse(_dataProtector.Unprotect(pOrgId));
		var customers = await _context.Set<ViewCustomerLocal>()
			.Where(s => s.OrganizationId == orgId)
			.OrderByDescending(s => s.CustomerId)
			.AsNoTracking()
			.ToListAsync();
		customers.ForEach(s => s.EncryptedId = _dataProtector.Protect(s.CustomerId.ToString()));
		return customers;
	}

	public async Task<IEnumerable<ViewCustomerForeign>> GetCustomerForeignListByOrg(string pOrgId)
	{
		var orgId = int.Parse(_dataProtector.Unprotect(pOrgId));
		var customers = await _context.Set<ViewCustomerForeign>()
			.Where(s => s.OrganizationId == orgId)
			.OrderByDescending(s => s.CustomerId)
			.AsNoTracking()
			.ToListAsync();
		customers.ForEach(s => s.EncryptedId = _dataProtector.Protect(s.CustomerId.ToString()));
		return customers;
	}

    public async Task<IEnumerable<ViewCustomerWithBranch>> GetCustomerWithBranchListByOrg(string pOrgId)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(pOrgId));
        var customers = await _context.Set<ViewCustomerWithBranch>()
            .Where(s => s.OrganizationId == orgId)
            .AsNoTracking()
            .ToListAsync();
        customers.ForEach(s => s.EncryptedId = _dataProtector.Protect(s.CustomerId.ToString()));
        return customers;
    }

    public async Task<IEnumerable<ViewCustomerWithBranch>> GetCustomerWithBranchListByBranch(int branchId)
    {
        var customers = await _context.Set<ViewCustomerWithBranch>()
            .Where(s => s.OrgBranchId == branchId)
            .AsNoTracking()
            .ToListAsync();
        customers.ForEach(s => s.EncryptedId = _dataProtector.Protect(s.CustomerId.ToString()));
        return customers;
    }

    public async Task<IEnumerable<Customer>> GetCustomers(int orgIdEnc)
	{
		var customers = await Query().Where(w => w.OrganizationId == orgIdEnc).SelectAsync();

		customers.ToList().ForEach(delegate(Customer customer)
		{
			customer.EncryptedId = _dataProtector.Protect(customer.CustomerId.ToString());
			customer.DeliveryAddress ??= customer.Address;
			customer.DeliveryContactPerson ??= customer.ContactPerson;
			customer.DeliveryContactPersonMobile ??= customer.ContactPersonMobile;
		});
		return customers;
	}

	public async Task<IEnumerable<Customer>> GetCustomerByExportType(int orgIdEnc, int exportTypeId)
	{
		var customers = await Query().Where(w =>
			w.OrganizationId == orgIdEnc && exportTypeId == (int)EnumExportType.Direct
				? w.IsForeignCustomer == true
				: w.IsFullExportOriented == true).SelectAsync();

		customers.ToList().ForEach(delegate(Customer customer)
		{
			customer.EncryptedId = _dataProtector.Protect(customer.CustomerId.ToString());
		});
		return customers;
	}

	public async Task<IEnumerable<Customer>> GetLocalCustomersByOrg(string pOrgId)
	{
		var id = int.Parse(_dataProtector.Unprotect(pOrgId));
		return await Query()
			.Where(v => v.OrganizationId == id && !v.IsForeignCustomer)
			.SelectAsync();
	}

	public async Task<IEnumerable<Customer>> GetForeignCustomersByOrg(string pOrgId)
	{
		var id = int.Parse(_dataProtector.Unprotect(pOrgId));
		return await Query()
			.Where(v => v.OrganizationId == id && v.IsForeignCustomer)
			.SelectAsync();
	}

	public Task<Customer> GetByReference(string refKey, int orgId)
	{
		return Query().FirstOrDefaultAsync(x => x.OrganizationId == orgId && x.ReferenceKey == refKey);
	}

	public Task<ViewCustomerLocal> GetLocalCustomersById(int id)
    {
        return _context.Set<ViewCustomerLocal>().FirstOrDefaultAsync(x => x.CustomerId == id);
    }
}