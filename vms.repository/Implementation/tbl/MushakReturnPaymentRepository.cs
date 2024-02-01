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

public class MushakReturnPaymentRepository : RepositoryBase<MushakReturnPayment>, IMushakReturnPaymentRepository
{
	private readonly DbContext _context;

	public MushakReturnPaymentRepository(DbContext context, IDataProtectionProvider pProtectionProvider,
		PurposeStringConstants pPurposeStringConstants) : base(context)
	{
		_context = context;
		pProtectionProvider.CreateProtector(pPurposeStringConstants.UserIdQueryString);
	}

	public async Task<List<SpGetMushakReturnPaymentChallan>> MushakReturnPaymentChallan(int mushakReturnPaymentId)
	{
		var parameter = new DynamicParameters();
		parameter.Add("@MushakReturnPaymentId", mushakReturnPaymentId);
		var result = await _context.Database.GetDbConnection()
			.QueryAsync<SpGetMushakReturnPaymentChallan>("SpGetMushakReturnPaymentChallan @MushakReturnPaymentId",
				parameter, commandTimeout: 500);

		return result.ToList();
	}


	public async Task<int> InsertVdsPayment(VmVdsPaymentPost vdsPayment)
	{
		try
		{
			string sql = $"EXEC [dbo].[SpInsertMushakReturnPayment]  " +
			             $"@OrganizationId," +
			             $"@CustomsAndVATCommissionarateId," +
			             $"@MushakYear," +
			             $"@MushakMonth," +
			             $"@MushakReturnPaymentTypeId," +
			             $"@PaidAmount," +
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
			             $"@MushakReturnPaymentForVdsJson";

			string paymentForVdsJson = null; //
			if (vdsPayment.paymentForVdsList != null)
			{
				paymentForVdsJson = Newtonsoft.Json.JsonConvert.SerializeObject(vdsPayment.paymentForVdsList);
			}

			var parameter = new DynamicParameters();

			parameter.Add("@OrganizationId", vdsPayment.OrganizationId);
			parameter.Add("@CustomsAndVATCommissionarateId", vdsPayment.CustomsAndVatcommissionarateId);
			parameter.Add("@MushakYear", vdsPayment.MushakYear);
			parameter.Add("@MushakMonth", vdsPayment.MushakMonth);
			parameter.Add("@MushakReturnPaymentTypeId", vdsPayment.MushakReturnPaymentTypeId);
			parameter.Add("@PaidAmount", vdsPayment.PaidAmount);
			parameter.Add("@PaymentDate",
				StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(vdsPayment.PaymentDate));
			parameter.Add("@BankId", vdsPayment.BankId);
			parameter.Add("@BankBranchName", vdsPayment.BankBranchName);
			parameter.Add("@BankBranchCountryId", vdsPayment.BankBranchCountryId);
			parameter.Add("@BankBranchDistrictOrCityId", vdsPayment.BankBranchDistrictOrCityId);
			parameter.Add("@EconomicCode1stDisit", vdsPayment.EconomicCode1stDisit);
			parameter.Add("@EconomicCode2ndDisit", vdsPayment.EconomicCode2ndDisit);
			parameter.Add("@EconomicCode3rdDisit", vdsPayment.EconomicCode3rdDisit);
			parameter.Add("@EconomicCode4thDisit", vdsPayment.EconomicCode4thDisit);
			parameter.Add("@EconomicCode5thDisit", vdsPayment.EconomicCode5thDisit);
			parameter.Add("@EconomicCode6thDisit", vdsPayment.EconomicCode6thDisit);
			parameter.Add("@EconomicCode7thDisit", vdsPayment.EconomicCode7thDisit);
			parameter.Add("@EconomicCode8thDisit", vdsPayment.EconomicCode8thDisit);
			parameter.Add("@EconomicCode9thDisit", vdsPayment.EconomicCode9thDisit);
			parameter.Add("@EconomicCode10thDisit", vdsPayment.EconomicCode10thDisit);
			parameter.Add("@EconomicCode11thDisit", vdsPayment.EconomicCode11thDisit);
			parameter.Add("@EconomicCode12thDisit", vdsPayment.EconomicCode12thDisit);
			parameter.Add("@EconomicCode13thDisit", vdsPayment.EconomicCode13thDisit);
			parameter.Add("@CreatedBy", vdsPayment.CreatedBy);
			parameter.Add("@CreatedTime",
				StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(vdsPayment.CreatedTime));
			parameter.Add("@MushakReturnPaymentForVdsJson", paymentForVdsJson);
			using (var queryMultiple = _context.Database.GetDbConnection()
				       .QueryMultiple(sql, parameter, commandTimeout: 500))
			{
				return await queryMultiple.ReadFirstAsync<int>();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public IQueryable<ViewMushakReturnSelfPayment> GetMushakReturnSelfPayments(int organizationId)
	{
		return _context.Set<ViewMushakReturnSelfPayment>().Where(x => x.OrganizationId == organizationId);
	}

	public IQueryable<ViewMushakReturnSelfPayment> GetMushakReturnSelfPayments()
	{
		return _context.Set<ViewMushakReturnSelfPayment>();
	}

	public Task<ViewMushakReturnSelfPayment> GetMushakReturnSelfPayment(int paymentId)
	{
		return _context.Set<ViewMushakReturnSelfPayment>()
			.FirstOrDefaultAsync(x => x.MushakReturnPaymentId == paymentId);
	}

	public IQueryable<ViewMushakReturnVdsPayment> GetMushakReturnVdsPayments(int organizationId)
	{
		return _context.Set<ViewMushakReturnVdsPayment>().Where(x => x.OrganizationId == organizationId);
	}

	public IQueryable<ViewMushakReturnVdsPayment> GetMushakReturnVdsPayments()
	{
		return _context.Set<ViewMushakReturnVdsPayment>();
	}

	public Task<ViewMushakReturnVdsPayment> GetMushakReturnVdsPayment(int paymentId)
	{
		return _context.Set<ViewMushakReturnVdsPayment>()
			.FirstOrDefaultAsync(x => x.MushakReturnPaymentId == paymentId);
	}
}