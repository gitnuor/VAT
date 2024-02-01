using System.Collections.Generic;
using vms.entity.StoredProcedureModel;

namespace vms.repository.Repository.sp;

public interface IProductStoredProcedureRepository
{
    IEnumerable<SpGetProductForBom> GetProductForBom(int organizationId);
    IEnumerable<SpGetProductForProductionReceive> GetProductForProductionReceive(int organizationId, int contractualProductionId = 0);
    IEnumerable<SpGetProductForPurchase> GetProductForPurchase(int organizationId);
    IEnumerable<SpGetProductForSale> GetProductForSale(int organizationId, int branchId);

    IEnumerable<SpGetRawMaterialForInputOutputCoEfficient> SpGetRawMaterialForInputOutputCoEfficient(int organizationId);

    int SpGetNumberOfRawMaterialWithNotifiableChange(int productId);
    int SpGetNumberOfFinishedProductsWithNotifiableChange(int productId, decimal unitPrice);
}