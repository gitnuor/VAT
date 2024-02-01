using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class ProductionRepository : RepositoryBase<ProductionReceive>, IProductionRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;

    public ProductionRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }

    public async Task<string> InsertData(vmProductionReceive production)
    {
        string contentJson = null;
        if (production.ContentInfoJson != null)
            contentJson = Newtonsoft.Json.JsonConvert.SerializeObject(production.ContentInfoJson);
        production.ProductionId = 0;
        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                $"EXEC [dbo].[SpInsertProductionReceive]  " +
                $"@BatchNo," +
                $"@OrganizationId," +
                $"@OrgBranchId," +
                $"@ProductionId," +
                $"@ProductId," +
                $"@ReceiveQuantity," +
                $"@MeasurementUnitId," +
                $"@ReceiveTime," +
                $"@CreatedBy," +
                $"@CreatedTime," +
                $"@IsContractual," +
                $"@ContractualProductionId," +
                $"@ContractualProductionChallanNo," +
                $"@ContentJson"
                , new SqlParameter("@BatchNo", (object)production.BatchNo ?? DBNull.Value)
                , new SqlParameter("@OrganizationId", production.OrganizationId)
                , new SqlParameter("@OrgBranchId", production.OrgBranchId)
                , new SqlParameter("@ProductionId", production.ProductionId)
                , new SqlParameter("@ProductId", production.ProductId)
                , new SqlParameter("@ReceiveQuantity", production.ReceiveQuantity)
                , new SqlParameter("@MeasurementUnitId", production.MeasurementUnitId)
                , new SqlParameter("@ReceiveTime", production.ReceiveTime)
                , new SqlParameter("@CreatedBy", (object)production.CreatedBy ?? DBNull.Value)
                , new SqlParameter("@CreatedTime", (object)production.CreatedTime ?? DBNull.Value)
                , new SqlParameter("@IsContractual", (object)production.IsContractual ?? DBNull.Value)
                , new SqlParameter("@ContractualProductionId", (object)production.ContractualProductionId ?? DBNull.Value)
                , new SqlParameter("@ContractualProductionChallanNo", (object)production.ContractualProductionChallanNo ?? DBNull.Value)
                , new SqlParameter("@ContentJson", (object)contentJson ?? DBNull.Value)
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return await Task.FromResult(e.Message);
        }

        return await Task.FromResult("Successful");
    }

    public async Task<IEnumerable<ProductionReceive>> GetProductions(int orgIdEnc)
    {
        var productions = await Query().Where(w => w.OrganizationId == orgIdEnc).Include(p => p.Product).SelectAsync();

        productions.ToList().ForEach(delegate (ProductionReceive production)
        {
            production.EncryptedId = _dataProtector.Protect(production.ProductionId.ToString());
        });
        return productions;
    }

    public async Task<IEnumerable<ViewProductionReceive>> ViewProductionReceive(string orgIdEnc)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
        var list = await _context.Set<ViewProductionReceive>()
            .Where(s => s.OrganizationId == orgId)
            .OrderByDescending(s => s.ProductionReceiveId)
            .AsNoTracking()
            .ToListAsync();
        list.ForEach(s => s.EncryptedId = _dataProtector.Protect(s.ProductionReceiveId.ToString()));
        return list;
    }

    public async Task<IEnumerable<ViewProductionReceive>> ViewProductionReceiveByOrgAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
        var list = await _context.Set<ViewProductionReceive>()
            .Where(s => s.OrganizationId == orgId)
            .OrderByDescending(s => s.ProductionReceiveId)
            .AsNoTracking()
            .ToListAsync();
        if (isRequiredBranch)
        {
            list = list.Where(s => branchIds.Contains(s.BranchId.Value)).ToList();
        }
        list.ForEach(s => s.EncryptedId = _dataProtector.Protect(s.ProductionReceiveId.ToString()));
        return list;
    }
}