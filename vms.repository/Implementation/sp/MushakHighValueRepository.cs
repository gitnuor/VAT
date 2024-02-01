using Dapper;
using Microsoft.EntityFrameworkCore;
using vms.entity.StoredProcedureModel.HighValue;
using vms.entity.viewModels.ReportsViewModel;
using vms.repository.Repository.sp;

namespace vms.repository.Implementation.sp;

public class MushakHighValueRepository : IMushakHighValueRepository
{
    private readonly DbContext _context;

    public MushakHighValueRepository(DbContext context)
    {
        _context = context;
    }

    public VmMushakHighValue GetMushakReturn(int organizationId, int year, int month,decimal low)
    {
        var mushakHighVal = new VmMushakHighValue();
        var parameter = new DynamicParameters();
        parameter.Add("@OrganizationId", organizationId);
        parameter.Add("@Year", year);
        parameter.Add("@Month", month);
        parameter.Add("@LowerLimitOfHighValSale", low);
        using (var queryMultiple = _context.Database.GetDbConnection().QueryMultiple("SpPurcSalesChallanForHighValue @OrganizationId, @Year, @Month , @LowerLimitOfHighValSale", parameter, commandTimeout: 500))
        {
            mushakHighVal.SpPurcSalesChallanForHighValuePurchaseList = queryMultiple.Read<SpPurcSalesChallanForHighValuePurchase>();
            mushakHighVal.SpPurcSalesChallanForHighValueSaleList = queryMultiple.Read<SpPurcSalesChallanForHighValueSale>();
               
        }
        return mushakHighVal;
    }
}