using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.StoredProcedureModel;

namespace vms.repository.Repository.tbl
{
    public interface IProductMeasurementUnitRepository : IRepositoryBase<ProductMeasurementUnit>
	{
		Task<IEnumerable<ProductMeasurementUnit>> GetProductMeasurementUnits(string orgIdEnc);
		Task<ProductMeasurementUnit> GetProductMeasurementUnit(string idEnc);	
			Task<IEnumerable<SpGetMeasurementUnitByProductModel>> SpGetMeasurementUnitByProduct(int productId);
	}
}
