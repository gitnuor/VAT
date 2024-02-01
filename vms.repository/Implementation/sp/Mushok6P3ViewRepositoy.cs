using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vms.entity.StoredProcedureModel;
using vms.entity.viewModels.ReportsViewModel;
using vms.repository.Repository.sp;

namespace vms.repository.Implementation.sp;

public class Mushok6P3ViewRepositoy : IMushok6P3ViewRepositoy
{
    private readonly DbContext _context;
    public Mushok6P3ViewRepositoy(DbContext context)
    {
        _context = context;
    }
    public async Task<List<spGet6P3View>> GetMushok6p3(vmSalesDetails vm)
    {

        try
        {
            var item= await _context.Set<spGet6P3View>().FromSqlRaw("spGet6P3View @InvoiceNo={0},@CustomerName={1},@FromDate={2},@ToDate={3}", vm.InvoiceNo,vm.CustomerName,vm.FromDate,vm.ToDate ).ToListAsync();

            return item;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}