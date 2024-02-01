using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.ProductService;

public interface IBrandService : IServiceBase<Brand>
{
    Task<IEnumerable<CustomSelectListItem>> GetBrandSelectList(string pOrgId);
    Task<IEnumerable<Brand>> GetBrandList(string pOrgId);
}