using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;

namespace vms.service.Services.ProductService;

public interface IOverHeadCostService : IServiceBase<OverHeadCost>
{
    Task<SelectList> GetOverHeadCostSelectList(int pOrgId);
}