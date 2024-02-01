using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.ProductService;

namespace vms.service.ServiceImplementations.ProductService;

public class OverHeadCostService : ServiceBase<OverHeadCost>, IOverHeadCostService
{
    private readonly IOverHeadCostRepository _repository;

    public OverHeadCostService(IOverHeadCostRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<SelectList> GetOverHeadCostSelectList(int pOrgId)
    {
        return new(await _repository.GetOverHeadCost(pOrgId),
            nameof(OverHeadCost.OverHeadCostId),
            nameof(OverHeadCost.Name));
    }
}