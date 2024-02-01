using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.StoredProcedureModel.MushakReturn;
using vms.entity.viewModels.Payment;

namespace vms.repository.Repository.tbl;

public interface IMushakReturnPaymentRepository : IRepositoryBase<MushakReturnPayment>
{
    Task<List<SpGetMushakReturnPaymentChallan>> MushakReturnPaymentChallan(int mushakReturnPaymentId);
    Task<int> InsertVdsPayment(VmVdsPaymentPost vdsPayment);
    IQueryable<ViewMushakReturnSelfPayment> GetMushakReturnSelfPayments(int organizationId);
    IQueryable<ViewMushakReturnSelfPayment> GetMushakReturnSelfPayments();
    Task<ViewMushakReturnSelfPayment> GetMushakReturnSelfPayment(int paymentId);
    IQueryable<ViewMushakReturnVdsPayment> GetMushakReturnVdsPayments(int organizationId);
    IQueryable<ViewMushakReturnVdsPayment> GetMushakReturnVdsPayments();
    Task<ViewMushakReturnVdsPayment> GetMushakReturnVdsPayment(int paymentId);
}