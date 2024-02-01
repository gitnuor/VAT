using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.StoredProcedureModel.MushakReturn;
using vms.entity.viewModels.Payment;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class MushakReturnPaymentService : ServiceBase<MushakReturnPayment>, IMushakReturnPaymentService
{
    private readonly IMushakReturnPaymentRepository _repository;

    public MushakReturnPaymentService(IMushakReturnPaymentRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<List<SpGetMushakReturnPaymentChallan>> MushakReturnPaymentChallan(int mushakReturnPaymentId)
    {
        return await _repository.MushakReturnPaymentChallan(mushakReturnPaymentId);
    }


    public async Task<int> InsertVdsPayment(VmVdsPaymentPost vdsPayment)
    {
        return await _repository.InsertVdsPayment(vdsPayment);
    }

    public IQueryable<ViewMushakReturnSelfPayment> GetMushakReturnSelfPayments(int organizationId)
    {
        return _repository.GetMushakReturnSelfPayments(organizationId);
    }

    public IQueryable<ViewMushakReturnSelfPayment> GetMushakReturnSelfPayments()
    {
        return _repository.GetMushakReturnSelfPayments();
    }

    public Task<ViewMushakReturnSelfPayment> GetMushakReturnSelfPayment(int paymentId)
    {
        return _repository.GetMushakReturnSelfPayment(paymentId);
    }

    public IQueryable<ViewMushakReturnVdsPayment> GetMushakReturnVdsPayments(int organizationId)
    {
        return _repository.GetMushakReturnVdsPayments(organizationId);
    }

    public IQueryable<ViewMushakReturnVdsPayment> GetMushakReturnVdsPayments()
    {
        return _repository.GetMushakReturnVdsPayments();
    }

    public Task<ViewMushakReturnVdsPayment> GetMushakReturnVdsPayment(int paymentId)
    {
        return _repository.GetMushakReturnVdsPayment(paymentId);
    }
}