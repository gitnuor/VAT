using System.Collections.Generic;
using Microsoft.AspNetCore.DataProtection;
using vms.entity.StoredProcedureModel;
using vms.entity.viewModels;
using vms.repository.Repository.sp;
using vms.service.Services.ProductService;

namespace vms.service.ServiceImplementations.ProductService;

public class ProductStoredProcedureService : IProductStoredProcedureService
{
    private readonly IProductStoredProcedureRepository _repository;
    private readonly IDataProtector _dataProtector;

    public ProductStoredProcedureService(IProductStoredProcedureRepository repository, IDataProtectionProvider protectionProvider, PurposeStringConstants purposeStringConstants)
    {
        _repository = repository;
        _dataProtector = protectionProvider.CreateProtector(purposeStringConstants.UserIdQueryString);
    }

    public IEnumerable<SpGetProductForBom> GetProductForBom(int organizationId)
    {
        return _repository.GetProductForBom(organizationId);
    }

    public IEnumerable<SpGetProductForProductionReceive> GetProductForProductionReceive(int organizationId, int contractualProductionId = 0)
    {
        return _repository.GetProductForProductionReceive(organizationId, contractualProductionId);
    }

    public IEnumerable<SpGetProductForPurchase> GetProductForPurchase(int organizationId)
    {
        return _repository.GetProductForPurchase(organizationId);
    }

    public IEnumerable<SpGetProductForPurchase> GetProductForPurchase(string pOrgId)
    {
        var id = int.Parse(_dataProtector.Unprotect(pOrgId));
        return _repository.GetProductForPurchase(id);
    }

    public IEnumerable<SpGetProductForSale> GetProductForSale(int organizationId, int branchId)
    {
        return _repository.GetProductForSale(organizationId, branchId);
    }

    public IEnumerable<SpGetRawMaterialForInputOutputCoEfficient> SpGetRawMaterialForInputOutputCoEfficient(int organizationId)
    {
        return _repository.SpGetRawMaterialForInputOutputCoEfficient(organizationId);
    }

    public int SpGetNumberOfRawMaterialWithNotifiableChange(int productId)
    {
        return _repository.SpGetNumberOfRawMaterialWithNotifiableChange(productId);
    }

    public int SpGetNumberOfFinishedProductsWithNotifiableChange(int productId, decimal unitPrice)
    {
        return _repository.SpGetNumberOfFinishedProductsWithNotifiableChange(productId, unitPrice);
    }
}