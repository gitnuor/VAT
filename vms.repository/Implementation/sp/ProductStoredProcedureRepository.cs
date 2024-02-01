using System;
using System.Collections.Generic;
using Dapper;
using Microsoft.EntityFrameworkCore;
using vms.entity.StoredProcedureModel;
using vms.repository.Repository.sp;

namespace vms.repository.Implementation.sp;

public class ProductStoredProcedureRepository : IProductStoredProcedureRepository
{
    private readonly DbContext _context;

    public ProductStoredProcedureRepository(DbContext context)
    {
        _context = context;
    }

    public IEnumerable<SpGetProductForBom> GetProductForBom(int organizationId)
    {
        var parameter = new DynamicParameters();
        parameter.Add("@OrganizationId", organizationId);
        try
        {
            return _context.Database.GetDbConnection()
                .Query<SpGetProductForBom>("SpGetProductForBom @OrganizationId", 
                    parameter, 
                    commandTimeout: 500);
        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message);
        }
    }

    public IEnumerable<SpGetProductForProductionReceive> GetProductForProductionReceive(int organizationId, int contractualProductionId = 0)
    {
        var parameter = new DynamicParameters();
        parameter.Add("@OrganizationId", organizationId);
        parameter.Add("@ContractualProductionId", contractualProductionId);
        try
        {
            return _context.Database.GetDbConnection()
                .Query<SpGetProductForProductionReceive>("SpGetProductForProductionReceive @OrganizationId, @ContractualProductionId",
                    parameter,
                    commandTimeout: 500);
        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message);
        }
    }

    public IEnumerable<SpGetProductForPurchase> GetProductForPurchase(int organizationId)
    {
        var parameter = new DynamicParameters();
        parameter.Add("@OrganizationId", organizationId);
        try
        {
            return _context.Database.GetDbConnection()
                .Query<SpGetProductForPurchase>("SpGetProductForPurchase @OrganizationId",
                    parameter,
                    commandTimeout: 500);
        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message);
        }
    }

    public IEnumerable<SpGetProductForSale> GetProductForSale(int organizationId, int branchId)
    {
        var parameter = new DynamicParameters();
        parameter.Add("@OrganizationId", organizationId);
        parameter.Add("@OrgBranchId", branchId);
        try
        {
            return _context.Database.GetDbConnection()
                .Query<SpGetProductForSale>("SpGetProductForSale @OrganizationId, @OrgBranchId",
                    parameter,
                    commandTimeout: 500);
        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message);
        }
    }

    public IEnumerable<SpGetRawMaterialForInputOutputCoEfficient> SpGetRawMaterialForInputOutputCoEfficient(int organizationId)
    {
        var parameter = new DynamicParameters();
        parameter.Add("@OrganizationId", organizationId);
        try
        {
            return _context.Database.GetDbConnection()
                .Query<SpGetRawMaterialForInputOutputCoEfficient>("SpGetRawMaterialForInputOutputCoEfficient @OrganizationId",
                    parameter,
                    commandTimeout: 500);
        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message);
        }
    }

    public int SpGetNumberOfRawMaterialWithNotifiableChange(int productId)
    {
        var parameter = new DynamicParameters();
        parameter.Add("@ProductId", productId);
        try
        {
            return _context.Database.GetDbConnection()
                .QuerySingle<int>("SpGetNumberOfRawMaterialWithNotifiableChange @ProductId",
                    parameter,
                    commandTimeout: 500);
        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message);
        }
    }

    public int SpGetNumberOfFinishedProductsWithNotifiableChange(int productId, decimal unitPrice)
    {
        var parameter = new DynamicParameters();
        parameter.Add("@ProductId", productId);
        parameter.Add("@UnitPrice", unitPrice);
        try
        {
            return _context.Database.GetDbConnection()
                .QuerySingle<int>("SpGetNumberOfFinishedProductsWithNotifiableChange @ProductId, @UnitPrice",
                    parameter,
                    commandTimeout: 500);
        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message);
        }
    }
}