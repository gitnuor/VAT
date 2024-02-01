using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class SalesPriceAdjustmentDetailRepository : RepositoryBase<SalesPriceAdjustmentDetail>, ISalesPriceAdjustmentDetailRepository
{
	private readonly IDataProtector _dataProtector;

    public SalesPriceAdjustmentDetailRepository(DbContext context, IDataProtectionProvider protectionProvider, PurposeStringConstants purposeStringConstants) : base(context)
    {
	    _dataProtector = protectionProvider.CreateProtector(purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<SalesPriceAdjustmentDetail>> GetSalesPriceAdjustmentDetailsBySalesPriceAdjustment(string salesPriceAdjustmentIdEnc)
	{
		var id = int.Parse(_dataProtector.Unprotect(salesPriceAdjustmentIdEnc));
		var salesPriceAdjustmentDetails = await Query()
			.Where(a => a.SalesPriceAdjustmentId == id)
			.Include(a => a.Product)
			.Include(a => a.SalesDetail)
			.Include(a => a.MeasurementUnit)
			.SelectAsync();
		var list = salesPriceAdjustmentDetails.ToList();
		list.ForEach(delegate (SalesPriceAdjustmentDetail salesPriceAdjustmentDetail)
		{
			salesPriceAdjustmentDetail.EncryptedId = _dataProtector.Protect(salesPriceAdjustmentDetail.SalesPriceAdjustmentDetailId.ToString());
		});
		return list;
	}
}