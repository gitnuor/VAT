using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vms.entity.StoredProcedureModel;
using vms.repository.Repository.sp;

namespace vms.repository.Implementation.sp;

public class DamageInvoiceListRepository : IDamageInvoiceListRepository
{
    private readonly DbContext _context;

    public DamageInvoiceListRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<List<SpDamageInvoiceList>> GetDamageInvoiceList(int organizationId, int productId)
    {
        return await _context.Set<SpDamageInvoiceList>().FromSqlRaw("SpDamageInfo @OrganizationId={0},@ProductId={1}", organizationId, productId).ToListAsync();
    }

    public async Task<List<SpDamage>> GetDamageList(int organizationId)
    {
        var item = await _context.Set<SpDamage>().FromSqlRaw("SpDamageList @OrganizationId={0}", organizationId).ToListAsync();
        return item;
    }

    public async Task<bool> InsertDamage(SpDamageInsert Damage)
    {
        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                $"EXEC [dbo].[SpDamageInsert]" +
                $"@OrganizationId " +
                $",@VoucherNo" +
                $",@ProductId" +
                $",@DamageQty" +
                $",@DamageTypeId" +
                $",@Description" +
                $",@CreatedBy " +
                $",@CreatedTime "
                , new SqlParameter("@OrganizationId", Damage.OrganizationId)
                , new SqlParameter("@VoucherNo", DBNull.Value)
                , new SqlParameter("@ProductId", Damage.ProductId)
                , new SqlParameter("@DamageQty", Damage.DamageQty)
                , new SqlParameter("@DamageTypeId", Damage.DamageTypeId)
                , new SqlParameter("@Description", Damage.Description)
                , new SqlParameter("@CreatedBy", Damage.CreatedBy)
                , new SqlParameter("@CreatedTime", Damage.CreatedTime)
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return await Task.FromResult(true);
    }
}