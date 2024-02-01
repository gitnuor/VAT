using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.viewModels;
using vms.entity.viewModels.ProductViewModel;

namespace vms.repository.Repository.tbl;

public interface IProductRepository : IRepositoryBase<Product>
{
    vmProduct GetProductAutoComplete();
    Task<bool> InsertBulk(List<ProductDataImportViewModel> products, int organizationId, int createBy);
    Task<IEnumerable<Product>> GetProducts(string orgIdEnc);
    Task<IEnumerable<ViewProduct>> GetProductListByOrg(string orgIdEnc);
    Task<Product> GetProduct(string idEnc);
    List<SpGetProductForSelfProductionReceive> SpGetProductForSelfProductionReceive(int orgId);
    List<SpGetProductForContractualProductionReceive> SpGetProductForContractualProductionReceive(int orgId, int conProId);
}