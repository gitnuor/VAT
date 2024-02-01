using System.Threading.Tasks;
using vms.entity.viewModels;
using vms.repository.Repository.sp;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class ApiSpInsertService : IApiSpInsertService
{
    private readonly IApiSpInsertRepository _repository;

    public ApiSpInsertService(IApiSpInsertRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> InsertPurchaseExcel(DataimportPurchaseFinal purchase, int orgID, int Uid, string security)
    {
        return await _repository.InsertPurchaseExcel(purchase, orgID, Uid, security);
    }

    public async Task<bool> InsertSaleExcel(DataimportSalesFinal Sale, int orgID, int Uid, string security)
    {
        return await _repository.InsertSaleExcel(Sale, orgID, Uid, security);
    }

    public async Task<bool> InsertPurchase(VmPurchase purchase)
    {
        return await _repository.InsertPurchase(purchase);
    }

    public async Task<bool> InsertPurchase(vmPurchasePost purchase)
    {
        return await _repository.InsertPurchase(purchase);
    }

    public async Task<bool> InsertBulkPurchase(vmPurchaseBulkPost purchase)
    {
        return await _repository.InsertBulkPurchase(purchase);
    }

    public async Task<bool> InsertSale(vmSaleOrder saleOrder)
    {
        return await _repository.InsertSale(saleOrder);
    }

    public async Task<bool> InsertSale(vmSalesPost sales)
    {
        return await _repository.InsertSale(sales);
    }

    public async Task<bool> InsertBulkSale(vmSaleBulkPost sale)
    {
        return await _repository.InsertBulkSale(sale);
    }
}