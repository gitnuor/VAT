using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class AdjustmentService : ServiceBase<Adjustment>, IAdjustmentService
{
    private readonly IAdjustmentRepository _repository;

    public AdjustmentService(IAdjustmentRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Adjustment>> Get(int organizationId, string searchQuery = null)
    {
        var queryableResult = _repository.Queryable().Where(q => q.OrganizationId == organizationId);

        if (string.IsNullOrEmpty(searchQuery)) return await queryableResult.ToListAsync(CancellationToken.None);

        searchQuery = searchQuery.ToLower();
        queryableResult = queryableResult.Where(q =>
                q.Amount.ToString(CultureInfo.InvariantCulture).Contains(searchQuery)
                || q.Year.ToString(CultureInfo.InvariantCulture).Contains(searchQuery)
                || CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(q.Month).ToLower().Contains(searchQuery)
        //|| q.Description.ToLower().Contains(searchQuery)
        );

        return await queryableResult.ToListAsync(CancellationToken.None);
    }

    public Task<IEnumerable<Adjustment>> GetByOrg(string pOrgId)
    {
        return _repository.GetByOrg(pOrgId);
    }
}