using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.TransactionService;

public interface IDamageService : IServiceBase<Damage>
{
    Task<IEnumerable<Damage>> GetDamage(int orgIdEnc);
    Task<Damage> GetDamage(string idEnc);
    void InsertPurchaseDamage(VmPurchaseDamagePost vmPurchaseDamagePost, VmUserSession userSession);
    void InsertSalesDamage(VmSalesDamagePost vmPurchaseDamagePost, VmUserSession userSession);
    void InsertDamage(VmCombineDamagePost vmCombineDamagePost, VmUserSession userSession);
    Task<Damage> GetDamagewithDetails(int orgId, int damageId);
}