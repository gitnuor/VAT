using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.StoredProcedureModel;

namespace vms.service.Services.TransactionService;

public interface IDamageInvoiceListService
{


    Task<List<SpDamageInvoiceList>> GetDamageInvoiceList(int organizationId, int productId);
    Task<bool> InsertDamage(SpDamageInsert Damage);
    Task<List<SpDamage>> GetDamageList(int organizationId);
}