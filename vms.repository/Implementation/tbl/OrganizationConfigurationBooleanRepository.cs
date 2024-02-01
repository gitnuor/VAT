using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl
{
    public class OrganizationConfigurationBooleanRepository : RepositoryBase<ViewOrganizationConfigurationBoolean>, IOrganizationConfigurationBooleanRepository
	{
		private readonly DbContext _context;
		private readonly IDataProtectionProvider _protectionProvider;
		private readonly PurposeStringConstants _purposeStringConstants;
		private readonly IDataProtector _dataProtector;

		public OrganizationConfigurationBooleanRepository(DbContext context, IDataProtectionProvider p_protectionProvider,
			PurposeStringConstants p_purposeStringConstants) : base(context)
		{
			_context = context;
			_protectionProvider = p_protectionProvider;
			_purposeStringConstants = p_purposeStringConstants;
			_dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
		}

		public async Task<IEnumerable<ViewOrganizationConfigurationBoolean>> GetOrganizationConfigurationBoolean(string orgIdEnc)
		{
			var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));

			var list = await _context.Set<ViewOrganizationConfigurationBoolean>()
				.Where(s => s.OrganizationId == orgId)
				.OrderByDescending(s => s.OrganizationConfigurationBooleanTypeId)
				.AsNoTracking()
				.ToListAsync();
			return list;
		}
	}
}
