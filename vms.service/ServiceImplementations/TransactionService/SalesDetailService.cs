using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class SalesDetailService : ServiceBase<SalesDetail>, ISalesDetailService
{
    private readonly ISalesDetailRepository _repository;
    public SalesDetailService(ISalesDetailRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<SalesDetail>> GetAllSalesDetails(string idEnc)
    {
        return await _repository.GetAllSalesDetails(idEnc);
    }

    public async Task<IEnumerable<SalesDetail>> GetSalesDetails(string idEnc)
    {
        return await _repository.GetSalesDetails(idEnc: idEnc);
    }
}