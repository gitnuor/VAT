using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vms.entity.StoredProcedureModel;
using vms.repository.Repository.sp;

namespace vms.repository.Implementation.sp;

public class AutocompleteRepository : IAutocompleteRepository
{
    private readonly DbContext _context;

    public AutocompleteRepository(DbContext context)
    {
        _context = context;
    }
    public async Task<List<SpGetProductAutocompleteForBom>> ProductionReceiveAutoCompleteBOM(int organizationId, string searchTerm)
    {
        try
        {
            return await _context.Set<SpGetProductAutocompleteForBom>().FromSqlRaw("SpGetProductAutocompleteForBom @OrganizationId={0}, @ProductSearchTerm={1}", organizationId, searchTerm).ToListAsync();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public async Task<List<SpGetProductAutocompleteForPriceSetu>> ProductionReceiveAutoCompletePriceSetup(int organizationId, string searchTerm)
    {
        try
        {
            return await _context.Set<SpGetProductAutocompleteForPriceSetu>().FromSqlRaw("SpGetProductAutocompleteForPriceSetu @OrganizationId={0}, @ProductSearchTerm={1}", organizationId, searchTerm).ToListAsync();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public async Task<List<SpGetProductAutocompleteForPurchase>> GetProductAutocompleteForPurchases(int organizationId, string searchTerm)
    {
        return await _context.Set<SpGetProductAutocompleteForPurchase>().FromSqlRaw("SpGetProductAutocompleteForPurchase @OrganizationId={0}, @ProductSearchTerm={1}", organizationId, searchTerm).ToListAsync();
    }
    public async Task<List<SpGetProductAutocompleteForProductionReceive>> ProductionReceiveAutoComplete(int organizationId, string searchTerm,int contructId=0)
    {
        return await _context.Set<SpGetProductAutocompleteForProductionReceive>().FromSqlRaw("SpGetProductAutocompleteForProductionReceive @OrganizationId={0},@ContractualProductionId={1}, @ProductSearchTerm={2}", organizationId,contructId, searchTerm).ToListAsync();
    }

    public async Task<List<SpGetProductAutocompleteForSale>> GetProductAutocompleteForSales(int organizationId, string searchTerm)
    {
        return await _context.Set<SpGetProductAutocompleteForSale>().FromSqlRaw("SpGetProductAutocompleteForSale @OrganizationId={0}, @ProductSearchTerm={1}", organizationId, searchTerm).ToListAsync();
    }

    public async Task<List<SpGetVatType>> GetProductVatType(bool IsLocalSale, bool IsExport, bool IsLocalPurchase, bool IsImport, bool IsVds)
    {
        return await _context.Set<SpGetVatType>().FromSqlRaw("SpGetVatType @IsLocalSale={0}, @IsExport={1}, @IsLocalPurchase={2}, @IsImport={3}, @IsVds={4}", IsLocalSale, IsExport, IsLocalPurchase, IsImport, IsVds).ToListAsync();
    }

    public async Task<List<SpGetRawMaterialForProduction>> GetRawMaterialForProduction(int orgId, int branchId, int prodId)
    {
        return await _context.Set<SpGetRawMaterialForProduction>().FromSqlRaw("SpGetRawMaterialForProduction @OrganizationId={0}, @OrgBranchId={1}, @ProductId={2}", orgId, branchId, prodId).ToListAsync();

    }
}