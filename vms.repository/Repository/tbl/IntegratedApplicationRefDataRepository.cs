using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Implementation;

namespace vms.repository.Repository.tbl;

public class IntegratedApplicationRefDataRepository : RepositoryBase<IntegratedApplicationRefDatum>, IIntegratedApplicationRefDataRepository
{
	// private readonly DbContext _context;
	private readonly IDataProtector _dataProtector;

	public IntegratedApplicationRefDataRepository(DbContext context, IDataProtectionProvider protectionProvider,
		PurposeStringConstants purposeStringConstants) : base(context)
	{
		// _context = context;
		_dataProtector = protectionProvider.CreateProtector(purposeStringConstants.UserIdQueryString);
	}

	//public async Task<IEnumerable<IntegratedApplication>> GetIntegratedApplications(int orgId)
	//{
	//	var integratedApplications = (await Query().Where(x => x.OrganizationId == orgId).SelectAsync()).ToList();

	//	integratedApplications.ForEach(delegate(IntegratedApplication integratedApplication)
	//	{
	//		integratedApplication.EncryptedId =
	//			_dataProtector.Protect(integratedApplication.IntegratedApplicationId.ToString());
	//	});
	//	return integratedApplications;
	//}

	public async Task<IntegratedApplicationRefDatum> GetIntegratedApplicationRefDatum(string idEnc)
	{
		var id = int.Parse(_dataProtector.Unprotect(idEnc));
		var integratedApplicationRefDatum = await Query()
			.SingleOrDefaultAsync(x => x.IntegratedApplicationRefDataId == id, System.Threading.CancellationToken.None);
		integratedApplicationRefDatum.EncryptedId =
			_dataProtector.Protect(integratedApplicationRefDatum.IntegratedApplicationRefDataId.ToString());

		return integratedApplicationRefDatum;
	}

	//public async Task<IntegratedApplication> GetIntegratedApplicationByAppKey(string appKey)
	//{
	//	var integratedApplication = await Query()
	//		.SingleOrDefaultAsync(x => x.ApplicationKey == appKey, System.Threading.CancellationToken.None);
	//	integratedApplication.EncryptedId =
	//		_dataProtector.Protect(integratedApplication.IntegratedApplicationId.ToString());

	//	return integratedApplication;
	//}
}