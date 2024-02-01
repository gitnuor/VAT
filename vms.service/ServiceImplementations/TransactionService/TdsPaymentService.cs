using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.StoredProcedureModel.MushakReturn;
using vms.entity.viewModels.Payment;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class TdsPaymentService : ServiceBase<TdsPayment>, ITdsPaymentService
{
    private readonly ITdsPaymentRepository _repository;

    public TdsPaymentService(ITdsPaymentRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<List<SpGetTdsPaymentChallan>> TdsPaymentChallan(int tdsPaymentId)
    {
        return await _repository.TdsPaymentChallan(tdsPaymentId);
    }


    public async Task<int> InsertTdsPayment(VmTdsPaymentPost tdsPayment)
    {
        return await _repository.InsertTdsPayment(tdsPayment);
    }


    public IQueryable<ViewTdsPayment> GetTdsPayments(int organizationId)
    {
        return _repository.GetTdsPayments(organizationId);
    }

    public IQueryable<ViewTdsPayment> GetTdsPayments()
    {
        return _repository.GetTdsPayments();
    }

    public Task<ViewTdsPayment> GetTdsPayment(int paymentId)
    {
        return _repository.GetTdsPayment(paymentId);
    }
}