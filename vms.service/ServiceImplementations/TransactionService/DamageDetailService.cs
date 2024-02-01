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

public class DamageDetailService : ServiceBase<DamageDetail>, IDamageDetailService
{
    private readonly IDamageDetailRepository _repository;

    public DamageDetailService(IDamageDetailRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DamageDetail>> Get(int organizationId, string searchQuery = null)
    {
        var queryableResult = _repository.Queryable();

        if (string.IsNullOrEmpty(searchQuery)) return await queryableResult.ToListAsync(CancellationToken.None);

        searchQuery = searchQuery.ToLower();
        queryableResult = queryableResult.Where(q =>
                q.DamageQty.ToString(CultureInfo.InvariantCulture).Contains(searchQuery)
                || q.DamageDescription.ToString(CultureInfo.InvariantCulture).Contains(searchQuery)

        //|| q.Description.ToLower().Contains(searchQuery)
        );

        return await queryableResult.ToListAsync(CancellationToken.None);
    }

    public async Task<IEnumerable<DamageDetail>> GetDamageDetails(int damageId)
    {
        return await _repository.GetDamageDetails(damageId);
    }
}