using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vms.entity.StoredProcedureModel;
using vms.repository.Repository.sp;

namespace vms.repository.Implementation.sp;

public class SpGetSalePagedRepository: ISpGetSalePagedRepository
{
    private readonly DbContext _context;
    public SpGetSalePagedRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<List<spGetSalePaged>> GetSalePaged(int PageSize, int PageNumber, int OrganizationId)
    {
        try
        {
            return await _context.Set<spGetSalePaged>().FromSqlRaw("spGetSalePaged @PageSize={0}, @PageNumber={1},@OrganizationId={2}", PageSize, PageNumber,OrganizationId).ToListAsync();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}