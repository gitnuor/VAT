using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.viewModels.ProductMeasurementUnit;

namespace vms.service.Services.ProductService
{
    public interface IProductMeasurementUnitService : IServiceBase<ProductMeasurementUnit>
    {
        Task<IEnumerable<ProductMeasurementUnitIndexViewModel>> GetProductMeasurementUnits(string orgIdEnc);
        Task<ProductMeasurementUnitIndexViewModel> GetProductMeasurementUnit(string idEnc);
        Task<SelectList> GetProductMeasurementUnitSelectList(string pOrgId);
        Task<IEnumerable<SpGetMeasurementUnitByProductModel>> SpGetMeasurementUnitByProduct(int productId);
        Task<SelectList> SpGetMeasurementUnitByProductSelectList(int productId);
    }
}
