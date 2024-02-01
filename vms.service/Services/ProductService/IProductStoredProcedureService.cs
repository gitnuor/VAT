using System.Collections.Generic;
using vms.entity.StoredProcedureModel;

namespace vms.service.Services.ProductService;

public interface IProductStoredProcedureService
{
    IEnumerable<SpGetProductForBom> GetProductForBom(int organizationId);
    IEnumerable<SpGetProductForProductionReceive> GetProductForProductionReceive(int organizationId, int contractualProductionId = 0);
    IEnumerable<SpGetProductForPurchase> GetProductForPurchase(int organizationId);
    IEnumerable<SpGetProductForPurchase> GetProductForPurchase(string pOrgId);
    IEnumerable<SpGetProductForSale> GetProductForSale(int organizationId, int branchId);

    IEnumerable<SpGetRawMaterialForInputOutputCoEfficient> SpGetRawMaterialForInputOutputCoEfficient(int organizationId);

    int SpGetNumberOfRawMaterialWithNotifiableChange(int productId);
    int SpGetNumberOfFinishedProductsWithNotifiableChange(int productId, decimal unitPrice);
}