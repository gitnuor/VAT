using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class SalesDetailRepository : RepositoryBase<SalesDetail>, ISalesDetailRepository
{
	private readonly DbContext _context;
	private readonly IDataProtectionProvider _protectionProvider;
	private readonly PurposeStringConstants _purposeStringConstants;
	private IDataProtector _dataProtector;

	public SalesDetailRepository(DbContext context, IDataProtectionProvider p_protectionProvider,
		PurposeStringConstants p_purposeStringConstants) : base(context)
	{
		_context = context;
		_protectionProvider = p_protectionProvider;
		_purposeStringConstants = p_purposeStringConstants;
		_dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
	}

	public async Task<IEnumerable<SalesDetail>> GetAllSalesDetails(string idEnc)
	{
		int id = int.Parse(_dataProtector.Unprotect(idEnc));
		var salesDetails = await Query()
			.Include(sd => sd.Product)
			.Include(sd => sd.Product.Brand)
			.Include(sd => sd.Product.ProductType)
			.Include(sd => sd.Product.ProductCategory)
			.Include(sd => sd.MeasurementUnit)
			.Include(c => c.ProductVattype).Where(sd => sd.SalesId == id)
			.Include(c => c.CreditNoteDetails)
			.Include(c => c.ProductTransactionBook)
			.SelectAsync();
		salesDetails.ToList().ForEach(delegate(SalesDetail sales)
		{
			sales.EncryptedId = _dataProtector.Protect(sales.SalesId.ToString());
		});
		return salesDetails;
	}

	public async Task<IEnumerable<SalesDetail>> GetSalesDetails(string idEnc)
	{
		int id = int.Parse(_dataProtector.Unprotect(idEnc));
		var purchase = await Queryable()
			.Include(x => x.DamageDetails)
			.Include(x => x.Product)
			.Where(x => x.SalesId == id).ToListAsync();
		//.ToListAync();
		// purchase.EncryptedId = _dataProtector.Protect(purchase.PurchaseId.ToString());

		return purchase;
		// throw new NotImplementedException();
	}

	public async Task<IEnumerable<ViewSalesDetail>> GetSalesDetailsBySales(string salesIdEnc)
	{
		var salesId = int.Parse(_dataProtector.Unprotect(salesIdEnc));
		var list = await _context.Set<ViewSalesDetail>()
			.Where(s => s.SalesId == salesId)
			.OrderByDescending(s => s.SalesId)
			.AsNoTracking()
			.ToListAsync();
		list.ForEach(s => s.EncryptedId = _dataProtector.Protect(s.SalesId.ToString()));
		return list;
	}
}