using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl
{
    public class ProductMeasurementUnitRepository : RepositoryBase<ProductMeasurementUnit>, IProductMeasurementUnitRepository
	{
		private readonly DbContext _context;
		private readonly IDataProtectionProvider _protectionProvider;
		private readonly PurposeStringConstants _purposeStringConstants;
		private IDataProtector _dataProtector;

		public ProductMeasurementUnitRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
		{
			_context = context;
			_protectionProvider = p_protectionProvider;
			_purposeStringConstants = p_purposeStringConstants;
			_dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
		}

		public async Task<IEnumerable<ProductMeasurementUnit>> GetProductMeasurementUnits(string orgIdEnc)
		{
			int id = int.Parse(_dataProtector.Unprotect(orgIdEnc));
			var measurementUnits = await Query().Where(w => w.OrganizationId == id)
				.Include(o => o.Product)
				.Include(o => o.MeasurementUnit)
				.SelectAsync();

			measurementUnits.ToList().ForEach(delegate (ProductMeasurementUnit measurementUnit)
			{
				measurementUnit.EncryptedId = _dataProtector.Protect(measurementUnit.ProductMeasurementUnitId.ToString());
			});
			return measurementUnits;
		}
		public async Task<ProductMeasurementUnit> GetProductMeasurementUnit(string idEnc)
		{
			int id = int.Parse(_dataProtector.Unprotect(idEnc));
			var measurementUnit = await Query()
				.Include(o => o.Product)
				.Include(o => o.MeasurementUnit)
				.SingleOrDefaultAsync(x => x.ProductMeasurementUnitId == id, System.Threading.CancellationToken.None);
			measurementUnit.EncryptedId = _dataProtector.Protect(measurementUnit.ProductMeasurementUnitId.ToString());

			return measurementUnit;
		}


		public Task<IEnumerable<SpGetMeasurementUnitByProductModel>> SpGetMeasurementUnitByProduct(int productId)
		{
			try
			{
				const string sql = "EXEC [dbo].[SpGetMeasurementUnitByProduct]" +
								   "@ProductId ";
				var parameter = new DynamicParameters();
				parameter.Add("@ProductId", productId);
				using (var queryMultiple =
					   _context.Database.GetDbConnection().QueryMultiple(sql, parameter, commandTimeout: 500))
				{
					return queryMultiple.ReadAsync<SpGetMeasurementUnitByProductModel>();
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}
