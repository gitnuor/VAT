using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.StoredProcedureModel.MushakReturn;
using vms.entity.viewModels;
using vms.entity.viewModels.Payment;
using vms.repository.Repository.tbl;
using vms.utility;

namespace vms.repository.Implementation.tbl;

public class TdsPaymentRepository : RepositoryBase<TdsPayment>, ITdsPaymentRepository
{
	private readonly DbContext _context;

	public TdsPaymentRepository(DbContext context, IDataProtectionProvider pProtectionProvider,
		PurposeStringConstants pPurposeStringConstants) : base(context)
	{
		_context = context;
		pProtectionProvider.CreateProtector(pPurposeStringConstants.UserIdQueryString);
	}

	public async Task<List<SpGetTdsPaymentChallan>> TdsPaymentChallan(int tdsPaymentId)
	{
		var parameter = new DynamicParameters();
		parameter.Add("@TdsPaymentId", tdsPaymentId);
		var result = await _context.Database.GetDbConnection()
			.QueryAsync<SpGetTdsPaymentChallan>("SpGetTdsPaymentChallan @TdsPaymentId", parameter, commandTimeout: 500);

		return result.ToList();
	}


	public async Task<int> InsertTdsPayment(VmTdsPaymentPost tdsPayment)
	{
		try
		{
			string sql = $"EXEC [dbo].[SpInsertTdsPayment]  " +
			             $"@OrganizationId," +
			             $"@CustomsAndVATCommissionarateId," +
			             $"@MushakYear," +
			             $"@MushakMonth," +
			             $"@PaymentDate," +
			             $"@BankId," +
			             $"@BankBranchName," +
			             $"@BankBranchCountryId," +
			             $"@BankBranchDistrictOrCityId," +
			             $"@EconomicCode1stDisit," +
			             $"@EconomicCode2ndDisit," +
			             $"@EconomicCode3rdDisit," +
			             $"@EconomicCode4thDisit," +
			             $"@EconomicCode5thDisit," +
			             $"@EconomicCode6thDisit," +
			             $"@EconomicCode7thDisit," +
			             $"@EconomicCode8thDisit," +
			             $"@EconomicCode9thDisit," +
			             $"@EconomicCode10thDisit," +
			             $"@EconomicCode11thDisit," +
			             $"@EconomicCode12thDisit," +
			             $"@EconomicCode13thDisit," +
			             $"@CreatedBy," +
			             $"@CreatedTime," +
			             $"@TdsPaymentForPurchaseJson";

			string paymentForTdsJson = null; //
			if (tdsPayment.paymentForTdsList != null)
			{
				paymentForTdsJson = Newtonsoft.Json.JsonConvert.SerializeObject(tdsPayment.paymentForTdsList);
			}

			var parameter = new DynamicParameters();

			parameter.Add("@OrganizationId", tdsPayment.OrganizationId);
			parameter.Add("@CustomsAndVATCommissionarateId", tdsPayment.CustomsAndVatcommissionarateId);
			parameter.Add("@MushakYear", tdsPayment.MushakYear);
			parameter.Add("@MushakMonth", tdsPayment.MushakMonth);
			parameter.Add("@PaymentDate",
				StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(tdsPayment.PaymentDate));
			parameter.Add("@BankId", tdsPayment.BankId);
			parameter.Add("@BankBranchName", tdsPayment.BankBranchName);
			parameter.Add("@BankBranchCountryId", tdsPayment.BankBranchCountryId);
			parameter.Add("@BankBranchDistrictOrCityId", tdsPayment.BankBranchDistrictOrCityId);
			parameter.Add("@EconomicCode1stDisit", tdsPayment.EconomicCode1stDisit);
			parameter.Add("@EconomicCode2ndDisit", tdsPayment.EconomicCode2ndDisit);
			parameter.Add("@EconomicCode3rdDisit", tdsPayment.EconomicCode3rdDisit);
			parameter.Add("@EconomicCode4thDisit", tdsPayment.EconomicCode4thDisit);
			parameter.Add("@EconomicCode5thDisit", tdsPayment.EconomicCode5thDisit);
			parameter.Add("@EconomicCode6thDisit", tdsPayment.EconomicCode6thDisit);
			parameter.Add("@EconomicCode7thDisit", tdsPayment.EconomicCode7thDisit);
			parameter.Add("@EconomicCode8thDisit", tdsPayment.EconomicCode8thDisit);
			parameter.Add("@EconomicCode9thDisit", tdsPayment.EconomicCode9thDisit);
			parameter.Add("@EconomicCode10thDisit", tdsPayment.EconomicCode10thDisit);
			parameter.Add("@EconomicCode11thDisit", tdsPayment.EconomicCode11thDisit);
			parameter.Add("@EconomicCode12thDisit", tdsPayment.EconomicCode12thDisit);
			parameter.Add("@EconomicCode13thDisit", tdsPayment.EconomicCode13thDisit);
			parameter.Add("@CreatedBy", tdsPayment.CreatedBy);
			parameter.Add("@CreatedTime",
				StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(tdsPayment.CreatedTime));
			parameter.Add("@TdsPaymentForPurchaseJson", paymentForTdsJson);
			using var queryMultiple = _context.Database.GetDbConnection()
				.QueryMultiple(sql, parameter, commandTimeout: 500);
			return await queryMultiple.ReadFirstAsync<int>();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public IQueryable<ViewTdsPayment> GetTdsPayments(int organizationId)
	{
		return _context.Set<ViewTdsPayment>().Where(x => x.OrganizationId == organizationId);
	}

	public IQueryable<ViewTdsPayment> GetTdsPayments()
	{
		return _context.Set<ViewTdsPayment>();
	}

	public Task<ViewTdsPayment> GetTdsPayment(int paymentId)
	{
		return _context.Set<ViewTdsPayment>()
			.FirstOrDefaultAsync(x => x.TdsPaymentId == paymentId);
	}
}