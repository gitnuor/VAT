using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using vms.entity.models;
using vms.entity.viewModels;
using vms.entity.viewModels.BranchTransferSend;
using vms.repository.Repository.tbl;
using vms.utility;

namespace vms.repository.Implementation.tbl;

public class BranchTransferSendRepository : RepositoryBase<BranchTransferSend>, IBranchTransferSendRepository
{
    private readonly DbContext _context;
    private readonly IDataProtector _dataProtector;

    public BranchTransferSendRepository(DbContext context, IDataProtectionProvider protectionProvider,
        PurposeStringConstants purposeStringConstants) : base(context)
    {
        _context = context;
        _dataProtector = protectionProvider.CreateProtector(purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<BranchTransferSend>> GetBranchTransferSendsByOrganization(string orgIdEnc)
    {
        var id = int.Parse(_dataProtector.Unprotect(orgIdEnc));
        var branchTransferSends = await Query()
            .Where(a => a.OrganizationId == id)
            .Include(a => a.OrgBranchSender)
            .Include(a => a.OrgBranchReceiver)
            .SelectAsync();
        var list = branchTransferSends.ToList();
        list.ForEach(delegate(BranchTransferSend branchTransferSend)
        {
            branchTransferSend.EncryptedId =
                _dataProtector.Protect(branchTransferSend.BranchTransferSendId.ToString());
        });
        return list;
    }

    public async Task<int> InsertBranchTransferSend(BranchTransferSendParamViewModel model)
    {
        var productDetails = JsonConvert.SerializeObject(model.Products);
        var contents = model.Documents == null ? null : JsonConvert.SerializeObject(model.Documents);


        var currentTime = DateTime.Today;
        try
        {
            const string sql = "EXEC [dbo].[SpInsertBranchTransferSend]" + " @OrganizationId" + ", @SenderBranchId " +
                               ", @ReceiverBranchId" + ", @InvoiceNo" + ", @InvoiceDate" + ", @IsTransferChallanPrined" +
                               ",@TransferChallanNo" + ",@TransferChallanPrintedTime" + ",@TransferChallanPrintedBy" +
                               ", @ReceiverName" + ", @ReceiverContactNo" + ", @ShippingAddress" +
                               ", @ShippingCountryId" + ", @BranchTransferDate" + ", @DeliveryDate" + ", @IsComplete" +
                               ", @IsPosted" + ", @PostedBy" + ", @PostedTime" + ", @VehicleTypeId" + ", @VehicleName" +
							   ", @VehicleRegNo" + ", @VehicleDriverName" + ", @VehicleDriverContactNo" +
                               ", @BranchTransferSendRemarks" + ", @ReferenceKey" + ", @CreatedBy" + ", @CreatedTime" +
                               ", @BranchTransferDetailsJson" + ", @ContentJson";

            var parameter = new DynamicParameters();
            parameter.Add("@OrganizationId", model.OrganizationId);
            parameter.Add("@SenderBranchId", model.SenderBranchId);
            parameter.Add("@ReceiverBranchId", model.ReceiverBranchId);
            parameter.Add("@InvoiceNo", model.InvoiceNo);
            parameter.Add("@InvoiceDate",
                StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(model.InvoiceDate));
            parameter.Add("@IsTransferChallanPrined", model.IsTransferChallanPrined);
            parameter.Add("@TransferChallanNo", model.IsTransferChallanPrined);
            parameter.Add("@TransferChallanPrintedTime",
                StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(model.TransferChallanPrintedTime));
            parameter.Add("@TransferChallanPrintedBy", model.TransferChallanPrintedBy);
            parameter.Add("@ReceiverName", model.ReceiverName);
            parameter.Add("@ReceiverContactNo", model.ReceiverContactNo);
            parameter.Add("@ShippingAddress", model.ShippingAddress);
            parameter.Add("@ShippingCountryId", model.ShippingCountryId);
            parameter.Add("@BranchTransferDate",
                StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(model.BranchTransferDate));
            parameter.Add("@DeliveryDate",
                StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(model.DeliveryDate ?? currentTime));
            parameter.Add("@IsComplete", model.IsComplete);
            parameter.Add("@IsPosted", model.IsPosted);
            parameter.Add("@PostedBy", model.PostedBy);
            parameter.Add("@PostedTime", StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(model.PostedTime));
            parameter.Add("@VehicleTypeId", model.VehicleTypeId);
            parameter.Add("@VehicleName", model.VehicleName);
            parameter.Add("@VehicleRegNo", model.VehicleRegistrationNo);
            parameter.Add("@VehicleDriverName", model.DriverName);
            parameter.Add("@VehicleDriverContactNo", model.DriverMobile);
            parameter.Add("@BranchTransferSendRemarks", model.BranchTransferSendRemarks);
            parameter.Add("@ReferenceKey", model.ReferenceKey);
            parameter.Add("@CreatedBy", model.CreatedBy);
            parameter.Add("@CreatedTime",
                StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(model.CreatedTime ?? currentTime));
            parameter.Add("@BranchTransferDetailsJson", productDetails);
            parameter.Add("@ContentJson", contents);

            using var queryMultiple = await _context.Database.GetDbConnection()
                .QueryMultipleAsync(sql, parameter, commandTimeout: 500);
            return await queryMultiple.ReadFirstAsync<int>();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<SpGetBranchTransferChallanModel> GetBranchTransferChallan(string transferIdEnc, int userId)
    {
	    try
		{
			var id = int.Parse(_dataProtector.Unprotect(transferIdEnc));
			var model = new SpGetBranchTransferChallanModel();
			var parameter = new DynamicParameters();
			parameter.Add("@BranchTransferSendId", id);
			parameter.Add("@UserId", userId);
			const string sql = "EXEC [dbo].[SpGetBranchTransferChallan] @BranchTransferSendId, @UserId";
			using var queryMultiple = await _context.Database.GetDbConnection().QueryMultipleAsync(sql, parameter, commandTimeout: 500);
			model.TransferChallan = await queryMultiple.ReadFirstAsync<SpGetBranchTransferChallanMainModel>();
			model.TransferChallanDetails = await queryMultiple.ReadAsync<SpGetBranchTransferChallanDetailModel>();
			return model;
	    }
	    catch (Exception ex)
	    {
			throw new Exception(ex.Message);
		}
    }
}