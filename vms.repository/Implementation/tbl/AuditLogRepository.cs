using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class AuditLogRepository : RepositoryBase<AuditLog>, IAuditLogRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;

    public AuditLogRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }


    public async Task<IEnumerable<AuditLog>> GetAuditLogs(int orgIdEnc)
    {
        var auditLogs = await Query().Where(w => w.OrganizationId == orgIdEnc &&   w.IsActive == true && w.AuditOperationId == (int)EnumOperations.Edit)
            .Include(a => a.ObjectType).Include(a => a.AuditOperation).Include(a => a.User).Where(c => c.OrganizationId == orgIdEnc).SelectAsync();

        auditLogs.ToList().ForEach(delegate (AuditLog auditLog)
        {
            auditLog.EncryptedId = _dataProtector.Protect(auditLog.AuditLogId.ToString());
        });
        return auditLogs;
    }
    public async Task<AuditLog> GetAuditLog(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var auditLog = await Query()
            .Include(p => p.ObjectType).Include(p => p.User).Include(p => p.AuditOperation)
            .SingleOrDefaultAsync(x => x.AuditLogId == id, System.Threading.CancellationToken.None);
        auditLog.EncryptedId = _dataProtector.Protect(auditLog.AuditLogId.ToString());

        return auditLog;
    }






    public async Task<bool> RestoreAuditAsync(vmRestore vmRestore)
    {
        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                $"EXEC [dbo].[USP_AuditRestore]" +
                $"@TableName " +
                $",@PrimaryKeyName" +
                $",@PrimaryKey " +
                $",@AuditLogId "

                , new SqlParameter("@TableName", vmRestore.TableName)
                , new SqlParameter("@PrimaryKeyName", vmRestore.PrimaryKeyName)
                , new SqlParameter("@PrimaryKey", vmRestore.PrimaryKey)
                , new SqlParameter("@AuditLogId", vmRestore.AuditLogId)

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