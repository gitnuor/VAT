using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.viewModels;
using vms.entity.viewModels.ProductViewModel;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class ProductRepository : RepositoryBase<Product>, IProductRepository
{
	private readonly DbContext _context;
	private readonly IDataProtectionProvider _protectionProvider;
	private readonly PurposeStringConstants _purposeStringConstants;
	private IDataProtector _dataProtector;

	public ProductRepository(DbContext context, IDataProtectionProvider p_protectionProvider,
		PurposeStringConstants p_purposeStringConstants) : base(context)
	{
		_context = context;
		_protectionProvider = p_protectionProvider;
		_purposeStringConstants = p_purposeStringConstants;
		_dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
	}


	public async Task<IEnumerable<Product>> GetProducts(string orgIdEnc)
	{
		int id = int.Parse(_dataProtector.Unprotect(orgIdEnc));
		var products = await Query().Where(w => w.OrganizationId == id).Include(p => p.MeasurementUnit)
			.Include(p => p.Organization)
			.Include(p => p.ProductGroup)
			.Include(p => p.ProductCategory)
			.Include(p => p.PriceSetups)
			.Include(p => p.ProductType)
			.Include(p => p.Brand)
			.SelectAsync();


		products.ToList().ForEach(delegate(Product product)
		{
			product.EncryptedId = _dataProtector.Protect(product.ProductId.ToString());
		});
		return products;
	}

	public async Task<IEnumerable<ViewProduct>> GetProductListByOrg(string orgIdEnc)
	{
		var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
		var list = await _context.Set<ViewProduct>()
			.Where(s => s.OrganizationId == orgId)
			.AsNoTracking()
			.ToListAsync();
		list.ForEach(s => s.EncryptedId = _dataProtector.Protect(s.ProductId.ToString()));
		return list;
	}

	public async Task<Product> GetProduct(string idEnc)
	{
		int id = int.Parse(_dataProtector.Unprotect(idEnc));
		var product = await Query()
			.Include(p => p.Organization)
			.Include(p => p.ProductGroup)
			.Include(p => p.ProductCategory)
			.Include(p => p.MeasurementUnit)
			.Include(p => p.PriceSetups)
			.Include(p => p.ProductVats)
			.Include(p => p.ProductType)
			.Include(p => p.Brand)
			.SingleOrDefaultAsync(x => x.ProductId == id, System.Threading.CancellationToken.None);
		product.EncryptedId = _dataProtector.Protect(product.ProductId.ToString());

		return product;
	}

	

	public List<SpGetProductForSelfProductionReceive> SpGetProductForSelfProductionReceive(int orgId)
	{
		try
		{
			const string sql = "EXEC [dbo].[SpGetProductForSelfProductionReceive]" +
			                   "@OrganizationId ";
			var parameter = new DynamicParameters();
			parameter.Add("@OrganizationId", orgId);
			using (var queryMultiple =
			       _context.Database.GetDbConnection().QueryMultiple(sql, parameter, commandTimeout: 500))
			{
				return queryMultiple.Read<SpGetProductForSelfProductionReceive>().ToList();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public List<SpGetProductForContractualProductionReceive> SpGetProductForContractualProductionReceive(int orgId,
		int conProId)
	{
		try
		{
			const string sql = "EXEC [dbo].[SpGetProductForContractualProductionReceive]  " +
			                   "@OrganizationId,"
			                   + "@ContractualProductionId";
			var parameter = new DynamicParameters();
			parameter.Add("@OrganizationId", orgId);
			parameter.Add("@ContractualProductionId", conProId);
			using (var queryMultiple =
			       _context.Database.GetDbConnection().QueryMultiple(sql, parameter, commandTimeout: 500))
			{
				return queryMultiple.Read<SpGetProductForContractualProductionReceive>().ToList();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public async Task<bool> InsertBulk(List<ProductDataImportViewModel> products, int organizationId, int createBy)
	{
		try
		{
			var createdTime = DateTime.Now;
			var jsonitems = string.Empty;
			if (products != null)
				jsonitems = Newtonsoft.Json.JsonConvert.SerializeObject(products);

			await _context.Database.ExecuteSqlRawAsync(
				$"EXEC [dbo].[SpInsertProductsWithRelatedData]  " +
				$"@OrganizationId," +
				$"@ProductsDetailsJson," +
				$"@CreateBy," +
				$"@CreatedTime"
				, new SqlParameter("@OrganizationId", (object)organizationId ?? DBNull.Value)
				, new SqlParameter("@ProductsDetailsJson", (object)jsonitems ?? DBNull.Value)
				, new SqlParameter("@CreateBy", (object)createBy ?? DBNull.Value)
				, new SqlParameter("@CreatedTime", (object)createdTime ?? DBNull.Value)
			);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}

		return await Task.FromResult(true);
	}

	public vmProduct GetProductAutoComplete()
	{
		var objectType = new vmProduct
		{
			ProductId = 1,
			Name = "Test",
			//Amount = 50,
			MeasurementUnitId = 1
		};
		new vmProduct
		{
			ProductId = 2,
			Name = "av",
			//Amount = 500,
			MeasurementUnitId = 1
		};
		new vmProduct
		{
		};
		return null;
	}
}