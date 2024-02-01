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
using vms.entity.viewModels.SalesPriceAdjustment;
using vms.repository.Repository.tbl;
using vms.utility;

namespace vms.repository.Implementation.tbl;

public class SalesPriceAdjustmentRepository : RepositoryBase<SalesPriceAdjustment>, ISalesPriceAdjustmentRepository
{
	private readonly DbContext _context;
	private readonly IDataProtector _dataProtector;

	public SalesPriceAdjustmentRepository(DbContext context, IDataProtectionProvider protectionProvider,
		PurposeStringConstants purposeStringConstants) : base(context)
	{
		_context = context;
		_dataProtector = protectionProvider.CreateProtector(purposeStringConstants.UserIdQueryString);
	}

	public async Task<IEnumerable<SalesPriceAdjustment>> GetSalesPriceAdjustmentsByOrganization(string orgIdEnc)
	{
		var id = int.Parse(_dataProtector.Unprotect(orgIdEnc));
		var salesPriceAdjustments = await Query()
			.Where(a => a.OrganizationId == id)
			.Include(a => a.Sales)
			.Include(a => a.OrgBranch)
			.SelectAsync();
		var list = salesPriceAdjustments.ToList();
		list.ForEach(delegate(SalesPriceAdjustment salesPriceAdjustment)
		{
			salesPriceAdjustment.EncryptedId =
				_dataProtector.Protect(salesPriceAdjustment.SalesPriceAdjustmentId.ToString());
		});
		return list;
	}

    public async Task<IEnumerable<SalesPriceAdjustment>> GetSalesPriceAdjustmentsByOrganizationAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch)
    {
        var id = int.Parse(_dataProtector.Unprotect(orgIdEnc));
        var salesPriceAdjustments = await Query()
            .Where(a => a.OrganizationId == id)
            .Include(a => a.Sales)
            .Include(a => a.OrgBranch)
            .SelectAsync();
        var list = salesPriceAdjustments.ToList();
        if (isRequiredBranch)
        {
            salesPriceAdjustments = salesPriceAdjustments.Where(s => branchIds.Contains(s.OrgBranchId.Value)).ToList();
        }
        list.ForEach(delegate (SalesPriceAdjustment salesPriceAdjustment)
        {
            salesPriceAdjustment.EncryptedId =
                _dataProtector.Protect(salesPriceAdjustment.SalesPriceAdjustmentId.ToString());
        });
        return list;
    }

    public async Task<IEnumerable<SalesPriceAdjustment>> GetSalesPriceDecrementedAdjustmentsByOrganization(string orgIdEnc)
	{
		var id = int.Parse(_dataProtector.Unprotect(orgIdEnc));
		var salesPriceAdjustments = await Query()
			.Where(a => a.OrganizationId == id && a.AdjustmentTypeId == 2)
			.Include(a => a.Sales.Customer)
			.Include(a => a.Organization)
			.Include(a => a.OrgBranch)
			.SelectAsync();
		var list = salesPriceAdjustments.ToList();
		list.ForEach(delegate (SalesPriceAdjustment salesPriceAdjustment)
		{
			salesPriceAdjustment.EncryptedId =
				_dataProtector.Protect(salesPriceAdjustment.SalesPriceAdjustmentId.ToString());
		});
		return list;
	}

    public async Task<IEnumerable<SalesPriceAdjustment>> GetSalesPriceDecrementedAdjustmentsByOrganizationAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch)
    {
        var id = int.Parse(_dataProtector.Unprotect(orgIdEnc));
        var salesPriceAdjustments = await Query()
            .Where(a => a.OrganizationId == id && a.AdjustmentTypeId == 2)
            .Include(a => a.Sales.Customer)
            .Include(a => a.Organization)
            .Include(a => a.OrgBranch)
            .SelectAsync();
        var list = salesPriceAdjustments.ToList();
        if (isRequiredBranch)
        {
            list = list.Where(s => branchIds.Contains(s.OrgBranchId.Value)).ToList();
        }
        list.ForEach(delegate (SalesPriceAdjustment salesPriceAdjustment)
        {
            salesPriceAdjustment.EncryptedId =
                _dataProtector.Protect(salesPriceAdjustment.SalesPriceAdjustmentId.ToString());
        });
        return list;
    }

    public async Task<IEnumerable<SalesPriceAdjustment>> GetSalesPriceIncrementedAdjustmentsByOrganization(string orgIdEnc)
	{
		var id = int.Parse(_dataProtector.Unprotect(orgIdEnc));
		var salesPriceAdjustments = await Query()
			.Where(a => a.OrganizationId == id && a.AdjustmentTypeId == 1)
			.Include(a => a.Sales.Customer)
			.Include(a => a.Organization)
			.Include(a => a.OrgBranch)
			.SelectAsync();
		var list = salesPriceAdjustments.ToList();
		list.ForEach(delegate (SalesPriceAdjustment salesPriceAdjustment)
		{
			salesPriceAdjustment.EncryptedId =
				_dataProtector.Protect(salesPriceAdjustment.SalesPriceAdjustmentId.ToString());
		});
		return list;
	}

	public async Task<int> InsertPriceAdjustment(SalesPriceAdjustmentCombineParamsViewModel model)
	{
		try
		{
			const string sql = "EXEC [dbo].[SpInsertSalePriceAdjustmentCombined]  " +
			                   "@AdjustmentTypeId," +
			                   "@OrganizationId," +
							   "@OrgBranchId," +
							   "@SalesId," +
			                   "@InvoiceNo," +
			                   "@InvoiceDate," +
			                   "@ClientNoteNo," +
			                   "@ClientNoteTime," +
			                   "@ReasonOfChange," +
			                   "@VehicleTypeId," +
			                   "@VehicleName," +
			                   "@VehicleRegNo," +
			                   "@VehicleDriverName," +
			                   "@VehicleDriverContactNo," +
			                   "@IsNotePrinted," +
			                   "@NoteNo ," +
			                   "@NotePrintedBy," +
			                   "@NotePrintedTime," +
			                   "@AdjustmentRemarks ," +
			                   "@ReferenceKey," +
			                   "@CreatedBy ," +
			                   "@CreatedTime," +
			                   "@SalesPriceAdjustmentDetailsJson," +
			                   "@ContentJson";
			var detailsJson = Newtonsoft.Json.JsonConvert.SerializeObject(model.DetailList);
			string contentJson = null; //
			if (model.ContentInfoJson != null)
			{
				contentJson = Newtonsoft.Json.JsonConvert.SerializeObject(model.ContentInfoJson);
			}

			model.CreatedTime = DateTime.Now;

			var parameter = new DynamicParameters();

			parameter.Add("@AdjustmentTypeId", model.AdjustmentTypeId);
			parameter.Add("@OrganizationId", model.OrganizationId);
			parameter.Add("@OrgBranchId", model.OrgBranchId);
			parameter.Add("@SalesId", model.SalesId);
			parameter.Add("@InvoiceNo", model.InvoiceNo);
			parameter.Add("@InvoiceDate", StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(model.InvoiceDate));
			parameter.Add("@ClientNoteNo", model.ClientNoteNo);
			parameter.Add("@ClientNoteTime", StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(model.ClientNoteTime));
			parameter.Add("@ReasonOfChange", model.ReasonOfChange);
			parameter.Add("@VehicleTypeId", model.VehicleTypeId);
			parameter.Add("@VehicleName", model.VehicleName);
			parameter.Add("@VehicleRegNo", model.VehicleRegNo);
			parameter.Add("@VehicleDriverName", model.VehicleDriverName);
			parameter.Add("@VehicleDriverContactNo", model.VehicleDriverContactNo);
			parameter.Add("@IsNotePrinted", model.IsNotePrinted);
			parameter.Add("@NoteNo", model.NoteNo);
			parameter.Add("@NotePrintedBy", model.NotePrintedBy);
			parameter.Add("@NotePrintedTime", StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(model.NotePrintedTime));
			parameter.Add("@AdjustmentRemarks", model.AdjustmentRemarks);
			parameter.Add("@ReferenceKey", model.ReferenceKey);
			parameter.Add("@CreatedBy", model.CreatedBy);
			parameter.Add("@CreatedTime", StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(model.CreatedTime));
			parameter.Add("@SalesPriceAdjustmentDetailsJson", detailsJson);
			parameter.Add("@ContentJson", contentJson);
			using var queryMultiple = await _context.Database.GetDbConnection()
				.QueryMultipleAsync(sql, parameter, commandTimeout: 500);
			return await queryMultiple.ReadFirstAsync<int>();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

    public async Task<IEnumerable<SpGetCreditNoteMushakForSalesPriceReduce>> GetCreditNoteMushakForSalesPriceReduce(string priceAdjustmentIdEnc, int userId)
    {
        var id = int.Parse(_dataProtector.Unprotect(priceAdjustmentIdEnc));
        try
        {
            const string sql = "EXEC [dbo].[SpGetCreditNoteMushakForSalesPriceReduce]  " +
                               "@SalesPriceAdjustmentId," +
                               "@UserId";

            var parameter = new DynamicParameters();

            parameter.Add("@SalesPriceAdjustmentId", id);
            parameter.Add("@UserId", userId);
            using var queryMultiple = await _context.Database.GetDbConnection()
                .QueryMultipleAsync(sql, parameter, commandTimeout: 500);
            return await queryMultiple.ReadAsync<SpGetCreditNoteMushakForSalesPriceReduce>();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}