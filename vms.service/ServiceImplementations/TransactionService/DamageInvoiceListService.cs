using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.StoredProcedureModel;
using vms.repository.Repository.sp;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class DamageInvoiceListService : IDamageInvoiceListService
{
    private readonly IDamageInvoiceListRepository _DamageInvoiceListRepository;


    public DamageInvoiceListService(IDamageInvoiceListRepository DamageInvoiceListRepository)
    {
        _DamageInvoiceListRepository = DamageInvoiceListRepository;
    }

    public async Task<List<SpDamageInvoiceList>> GetDamageInvoiceList(int organizationId, int productId)
    {
        return await _DamageInvoiceListRepository.GetDamageInvoiceList(organizationId, productId);
    }
    public async Task<List<SpDamage>> GetDamageList(int organizationId)
    {
        return await _DamageInvoiceListRepository.GetDamageList(organizationId);
    }


    public async Task<bool> InsertDamage(SpDamageInsert Damage)
    {

        return await _DamageInvoiceListRepository.InsertDamage(Damage);
    }

}