using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class PurchaseDetailService : ServiceBase<PurchaseDetail>, IPurchaseDetailService
{
    private readonly IPurchaseDetailRepository _repository;
    public PurchaseDetailService(IPurchaseDetailRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PurchaseDetail>> GetPurchaseDetails(string idEnc)
    {
        return await _repository.GetPurchaseDetails(idEnc: idEnc);
    }
}